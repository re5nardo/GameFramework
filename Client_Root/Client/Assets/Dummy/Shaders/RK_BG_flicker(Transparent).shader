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

Shader "Shaders/TA/BackGround/RK_BG_flicker(Transparent)"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BGColor("BG Color(Client Only)", Color) = (1,1,1,1)
     	_MainColor("MainColor", Color) = (1.0,1.0,1.0,1.0)
		_Rim("Rim", Float) = 1.0
		_Speed("Scroll Speed", Float) = 1.0

	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "Render Type" = "Transparent" }
	
		Lighting Off
		ZWrite Off

		Blend One OneMinusSrcAlpha
		//Blend SrcAlpha OneMinusSrcAlpha
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"


			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};


			sampler2D _MainTex;
			fixed4 _MainTex_ST;
			fixed4 _MainColor;
			fixed4 _BGColor;
			uniform fixed _Rim;
			fixed _Speed;

			v2f vert (appdata v)
			{
				v2f o;
				#if UNITY_VERSION >= 540
				o.vertex = UnityObjectToClipPos(v.vertex);
				#else
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
				#endif
				
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
//				o.uv2 = TRANSFORM_TEX(v.uv2, _MainTex);

				o.color = v.color;

				return o;
			}
			
			fixed4 frag (v2f IN) : COLOR
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, IN.uv) * IN.color;

				fixed3 emissive = col *_MainColor  * _BGColor;

				fixed time = sin(_Speed * _Time.z);

				return half4(emissive * time * _Rim , 0);

//				return fixed4(emissive,col.r * _MainColor.a);
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}
