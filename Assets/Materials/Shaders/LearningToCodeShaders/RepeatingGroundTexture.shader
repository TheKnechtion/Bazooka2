Shader "Unlit/RepeatingGroundTexture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "bump" {}
        _textureScale ("Texture Scale", float) = 1

        //With MIP maps, 0 is the highest level of detail
        _MipSampleLevel ("MIP", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            #define TAU 6.28318530718

            //mesh data
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;

            };

            //interpolators
            struct v2f
            {
                float2 uv : TEXCOORD0;
                
                float4 vertex : SV_POSITION;
                float3 worldCoords : TEXCOORD1;

            };


            //this is required to create a texture in a shader
            sampler2D _MainTex;

            float _textureScale;

            float _MipSampleLevel;

            

            v2f vert (appdata v)
            {
                v2f o;

                o.worldCoords = mul(UNITY_MATRIX_M, v.vertex); //object to world

                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = v.uv;

                return o;
            }


            fixed4 frag (v2f i) : SV_Target
            {
                float2 topDownProjection = i.worldCoords.xz;

                fixed4 mainTex = tex2Dlod(_MainTex, float4(_textureScale*topDownProjection, _MipSampleLevel.xx));
                
                return mainTex;
            }
            ENDCG
        }
    }
}
