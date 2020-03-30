Shader "Learning/BasicBlinn" {
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1)
    }
    
    //Alpha transparencies do not write to Z buffer
    
    SubShader
    {
        Tags {
            "Queue" = "Geometry"
        }
        
        CGPROGRAM
        #pragma surface surf BasicBlinn
        
        half4 LightingBasicBlinn(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten){
            half3 h = normalize(viewDir + lightDir);
            
            half diffuse = max(0, dot(s.Normal, lightDir));
            
            float nh = max(0, dot(s.Normal, h));
            float spec = pow(nh, 48.0);
            
            half4 c;
            c.rgb = (s.Albedo * diffuse * _LightColor0.rgb + _LightColor0.rbg * spec) * atten;
            c.a = s.Alpha;
            return c;
        }
        
        struct Input {
            float2 uv_SmoothnessTex;
        };
        
        float4 _Color;
        
        void surf(Input IN, inout SurfaceOutput o)
        {
            o.Albedo = _Color.rgb;
        }
        
        ENDCG
    }
    FallBack "Diffuse"
}