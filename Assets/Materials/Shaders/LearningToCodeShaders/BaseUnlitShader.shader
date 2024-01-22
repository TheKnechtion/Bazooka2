Shader "Unlit/BaseUnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "black" {}
        _textureScale ("Texture Scale", float) = 1
    }
    SubShader{
        
        Tags { "RenderType"="Opaque" }

        Pass{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normals : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
                float3 worldCoords : TEXCOORD2;

            };

            sampler2D _MainTex;
            float _textureScale;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normals);
                o.uv = v.uv;
                
                o.worldCoords = mul(UNITY_MATRIX_M, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, _textureScale*i.worldCoords.xz);   
                return col;
            }
            ENDCG
        }
    }
}
