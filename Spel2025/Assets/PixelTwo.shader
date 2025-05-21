Shader "Assets/PixelTwo"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // source texture from Blit
        _MinDepth ("Min Depth", Float) = 5.0
        _MaxDepth ("Max Depth", Float) = 100.0
        _NearSimulatedHeight ("Near Pixel Height", Float) = 720.0 // Higher value = less pixelation
        _FarSimulatedHeight ("Far Pixel Height", Float) = 64.0   // Lower value = more pixelation
    }

    //only one subshader here since we dont care about different hardware heheh
    SubShader
    {
        //these tags caterogize the shader for URP
        Tags
        {
            "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque"
        }
        // Standard for post-processing: no depth write, no culling, always pass depth test
        ZWrite Off //dont modify the depth / z-buffer
        Cull Off //no culling for post processing effects
        ZTest Always // the effect is drawn over everything else, it is never "behind" anything else in the z-buffer

        Pass
        {
            Name "PixelationDepthAware"

            HLSLINCLUDE
            #pragma vertex Vert // designates the vertex shader for the compiler
            #pragma fragment Frag // designates the fragment shader for the compiler

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            // needed for _CameraDepthTexture and depth conversion functions
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            // struct containing inputs for thge vertex shader
            struct Attributes
            {
                float4 positionOS   : POSITION; // OS means object space, these are the object space vertex positions
                float2 uv           : TEXCOORD0; // uv texture coordinates
            };

            //input struct for the fragment shader
            struct Varyings
            {
                float4 positionHCS  : SV_POSITION; // HCS is homogeneous clip space (calculated with built in matricies in the vertex shader)
                float2 uv           : TEXCOORD0; // more uv's for sampling _MainTex
                float4 screenPos    : TEXCOORD1; // screenPos is used to get screen-space UVs for depth sampling
            };

            TEXTURE2D(_MainTex); // declares that MainTex is a 2d texture
            SAMPLER(sampler_MainTex); // unity has a built in sampler for the camera color texture

            // from the pass scrips
            float _MinDepth;
            float _MaxDepth;
            float _NearSimulatedHeight;
            float _FarSimulatedHeight;

            //vertex shader, outputs input struct to be passed to the fragment shader
            Varyings Vert(Attributes IN)
            {
                Varyings OUT; //define output
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz); //transform verticies to HCS
                OUT.uv = IN.uv; //same UV's
                OUT.screenPos = OUT.positionHCS; // used to derive screen UVs for depth sampling
                return OUT;
            }
            ENDHLSL

            HLSLPROGRAM // specifies that the following is shader code for this pass

            half4 Frag(Varyings IN) : SV_Target
            {
                // calculate screen UV for depth sampling from screenPos
                // this converts clip space position to [0,1] UV range
                // these are also called Normalized Device Coordinates
                float2 screenUVForDepth = IN.screenPos.xy / IN.screenPos.w;
                screenUVForDepth = screenUVForDepth * 0.5 + 0.5; // converts Normalized Device Coordinates from -1,1 to 0,1 for UV

                //this may not be neccesary, but it was in Unity documentation
                #if UNITY_UV_STARTS_AT_TOP
                screenUVForDepth.y = 1.0 - screenUVForDepth.y;
                #endif

                // 1. get depth and convert to linear view space depth
                float rawDepth = SampleSceneDepth(screenUVForDepth);
                float viewSpaceDepth = LinearEyeDepth(rawDepth, _ZBufferParams); //makes sure that the depth values from the camera are linnear
                                                                                // we want this because we want a linear even spread of the pixelization in the range

                // 2. calculate current simulated screen height based on depth
                float depthFactor = saturate((viewSpaceDepth - _MinDepth) / (_MaxDepth - _MinDepth)); //goes from 0 to 1 in the interval between MinDepth and MaxDepth
                float currentSimulatedHeight = lerp(_NearSimulatedHeight, _FarSimulatedHeight, depthFactor); //lerp in the interval
                currentSimulatedHeight = max(1.0, currentSimulatedHeight); // ensure height is at least 1 to avoid division by zero or visual glitches

                // 3. calculate corresponding simulated screen width
                float screenAspectRatio = _ScreenParams.x / _ScreenParams.y;
                float currentSimulatedWidth = currentSimulatedHeight * screenAspectRatio;
                currentSimulatedWidth = max(1.0, currentSimulatedWidth); //same setup from above

                // 4. calculate block/pixel count for the current pixel based on its depth
                float2 currentBlockCount = float2(currentSimulatedWidth, currentSimulatedHeight);

                // 5. calculate UV dimensions of one "pixel block" and its half for centering
                float2 currentBlockUVDim = rcp(currentBlockCount); // equivalent to 1.0f / currentBlockCount. this is the size of a block in UV space
                float2 currentHalfBlockUVDim = currentBlockUVDim * 0.5f; //half of the value, used to sample the middle of the block

                // 6. perform pixelation: find the center of the block this UV falls into
                float2 blockGridPos = floor(IN.uv * currentBlockCount); // gives the integer "index" telling us which block we are in
                float2 blockCenterUV = blockGridPos * currentBlockUVDim + currentHalfBlockUVDim; // UV for the center of that block, since we land on the edge of it otherwise

                // 7. Sample the main texture using the calculated block-center UV
                // all the pixels in the original camera color (_MaixTex) will now sample
                // the same color if they are in the same simulated block
                half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, blockCenterUV);

                return color; //returns the final color of the pixel
            }
            ENDHLSL
        }
    }
    Fallback "Hidden/Universal Render Pipeline/FallbackError" //use the deault fallback shader if something goes wrong, also from docs hahah
}