using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

class PixelTwoPass : ScriptableRenderPass
{
    private PixelTwoFeature.CustomPassSettings settings; //fetches user settings
    private Material material; //material that will contain the shader
    private string shaderPath; //file path to the shader

    // using RednerTargetIdentifier instead of RTHandles because of version differences
    /*
    private RenderTargetIdentifier colorBuffer, pixelBuffer;
    private int pixelBufferID = Shader.PropertyToID("_PixelBuffer"); // ID for intermediate buffer for the effects application
    */

    private RTHandle m_CameraColorTargetHandle2;
    private RTHandle m_PixelationBufferHandle2;

    // below is the constructor for the render pass itself
    public PixelTwoPass(PixelTwoFeature.CustomPassSettings settings, string pathToShader)
    {
        this.settings = settings;
        this.renderPassEvent = settings.renderPassEvent;
        this.shaderPath = pathToShader;

        if (this.material == null)
        {
            // Ensure the shader path is correct
            this.material = CoreUtils.CreateEngineMaterial("Assets/PixelTwo");
            if (this.material == null)
            {
                Debug.LogError("PixelizePass: failed to create material from shader");
            }
        }

        //allocate the pixelation intermediate buffer
        m_PixelationBufferHandle2 = RTHandles.Alloc("_PixelationBuffer2");
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        m_CameraColorTargetHandle2 = renderingData.cameraData.renderer.cameraColorTargetHandle;

        // the temporary buffer will be full resolution, the pixelation effect itself is handled by UV manipulation in the shader
        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
        descriptor.depthBufferBits = 0; // no depth buffer needed for this intermediate buffer

        // allows this pass to read scene depth and the camera color
        ConfigureInput(ScriptableRenderPassInput.Depth | ScriptableRenderPassInput.Color);

        //since the pixelation happens in the shader, we pass all needed settings as properties on the shader material
        if (material != null)
        {
            material.SetFloat("_MinDepth", settings.minDepth);
            material.SetFloat("_MaxDepth", settings.maxDepth);
            material.SetFloat("_NearSimulatedHeight", settings.nearSimulatedScreenHeight);
            material.SetFloat("_FarSimulatedHeight", settings.farSimulatedScreenHeight);
            // _ScreenParams is available globally in URP shaders
        }

        // here we fetch a temporary render target (buffer) using the ID from before
        RenderingUtils.ReAllocateIfNeeded(ref m_PixelationBufferHandle2, descriptor, FilterMode.Point, TextureWrapMode.Clamp, name: "_PixelationBuffer2");
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (material == null ||
            m_CameraColorTargetHandle2?.rt == null || !m_CameraColorTargetHandle2.rt.IsCreated() ||
            m_PixelationBufferHandle2?.rt == null || !m_PixelationBufferHandle2.rt.IsCreated())
        {
            Debug.LogError("PixelizePass: Execute() called with null material or invalid RTHandles.");
            return;
        }

        CommandBuffer cmd = CommandBufferPool.Get();
        using (new ProfilingScope(cmd, new ProfilingSampler("Depth Aware Pixelization"))) // allows us to see the pass in the inspector / frame debugger
        {
            // Blit from camera target to the temporary texture, applying the shader
            Blit(cmd, m_CameraColorTargetHandle2, m_PixelationBufferHandle2, material);

            // Blit the result from the temporary texture back to the cameras color target
            Blit(cmd, m_PixelationBufferHandle2, m_CameraColorTargetHandle2);
        }

        //executes all the commands that are needed for the pass, then releases the command buffer from memory
        context.ExecuteCommandBuffer(cmd); 
        CommandBufferPool.Release(cmd);
    }

    // a built in function we override, just like OnCameraSetup and Execute. This one is run when the camera is disabled
    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        if (cmd == null) throw new System.ArgumentNullException("cmd"); //had some issues with this
        RTHandles.Release(m_PixelationBufferHandle2); //releases the intermediary render target
    }

    public void DisposeMaterial()
    {
        CoreUtils.Destroy(material);
        material = null;
        if (m_PixelationBufferHandle2 != null) // check if it was ever allocated
        {
            RTHandles.Release(m_PixelationBufferHandle2);
            m_PixelationBufferHandle2 = null;
        }
    }
}
