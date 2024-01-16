Shader "Unlit/CandyCane"{

    //user defined iput data
    //Each Property requires a variable (defined in Pass) associated with it
    Properties{

        //float4 is often used for vertices.
        //often, most float values are float4 values (floats defined in clusters of 4)
        _Value ("Value", float) = 1.0

        _RepeatPattern ("Repeat Pattern", float) = 1.0

        _ColorStart ("Color Start", Range(0,1)) = 0.0

        _ColorEnd ("Color End", Range(0,1)) = 1.0

        //_Color ("Color",Color) = (1,1,1,1)

        _ColorA ("Color A",Color) = (1,0,0,1)
        _ColorB ("Color B",Color) = (0,1,0,1)
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

            float _ColorEnd;

            //float4 _Color;
            float4 _ColorA;
            float4 _ColorB;

            

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

                //you can have a bunch of different interpolators
                /*
                float4 uv1 : TEXCOORD1;
                float4 uv2 : TEXCOORD2;
                float4 uv3 : TEXCOORD3;
                float4 uv4 : TEXCOORD4;
                */

                //TEXCOORD is a general index used to store data for shaders
                //It's basically just a channel you want data passed into
                float3 normal : TEXCOORD0;
                //float3 normal2 : TEXCOORD1;


                //ALWAYS REQUIRED
                //clip space positon
                float4 vertex : SV_POSITION;
            };


            //this is the vertex shader
            //it interpolates data between the defined vertices
            Interpolators vert (MeshData v){

                //o is used because it stands for "output"
                //this is data we want passed to the vertex shader
                Interpolators o;

                //this multiples the vertex by the Model-View Projection (MVP) Matrix
                //this converts from local space to clip space
                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.vertex = UnityObjectToClipPos(v.vertex * _Value);
                


                //pass the normals from the mesh data to the fragment shader
                //o.normal = v.normals;
                o.normal = UnityObjectToWorldNormal(v.normals);
                //the above shortcut can be written directly as these:
                //o.normal = mul(v.normals,(float3x3)unity_WorldToObject);
                //o.normal = mul((float3x3)unity_ObjectToWorld, v.normals);
                //o.normal = mul((float3x3)UNITY_MATRIX_M, v.normals); 

                //o.uv = (v.uv0 + _ColorEnd) * _ColorStart;

                o.uv = v.uv0;

                //o.uv = v.uv0;

                //this renders an object directly into clip-space.
                //It essentially freezes to the camera.
                //This is useful for post-processing shaders
                //o.vertex = v.vertex;
                


                
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }



            //bool - 0 or 1
            //int - can use int, but will often become float
            //float (32 bit float)
            //half (16 bit float)
            //fixed (lower precision float, only precise within -1 to 1)
            //float4 = vector4 -> half4 -> fixed4
            //float4x4 = Matrix4x4 -> half4x4
            
            fixed4 frag (Interpolators i) : SV_Target{
                /*
                //Swizzling - this allows us to cast portions of a dataset into another
                float 4 myValue;

                float2 otherValue = myValue.xy;
                float2 otherValue2 = myValue.yx;
                float3 otherValue3 = myValue.xzy;
                float4 otherValue4 = myValue.xxxx; //this specific example allows for easy gray-scaling
                */

                //red color
                //return float4(0,1,0,1);

                //return _Color;

                //auto populates the float with the normal float 3 and 1 
                //return float4 (i.normal,1);
                //return float4 (i.uv,0,1);


                //return float4 (i.normal.yyy,0);
                //return float4 (UnityObjectToWorldNormal(i.normal),1);

                //this is just be messing around to learn
                /*
                if(i.normal.x < 0 || i.normal.y < 0 || i.normal.z < 0)
                {
                    return float4 (-i.normal,1);
                }
                else
                {
                    return float4 (i.normal,1);
                }
                */




                //lerp - allows you to interpolate between values of 0 to 1
                //this example blends between two colors based on the x uv coordinate
                //float4 outColor = lerp(_ColorA,_ColorB, i.uv.y);
                //float4 outColor = lerp(_ColorA,_ColorB, (-i.normal.zzzz));
                
                //float t = InverseLerp(_ColorStart,_ColorEnd,i.normal.yyyy);

                //saturate is how you clamp a value for shaders
                //If a value is less than 0, make it 0
                //If a value is greater than 1, make it 1
                //It's essentially the clamp(0,1) function
                //float t = saturate(InverseLerp(_ColorStart,_ColorEnd,i.normal.yyyy));
                //float t = saturate(InverseLerp(_ColorStart,_ColorEnd,i.uv.y));
                //float t = smoothstep(_ColorStart,_ColorEnd,i.uv.y);

                //frac = v - floor(v) - this means that values will repeat on the 0 to 1 interval
                //allows us to check for repeating patterns and values outside of 0-1
                //t = frac( t );


                /*
                float t = saturate(InverseLerp(_ColorStart,_ColorEnd,i.uv.y));
                float4 outColor = lerp(_ColorA,_ColorB, t);
                return outColor;
                */


                //shaders automatically cast from a single float to a float4
                //returning a single float automatically swizzles it into a float4
                //i.e. - "return t" is the same as "return t.xxxx"
                //float t = i.uv.x;
                //return t;


                //this is how you apply a wave to a function, allowing a repeating pattern
                //float t = abs(frac(i.uv.y * 5)*2 - 1);

                float yOffset = i.uv.x;

                //TAU ensures that the pattern will seamlessly repeat
                float t = cos((_RepeatPattern*(i.uv.y + yOffset) + (_Time.y * _Value)) * TAU) * 0.5 + 0.5;
                


                //return t;
                
                float4 outColor = lerp(_ColorA,_ColorB, t);
                return outColor;

                //float2 ut = cos((_RepeatPattern*i.uv.xy + _Value) * TAU) * 0.5 + 0.5;
                
                //return t;
                //return float4(ut.xy,0,1);
                

                //return float4 (i.normal, 0);

            }

            ENDCG
        }
    }
}
