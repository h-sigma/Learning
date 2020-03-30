Shader "Learning/BumpDiffuseStencil" {
    Properties {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _DiffuseTex ("Diffuse Texture", 2D) = "white" {}
        _BumpTex ("Bump Texture", 2D) = "bump" {}
        _Slider ("Bump Amount", Range(0, 10)) = 1
        
        _SRef ("Stencil Ref", Float) = 1
        [Enum(UnityEngine.Rendering.CompareFunction)] _SComp("Stencil Comp", Float) = 8
        [Enum(UnityEngine.Rendering.StencilOp)] _SOp("Stencil Op", Float) = 2
    }
    
    SubShader {
        
        Tags {
            "Queue" = "Geometry"
        }
        
        Stencil {
            Ref [_SRef]
            Comp [_SComp]
            Pass [_SOp]
        }
        
        CGPROGRAM
        #pragma surface surf Lambert
        
        sampler2D _DiffuseTex;
        sampler2D _BumpTex;
        half _Slider;
        fixed4 _Color;
        
        struct Input {
            float2 uv_DiffuseTex;
            float2 uv_BumpTex;
        };
        
        void surf(Input IN, inout SurfaceOutput o)
        {
            o.Albedo = tex2D(_DiffuseTex, IN.uv_DiffuseTex).rgb * _Color.rgb;
            half3 n = UnpackNormal(tex2D(_BumpTex, IN.uv_BumpTex));
            n *= float3(_Slider, _Slider, 1);
            o.Normal = n;
        }
        
        ENDCG
    }
    FallBack "Diffuse"
}