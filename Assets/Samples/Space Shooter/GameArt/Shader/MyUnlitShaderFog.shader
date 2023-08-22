Shader "Unlit/MyUnlitShaderFog"
{
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                UNITY_FOG_COORDS(1)
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

        fixed4 frag (v2f i) : SV_Target
        {
            fixed4 col =fixed4(1.0,1.0,1.0,1.0);
            UNITY_APPLY_FOG_COLOR(i.fogCoord, col, fixed4(0,0,0,0));
            return col;
        }
        ENDCG
        }
    }
}
