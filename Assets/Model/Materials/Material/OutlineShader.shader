Shader "Custom/GlowingOutlineShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}  
        _OutlineColor ("Outline Color", Color) = (0, 1, 0, 1) // Green
        _OutlineWidth ("Outline Width", Range(0.0, 0.1)) = 0.02  
        _EmissionColor ("Emission Color", Color) = (0, 1, 0, 1)  // Green Glow
        _EmissionStrength ("Emission Strength", Range(0.1, 5)) = 1.5  // Glow Strength
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Opaque" }
        Pass
        {
            Name "OUTLINE"
            Cull Front  
            ZWrite On
            ZTest LEqual

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            float _OutlineWidth;
            fixed4 _OutlineColor;
            fixed4 _EmissionColor;
            float _EmissionStrength;

            v2f vert(appdata_t v)
            {
                v2f o;
                float3 normal = normalize(UnityObjectToWorldNormal(v.normal));
                v.vertex.xyz += normal * _OutlineWidth;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _OutlineColor + (_EmissionColor * _EmissionStrength);
            }
            ENDCG
        }
        
        Pass
        {
            Name "TEXTURE"
            Cull Back  

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
