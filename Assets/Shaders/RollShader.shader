Shader "Unlit/RollShader"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _MainTex ("Texture", 2D) = "white" {}
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
            #pragma multi_compile_fog
            #pragma shader_feature GroundRollOnlyZ
            #include "RollInclude.cginc"

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed3 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = GetFixedRollClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normal = UnityObjectToWorldNormal(v.normal);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb *= _Color;
				fixed3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
				fixed3 normalDir = normalize(i.normal);
				fixed diffuse = max(0, dot(normalDir, lightDir));
				col.rgb *= diffuse * 0.5 + 0.5;
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
