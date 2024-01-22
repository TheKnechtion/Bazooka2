Shader "Unlit/SnowShader"{

    //user defined iput data
    //Each Property requires a variable (defined in Pass) associated with it
    Properties{

        //float4 is often used for vertices.
        //often, most float values are float4 values (floats defined in clusters of 4)
        _Value ("Value", float) = 1.0

        _RepeatPattern ("Repeat Pattern", float) = 1.0

        _ColorStart ("Color Start", Range(0,1)) = 0.0

        _ColorEnd ("Color End", Range(0,1)) = 1.0

        _yOffset ("Y-Offset", float) = 0
        _waviness ("Waviness", Range(-0.02,0.02)) = 0

        _Amplitude ("Amplitude", float) = 1

        //_Color ("Color",Color) = (1,1,1,1)

        _ColorA ("Color A",Color) = (1,0,0,1)
        _ColorB ("Color B",Color) = (0,1,0,1)

        _MainTex ("Texture", 2D) = "white" {}
        _FogTex ("Fog Texture", 2D) = "white" {}
        _HeightMap ("Height Map", 2D) = "white" {}

        _textureScale ("Texture Scale", float) = 1
    }

    //contains a pass that contains our shader code
    SubShader{
        //SubShader tags
        Tags { "RenderType"="Opaque" }
        
        
        //level of detail
        //You can have multiple SubShader in a single shader, and the game will
        //pick the appropriate SubShader based on LOD
        //LOD 100




        //Set explicit render information for each render pass
        Pass{

            //pass tags
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag


            #include "UnityCG.cginc"
            
            #define TAU 6.28318530718

            //automatically gets the _Value from Properties
            float _Value;

            float _RepeatPattern;

            float _ColorStart;

            float _Amplitude;

            float _textureScale;

            float _ColorEnd;

            float _yOffset;
            float _waviness;

            //float4 _Color;
            float4 _ColorA;
            float4 _ColorB;


            //this is required to create a texture in a shader
            sampler2D _MainTex;

            //this is optional, but it allows for offset and tiling
            float4 _MainTex_ST;
            
            sampler2D _HeightMap;

            sampler2D _FogTex;



            //per-vertex mesh data
            //this is automatically filled out by unity
            struct MeshData{

                //vertex position
                float4 vertex : POSITION;

                //Tangent to each vertex, w value is signed tangent direction
                //float4 tangent : TANGENT;

                //the local normal direction of each vertex
                float3 normals : NORMAL;

                //the color of each vertex, rgba
                //float4 color : COLOR;

                //uv coordinates
                //can use multiple uv for multiple different uses
                //i.e. uv0 could be the normal map texture and uv1 could be the lightmap texture
                float2 uv0 : TEXCOORD0;
                //float2 uv1 : TEXCOORD1;

                //uv maps can be input as a float4 as well, which is essentially used to store data.
                //Especially useful for procedural generation
                //float4 uv2 : TEXCOORD2;

                //the : lets us tell the computer what value we want passed in
            };



            //this is a user created inverseLerp function
            //it has a start value, end value, and input value
            float InverseLerp(float a, float b, float v)
            {
                return ((v-a)/(b-a));
            }


            float2 InverseLerp2(float a, float b, float2 v)
            {
                return float2 ((v.x-a)/(b-a),(v.y-a)/(b-a));
            }



            //data passed from vertex shader to fragment shader
            struct Interpolators{
                
                //store data to be passed
                float2 uv : TEXCOORD1;


                //TEXCOORD is a general index used to store data for shaders
                //It's basically just a channel you want data passed into
                float3 normal : TEXCOORD0;
                //float3 normal2 : TEXCOORD1;


                //ALWAYS REQUIRED
                //clip space positon
                float4 vertex : SV_POSITION;

                float3 worldCoords : TEXCOORD2;
            };


            //this is the vertex shader
            //it interpolates data between the defined vertices
            Interpolators vert (MeshData v){

                //o is used because it stands for "output"
                //this is data we want passed to the vertex shader
                Interpolators o;


                
                o.worldCoords = mul(UNITY_MATRIX_M, v.vertex);

                //float yOffset = i.uv.x * _yOffset;
                float yOffset = cos(v.uv0.x * TAU * _yOffset) * _waviness;

                //TAU ensures that the pattern will seamlessly repeat
                float t = cos((_RepeatPattern*(v.uv0.y + yOffset) + _Value) * TAU) * 0.5 + 0.5;
                float b = cos((_RepeatPattern*(v.uv0.x + yOffset) + _Value) * TAU) * 0.5 + 0.5;

                float heightMap = tex2Dlod(_HeightMap, float4(v.uv0.xy, 0,0));

                v.vertex.y = heightMap * _Amplitude;

                //v.vertex.y = yOffset * _Amplitude;

                //v.vertex.y = t*b*_Amplitude;



                //this multiples the vertex by the Model-View Projection (MVP) Matrix
                //this converts from local space to clip space
                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.vertex = UnityObjectToClipPos(v.vertex * _Value);
                


                //pass the normals from the mesh data to the fragment shader
                //o.normal = v.normals;
                o.normal = UnityObjectToWorldNormal(v.normals);

                //o.uv = TRANSFORM_TEX(v.uv0, _MainTex);
                o.uv = v.uv0;


                //this moves the uv over time
                //o.uv.x += _Time.y * 0.01;
 
                return o;
            }



            fixed4 frag (Interpolators i) : SV_Target{
                
                float2 topDownProjection = i.worldCoords.xz;

                fixed4 fogCol = tex2D(_FogTex, i.uv);
                
                fixed4 col = tex2D(_MainTex, _textureScale*i.worldCoords.xz);
                

                //float yOffset = i.uv.x * _yOffset;
                float yOffset = cos(i.uv.x * TAU * _yOffset) * _waviness;

                //TAU ensures that the pattern will seamlessly repeat
                float t = cos((_RepeatPattern*(i.uv.y + yOffset) + _Value) * TAU) * 0.5 + 0.5;
                

                

                //return heightMap;
                
                return (1-i.uv.yyyy) * col * fogCol;
                
                //float4 outColor = lerp(_ColorA,_ColorB, t);
                //return outColor;

                //float2 ut = cos((_RepeatPattern*i.uv.xy + _Value) * TAU) * 0.5 + 0.5;
                
                //return t;
                //return float4(ut.xy,0,1);
                

                //return float4 (i.normal, 0);

            }

            ENDCG
        }
    }
}
