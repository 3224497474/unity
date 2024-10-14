Shader "Custom/SpriteFadeToDark"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FadeColor ("Fade Color", Color) = (0,0,0,1)
        _FadeIntensity ("Fade Intensity", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType"="Transparent"}
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _FadeColor;
            float _FadeIntensity;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.texcoord);

                // 计算从中心到边缘的距离
                float2 center = float2(0.5, 0.5);
                float dist = distance(i.texcoord, center);

                // 计算透明度和颜色渐变
                float fade = smoothstep(0.0, _FadeIntensity, dist);
                float alpha = lerp(1.0, 0.0, fade);

                fixed4 fadeColor = lerp(texColor, _FadeColor, fade);
                fadeColor.a *= alpha;
                fadeColor.a = 1 - fadeColor.a;
                return fadeColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
