Shader "Learning/FourthShader" {
    Properties
    {
        _Color("Color", Color) = (0, 0.5, 0.5, 0)
        _SpecColor("Specular Color", Color) = (0, 0.5, 0.5, 0)
        _Spec("Specular", Range(0, 1)) = 0.5
        _Gloss("Gloss", Range(0, 1)) = 0.5
        
        //_rimStrength("Rim Strength", Range(0.5, 8.0)) = 3.0
    }
    
    SubShader
    {
        Tags {
            "Queue" = "Geometry"
        }
        CGPROGRAM
        #pragma surface surf BlinnPhong
        
        struct Input {
            float2 uv_MainTex;
        };
        
        float4 _Color;
        half _Spec;
        fixed _Gloss;
        
        void surf(Input IN, inout SurfaceOutput o)
        {
            o.Albedo = _Color.rgb;
            o.Specular = _Spec;
            o.Gloss = _Gloss;
        }
        
        ENDCG
    }
    FallBack "Diffuse"
}