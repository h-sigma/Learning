Shader "Learning/Outline"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _OutlineColor ("Color", Color) = (1,1,1,1)
        _Outline ("Outline Width", Range(0.0, 0.2)) = 0.0
    }
    SubShader
    {
        Tags {"Queue" = "Transparent+1"}
        ZWrite Off
        CGPROGRAM
        #pragma surface surf Lambert vertex:vert
        
        struct Input {
            float2 uv_MainTex;
        };
        
        float _Outline;
        float4 _OutlineColor;
        
        void vert(inout appdata_full v)
        {
            v.vertex.xyz += v.normal * _Outline;
        }
        
        sampler2D _MainTex;
        
        void surf (Input IN, inout SurfaceOutput o)
        {
            o.Emission = _OutlineColor.rgb;
        }
        
        ENDCG
        
        ZWrite On
        CGPROGRAM
        #pragma surface surf Lambert
        
        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
