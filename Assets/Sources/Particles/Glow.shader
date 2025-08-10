Shader "Custom/Glow"
{
    Properties 
    {
        _Color ("Color", Color) = (0.5,0.5,0.5,0.5)
        _MainTex ("Particle Texture", 2D) = "white" {}
        _ScaleX ("Scale X", Float) = 1.0
        _ScaleY ("Scale Y", Float) = 1.0
    }

    Category 
    {
        Tags { "Queue"="Transparent+1" "IgnoreProjector"="True" "RenderType"="Transparent" "CanUseSpriteAtlas"="True"  }
        Blend SrcAlpha One
        Cull Off 
        Lighting Off 
        ZWrite Off
        //ZTest Always

        SubShader 
        {
            Pass 
            {
            
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
               // #pragma target 2.0
                //#pragma multi_compile_particles
                //#pragma multi_compile_fog
                
                #include "UnityCG.cginc"
                #include "UnityUI.cginc"

                //#pragma multi_compile __ UNITY_UI_ALPHACLIP

                sampler2D _MainTex;
                fixed4 _Color;
                float _ScaleX;
                float _ScaleY;
                
                struct appdata_t 
                {
                    float4 vertex : POSITION;
                    fixed4 color : COLOR;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f 
                {
                    float4 vertex : SV_POSITION;
                    fixed4 color : COLOR;
                    float2 texcoord : TEXCOORD0;
                };
                
                float4 _MainTex_ST;

                v2f vert (appdata_t IN)
                {
                    v2f v;
                    v.vertex = UnityObjectToClipPos(IN.vertex);

                    v.vertex = mul(UNITY_MATRIX_P, mul(UNITY_MATRIX_MV, float4(0.0, 0.0, 0.0, 1.0))
                    + float4(IN.vertex.x, IN.vertex.y, 0.0, 0.0)
                    * float4(_ScaleX, _ScaleY, 1.0, 1.0));
 
                    v.color = IN.color;
                    v.texcoord = TRANSFORM_TEX(IN.texcoord,_MainTex);
                    return v;
                }
                
                fixed4 frag (v2f i) : SV_Target
                {
                    fixed4 texcol = tex2D (_MainTex, i.texcoord);
                    fixed4 col = texcol * i.color * _Color; 
                    col.a *= .25;
               
                    return col;
                }
                ENDCG 
            }
        }   
    }
}
