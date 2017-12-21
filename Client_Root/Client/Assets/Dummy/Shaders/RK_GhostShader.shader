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

Shader "Shaders/TA/Character/RK_GhostShader" {
Properties {

_TintColor ("Tint Color", Color) = (1.0,1.0,1.0,1.0)
_MainTex ("MainTex",2D) = "white"{}
_RimColor ("Rim Color", Color) = (0.5,0.5,0.5,0.5)
_InnerColor ("Inner Color", Color) = (0.5,0.5,0.5,0.5)
_InnerColorPower ("Inner Color Power", Range(0.0,1.0)) = 0.5
_RimPower ("Rim Power", Range(0.0,5.0)) = 2.5
_AlphaPower ("Alpha Rim Power", Range(0.0,8.0)) = 4.0
_AllPower ("All Power", Range(0.0, 10.0)) = 1.0

//	  	[MaterialToggle] _IsDeath	("[Death Color] Black = 0 white = 1", Float)					= 0
	  	_Death ("_Death", Range(-0.001,1)) = -0.001
}
SubShader {
	Tags { "Queue" = "Transparent""IgnoreProjector" = "True" "Render Type" = "Transparent" }

		    Pass
	    {
			ColorMask 0
	    }

		//Cull front
//	Lighting Off
	//ZWrite Off
//	fog{mode Off}




//	Pass
//	{
//		Blend SrcColor One
//
//		CGPROGRAM
//		#pragma vertex vert
//		#pragma fragment frag
//		#include "UnityCG.cginc"
//
//		fixed4 _TintColor;
//		sampler2D _MainTex;
//
//		struct v2f {
//			fixed4 vertex : POSITION;
//			fixed2 texcoord : TEXCOORD0;
//		};
//
//		v2f vert(appdata_full v){
//			v2f o;
//			o.vertex = mul(UNITY_MATRIX_MVP , v.vertex);
////			o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
//			return o;
//		}
//
//		fixed4 frag(v2f i) : COLOR {
//			fixed4 FinalTex = tex2D(_MainTex, i.texcoord);
//			return FinalTex * _TintColor;
//		}
//
//		ENDCG
//	}

	CGPROGRAM
	#pragma surface surf Lambert alpha

		struct Input
		{
			half2 uv_MainTex;
			float3 viewDir;
			INTERNAL_DATA
		};
			sampler2D _MainTex;
			fixed4 _RimColor;
			float _RimPower;
			fixed4 _TintColor;
			float _AlphaPower;
			float _AlphaMin;
			float _InnerColorPower;
			float _AllPower;
			fixed4 _InnerColor;

//			uniform fixed	_IsDeath;
						uniform fixed _Death;

			void surf (Input IN, inout SurfaceOutput o) {

				fixed4 mainTex = tex2D(_MainTex, IN.uv_MainTex) + _TintColor;
				half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));

				clip((mainTex.a * ceil(mainTex.b -_Death)) - 0.5);

				half3 finalColor = lerp(mainTex.rgb, mainTex.rgb , _Death*2);

				o.Albedo = mainTex.rgb;
//				half3 finalTex = mainTex.rgb * _TintColor;
				o.Emission = finalColor * mainTex.rgb * _RimColor.rgb * pow (rim, _RimPower)*_AllPower+(_InnerColor.rgb*2*_InnerColorPower);
				o.Alpha = mainTex.r * (pow (rim, _AlphaPower))*_AllPower;
				}
		ENDCG
	}
	Fallback "Diffuse"
} 