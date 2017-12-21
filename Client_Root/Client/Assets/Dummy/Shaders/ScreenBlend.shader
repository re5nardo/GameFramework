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

Shader "Shaders/TA/ImageEffect/BloomAndFlare/ScreenBlend" {
	Properties {
		_MainTex ("Screen Blended", 2D) = "" {}
		_ColorBuffer ("Color", 2D) = "" {}

		_Intensity ("", Float) = 0.5 

//		_ColorBuffer_TexelSize ("", RECT) = "white" {} 
//		_MainTex_TexelSize ("", RECT) = "white" {} 

	}
	
	CGINCLUDE

	#include "UnityCG.cginc"
	
	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv[2] : TEXCOORD0;
	};
	struct v2f_mt {
		float4 pos : SV_POSITION;
		float2 uv[4] : TEXCOORD0;
	};


	struct appdata_mt {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD;
	};

			
	sampler2D _ColorBuffer;
	sampler2D _MainTex;

	float4 _MainTex_ST;
	
	half _Intensity;
	half4 _ColorBuffer_TexelSize;
	half4 _MainTex_TexelSize;
		
	v2f vert( appdata_mt v ) {
		v2f o;

		#if UNITY_VERSION >= 540
		o.pos = UnityObjectToClipPos(v.vertex);
		#else
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
		#endif

		o.uv[0] =  v.texcoord.xy;
		o.uv[1] =  v.texcoord.xy;
		
		#if UNITY_UV_STARTS_AT_TOP
		if (_ColorBuffer_TexelSize.y < 0) 
			o.uv[1].y = 1-o.uv[1].y;
		#endif	
		
		return o;
	}


	struct appdata_t {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD;
	};



	v2f_mt vertMultiTap( appdata_t v ) {
		v2f_mt o;

		#if UNITY_VERSION >= 540
		o.pos = UnityObjectToClipPos(v.vertex);
		#else
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
		#endif

		o.uv[0] = v.texcoord.xy + _MainTex_TexelSize.xy * 0.5;
		o.uv[1] = v.texcoord.xy - _MainTex_TexelSize.xy * 0.5;	
		o.uv[2] = v.texcoord.xy - _MainTex_TexelSize.xy * half2(1,-1) * 0.5;	
		o.uv[3] = v.texcoord.xy + _MainTex_TexelSize.xy * half2(1,-1) * 0.5;	
		return o;
	}
	
	half4 fragScreen (v2f i) : COLOR {
		half4 toBlend = saturate (tex2D(_MainTex, i.uv[0]) * _Intensity);
		return 1-(1-toBlend)*(1-tex2D(_ColorBuffer, i.uv[1]));
	}
	
	half4 fragMultiTap (v2f_mt i) : SV_Target {
		half4 outColor = tex2D(_MainTex, i.uv[0].xy);
		outColor += tex2D(_MainTex, i.uv[1].xy);
		outColor += tex2D(_MainTex, i.uv[2].xy);
		outColor += tex2D(_MainTex, i.uv[3].xy);
		return outColor * 0.25;
	}

	ENDCG 
	
Subshader 
{
//	LOD 600
	  ZTest Always Cull Off ZWrite Off


 // 0: nicer & softer "screen" blend mode	  		  	
 Pass {    

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment fragScreen
      ENDCG
  }
 // 2: used for "stable" downsampling
 Pass {    

      CGPROGRAM
      #pragma vertex vertMultiTap
      #pragma fragment fragMultiTap
      ENDCG
  } 
}

Fallback off
	
} // shader
