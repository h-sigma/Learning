Shader "Learning/Reflective" {
    Properties
    {
        _myBump ("Bump Texture", 2D) = "bump"{}
        _myCube ("Skybox", CUBE) = "white"{}
    }
    
    SubShader
    {
        CGPROGRAM
        #pragma surface surf Lambert
        
        struct Input {
            float2 uv_myBump;
            float3 worldRefl; INTERNAL_DATA
        };
        samplerCUBE _myCube;
        sampler2D _myBump;
        
        void surf(Input IN, inout SurfaceOutput o)
        {
            o.Normal *= UnpackNormal(tex2D(_myBump, IN.uv_myBump)) * 0.3;
            o.Albedo = texCUBE(_myCube, WorldReflectionVector(IN, o.Normal)).rgb;
        }
        
        ENDCG
    }
    FallBack "Diffuse"
}