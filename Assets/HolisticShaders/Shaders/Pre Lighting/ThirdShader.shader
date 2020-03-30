Shader "Learning/ThirdShader" {
    Properties
    {
    _rimColor("Rim Color", Color) = (0, 0.5, 0.5, 0)
    _rimStrength("Rim Strength", Range(0.5, 8.0)) = 3.0
    }
    
    SubShader
    {
        CGPROGRAM
        
        #pragma surface surf Lambert
        
        struct Input {
        float3 viewDir;
        };
        
        float4 _rimColor;
        float _rimStrength;
        
        void surf(Input IN, inout SurfaceOutput o)
        {
            half rim = 1 - saturate(dot(IN.viewDir, o.Normal));
            o.Albedo = _rimColor.rgb * pow(rim, _rimStrength);
        }
        
        ENDCG
    }
    FallBack "Diffuse"
}