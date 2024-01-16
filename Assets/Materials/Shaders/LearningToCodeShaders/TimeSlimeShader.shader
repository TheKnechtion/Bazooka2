Shader "Unlit/TestShader"{

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
        _waviness ("Waviness", float) = 0

        //_Color ("Color",Color) = (1,1,1,1)

        _ColorA ("Color A",Color) = (1,0,0,1)
        _ColorB ("Color B",Color) = (0,1,0,1)
    }

    //contains a pass that contains our shader code
    SubShader{
        //SubShader tags
        Tags { "RenderType"="Opaque" }
        


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

            float _ColorEnd;

            float _yOffset;
            float _waviness;

            //float4 _Color;
            float4 _ColorA;
            float4 _ColorB;

            

            //per-vertex mesh data
            //this is automatically filled out by unity
            struct MeshData{

                //vertex position
                float4 vertex : POSITION;


                //the local normal direction of each vertex
                float3 normals : NORMAL;

                //uv coordinates
                //can use multiple uv for multiple different uses
                //i.e. uv0 could be the normal map texture and uv1 could be the lightmap texture
                float2 uv0 : TEXCOORD0;
            };



            //this is a user created inverseLerp function
            //it has a start value, end value, and input value
            float InverseLerp(float a, float b, float v)
            {
                return ((v-a)/(b-a));
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
            };


            Interpolators vert (MeshData v){
                Interpolators o;

                //this multiples the vertex by the Model-View Projection (MVP) Matrix
                //this converts from local space to clip space
                o.vertex = UnityObjectToClipPos(v.vertex);

                //pass the normals from the mesh data to the fragment shader
                //o.normal = v.normals;
                o.normal = UnityObjectToWorldNormal(v.normals);

                o.uv = v.uv0;

                return o;
            }

            
            fixed4 frag (Interpolators i) : SV_Target{

                
                //_Time.xyzw mean different things
                //y is seconds
                //w is something like seconds/20


                //float yOffset = i.uv.x * _yOffset;
                float yOffset = cos(i.uv.x * TAU * _yOffset) * _waviness;

                //TAU ensures that the pattern will seamlessly repeat
                float t = cos((_RepeatPattern*(i.uv.y + yOffset) + _Time.y) * TAU) * 0.5 + 0.5;
                
                

                float4 outColor = lerp(_ColorA,_ColorB, t);
                return outColor;      

            }

            ENDCG
        }
    }
}
