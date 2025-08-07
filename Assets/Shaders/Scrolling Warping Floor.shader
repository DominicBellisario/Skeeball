Shader "Custom/Scrolling Warping Floor"
{
    Properties
    {
        _MainTex("Water Texture 1", 2D) = "white" {}
        _MainTex2("Water Texture 2", 2D) = "white" {}
        _BumpMap("Normal Map", 2D) = "bump" {}
        _EmissionMap("Emission Map", 2D) = "white" {}
        _EmissionColor("Emission Color", Color) = (1, 1, 1, 1)
        _EmissionPulseSpeed("Emission Pulse Speed", float) = 2.0
        _Color("Tint", Color) = (0.2, 0.5, 1, 1)
        _ScrollSpeedX("Scroll Speed X", float) = 0.1
        _ScrollSpeedY("Scroll Speed Y", float) = 0.1
        _WarpStrength("Warp Strength", float) = 0.05
        _WarpFrequency("Warp Frequency", float) = 2.0
        _BumpStrength("Normal Strength", Range(0, 2)) = 1.0
        _BlendSpeed("Blend Speed", float) = 1.0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #include "UnityCG.cginc"

        sampler2D _MainTex;
        sampler2D _MainTex2;
        sampler2D _BumpMap;
        sampler2D _EmissionMap;

        float4 _Color;
        float _ScrollSpeedX;
        float _ScrollSpeedY;
        float _WarpStrength;
        float _WarpFrequency;
        float _BumpStrength;
        float _BlendSpeed;
        float4 _EmissionColor;
        float _EmissionPulseSpeed;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // Animate and warp UVs
            float2 uv = IN.uv_MainTex;
            uv.x += _Time.y * _ScrollSpeedX;
            uv.y += _Time.y * _ScrollSpeedY;
            uv.x += sin((uv.y + _Time.y) * _WarpFrequency) * _WarpStrength;

            // Blend two textures over time
            fixed4 tex1 = tex2D(_MainTex, uv);
            fixed4 tex2 = tex2D(_MainTex2, uv);
            float blendFactor = 0.5 + 0.5 * sin(_Time.y * _BlendSpeed);
            fixed4 blendedTex = lerp(tex1, tex2, blendFactor) * _Color;

            // Normal mapping
            float3 bump = UnpackNormal(tex2D(_BumpMap, uv));
            bump = normalize(lerp(float3(0, 0, 1), bump, _BumpStrength));

            // Emission map (pulsed and warped)
            float pulse = 0.5 + 0.5 * sin(_Time.y * _EmissionPulseSpeed);
            fixed4 emissionMap = tex2D(_EmissionMap, uv);
            fixed3 emission = emissionMap.rgb * _EmissionColor.rgb * pulse;

            // Output surface
            o.Albedo = blendedTex.rgb;
            o.Normal = bump;
            o.Metallic = 0.0;
            o.Smoothness = 0.5;
            o.Alpha = 1.0;
            o.Emission = emission;
        }
        ENDCG
    }

    FallBack "Diffuse"
}
