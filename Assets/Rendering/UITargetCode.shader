Shader "Floodgame/UI/Target"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Alpha ("Transparency", Range (0, 1)) = 0.5
        _Progress ("Progress", Range (0, 1)) = 1
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "Queue"="Geometry"
        }
 
        Pass
        {
            Cull Back
            Blend SrcAlpha OneMinusSrcAlpha
            ZTest LEqual
            ZWrite On
 
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
 
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
 
            // user defined variables
            sampler2D _MainTex;
            float4 _MainTex_ST;
            half _Alpha;
            float _Progress;
 
            // vertex input
            struct appdata
            {
                float4 pos : POSITION;
                half2 uv : TEXCOORD0;
            };
 
            // vertex output
            struct v2f
            {
                float4 pos : SV_POSITION;
                half2 uv : TEXCOORD0;
            };
 
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.pos);
                o.uv = v.uv;
                return o;
            }
 
            half4 frag (v2f i) : SV_Target
            {
                half2 uv = i.uv;
                uv *= 2;
                uv -= 1;
                float len = length(uv);

                float edge1 = lerp(.4, 1, _Progress);
                float edge2 = edge1 - .6;
                
                float val1 = step(len, edge1);
                float val2 = step(edge2, len);

                return half4(float3(1, 1, 1), _Alpha * min(val1, val2));
            }
            ENDHLSL
        }
    }
}