Shader "Learning/FirstShader" {
    Properties
    {
        _myColor ("Albedo (RGB)", 2D) = "white"{}
        _myEmission ("Emission", COLOR) = (0, 0, 0, 1)
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
        float4 _myEmission;
        
        void surf(Input IN, inout SurfaceOutput o)
        {
            o.Albedo = (tex2D(_myColor, IN.uv_myColor)).rgb;
            o.Emission = _myEmission.rgb * _myEmission.a;
        }
        
        ENDCG
    }
    FallBack "Diffuse"
}