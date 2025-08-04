Shader "Unlit/Triplanar"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
        _Scale ("Texture Scale", Float) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        float4 _Color;
        float _Scale;

        struct Input
        {
            float3 worldPos;
            float3 worldNormal;
        };

        // Triplanar blend based on world normal and position
        float4 TriplanarTexture(float3 worldPos, float3 normal)
        {
            float3 blend = abs(normal);
            blend = pow(blend, 4.0); // Sharpen blend
            blend /= (blend.x + blend.y + blend.z); // Normalize

            float2 xUV = worldPos.yz * _Scale;
            float2 yUV = worldPos.xz * _Scale;
            float2 zUV = worldPos.xy * _Scale;

            float4 xTex = tex2D(_MainTex, xUV);
            float4 yTex = tex2D(_MainTex, yUV);
            float4 zTex = tex2D(_MainTex, zUV);

            return xTex * blend.x + yTex * blend.y + zTex * blend.z;
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            float4 tex = TriplanarTexture(IN.worldPos, IN.worldNormal);
            tex *= _Color;

            o.Albedo = tex.rgb;
            o.Alpha = tex.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
