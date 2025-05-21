using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Add this attribute here!
[CreateAssetMenu(fileName = "PixelizeFeature", menuName = "Rendering/URP/Renderer Feature/Pixelize Feature")]
public class PixelizeFeature : ScriptableRendererFeature
{
    // ... rest of your PixelizeFeature code
    [SerializeField]
    private RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    [SerializeField]
    private int screenHeight = 144;

    private PixelizePass customPass;

    public override void Create()
    {
        customPass = new PixelizePass(renderPassEvent, screenHeight);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(customPass);
    }
}

