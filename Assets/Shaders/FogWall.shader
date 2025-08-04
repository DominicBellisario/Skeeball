Shader "Unlit/FogWall"
{
    Properties
    {
        _Color ("Fog Color", Color) = (0.8, 0.8, 0.8, 1)
        _FadeStart ("Fade Start (Local Z)", Float) = 0
        _FadeEnd ("Fade End (Local Z)", Float) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;
            float _FadeStart;
            float _FadeEnd;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Use world Z to compute fog fade
                float localZ = mul(unity_WorldToObject, float4(i.worldPos,1)).z;
                float alpha = saturate( (localZ - _FadeStart) / (_FadeEnd - _FadeStart) );
                return fixed4(_Color.rgb, alpha * _Color.a);
            }
            ENDCG
        }
    }
}
