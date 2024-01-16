Shader "Unlit/Textured"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "bump" {}
        _SecondTex ("Second Texture", 2D) = "black" {}
        _Pattern ("Pattern Texture", 2D) = "white" {}
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
            sampler2D _Pattern;

            sampler2D _SecondTex;
            
            
            
            //this is optional, but it allows for offset and tiling
            float4 _MainTex_ST;

            float _textureScale;

            float _MipSampleLevel;
            
            float GetWaveUV(float coord)
            {                                
                float wave = cos((coord + _Time.y * 0.1) * TAU * 5) * 0.5 + 0.5;

                wave *= (1-coord);

                return wave;
            }
            

            v2f vert (appdata v)
            {
                v2f o;

                o.worldCoords = mul(UNITY_MATRIX_M, v.vertex); //object to world
                //o.worldCoords = mul(unity_ObjectToWorld,v.vertex); //also object to world


                o.vertex = UnityObjectToClipPos(v.vertex);

                //_MainTex_ST is used via this function
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv = v.uv;

                //this allows for a scrolling Texture
                //o.uv.x += cos(_Time.y * 0.1);
                //o.uv.y += sin(_Time.y * 0.1);

                return o;
            }


            fixed4 frag (v2f i) : SV_Target
            {
                //float2 uvCenter = float2(i.uv.x-0.5,i.uv.y-0.5);

                //float radius = length(uvCenter);


                //return float4(i.worldCoords.xyz,1);

                //creates a vector2 from the xz position of obj in world space
                //this can be used as coordinates for our projections
                float2 topDownProjection = i.worldCoords.xz;
                
                //return float4(topDownProjection,0,1);


                // sample the texture
                //the first part is what texture we want to use
                //the second part is what portion of the texture do we want to sample
                fixed4 mainTex = tex2D(_MainTex, _textureScale*topDownProjection);
                
                //tex2Dlod allows you to manually set the Level of Detail aka the MIP map
                //tex2D CAN'T BE USED IN VERTEX SHADER FRAGMENTS. ONLY TEX2DLOD MAY BE USED THERE.
                fixed4 secondTex = tex2Dlod(_SecondTex, float4(_textureScale*topDownProjection, _MipSampleLevel.xx));
                


                float patternTex = tex2D(_Pattern,i.uv);

                //float4 finalColor = lerp(float4(1,0,0,1),secondTex,patternTex);

                float4 finalColor = lerp(mainTex,secondTex,patternTex);
                
                //fixed4 patternTex = tex2D(_Pattern,_textureScale*topDownProjection);

                //return mainTex*GetWaveUV(patternTex);

                return finalColor;
            }
            ENDCG
        }
    }
}
