Shader "Custom/Lava"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _OverlayTexture("Overlay Texture", 2D) = "black" {}
        _Color("Color", Color) = (1, 1, 0, 1)
        _SecondaryColor("Secondary Color", Color) = (1, 0, 0, 1)
        _NoiseScale("Noise Scale", Range(1, 100)) = 25
        _Speed("Speed", Range(1, 20)) = 5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vertex
            #pragma fragment fragment

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct vertex_input
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct fragment_input
            {
                float2 uv : TEXCOORD0;
                float2 overlay_uv : TEXCOORD1;
                float4 vertex : SV_POSITION;
                float3 normal_ws : TEXCOORD2;
            };

            CBUFFER_START(UnityPerMaterial)

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_OverlayTexture);
            SAMPLER(sampler_OverlayTexture);
            float4 _MainTex_ST;
            float4 _OverlayTexture_ST;
            float4 _Color;
            float4 _SecondaryColor;
            float _NoiseScale;
            float _Speed;
            
            CBUFFER_END

            // Unity's Perlin noise
            // Source: https://docs.unity3d.com/Packages/com.unity.shadergraph@6.9/manual/Gradient-Noise-Node.html
            float2 noiseDirection(float2 p)
            {
                p = p % 289;
                float x = (34 * p.x + 1) * p.x % 289 + p.y;
                x = (34 * x + 1) * x % 289;
                x = frac(x / 41) * 2 - 1;
                return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
            }
            
            // Unity's Perlin noise
            // Source: https://docs.unity3d.com/Packages/com.unity.shadergraph@6.9/manual/Gradient-Noise-Node.html
            float noise(float2 p)
            {
                float2 ip = floor(p);
                float2 fp = frac(p);
                float d00 = dot(noiseDirection(ip), fp);
                float d01 = dot(noiseDirection(ip + float2(0, 1)), fp - float2(0, 1));
                float d10 = dot(noiseDirection(ip + float2(1, 0)), fp - float2(1, 0));
                float d11 = dot(noiseDirection(ip + float2(1, 1)), fp - float2(1, 1));
                fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
                return lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x);
            }

            // Get the light attenuation, which is a dot product between a normal to the object surface
            // and the light source direction
            float light_attenuation(const Light light, const fragment_input fragment_data)
            {
                return max(0, dot(fragment_data.normal_ws, light.direction));
            }

            // Main vertex program
            fragment_input vertex (vertex_input vertex_data)
            {
                fragment_input fragment_data;
                const float3 object_position = vertex_data.vertex.xyz;
                
                fragment_data.vertex = TransformObjectToHClip(object_position);
                fragment_data.uv = TRANSFORM_TEX(vertex_data.uv, _MainTex);
                fragment_data.overlay_uv = TRANSFORM_TEX(vertex_data.uv, _OverlayTexture);
                fragment_data.normal_ws = TransformObjectToWorldNormal(vertex_data.normal);
                
                return fragment_data;
            }

            // Main fragment program
            float4 fragment (const fragment_input fragment_data) : SV_Target
            {
                // Get albedo for main and shadow colors
                const float4 texture_sample = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, fragment_data.uv);
                const float4 main_albedo = texture_sample * _Color;
                const float4 secondary_albedo = texture_sample * _SecondaryColor;

                // Get the light attenuation
                const Light light = GetMainLight();
                const float attenuation = light_attenuation(light, fragment_data);

                // Get the noise
                float value = noise(fragment_data.vertex.xy / _NoiseScale + _Time.x * _Speed);
                const float4 main_color = (1.0 - value) * main_albedo;
                const float4 shadow_color = value * secondary_albedo;

                // Get the overlay texture and return the result
                const float4 overlay = SAMPLE_TEXTURE2D(_OverlayTexture, sampler_OverlayTexture, fragment_data.overlay_uv);
                //return overlay;
                return max(main_color + shadow_color, overlay) * attenuation;
            }
            ENDHLSL
        }
    }
}