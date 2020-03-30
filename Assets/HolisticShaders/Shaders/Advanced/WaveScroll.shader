Shader "Learning/WaveScroll"
{
    Properties
    {
        _Tint ("Tint", Color) = (1, 1, 1, 1)
        _FasterTex ("Faster Texture", 2D) = "white" {}
        _SlowerTex ("Slower Texture", 2D) = "white" {}
        _ScrollX ("Scroll Horizontal", Range (-5, 5)) = 1
        _ScrollY ("Scroll Vertical", Range (-5, 5)) = 1
        _Speed("Wave Speed", Range(0, 100)) = 10
        _Freq("Frequency", Range(0, 10)) = 1
        _Amp("Amplitude", Range(0, 10)) = 1
    }
    SubShader
    {
        Tags {"Queue" = "Transparent"}
        
        CGPROGRAM
        #pragma surface surf Lambert vertex:vert
        
        struct appdata {
            float4 vertex : POSITION;
            float4 texcoord : TEXCOORD0;
            float4 texcoord1 : TEXCOORD1;  //lightmaps, meta pass
            float4 texcoord2 : TEXCOORD2; //dynamic GI, meta pass
            float3 normal : NORMAL;
        };
        
        struct Input {
            float2 uv_FasterTex;
            float2 uv_SlowerTex;
        }; 
        
        float _Speed;
        float _Freq;
        float _Amp;
        
        void vert(inout appdata v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            float t = _Time * _Speed;
            float waveHeight = sin(v.vertex.x * _Freq + t) * _Amp;
            v.vertex.y = v.vertex.y + waveHeight;
            v.normal = normalize(float3(v.normal.x, v.normal.y + waveHeight, v.normal.z));
        }
        
        float4 _Tint;
        sampler2D _FasterTex;
        sampler2D _SlowerTex;
        half _ScrollX;
        half _ScrollY;
        
        void surf(Input IN, inout SurfaceOutput o)
        {
            _ScrollX *= _Time;
            _ScrollY *= _Time;
            float2 newUV1 = IN.uv_FasterTex + float2(_ScrollX, _ScrollY);
            float2 newUV2 = IN.uv_SlowerTex + float2(_ScrollX/2.0, _ScrollY/2.0);
            fixed4 col1 = tex2D(_FasterTex, newUV1);
            fixed4 col2 = tex2D(_SlowerTex, newUV2);
            o.Albedo = (col1 + col2) / 2.0;
            o.Albedo *= _Tint.rgb; 
        }        
         
        ENDCG
    }
    FallBack "Diffuse"
}
