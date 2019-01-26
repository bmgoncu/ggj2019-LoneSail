Shader "Unlit/NewUnlitShader"
{
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _FoamColor ("Foam color", Color) = (.34, .85, .92, 1) // color
     }
 
     SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

         Pass {
             CGPROGRAM
 
             #pragma vertex vert
             #pragma fragment frag
             #include "UnityCG.cginc"
 
             sampler2D _MainTex;
             float4 _MainTex_ST;
             float4 _FoamColor;
 
             struct v2f{
                 float4 pos: SV_POSITION;
                 fixed2 uv: TEXCOORD0;
             };
 
             v2f vert (appdata_base v) {
                 v2f vOutput;
                 vOutput.pos = UnityObjectToClipPos(v.vertex);
                 vOutput.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
                 return vOutput;
             }
 
             fixed4 frag (v2f o) : SV_Target {
                 fixed4 texcol = fixed4 (o.uv[0], o.uv[1], 1, 1.0);
                 fixed4 col = tex2D(_MainTex, o.uv);
                 return fixed4 (_FoamColor[0],_FoamColor[1],_FoamColor[2], col[0]);
             }
 
             ENDCG
         }
     }
 }