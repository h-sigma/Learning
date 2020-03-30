Shader "Learning/ToonRampSurface" {
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
        #pragma surface surf Lambert
        
        float4 _Color;
        //sampler2D _ColorTex;
        sampler2D _RampTex;
        
        struct Input {
            //float2 uv_ColorTex;
            float3 viewDir;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            float diff = dot(o.Normal, IN.viewDir);
            float h = diff * 0.5 + 0.5;
            float2 rh = h;
            o.Albedo = tex2D(_RampTex, rh).rgb * _Color;
        }
        
        ENDCG
    }
    FallBack "Diffuse"
}