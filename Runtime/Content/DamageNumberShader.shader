Shader "ErenAydin/DamageNumberShader"
{
    Properties
    {
        [NoScaleOffset]_Atlas("Atlas", 2D) = "white" {}
        _UVY ("UVY", Vector) = (0.2, 0.55, 0, 0) 
        _BaseScale ("BaseScale", float) = 1.0 

// instanced
        _AnimTime01 ("AnimTime01", Range(0,1)) = 0
        _AnimDirection ("AnimDirection", Vector) = ( 0, 0, 0, 0 )
        _Color("Color", Color) = (0, 0, 0, 0)
        _Scale ("Scale", float) = 1.0 
        _Number0("Number0", float) = -1
        _Number1("Number1", float) = -1
        _Number2("Number3", float) = -1
        _Number3("Number4", float) = -1
        _Number4("Number5", float) = -1
        _Number5("Number6", float) = -1
        _ValidNumberCount("ValidNumberCount", float) = -1
//
    }

    SubShader
    {
		Tags 
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue"="Transparent"
            "DisableBatching"="False"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalUnlitSubTarget"
		}

        Pass
        {
            Name "Forward+"
            Tags
            {
            }

            // Render State
            ZWrite Off
            ZTest Always
            Cull Back
            AlphaToMask On
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha


            HLSLPROGRAM
            #pragma target 2.0
            #pragma vertex UnlitPassVertex
            #pragma fragment UnlitPassFragment
            #pragma multi_compile_instancing

            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            
            struct Attributes
            {
                float4 positionOS : POSITION; // Vertex position in object space
                float3 normalOS : NORMAL; // Vertex normal vector in object space
                float4 tangentOS : TANGENT; // Vertex tangent vector in object space (plus bitangent sign)
                float2 uv : TEXCOORD0;
                uint vertexID : VERTEXID_SEMANTIC;
            
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            struct Varyings
            {
                float3 positionWS : TEXCOORD3; // Position in world space
                float3 normalWS : TEXCOORD4; // Normal vector in world space
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            CBUFFER_START(UnityPerMaterial)
            float4 _Atlas_ST;
            float _BaseScale;

            float _AnimTime01;
            float3 _AnimDirection;

            float2 _UVY;

            float4 _Color;
            float _Scale;
            
            float _Number0;
            float _Number1;
            float _Number2;
            float _Number3;
            float _Number4;
            float _Number5;
            float _ValidNumberCount;

            CBUFFER_END
            
            #if defined(DOTS_INSTANCING_ON)
            // DOTS instancing definitions
            UNITY_DOTS_INSTANCING_START(MaterialPropertyMetadata)
                UNITY_DOTS_INSTANCED_PROP_OVERRIDE_SUPPORTED(float, _AnimTime01)
                UNITY_DOTS_INSTANCED_PROP_OVERRIDE_SUPPORTED(float3, _AnimDirection)
                UNITY_DOTS_INSTANCED_PROP_OVERRIDE_SUPPORTED(float, _Number0)
                UNITY_DOTS_INSTANCED_PROP_OVERRIDE_SUPPORTED(float, _Number1)
                UNITY_DOTS_INSTANCED_PROP_OVERRIDE_SUPPORTED(float, _Number2)
                UNITY_DOTS_INSTANCED_PROP_OVERRIDE_SUPPORTED(float, _Number3)
                UNITY_DOTS_INSTANCED_PROP_OVERRIDE_SUPPORTED(float, _Number4)
                UNITY_DOTS_INSTANCED_PROP_OVERRIDE_SUPPORTED(float, _Number5)
                UNITY_DOTS_INSTANCED_PROP_OVERRIDE_SUPPORTED(float, _ValidNumberCount)
                UNITY_DOTS_INSTANCED_PROP_OVERRIDE_SUPPORTED(float, _Scale)
                UNITY_DOTS_INSTANCED_PROP_OVERRIDE_SUPPORTED(float4, _Color)
            UNITY_DOTS_INSTANCING_END(MaterialPropertyMetadata)
            // DOTS instancing usage macros
            #define UNITY_ACCESS_HYBRID_INSTANCED_PROP(var, type) UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(type, var)
            #elif defined(UNITY_INSTANCING_ENABLED)
            // Unity instancing definitions
            UNITY_INSTANCING_BUFFER_START(SGPerInstanceData)
                UNITY_DEFINE_INSTANCED_PROP(float, _AnimTime01)
                UNITY_DEFINE_INSTANCED_PROP(float3, _AnimDirection)
                UNITY_DEFINE_INSTANCED_PROP(float, _Number0)
                UNITY_DEFINE_INSTANCED_PROP(float, _Number1)
                UNITY_DEFINE_INSTANCED_PROP(float, _Number2)
                UNITY_DEFINE_INSTANCED_PROP(float, _Number3)
                UNITY_DEFINE_INSTANCED_PROP(float, _Number4)
                UNITY_DEFINE_INSTANCED_PROP(float, _Number5)
                UNITY_DEFINE_INSTANCED_PROP(float, _ValidNumberCount)
                UNITY_DEFINE_INSTANCED_PROP(float, _Scale)
                UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
            UNITY_INSTANCING_BUFFER_END(SGPerInstanceData)
            // Unity instancing usage macros
            #define UNITY_ACCESS_HYBRID_INSTANCED_PROP(var, type) UNITY_ACCESS_INSTANCED_PROP(SGPerInstanceData, var)
            #else
            #define UNITY_ACCESS_HYBRID_INSTANCED_PROP(var, type) var
            #endif

            TEXTURE2D(_Atlas);
            SAMPLER(sampler_Atlas);
			
            Varyings UnlitPassVertex(Attributes input)
            {
                Varyings output;
    
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
    
                float scale = UNITY_ACCESS_HYBRID_INSTANCED_PROP(_Scale, float);
                float numberCount = UNITY_ACCESS_HYBRID_INSTANCED_PROP(_ValidNumberCount, float);
                float animTime01 = UNITY_ACCESS_HYBRID_INSTANCED_PROP(_AnimTime01, float);
                float3 animDirection = UNITY_ACCESS_HYBRID_INSTANCED_PROP(_AnimDirection, float3);

                float4 camPos = float4(TransformWorldToView(TransformObjectToWorld(float3(0, 0, 0))), 1.0);
    
                float4 viewDir = float4(input.positionOS.x * numberCount, input.positionOS.y, 0.0, 0.0);
                viewDir.xyz += animDirection.xyz * sqrt(animTime01) * 2; // anim direction;
                float4 outPos = mul(UNITY_MATRIX_P, camPos + viewDir * scale * _BaseScale);

                output.uv = TRANSFORM_TEX(input.uv, _Atlas);
                output.positionCS = outPos;
                return output;
            }

            half4 UnlitPassFragment(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                float3 animDirection = UNITY_ACCESS_HYBRID_INSTANCED_PROP(_AnimDirection, float3);

                float numberCount = UNITY_ACCESS_HYBRID_INSTANCED_PROP(_ValidNumberCount, float);

                float myNumberIndex = floor(input.uv.x * numberCount);
 
				float myNumber = 0;
				if (myNumberIndex == 0) {
					myNumber = UNITY_ACCESS_HYBRID_INSTANCED_PROP(_Number0, float);
				} else if (myNumberIndex == 1) {
					myNumber = UNITY_ACCESS_HYBRID_INSTANCED_PROP(_Number1, float);
				} else if (myNumberIndex == 2) {
					myNumber = UNITY_ACCESS_HYBRID_INSTANCED_PROP(_Number2, float);
				} else if (myNumberIndex == 3) {
					myNumber = UNITY_ACCESS_HYBRID_INSTANCED_PROP(_Number3, float);
				} else if (myNumberIndex == 4) {
					myNumber = UNITY_ACCESS_HYBRID_INSTANCED_PROP(_Number4, float);
				} else if (myNumberIndex == 5) {
					myNumber = UNITY_ACCESS_HYBRID_INSTANCED_PROP(_Number5, float);
				}
 
                float texturePitPoint = myNumber / 10;
                float uvProgress = input.uv.x - myNumberIndex / numberCount;
                float textureProgress = uvProgress * numberCount / 10;
	
                float fX = texturePitPoint + textureProgress;
    
                float2 uv = float2(fX, input.uv.y * _UVY.x - _UVY.y);
   
                float4 color = UNITY_ACCESS_HYBRID_INSTANCED_PROP(_Color, float4);
                half4 baseMap = half4(SAMPLE_TEXTURE2D(_Atlas, sampler_Atlas, uv));
                baseMap.rgb = lerp(baseMap.rgb, baseMap.rgb * color.rgb, 1);

                float animTime01 = UNITY_ACCESS_HYBRID_INSTANCED_PROP(_AnimTime01, float);
                baseMap.a = lerp(baseMap.a * color.a, 0, pow(animTime01, 2));

                return baseMap;
            	//
            }

            ENDHLSL
        }
    }
}