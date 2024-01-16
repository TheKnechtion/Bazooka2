Shader "Unlit/VertexOffset"{
    
    //user defined iput data
    //Each Property requires a variable (defined in Pass) associated with it
    Properties{
        _ColorA ("Color A",Color) = (1,0,0,1)
        _ColorB ("Color B",Color) = (0,1,0,1)

        _ColorStart ("Color Start", Range(0,1)) = 0.0

        _ColorEnd ("Color End", Range(0,1)) = 1.0

        _timeScale ("Time Scale", float) = 1

        _waveAmp ("Wave Amplitude", Range(0,1)) = 1

        _RepeatPattern ("Repeat Pattern", float) = 1.0
    }

    //contains a pass that contains our shader code
    SubShader{
        //SubShader tags
        //This is how the shader type and queue must be set for Transparent shaders
        //Using transparent means that this shader will render after everything opaque has rendered
        Tags { 
            "RenderType"="Opaque" 
            "Queue"="Geometry" 
        
            //"RenderType"="Transparent" 
            //"Queue"="Transparent" 
        
        
        }
        


        //Set explicit render information for each render pass
        Pass{
            /*
            Blend One One 
            ZWrite Off
            Cull off
            */
            //pass tags
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag


            #include "UnityCG.cginc"
            
            #define TAU 6.28318530718

            float _ColorStart;
            float _ColorEnd;
            float4 _ColorA;
            float4 _ColorB;

            float _waveAmp;

            float _timeScale;

            float _RepeatPattern;

            struct MeshData{

                float4 vertex : POSITION;

                float3 normals : NORMAL;

                float2 uv0 : TEXCOORD0;
            };




            float InverseLerp(float a, float b, float v)
            {
                return ((v-a)/(b-a));
            }



            float GetWaveUV(float2 uv)
            {                                
                float2 uvCenter = float2(uv.x-0.5,uv.y-0.5);

                float radialDistance = length(uvCenter);

                float wave = cos((radialDistance + _Time.y * _timeScale) * TAU * _RepeatPattern) * 0.5 + 0.5;

                return (wave * (1-(2*radialDistance)));
            }

            float GetWaveVert(float2 uv)
            {                                
                float2 uvCenter = float2(uv.x-0.5,uv.y-0.5);

                float radialDistance = length(uvCenter);

                float wave = cos((radialDistance + _Time.y * _timeScale) * TAU * _RepeatPattern);

                return (wave * (1-(2*radialDistance)));
            }



            struct Interpolators{
                

                float2 uv : TEXCOORD1;

                float3 normal : TEXCOORD0;

                float4 vertex : SV_POSITION;
            };


            Interpolators vert (MeshData v){
                Interpolators o;

                v.vertex.y = GetWaveVert(v.uv0) * _waveAmp;

                o.vertex = UnityObjectToClipPos(v.vertex);

                o.normal = UnityObjectToWorldNormal(v.normals);

                o.uv = v.uv0;



                return o;
            }

            
            fixed4 frag (Interpolators i) : SV_Target{


                return GetWaveUV(i.uv);


            }

            ENDCG
        }
    }
}

