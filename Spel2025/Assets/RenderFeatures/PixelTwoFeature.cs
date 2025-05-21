using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PixelTwoFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class CustomPassSettings
    {
        // This class contains the user settings for the pass, like the simulated resolution and depth ranges
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

        [Header("Pixelization Settings")]
        public float minDepth = 5.0f;
        public float maxDepth = 100.0f;

        public float nearSimulatedScreenHeight = 720f;
        public float farSimulatedScreenHeight = 64f;
    }

    [SerializeField] private CustomPassSettings settings; //initialize the settings class
    private PixelTwoPass customPass; //field for the custom render pass
    private const string SHADER_NAME = "Assets/PixelTwo"; //hardcoded path to the shader in the assets folder

    public override void Create()
    {
        customPass = new PixelTwoPass(settings, SHADER_NAME); //creates the custom pass through its constructor
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        //if (renderingData.cameraData.isPreviewCamera) return; <-- This would leave the preview cam unaffected, but we can't get it working

        renderer.EnqueuePass(customPass); // inject the pixelization pass into the URP
    }
}