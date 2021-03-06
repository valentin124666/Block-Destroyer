// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "CustomMaterialCausticEarly" {
   Properties {
      _ColorMain ("Diffuse Material Color", Color) = (1,1,1,1) 
      _MidColor ("Semishadow Material Color", Color) = (1,1,1,1) 
      _SpecColor ("Specular Material Color", Color) = (1,1,1,1) 
      _RefractiveIndex ("RefractiveIndex", Float) = 1
      _RefractivePower ("RefractivePower", Float) = 10
      _Fresnel_power ("_Fresnel_power", Float) = 0
      _Shininess ("Shininess", Float) = 1
      _Transparency ("Transparency", Float) = 1
   }
   SubShader {
   GrabPass { }
      Tags {"Queue"="Transparent+2" "RenderType" = "Transparent"}
    Pass {   

         Tags { "LightMode" = "ForwardBase" } 
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Back
            ZTest LEqual
            ZWrite On
         CGPROGRAM


 
         #pragma vertex vert approxview
         #pragma fragment frag 
         #pragma multi_compile_fog
         #pragma multi_compile_fwdbase
         #pragma fragmentoption ARB_precision_hint_fastest
         #pragma fragmentoption ARB_fog_linear


         #include "AutoLight.cginc"
         #include "UnityCG.cginc"
         #include "WaterInclude.cginc"
        
         uniform half4 _LightColor0; 
         uniform half3 _ColorMain; 
         uniform half3 _SpecColor; 
         uniform half3 _MidColor; 
         uniform half _RefractiveIndex;
         uniform half _RefractivePower;
         uniform half _Fresnel_power;
         uniform half4 unity_FogStart;
         uniform half4 unity_FogEnd;
         uniform sampler2D _GrabTexture;
         uniform float _Shininess;
         uniform float _Transparency;

         struct vertexOutput 
         {
            float2 fogColorFactor:FLOAT;
            float4 pos : SV_POSITION;
            float4 proj : TEXCOORD0;
            half4  normalColor:NORMAL;
            LIGHTING_COORDS(1,2)

         };

         struct appdata_t
         {
            float4 vertex   : POSITION;
            float3 normal   : NORMAL;
            float4 color    : COLOR;
            float4 proj : TEXCOORD;
         };

         vertexOutput vert(appdata_t v) 
         {
            vertexOutput output;
            float4x4 modelMatrix = unity_ObjectToWorld;
            float4x4 modelMatrixInverse = unity_WorldToObject;
            float4 outpos = UnityObjectToClipPos(v.vertex);
            half3 normalDirection = normalize(
               mul(half4(v.normal, 0.0), modelMatrixInverse).xyz);
            half3 viewDirection = normalize(_WorldSpaceCameraPos 
               - mul(modelMatrix, v.vertex).xyz);
            float3 lightDirection;
            float attenuation;

            attenuation = 1.0; // no attenuation
            lightDirection = normalize(_WorldSpaceLightPos0.xyz);


            float factor = max(0.0,dot(normalDirection, lightDirection) * 0.5 + 0.5 * pow(max(0.0, dot(reflect(-lightDirection, normalDirection), viewDirection)), _Shininess));
            float factorl = floor(factor + 0.5);

            //FOG
            float pos = length(outpos.xyz);
            float diff = unity_FogEnd.x - unity_FogStart.x;
            float invDiff = 1.0f / diff;
            output.fogColorFactor.x = clamp ((unity_FogEnd.x - pos) * invDiff, 0.0, 1.0);
            output.fogColorFactor.y = factor;





            float3 refractedDir = refract(viewDirection, 
               normalDirection, 1.0 / _RefractiveIndex);
            output.normalColor.xyz = refractedDir;
            half refl2Refr = pow(dot(viewDirection,normalDirection),_Fresnel_power);//Fresnel(viewDirection, normalDirection, _Fresnel_bias, _Fresnel_power);
        
            #if UNITY_UV_STARTS_AT_TOP
                float scale = -1.0;
            #else
                float scale = 1.0;
            #endif

            output.proj.xy = (float2(outpos.x, outpos.y*scale) + outpos.w) * 0.5;
            output.proj.zw = outpos.zw;
            output.normalColor.w = refl2Refr;
            output.pos = outpos;
            TRANSFER_VERTEX_TO_FRAGMENT(output);
            return output;
         }
 
         half4 frag(vertexOutput input) : COLOR
         {
            float factor = input.fogColorFactor.y* LIGHT_ATTENUATION(input);

            float factorl = floor(factor  + 0.5);
            float3 tempcolshadowmid = lerp(_ColorMain,_MidColor,factor * 2.0);
            float3 tempcolmidlight = lerp(_MidColor,_SpecColor,(factor - 0.5 ) * 2.0);
            float3 tempcol = tempcolshadowmid * (1 - factorl) + tempcolmidlight * factorl;
           
            float3 finalColor = lerp(unity_FogColor.rgb, tempcol.rgb, input.fogColorFactor.x);
            half4 baseColor = float4(finalColor, _Transparency) ;


            float4 offset = input.proj; // We shift our pixel to the desired position

            float4 tempCoord = UNITY_PROJ_COORD(offset);
            tempCoord.zw +=(input.normalColor.rg ) * _RefractivePower;
            tempCoord.xy +=(input.normalColor.rg ) * _RefractivePower;

            half4 col = tex2Dproj(_GrabTexture, tempCoord);//half4(input.normalColor, 1.0f) ;
            baseColor = lerp (baseColor, col, saturate(input.normalColor.w * 2.0));
            return baseColor;
         }
 
         ENDCG
      }
 
    }
 
   Fallback "Mobile/VertexLit"
}