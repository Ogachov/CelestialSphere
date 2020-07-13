Shader "Unlit/Lines"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="Transparent"  "RenderType"="Transparent" }
        LOD 100
        Cull Off
        ZWrite Off
        Blend One One

        Pass
        {
            CGPROGRAM
            #pragma geometry geom
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float3 vertex : POSITION;
                half2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };
            
            struct v2g
            {
                float4 vertex : SV_POSITION;
                half2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct g2f
            {
                float4 vertex : SV_POSITION;
                half2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            float _Width;
            float3 _Center;
            float _Margin;

            v2g vert (appdata v)
            {
                v2g o;
//                o.vertex = mul(unity_ObjectToWorld, float4(v.vertex, 1));
                o.vertex = float4(v.vertex + _Center, 1);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                
                return o;
            }

            [maxvertexcount(18)]
            void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream)
            {
                v2g v0 = IN[0];
                v2g v1 = IN[1];
                float3 eye = normalize(v0.vertex - _WorldSpaceCameraPos);
                float3 next = normalize(v1.vertex - v0.vertex);
                float3 side = normalize(cross(eye, next));
                float width = sqrt(dot(next, next));
            
                g2f o;
                o.color = v0.color;
                o.color = fixed4(next * 0.5 + 0.5,1);
                
                float3 p0 = v0.vertex + next * _Margin;
                float3 p1 = v1.vertex - next * _Margin;
                
                g2f v[8];
                v[0].vertex = UnityObjectToClipPos(p0 + side * _Width - next * _Width);
                v[0].uv = half2(0, 0);
                v[0].color = v0.color;
                v[1].vertex = UnityObjectToClipPos(p0 - side * _Width - next * _Width);
                v[1].uv = half2(0, 1);
                v[1].color = v0.color;
                //  真ん中の棒の部分
                v[2].vertex = UnityObjectToClipPos(p0 + side * _Width);
                v[2].uv = half2(0.5, 0);
                v[2].color = v0.color;
                v[3].vertex = UnityObjectToClipPos(p0 - side * _Width);
                v[3].uv = half2(0.5, 1);
                v[3].color = v0.color;
                v[4].vertex = UnityObjectToClipPos(p1 + side * _Width);
                v[4].uv = half2(0.5, 0);
                v[4].color = v0.color;
                v[5].vertex = UnityObjectToClipPos(p1 - side * _Width);
                v[5].uv = half2(0.5, 1);
                v[5].color = v0.color;

                v[6].vertex = UnityObjectToClipPos(p1 + side * _Width + next * _Width);
                v[6].uv = half2(1, 0);
                v[6].color = v0.color;
                v[7].vertex = UnityObjectToClipPos(p1 - side * _Width + next * _Width);
                v[7].uv = half2(1, 1);
                v[7].color = v0.color;
                
                int i;
                for(i = 0; i < 6; i++)
                {                
                    triStream.Append(v[i]);
                    triStream.Append(v[i+1]);
                    triStream.Append(v[i+2]);
                }    
                triStream.RestartStrip();
                                
//                o.vertex = v[3];
//                o.uv = half2(0.5, 1);
//                triStream.Append(o);

/*
                for (int i = 0; i < 3; i++)
                {
                    o.vertex = UnityObjectToClipPos(IN[i].vertex);
                    o.uv = IN[i].uv;
                    triStream.Append(o);
                }
                triStream.RestartStrip();
*/                
            }

            fixed4 frag (g2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                
//                col = i.color;
//                col = fixed4(i.uv * 0.5 + 0.5, 0, 1);
                
                return col;
            }
            ENDCG
        }
    }
}
