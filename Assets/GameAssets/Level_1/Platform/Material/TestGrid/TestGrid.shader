Shader "Custom/SpriteGridLines"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GridColor ("Grid Color", Color) = (1,1,1,1)
        _GridSize ("Grid Size", Float) = 1
    }
    SubShader
    {
        Tags {"Queue" = "Transparent"}
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
                float4 worldPos : TEXCOORD1;
            };

            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _GridColor;
            float _GridSize;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.texcoord);
                float2 gridCoord = frac(i.worldPos.xy / _GridSize);
                float lines = step(0.05, min(gridCoord.x, gridCoord.y)) * step(0.05, min(1.0 - gridCoord.x, 1.0 - gridCoord.y));
                fixed4 gridColor = fixed4(_GridColor.rgb, _GridColor.a * lines);
                return lerp(texColor, gridColor, gridColor.a);
            }
            ENDCG
        }
    }
}
