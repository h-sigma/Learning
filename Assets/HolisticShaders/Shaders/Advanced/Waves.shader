Shader "Learning/Waves"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Color ("Tint", Color) = (1, 1, 1, 1)
        _Freq ("Frequency", Range(0, 2)) = 0
        _Speed ("Speed", Range(1, 100)) = 1.0
        _Amp ("Amplitude", Range(0, 2)) = 0.0
    }
    SubShader
    {
        Tags {"Queue" = "Geometry"}
        
        CGPROGRAM
        #pragma surface surf Lambert vertex:vert
        
        struct appdata {
            float4 vertex : POSITION;
            float4 texcoord : TEXCOORD0;
            float4 texcoord1 : TEXCOORD1;
            float4 texcoord2 : TEXCOORD2;
            float3 normal : NORMAL;
        };
        
        struct Input {
            float4 color;
            float2 uv_MainTex;
        }; 
        
        float _Speed;
        float _Amp;
        float _Freq;
        sampler2D _MainTex;
        float4 _Color;
        
        void vert(inout appdata v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            float t = _Time * _Speed;
            float waveHeight = sin(t + v.vertex.x * _Freq) * _Amp;
            v.vertex.y = waveHeight + v.vertex.y;
            v.normal = normalize(float3(v.normal.x, v.normal.y + waveHeight, v.normal.z));
            o.color = waveHeight + 2;
        }
        
        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 col = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = col.rgb * IN.color * _Color; 
        }        
        
        ENDCG
        
    }
    FallBack "Diffuse"
}
