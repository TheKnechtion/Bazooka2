Shader "Unlit/AmmoIconPopup"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorA ("ColorA",Color) = (1,0,0,1)
        _ColorB ("ColorB",Color) = (1,0,0,0)
        _alphaControl ("currentAlpha",float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent"
                "IgnoreProjector"="True"
                "Queue"="Transparent"    
        }


        LOD 100

        Pass
        {
            Cull Off
            ZWrite Off
            ZTest ALWAYS
            Blend SrcAlpha OneMinusSrcAlpha
            //Cull front
            //Blend DstColor One

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _ColorA;
            float4 _ColorB;
            float4 _MainTex_ST;
            float _alphaControl;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                //v.uv.x = 1-v.texcoord.x;

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);


                return col * _alphaControl;
            }
            ENDCG
        }
    }
}
