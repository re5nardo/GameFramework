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

Shader "Shaders/TA/BackGround/RK_BG_Unlit_Simbol(Opaque)"{
Properties	{
		_DyeColor ("DyeColor", Color) = (1.0,1.0,1.0,1.0)

		_BGColor("BG Color(Client Only)", Color) = (1,1,1,1)
		_MainTex ("MainTex", 2D) = "white" {}

		[MaterialToggle(_ISDYETEX_ON)] _IsDyeTex("IsDyeTex", float) = 0
		_DyeTex ("DyeTex", 2D) = "white" {}

		_Rim ("Rim", Float) = 1.0

//		_DyeColor ("MainColor", Color) = (1.0,1.0,1.0,1.0)

		[MaterialToggle(_ISFIRSTMASK_ON)] _IsFirstMask("FirstMask", float) = 0
		_MainTex2 ("Frame_Tex", 2D) = "white" {}

		[MaterialToggle(_ISSECONDMASK_ON)] _IsSecondMask("SecondMask", float) = 0
		_MainTex3 ("Thumbnail_Tex", 2D) = "white" {}
	}



	Category {

	Tags {"Render Type" = "Opaque" }

//	Blend One OneMinusDstAlpha// 소스 알파값에 의해 곱해지며 (1-소스색상)에 의 곱해집니다.
//	Cull Off
	Lighting Off
//	ZWrite Off
//	fog{mode Off}

	SubShader{
			pass{

//			Blend One Zero
				CGPROGRAM
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma vertex vert
				#pragma fragment frag
				#pragma shader_feature _ISFIRSTMASK_ON
				#pragma shader_feature _ISSECONDMASK_ON
				#pragma shader_feature _ISDYETEX_ON
//				#pragma multi_compile_fog
				#include "UnityCG.cginc"
//				#include "TerrainEngine.cginc"
//				#include "RK_SHADER.cginc"

				fixed4 _DyeColor;
				fixed4 _BGColor;
				sampler2D _MainTex;
				sampler2D _DyeTex;
				uniform fixed _Rim;

//				uniform fixed _IsSecondMask;
//				uniform fixed _IsFirstMask;
//				uniform fixed _IsDyeTex;

				sampler2D _MainTex2;
				sampler2D _MainTex3;

				fixed4 _MainTex_ST;
				fixed4 _MainTex2_ST;
				fixed4 _MainTex3_ST;

				struct v2f {
					fixed4 pos : POSITION;
					fixed2 uv : TEXCOORD0;
					fixed2 uv2 : TEXCOORD1;
					fixed2 uv3 : TEXCOORD2;

					fixed4 color : COLOR;

//					UNITY_FOG_COORDS(5)
				};


				inline fixed3 GetGrayColor(fixed3 col)
				{
				//	float lightness = (max(c.r, c.g, c.b) + min(c.r, c.g, c.b)) * 0.5;
				//	c = float3(lightness, lightness, lightness);
				//	c.r = lightness;
				//	c.g = lightness;
				//	c.b = lightness;

					fixed average = ((col.r + col.g + col.b) / 3);
					col.r = average;
					col.g = average;
					col.b = average;
					
					return col;
				}

				v2f vert(appdata_full v){
					v2f o;
					o.color = v.color;
					
					#if UNITY_VERSION >= 540
					o.pos = UnityObjectToClipPos(v.vertex);
					#else
					o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
					#endif

					o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
					o.uv2 = TRANSFORM_TEX(v.texcoord, _MainTex2);
					o.uv3 = TRANSFORM_TEX(v.texcoord, _MainTex3);

//					UNITY_TRANSFER_FOG(o, o.vertex);

					return o;
				}

				fixed4 frag(v2f i) : COLOR {

					fixed4 MainTex_var = tex2D(_MainTex, i.uv);



					#if _ISDYETEX_ON
						fixed4 DyeTex_var = tex2D(_DyeTex, i.uv);
						MainTex_var.rgb = lerp(MainTex_var.rgb, GetGrayColor(MainTex_var.rgb), DyeTex_var.r);
						MainTex_var.rgb = lerp(MainTex_var.rgb, _DyeColor.rgb , DyeTex_var.r);
					#endif
						MainTex_var.rgb;



					fixed4 FinalTex = MainTex_var;

//					

					#if _ISFIRSTMASK_ON
						fixed4 MainTex_var2 = tex2D(_MainTex2, i.uv2);
						fixed4 useframe = MainTex_var2;
	                    FinalTex = lerp(FinalTex, MainTex_var2, useframe.a);
	                #endif
	                	FinalTex;
				       


				    #if _ISSECONDMASK_ON
				    	fixed4 MainTex_var3 = tex2D(_MainTex3, i.uv3);
	                    fixed4 usethumbnail = MainTex_var3;

	     				FinalTex = lerp(FinalTex, MainTex_var3, usethumbnail.a);
	     			#endif
	     				FinalTex;


					return FinalTex * _BGColor * _Rim;
				}
				ENDCG

			}//pass


//			pass{
//
//			Blend SrcAlpha OneMinusSrcAlpha
//
//				CGPROGRAM
////				#pragma fragmentoption ARB_precision_hint_fastest
//				#pragma vertex vert
//				#pragma fragment frag
//				#pragma multi_compile_fog
//				#include "UnityCG.cginc"
//				#include "TerrainEngine.cginc"
//
////				fixed4 _DyeColor;
//				sampler2D _MainTex2;
////				uniform fixed _Rim;
//
//				struct v2f {
//					fixed4 pos : SV_POSITION;
//					fixed2 texcoord1 : TEXCOORD1;
//					fixed4 color : COLOR;
//
//					UNITY_FOG_COORDS(5)
//				};
//
//				fixed4 _MainTex2_ST;
//
//				v2f vert(appdata_base v){
//					v2f o;
//
//					#if UNITY_VERSION >= 540
//					o.pos = UnityObjectToClipPos(v.vertex);
//					#else
//					o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
//					#endif
//
//					o.texcoord1 = TRANSFORM_TEX(v.texcoord, _MainTex2);
//
//					UNITY_TRANSFER_FOG(o, o.vertex);
//
//					return o;
//				}
//
//				fixed4 frag(v2f i) : COLOR {
//					fixed4 FinalTex2 = tex2D(_MainTex2, i.texcoord1);
//					UNITY_APPLY_FOG(i.fogCoord, FinalTex);
//
//					return fixed4(FinalTex2.rgb , FinalTex2.a * i.color.a);
//				}
//				ENDCG
//
//			}//pass

		}//subshader


	}//category

	Fallback "Diffuse"

}
