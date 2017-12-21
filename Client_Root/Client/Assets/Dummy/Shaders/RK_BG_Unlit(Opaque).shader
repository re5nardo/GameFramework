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

Shader "Shaders/TA/BackGround/RK_BG_Unlit(Opaque)"{
Properties	{
		_MainColor ("MainColor", Color) = (1.0,1.0,1.0,1.0)
		_BGColor("BG Color(Client Only)", Color) = (1,1,1,1)
		_MainTex ("MainTex", 2D) = "white" {}
		_Rim ("Rim", Float) = 1.0

		[MaterialToggle(_UVSCROLL_ON)] _UseUVSCROLL ("UVSCROLL", Float ) = 0
		_ScrollxSpeed ("Scroll_X_Speed", float) = 0
		_ScrollySpeed ("Scroll_Y_Speed", float) = 0
	}


	SubShader
	{
		Tags {"RenderType"="Opaque"}

		CGINCLUDE
		#include "UnityCG.cginc"
		#pragma target 3.0




		uniform fixed4 _MainColor;
		uniform fixed4 _BGColor;
		uniform fixed _ScrollxSpeed;
		uniform fixed _ScrollySpeed;
		uniform sampler2D _MainTex;
		uniform fixed4 _MainTex_ST;

		uniform fixed _Rim;
		ENDCG


		Pass
		{
			CGPROGRAM
			#pragma debug
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_fog
			#pragma shader_feature _UVSCROLL_ON

			struct v2f
			{
				fixed4 vertex : POSITION;
				fixed2 uv : TEXCOORD0;

				UNITY_FOG_COORDS(5)
			};

		v2f vert (appdata_full v)
		{
			v2f o;

			#if UNITY_VERSION >= 540
			o.vertex = UnityObjectToClipPos(v.vertex);
			#else
			o.vertex = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
			#endif
			
			o.uv = v.texcoord;

			#if _UVSCROLL_ON
				fixed timeX = -_Time.x * _ScrollxSpeed;
				fixed timeY = -_Time.y * _ScrollySpeed;

				o.uv = o.uv + fixed2(timeX,timeY);
			#endif 
				o.uv = o.uv;

			o.uv = TRANSFORM_TEX(o.uv, _MainTex);//+ fixed2(timeX,timeY);

			UNITY_TRANSFER_FOG(o, o.vertex);

			return o;
		}

			fixed4 frag (v2f i) : COLOR
			{
				fixed4 c;

				fixed4 mainTex = tex2D(_MainTex, i.uv);

				UNITY_APPLY_FOG(i.fogCoord, mainTex);
				//Color
				c.rgb =  mainTex.rgb  * _MainColor.rgb * _BGColor.rgb * _Rim;// * maskTex;

				c.a = 1;
				return c;
			}
			ENDCG
		}//pass2 림 컬러 외 적용 완료

	}//SubShader

	Fallback "Diffuse"



}
