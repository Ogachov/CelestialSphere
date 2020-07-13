// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/CelestialSphere"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Size ("Size", Float) = 1.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        ZWrite Off
        Cull Off
        LOD 100
        Blend One One

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Size;

            float3 _Center;
            float3 _CamUp;

            v2f vert (appdata v)
            {
                v2f o;
    
                float3 temp = v.vertex.xyz + _Center;
                float3 eyeVector = ObjSpaceViewDir(float4(temp,0));
                float3 sideVector = normalize(cross(eyeVector, _CamUp)) * _Size * v.color.a;
                float3 pos = temp + (v.uv.x - 0.5) * sideVector + (v.uv.y - 0.5) * _CamUp * _Size * v.color.a;
                o.vertex = UnityObjectToClipPos(float4(pos,1));

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                col = fixed4(col.xyz * i.color.xyz, col.a);

                return col;
            }
            ENDCG
        }
    }
}
