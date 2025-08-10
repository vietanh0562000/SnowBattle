Shader "Custom/Water" 
{
	Properties 
	{
		_MainColor ("Main Color", Color) = (1,1,1,1)
		
    	_CenterOffset ("Center Offset", Vector) = (0,0,0,0)
    	_PlaneSize ("Plane Size", Vector) = (0,0,0,0)
        
    	_WavesHeight ("Waves Height", float) = 0.13
    	_WavesSpeed ("Waves Speed", Range(0.0,100.0)) = 50.0
    	
    	_FoamTex   ("Foam", 2D) = "white" {} 
        _FoamSpeed ("Foam Speed", Range(0.0,1.0)) = .025
   		_FoamMask  ("Foam Mask ", 2D) = "white" {}
        		
   		_SpecularCubeMap ("Specular CubeMap", CUBE) = "" {}
	}

	SubShader 
	{
		Tags { "IgnoreProjector" = "True" "RenderType" = "Transparent" "Queue" = "Transparent+1"}
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha
		
		Pass 
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"

			// Main color &H horizon
			half4 _MainColor;
			
			half3 _CenterOffset;
			half3 _PlaneSize;
			
			// Waves
			half _WavesHeight;
			half _WavesSpeed;
			
			// Foam
			sampler2D _FoamTex;
			sampler2D _FoamMask;
			half4 _FoamTex_ST;
			half4 _FoamMask_ST;
            half _FoamSpeed;
           		
			// Especular
			samplerCUBE _SpecularCubeMap;
	
			struct appdata 
			{
				float4 vertex   : POSITION;
				float4 texcoord : TEXCOORD0;  
			};

			struct v2f 
			{
				float4 pos		 : POSITION;
		    	float2 foamUV1     : TEXCOORD0;
                float2 foamUV2     : TEXCOORD1;
		    	float2 foamMaskUV : TEXCOORD2;
				float4 waveColors : COLOR; // r = waveHeight, alpha == hotizon fade
			};

			v2f vert (appdata v) 
			{	
				v2f o;

				//UNITY_INITIALIZE_OUTPUT(Input, o);
				
				//////////////////////////
				// VERTICES POS (Affected by waves * center offset)
				o.waveColors.a = 1;
		
				// Waves		   
				half waveTime = _Time * _WavesSpeed;	   
				half waveHeight = sin((v.vertex.x * .5) + waveTime) * cos ((v.vertex.z * 0.5f) + waveTime);
				
				// Displace the vertex accorning to wave height
				v.vertex.y+=  waveHeight * _WavesHeight;
				
				// Transform the vertex using the modelview matrix
				o.pos = UnityObjectToClipPos ( v.vertex);
				
				//////////////////////////
				// TEXTURE COORDS
				
				// Foam
				half2 foamUVOffset1 = half2 ( _CenterOffset.x / _PlaneSize.x, _CenterOffset.z / _PlaneSize.z) + (_Time * _FoamSpeed);
                half2 foamUVOffset2 = half2 ( _CenterOffset.x / _PlaneSize.x, _CenterOffset.z / _PlaneSize.z) + (_Time * -_FoamSpeed);
				half2 foamMaskUVOffset = half2 (_CenterOffset.x / _PlaneSize.x, _CenterOffset.z / _PlaneSize.z) - (waveTime * .025);;
			
				o.foamUV1 = TRANSFORM_TEX(v.texcoord, _FoamTex) + TRANSFORM_TEX(foamUVOffset1, _FoamTex);
                o.foamUV2 = TRANSFORM_TEX(v.texcoord, _FoamTex) + TRANSFORM_TEX(foamUVOffset2, _FoamTex);

				o.foamMaskUV = TRANSFORM_TEX(v.texcoord, _FoamMask) + TRANSFORM_TEX(foamMaskUVOffset, _FoamMask) + (waveTime * .00125);
              
				return o;
			}

			half4 frag (v2f i) : COLOR
			{	
				fixed4 ret;
				
				fixed4 foamMask = 1;//tex2D (_FoamMask, i.foamMaskUV);
				fixed4 foam1 = tex2D (_FoamTex, i.foamUV1) * foamMask.b;
                fixed4 foam2 = tex2D (_FoamTex, i.foamUV2) * foamMask.b;
	            
				ret = _MainColor + ((foam1 + foam2) * 0.25f);
				ret.a =  _MainColor.a;
				return ret;
		}
		ENDCG
    }
}

	
Fallback "VertexLit"
}
