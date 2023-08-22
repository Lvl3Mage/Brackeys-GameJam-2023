Shader "Unlit/WaterDistortion"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DistortionMap1 ("DistortionMap1", 2D) = "black" {}
        _NoiseScale1 ("NoiseScale1", Float) = 1
        _NoiseScrollSpeed1 ("NoiseScrollSpeed1", Float) = 1
        _DistortionMap2 ("DistortionMap2", 2D) = "black" {}
        _NoiseScale2 ("NoiseScale2", Float) = 1
        _NoiseScrollSpeed2 ("NoiseScrollSpeed2", Float) = 1
        _MaskMap ("MaskMap", 2D) = "black" {}
        _MaskScale ("MaskScale", Float) = 1
        _MaskScrollSpeed ("MaskScrollSpeed", Float) = 1
        _OffsetSize ("OffsetSize", Float) = 0.1
        _CameraMovementMultiplier ("CameraMovementMultiplier", Float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _DistortionMap1;
            float _NoiseScale1;
            float _NoiseScrollSpeed1;
            sampler2D _DistortionMap2;
            float _NoiseScale2;
            float _NoiseScrollSpeed2;
            sampler2D _MaskMap;
            float _MaskScale;
            float _MaskScrollSpeed;
            float _OffsetSize;
            float _CameraMovementMultiplier;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                // float angle = _Time.x;
                float2 distortionUV = i.uv.xy;
                float2 cameraMuls = float2(_WorldSpaceCameraPos.x/unity_OrthoParams.x,_WorldSpaceCameraPos.y/unity_OrthoParams.y)/2;
                float4 offset1 = tex2D(_DistortionMap1, (distortionUV + cameraMuls + _Time.y*_NoiseScrollSpeed1*float2(1,-1))*_NoiseScale1);
                float4 offset2 = tex2D(_DistortionMap2, (distortionUV + cameraMuls + _Time.y*_NoiseScrollSpeed2*float2(-1,1))*_NoiseScale2);
                float mask = tex2D(_MaskMap, (distortionUV + cameraMuls + _Time.y*_MaskScrollSpeed*float2(1,1))*_MaskScale);

                float2 offset = (lerp(offset1, offset2, mask.x)).xy;
                offset = (offset - float2(0.5,0.5))*2*_OffsetSize;
                fixed4 col = tex2D(_MainTex, i.uv + offset);
                // return mask;
                return col;
            }
            ENDCG
        }
    }
}
