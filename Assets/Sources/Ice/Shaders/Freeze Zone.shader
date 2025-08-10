Shader "Custom/Freeze Zone"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
        _ReflectTex("Reflection Texture", Cube) = "" {}
        _ReflectionColor("Reflection Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
		LOD 100

		Pass
		{
            Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
                float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex    : SV_POSITION;
                float3 normalDir : TEXCOORD1;
                float3 viewDir   : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
            samplerCUBE _ReflectTex;
            float4 _ReflectionColor;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);

                float4x4 modelMatrix = unity_ObjectToWorld;
                float4x4 modelMatrixInverse = unity_WorldToObject;

                o.viewDir = mul(modelMatrix, v.vertex).xyz - _WorldSpaceCameraPos;
                o.normalDir = normalize(mul(float4(v.normal, 0.0), modelMatrixInverse).xyz);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
                fixed apha = col.a;

                // Sample reflection texture
                float3 reflectedDir = reflect (i.viewDir, normalize(i.normalDir));
                fixed4 ref = texCUBE(_ReflectTex, reflectedDir) * _ReflectionColor;
                ref = lerp (ref, _ReflectionColor, _ReflectionColor.a);
                col*= ref;
                col+= .25f;
                col.a = apha;

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col; 
			}
			ENDCG
		}
	}
}
