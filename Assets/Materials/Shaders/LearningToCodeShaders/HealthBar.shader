Shader "Unlit/HealthBar"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _uvVector ("UV Vector", Vector) = (0,0,0,1)
        _currentHealth ("Current Health", float) = .8
        
        _startColor ("Start Color", float) = 0
        _endColor ("End Color", float) = 1

        _ColorA ("Color A",Color) = (1,0,0,1)
        _ColorB ("Color B",Color) = (0,1,0,1)
    }
    SubShader
    {

        Tags {             
            //"RenderType"="Transparent" 
            //"Queue"="Transparent"  
        
            "RenderType"="Opaque" 
        }

        Pass
        {
            Cull off

            /*
            ZWrite Off
            Blend One One
            */

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #define TAU 6.28318530718


            float4 _uvVector;
            float4 _ColorA;
            float4 _ColorB;

            float _currentHealth;
            float _startColor;
            float _endColor;

            sampler2D _MainTex;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };


            float InverseLerp(float a, float b, float v)
            {
                return ((v-a)/(b-a));
            }
            


            struct v2f
            {
                float2 uv : TEXCOORD0;
         
                float4 vertex : SV_POSITION;
            };



            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                
                return o;
            }
     

            
            float4 createRect(float2 uvRef)
            {
                if(uvRef.x > _currentHealth)
                {
                    return float4(0,0,0,1);
                }
                else
                {
                    return float4(1,1,1,1);
                }

            }




            

            fixed4 frag (v2f i) : SV_Target
            {
                
                //alternative way to split the hp bar
                //float currentHP = InverseLerp(_currentHealth,_currentHealth,i.uv.x);
                //float t = saturate(InverseLerp(0,1,1-currentHP));                



                float4 currentHP = createRect(i.uv);

                float colorGradient = InverseLerp(_startColor,_endColor,_currentHealth);
                //lerp(a,b,t) = a + (b-a)t
                float4 outColor = lerp(_ColorA,_ColorB, colorGradient);

                fixed4 col = tex2D(_MainTex, float2(_currentHealth,i.uv.y));
                
                //float4 hpBarShape = createRect(i.uv);

                //clip(currentHP-.001);

                //return currentHP * outColor;

                //return currentHP * col;

                return currentHP * col;

                //_uvVector = float4(i.uv.xxx,1);

                // sample the texture


                //return _uvVector;
            }
            ENDCG
        }
    }
}
