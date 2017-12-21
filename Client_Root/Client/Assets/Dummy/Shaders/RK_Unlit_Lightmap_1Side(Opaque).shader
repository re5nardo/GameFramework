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

Shader "Shaders/TA/BackGround/RK_Unlit_Lightmap_1Side(Opaque)"
{
	Properties
	{
		_MainColor 		("Color", Color)	= (1.0,1.0,1.0,1.0)
		_BGColor("BG Color(Client Only)", Color) = (1,1,1,1)
		_MainTex	("Base (RGB)", 2D)	= "white"{}
	}

	SubShader
	{
		Tags {"LIGHTMODE"="ForwardBase" "RenderType"="Opaque"}
		
		
		CGINCLUDE
		#include "UnityCG.cginc"
		
		fixed4		_MainColor;
		fixed4 		_BGColor;
		sampler2D	_MainTex;
		fixed4		_MainTex_ST;				
		
		struct v2f
		{
			float4 pos		: SV_POSITION;
			float2 uv		: TEXCOORD0;
			
			#if LIGHTMAP_ON
        	float2 lmap		: TEXCOORD1;
        	#endif			
						
			UNITY_FOG_COORDS(2)			
		};

		v2f vert (appdata_full v)
		{
			v2f o;
			
			#if UNITY_VERSION >= 540
			o.pos = UnityObjectToClipPos(v.vertex);
			#else
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
			#endif

			o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
						
			UNITY_TRANSFER_FOG(o,o.pos);
			
			#if LIGHTMAP_ON			
				o.lmap = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw ;
        	#endif			
			
			return o;
		}
		ENDCG
		
		
		Pass
		{
			CGPROGRAM
			#pragma debug
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog			
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile _ LIGHTMAP_ON
			
			fixed4 frag (v2f i) : COLOR
			{
				half4 c = tex2D (_MainTex, i.uv);
				c.rgb *= _MainColor.rgb * _BGColor.rgb;				
				
			#if LIGHTMAP_ON
				fixed3 lm = UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lmap)*2;
          		c.rgb *= lm;				
			#endif
				
				UNITY_APPLY_FOG(i.fogCoord, c);
				
				return c;
			}
			ENDCG 
		}	
	}

	FallBack "Diffuse"
}


