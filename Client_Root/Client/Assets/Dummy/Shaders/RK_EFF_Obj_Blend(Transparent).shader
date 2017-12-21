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

Shader "Shaders/TA/Effects/RK_EFF_Blend(Transparent)"{
Properties	{
		_TintColor ("Tint Color", Color) = (1.0,1.0,1.0,1.0)
		_MainTex ("MainTex", 2D) = "white" {}
	}

	Category {

	Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "Render Type" = "Transparent" }

	Blend SrcAlpha OneMinusSrcAlpha // 소스 알파값에 의해 곱해지며 (1-소스색상)에 의 곱해집니다.
	Cull Off
	Lighting Off
	//ZWrite Off
	fog{mode Off}

	SubShader{
			pass{
				CGPROGRAM
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				fixed4 _TintColor;
				sampler2D _MainTex;

				struct v2f {
					fixed4 vertex : POSITION;
					fixed2 texcoord : TEXCOORD0;
				};

				fixed4 _MainTex_ST;

				v2f vert(appdata_full v){
					v2f o;

					#if UNITY_VERSION >= 540
					o.vertex = UnityObjectToClipPos(v.vertex);
					#else
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
					#endif

					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
					return o;
				}

				fixed4 frag(v2f i) : COLOR {
					fixed4 FinalTex = tex2D(_MainTex, i.texcoord);
					return FinalTex * _TintColor;
				}
				ENDCG

			}//pass


		}//subshader


	}//category

	Fallback "Diffuse"

}
