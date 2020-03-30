Shader "Learning/FirstShader" {
    Properties
    {
        _myColor ("Albedo (RGB)", 2D) = "white"{}
        _myEmission ("Emission", 2D) = "black"{}
    }
    
    SubShader
    {
        CGPROGRAM
        #pragma surface surf Lambert
        
        struct Input {
            float2 uv_myColor;
            float2 uv_myEmission;
            float3 worldRefl;
        };
        
        sampler2D _myColor;
        sampler2D _myEmission;
        
        void surf(Input IN, inout SurfaceOutput o)
        {
            o.Albedo = (tex2D(_myColor, IN.uv_myColor)).rgb;
            o.Emission = tex2D(_myEmission, IN.uv_myEmission).rgb;
        }
        
        ENDCG
    }
    FallBack "Diffuse"
}