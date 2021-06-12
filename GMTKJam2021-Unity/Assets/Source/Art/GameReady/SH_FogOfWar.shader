Shader "GMTK2021/FogOfWar"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _FogMaskTexture ("Fog Mask Texture", 2D) = "white" {}
        _WorldRenderTexture ("World Render Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha:fade

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _FogMaskTexture;
        sampler2D _WorldRenderTexture;

        struct Input
        {
            float2 uv_FogMaskTexture;
            float2 uv_WorldRenderTexture;
        };

        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_FogMaskTexture, IN.uv_FogMaskTexture);
            fixed4 c2 = tex2D (_WorldRenderTexture, IN.uv_WorldRenderTexture) * _Color;
            o.Albedo = c.rgb * c2.rgb;
            o.Alpha = c.r;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
