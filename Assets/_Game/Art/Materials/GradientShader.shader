Shader "Unlit/GradientShader"
{
    Properties
    {
        _TopColor ("Top Color", Color) = (0.5,0.8,0.8,1)
        _BottomColor ("Bottom Color", Color) = (0.3,0.7,0.7,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Background" }
        LOD 100

        Pass
        {
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _TopColor;
            fixed4 _BottomColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float t = i.uv.y;
                return lerp(_BottomColor, _TopColor, t);
            }
            ENDCG
        }
    }
}
