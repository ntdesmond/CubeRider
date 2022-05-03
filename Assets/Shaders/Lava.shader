Shader "Custom/Lava"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _OverlayTexture("Overlay Texture", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 0, 0.5)
        _SecondaryColor("Secondary Color", Color) = (1, 0, 0, 0.5)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }

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
                float4 vertex : SV_POSITION;
                float3 normal_ws : TEXCOORD1;
            };

            CBUFFER_START(UnityPerMaterial)

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;
            float4 _Color;
            float4 _SecondaryColor;
            
            CBUFFER_END
            
            fragment_input vertex (vertex_input vertex_data)
            {
                fragment_input fragment_data;
                float3 pos_os = vertex_data.vertex.xyz;
                
                fragment_data.vertex = TransformObjectToHClip(pos_os);
                fragment_data.uv = TRANSFORM_TEX(vertex_data.uv, _MainTex);
                fragment_data.normal_ws = TransformObjectToWorldNormal(vertex_data.normal);
                
                return fragment_data;
            }

            float light_attenuation(const Light light, const fragment_input fragment_data)
            {
                return max(0, dot(fragment_data.normal_ws, light.direction));
            }
            
            float4 fragment (const fragment_input fragment_data) : SV_Target
            {
                // Get albedo for main and shadow colors
                const float4 texture_sample = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, fragment_data.uv);
                const float4 main_albedo = texture_sample * _Color;
                const float4 secondary_albedo = texture_sample * _SecondaryColor;

                // Get the light attenuation
                const Light light = GetMainLight();
                const float attenuation = light_attenuation(light, fragment_data);

                float random = GenerateHashedRandomFloat(fragment_data.vertex);
                const float4 main_color = random * main_albedo;
                const float4 shadow_color = (1.0 - random) * secondary_albedo;

                return (main_color + shadow_color) * attenuation;
            }
            ENDHLSL
        }
    }
}