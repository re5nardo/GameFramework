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

Shader "Shaders/TA/ImageEffect/Vignetting/RK_Vignetting" 
{
	Properties {
		_MainTex ("Base", 2D) = "white" {}
		_VignetteTex ("Vignette", 2D) = "white" {}
		_Intensity ("Intensity" , float) = 0.0
	}
	
	CGINCLUDE
	
	#include "UnityCG.cginc"
	
	struct v2f
	{
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		float2 uv2 : TEXCOORD1;
	};
	
	sampler2D _MainTex;
	sampler2D _VignetteTex;
	
	half _Intensity;
	half _Blur;

	float4 _MainTex_TexelSize;
		
	v2f vert( appdata_img v )
	{
		v2f o;

		#if UNITY_VERSION >= 540
		o.pos = UnityObjectToClipPos(v.vertex);
		#else
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
		#endif

		o.uv = v.texcoord.xy;
		o.uv2 = v.texcoord.xy;

		#if UNITY_UV_STARTS_AT_TOP
		if (_MainTex_TexelSize.y < 0)
			 o.uv2.y = 1.0 - o.uv2.y;
		#endif

		return o;
	} 

	v2f vertDS( appdata_img v ) 
	{
		v2f o;

		#if UNITY_VERSION >= 540
		o.pos = UnityObjectToClipPos(v.vertex);
		#else
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
		#endif

		o.uv = v.texcoord.xy;
		o.uv2 = v.texcoord.xy;
		
		return o;
	} 
	
	half4 fragDs(v2f i) : SV_Target 
	{
		half4 c = tex2D (_MainTex, i.uv.xy + _MainTex_TexelSize.xy * 0.5);
		c += tex2D (_MainTex, i.uv.xy - _MainTex_TexelSize.xy * 0.5);
		c += tex2D (_MainTex, i.uv.xy + _MainTex_TexelSize.xy * float2(0.5,-0.5));
		c += tex2D (_MainTex, i.uv.xy - _MainTex_TexelSize.xy * float2(0.5,-0.5));
		return c/4.0;
	}
	
	half4 frag(v2f i) : SV_Target 
	{
		half2 coords = i.uv;
		half2 uv = i.uv;
		
		coords = (coords - 0.5) * 2.0;		
		half coordDot = dot (coords,coords);
		half4 color = tex2D (_MainTex, uv);	 

		float mask = 1.0 - coordDot * _Intensity; 
		
		half4 colorBlur = tex2D (_VignetteTex, i.uv2);
		color = lerp (color, colorBlur, saturate (_Blur * coordDot));
		
		return color * mask;
	}
	ENDCG 
	
	Subshader 
	{
//		LOD 600
		Pass 
		{
			ZTest Always Cull Off ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}

		Pass
		{
			ZTest Always Cull Off ZWrite Off
			CGPROGRAM
			#pragma vertex vertDS
			#pragma fragment fragDs
			ENDCG
		}
	}
	Fallback off	
} 
