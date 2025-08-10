Shader "Custom/Fade" 
{
	Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Color ("Main Color", Color) = (0,0,0,1)
	}
	
	SubShader {
		Tags {"Queue"="Geometry"}
		
   	Pass {
			Blend SrcAlpha OneMinusSrcAlpha
		 	Cull Off
        	ZTest Always				
		 	SetTexture [_MainTex] 
		 	{
               constantColor [_Color]
		 		Combine Constant
 		 	}
    	}
	}
} 