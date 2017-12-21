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

Shader "Shaders/TA/BackGround/RK_BG_Fog(Transparent)"
{
	Properties
	{
		_MainTex("Fog Tex", 2D) = "black" {}
//		_FogColor("Fog Color", Color) = (1,1,1,1)
		_MainColor("Main Color", Color) = (1,1,1,1)
		_BGColor("BG Color(Client Only)", Color)		= (1,1,1,1)

		_BackAlpha("Back Fog Alpha",Range(0,1)) = 0
		_ScrollSpeed_1("Back Tex Speed",float) = 0

		_FrontAlpha("Front Fog Alpha",Range(0,1)) = 0
		_ScrollSpeed_2("Front Tex Speed",float) = 0
	}



	SubShader
	{

		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "Render Type" = "Transparent" }
	
		Lighting Off
			ZWrite Off

		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
//			#pragma exclude_renderers d3d11 gles3 d3d11_9x xbox360 xboxone ps3 ps4 psp2
			// make fog work
//			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct v2f
			{

				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
//				UNITY_FOG_COORDS(1)

			};

			sampler2D _MainTex;
			fixed4 _MainTex_ST;
			fixed4 _BGColor;
			fixed4 _MainColor;

			fixed _ScrollSpeed_1;
			fixed _ScrollSpeed_2;

			fixed _BackAlpha;
			fixed _FrontAlpha;

			v2f vert (appdata_base v)
			{
				v2f o;
				#if UNITY_VERSION >= 540
				o.vertex = UnityObjectToClipPos(v.vertex);
				#else
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
				#endif
				
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
//				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{

				// sample the texture
				fixed4 final;
				
				fixed4 Back = tex2D(_MainTex, float2(i.uv.x + _Time.y*_ScrollSpeed_1*0.5,i.uv.y));
				fixed4 Front = tex2D(_MainTex, float2(i.uv.x+_Time.y*_ScrollSpeed_2*0.5, i.uv.y));
				fixed4 Mask = tex2D(_MainTex, i.uv);

				final.rgb = _MainColor * _BGColor;
				final.a = ((Back.r*_BackAlpha) + (Front.g*_FrontAlpha)) * Mask.b * _MainColor.a;
				
				// apply fog
//				UNITY_APPLY_FOG(i.fogCoord, final);

				return final;
			}
			ENDCG
		}
	}
}
