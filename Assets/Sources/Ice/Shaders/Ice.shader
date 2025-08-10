Shader "Custom/Ice" 
{
	Properties
	{
		_InteriorColor("Interior Color", Color) = (1,1,1,1)
        _ExteriorColor("Exterior Color", Color) = (1,1,1,1)
		_ReflectTex("Reflection Texture", Cube) = "" {}
		_RefractTex("Refraction Texture", Cube) = "" {}
	}
	
	SubShader
	{
		Tags
		{
			"RenderType" = "Transparent" "Queue" = "Transparent" 
		}

        GrabPass
        {
            "_BackgroundTexture"
        }

		Pass
		{
			// First pass - here we render the backfaces of the ice.
			offset -1, -1
			cull front
            Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert  
			#pragma fragment frag 
			#include "UnityCG.cginc"

			fixed4 _InteriorColor;
			samplerCUBE _ReflectTex;
			samplerCUBE _RefractTex;
            sampler2D _BackgroundTexture;

			struct vertexInput 
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct vertexOutput 
			{
				float4 pos : SV_POSITION;
				float3 normalDir : TEXCOORD0;
				float3 viewDir : TEXCOORD1;
                float4 grabPos : TEXCOORD2;
			};

			vertexOutput vert(vertexInput input)
			{
				vertexOutput output;

				float4x4 modelMatrix = unity_ObjectToWorld;
				float4x4 modelMatrixInverse = unity_WorldToObject;

				output.viewDir = mul(modelMatrix, input.vertex).xyz - _WorldSpaceCameraPos;
				output.normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
				output.pos = UnityObjectToClipPos(input.vertex);

                // Use grabpass for refraction
                output.grabPos = ComputeGrabScreenPos(output.pos);
                output.grabPos.y = 1 - output.grabPos.y;
  
				return output;
			}

			fixed4 frag(vertexOutput input) : COLOR
			{
                float3 reflectedDir = reflect (input.viewDir, normalize(input.normalDir));
                fixed4 bgColor = tex2Dproj(_BackgroundTexture, input.grabPos);
                fixed4 color = texCUBE(_RefractTex, reflectedDir) * _InteriorColor;
				
				return (color *  bgColor) + 0.5;
			}

			ENDCG
		}

        
		Pass
		{
        
			// Second pass - here we render the front faces of the gems.
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert  
			#pragma fragment frag 
			#include "UnityCG.cginc"
            
            fixed4 _ExteriorColor;
			samplerCUBE _ReflectTex;
			samplerCUBE _RefractTex;

			struct vertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float3 normalDir : TEXCOORD0;
				float3 viewDir : TEXCOORD1;
			};

			vertexOutput vert(vertexInput input)
			{
				vertexOutput output;

				float4x4 modelMatrix = unity_ObjectToWorld;
				float4x4 modelMatrixInverse = unity_WorldToObject;

				output.viewDir = mul(modelMatrix, input.vertex).xyz - _WorldSpaceCameraPos;
				output.normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
				output.pos = UnityObjectToClipPos(input.vertex);
				return output;
			}

			fixed4 frag(vertexOutput input) : COLOR
			{
				float3 reflectedDir = reflect(input.viewDir, normalize(input.normalDir));
                fixed4 reflection = texCUBE(_RefractTex, reflectedDir) ;
                fixed4 ret = lerp (reflection, 1, 0.25) + (_ExteriorColor * 0.5) * 2;
                ret.a = _ExteriorColor.a;
				return ret;
			}

			ENDCG
		}
        
		Pass
		{
			// Third pass specular
			ZWrite Off
			Blend One One

			CGPROGRAM
			#pragma vertex vert  
			#pragma fragment frag 
			#include "UnityCG.cginc"

			fixed4 _Color;
			samplerCUBE _ReflectTex;
			samplerCUBE _RefractTex;

			struct vertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float3 normalDir : TEXCOORD0;
				float3 viewDir : TEXCOORD1;
			};

			vertexOutput vert(vertexInput input)
			{
				vertexOutput output;

				float4x4 modelMatrix = unity_ObjectToWorld;
				float4x4 modelMatrixInverse = unity_WorldToObject;

				output.viewDir = mul(modelMatrix, input.vertex).xyz - _WorldSpaceCameraPos;
				output.normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
				output.pos = UnityObjectToClipPos(input.vertex);
				return output;
			}

			fixed4 frag(vertexOutput input) : COLOR
			{
				float3 reflectedDir = reflect(input.viewDir, normalize(input.normalDir));
				return texCUBE(_ReflectTex, reflectedDir);
			}

			ENDCG
		}
	}
}
