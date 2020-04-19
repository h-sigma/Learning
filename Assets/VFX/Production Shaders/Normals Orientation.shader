Shader "Production/Normals Orientation"
{
    Properties
    {
        _TowardColor ("Color Facing Toward View", COLOR) = (0.1, 0.1, 0.9, 1.0)
        _AwayColor ("Color Facing Away View", COLOR) = (0.9, 0.1, 0.1, 1.0)
    }
    SubShader
    {
        Pass 
        {
            Cull Front
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
    
            fixed4 _AwayColor;
    
            struct appdata {
                float4 position : POSITION;
            };
            
            struct v2f {
                float4 pos : SV_POSITION;
            };
            
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.position);
                return o;
            }
            
            fixed4 frag (v2f input) : SV_Target
            {
                return _AwayColor;
            }
            ENDCG
        }
        
        Pass 
        {
            Cull Back
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
    
            fixed4 _TowardColor;
    
            struct appdata {
                float4 position : POSITION;
            };
            
            struct v2f {
                float4 pos : SV_POSITION;
            };
            
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.position);
                return o;
            }
            
            fixed4 frag (v2f input) : SV_Target
            {
                return _TowardColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
