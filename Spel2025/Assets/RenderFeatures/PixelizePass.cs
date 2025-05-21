using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


class PixelizePass : ScriptableRenderPass
{
    //private PixelizeFeature.CustomPassSettings settings;
    private int screenHeight;
    private RenderPassEvent renderPassEvent;

    /*
    private RenderTargetIdentifier colorBuffer, pixelBuffer;
    private int pixelBufferID = Shader.PropertyToID("_PixelBuffer");
    */
    private RTHandle m_CameraColorTargetHandle;
    private RTHandle m_PixelationBufferHandle;

    private Material material;
    private int pixelScreenHeight, pixelScreenWidth;

    public PixelizePass(RenderPassEvent renderEvent, int screenHeight)
    {
        this.screenHeight = screenHeight;
        this.renderPassEvent = renderEvent;

        if (this.material == null) // Check instance material
        {
            // Ensure the shader path is correct
            this.material = CoreUtils.CreateEngineMaterial("Assets/Pixelize");
            if (this.material == null)
            {
                Debug.LogError("PixelizePass: failed to create material from shader");
            }
        }

        //allocate the pixelation intermediate buffer
        m_PixelationBufferHandle = RTHandles.Alloc("_PixelationBuffer");
    }

    // This method is called before executing the render pass.
    // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
    // When empty this render pass will render to the active camera render target.
    // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
    // The render pipeline will ensure target setup and clearing happens in a performant manner.
    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        m_CameraColorTargetHandle = renderingData.cameraData.renderer.cameraColorTargetHandle;
        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;

        pixelScreenHeight = screenHeight;
        pixelScreenWidth = (int)(pixelScreenHeight * renderingData.cameraData.camera.aspect + 0.5f);

        material.SetVector("_BlockCount", new Vector2(pixelScreenWidth, pixelScreenHeight));
        material.SetVector("_BlockSize", new Vector2(1.0f / pixelScreenWidth, 1.0f / pixelScreenHeight));
        material.SetVector("_HalfBlockSize", new Vector2(0.5f / pixelScreenWidth, 0.5f / pixelScreenHeight));

        descriptor.height = pixelScreenHeight;
        descriptor.width = pixelScreenWidth;
        descriptor.depthBufferBits = 0;

        RenderingUtils.ReAllocateIfNeeded(ref m_PixelationBufferHandle, descriptor, FilterMode.Point, TextureWrapMode.Clamp, name: "_PixelationBuffer");
    }

    // Here you can implement the rendering logic.
    // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
    // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
    // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (material == null ||
            m_CameraColorTargetHandle?.rt == null || !m_CameraColorTargetHandle.rt.IsCreated() ||
            m_PixelationBufferHandle?.rt == null || !m_PixelationBufferHandle.rt.IsCreated())
        {
            Debug.LogError("PixelizePass: Execute() called with null material or invalid RTHandles.");
            return;
        }

        CommandBuffer cmd = CommandBufferPool.Get();
        using (new ProfilingScope(cmd, new ProfilingSampler("Pixelize Pass")))
        {
            Blit(cmd, m_CameraColorTargetHandle, m_PixelationBufferHandle, material); 
            Blit(cmd, m_PixelationBufferHandle, m_CameraColorTargetHandle);
        }

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    // cleanup any allocated resources that were created during the execution of this render pass.
    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        if (cmd == null) throw new System.ArgumentNullException("cmd");
        RTHandles.Release(m_PixelationBufferHandle);
    }

    public void DisposeMaterial()
    {
        CoreUtils.Destroy(material);
        material = null;
        if (m_PixelationBufferHandle != null) // check if it was ever allocated
        {
            RTHandles.Release(m_PixelationBufferHandle);
            m_PixelationBufferHandle = null;
        }
    }
}


