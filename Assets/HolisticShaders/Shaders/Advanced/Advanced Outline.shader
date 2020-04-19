Shader "Learning/AdvancedOutline"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _OutlineColor ("Color", Color) = (1,1,1,1)
        _Outline ("Outline Width", Range(0.0, 1.0)) = 0.2
    }
    SubShader
    {
        CGPROGRAM
        #pragma surface surf Lambert
        
        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
        }
        ENDCG
        
        Pass {
            Cull Front
        
            CGPROGRAM 
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            
            #include "UnityCG.cginc"
            
            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            
            struct v2f {
                float4 pos : SV_POSITION;
                float4 color : COLOR0; 
            };
            
            float _Outline;
            float4 _OutlineColor;
            
            v2f vert(appdata v)
            {   //what is happening?
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                float3 norm = normalize(mul( (float3x3)UNITY_MATRIX_IT_MV, v.normal));
                float2 offset = TransformViewToProjection(norm.xy);
                o.pos.xy += offset * _Outline;
                o.color = _OutlineColor;
                return o;
            }
            
            fixed4 frag(v2f i) : SV_Target
            {
                return i.color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
