Shader "Learning/ToonRamp" {
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1)
        //_ColorTex("Albedo", 2D) = "white"{}
        _RampTex ("Ramp Texture", 2D) = "white" {}
    }
    
    //Alpha transparencies do not write to Z buffer
    
    SubShader
    {
        Tags {
            "Queue" = "Geometry"
        }
        
        CGPROGRAM
        #pragma surface surf ToonRamp
        
        float4 _Color;
        //sampler2D _ColorTex;
        sampler2D _RampTex;
        
        float4 LightingToonRamp(SurfaceOutput s, fixed3 lightDir, fixed atten){
            float diff = max(0, dot(s.Normal, lightDir));
            float h = diff * 0.5 + 0.5;
            float2 rh =  h;
            float3 ramp = tex2D(_RampTex, rh).rgb;
            
            float4 c;
            c.rgb = s.Albedo * _LightColor0 * ramp;
            c.a = s.Alpha;
            return c;
        }
        
        struct Input {
            float2 uv_ColorTex;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            o.Albedo = _Color.rgb;
        }
        
        ENDCG
    }
    FallBack "Diffuse"
}