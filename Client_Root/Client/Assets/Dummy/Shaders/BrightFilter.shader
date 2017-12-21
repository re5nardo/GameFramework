/*******************************************************************************

Copyright (C) [2017] NCSOFT Corporation. All Rights Reserved. 

This software is provided 'as-is', without any express or implied warranty. 
In no event will NCSOFT Corporation (“NCSOFT”) be held liable for any damages
arising from the use of this software.

Permission is granted to anyone to use this software subject to acceptance 
and compliance with any agreement entered into between NCSOFT (or any of its affiliates) and the recipient. 
The following restrictions shall also apply:

1. The origin of this software must not be misrepresented; you must not
claim that you wrote the original software. 
2. You may not modify, alter or redistribute this software, in whole or part, unless you are entitled to 
do so by express authorization in a separate agreement between you and NCSOFT.
3. This notice may not be removed or altered from any source distribution.

*******************************************************************************/

// Created By taeyo

Shader "Shaders/TA/ImageEffect/BloomAndFlare/BrightFilter"
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "" {}
         threshold ("", RECT) = "white" {} 
         useSrcAlphaAsMask ("", Float) = 0.5 

	}
	
//	CGINCLUDE
//	
//	#include "UnityCG.cginc"

	
	Subshader 
	{
//		LOD 600
		Pass 
 		{
			  ZTest Always Cull Off ZWrite Off
		
		      CGPROGRAM
		      
		      #pragma vertex vert
		      #pragma fragment frag
		       #pragma fragmentoption ARB_precision_hint_fastest 

		      #include "UnityCG.cginc"


  			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD;
			};

	      	struct v2f 
			{
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD;
			};
			
			sampler2D _MainTex;	

			float4 _MainTex_ST;
			
			half4 threshold;
			half useSrcAlphaAsMask;
				
			v2f vert( appdata_t v ) 
			{
				v2f o;
				#if UNITY_VERSION >= 540
				o.vertex = UnityObjectToClipPos(v.vertex);
				#else
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
				#endif
				
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			} 
			
			half4 frag(v2f i) : COLOR 
			{
				half4 color = tex2D(_MainTex, i.texcoord);
				//color = color * saturate((color-threshhold.x) * 75.0); // didn't go well with HDR and din't make sense
				color = color * lerp(1.0, color.a, useSrcAlphaAsMask);
				color = max(half4(0,0,0,0), color-threshold.x);
				return color;
			}
		 ENDCG
		     
		}
//		 ENDCG
	}
	Fallback off
}
