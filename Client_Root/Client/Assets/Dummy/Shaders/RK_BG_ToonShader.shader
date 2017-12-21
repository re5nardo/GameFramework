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

Shader "Shaders/TA/BackGround/RK_BG_ToonShader" 
{
Properties
	{
		//TOONY COLORS
		_MainColor ("Color", Color) = (1.0,1.0,1.0,1.0)
		_BGColor("BG Color(Client Only)", Color) = (1,1,1,1)
		_Plus ("Plus", Color) = (0.0,0.0,0.0,1.0)
		_HColor ("Highlight Color", Color) = (0.36,0.36,0.36,1.0)
		_SColor ("Shadow Color", Color) = (0.3,0.3,0.3,1.0)
		
		//DIFFUSE
		_MainTex ("Main Texture (RGB)", 2D) = "white" {}
		_Mask1 ("Mask 1 (Specular)", 2D) = "black" {}
		
		//TOONY COLORS RAMP
		_RampThreshold ("#RAMPF# Ramp Threshold", Range(0,1)) = 0.5
		_RampSmooth ("#RAMPF# Ramp Smoothing", Range(0.001,1)) = 0.1
		
		//SPECULAR
		_SpecColor ("#SPEC# Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess ("#SPEC# Shininess", Range(0.0,2)) = 0.1

		//Rim Color
		[MaterialToggle(_ISRIM_ON)] _IsRim("Rim", float) = 0
		_RimColor ("Rim Color", Color) = (0,0,0,0)
		_RimPower ("Rim Power", float) = 1

	}

		CGINCLUDE
		#include "UnityCG.cginc"
		#pragma target 3.0


		ENDCG

	//=====================================================================================
	SubShader
	{
		LOD 600
		Tags {"RenderType"="Opaque"}
//		Blend [_SrcBlend] [_DstBlend]


//		{
			CGPROGRAM
			
			#pragma surface surf ToonyColorsCustom noforwardadd keepalpha
			#pragma target 3.0
			#pragma glsl
			#pragma shader_feature _ISRIM_ON


			
			//================================================================
			// VARIABLES
			
			uniform fixed4 _MainColor;
			uniform fixed3 _BGColor;
			uniform fixed3 _Plus;
			uniform sampler2D _MainTex;
			uniform sampler2D _Mask1;

			uniform fixed _Shininess;

//			uniform fixed _IsRim;
			uniform fixed3 _RimColor;
			uniform fixed _RimPower;


			struct Input
			{
				half2 uv_MainTex;
				half3 viewDir;

			};
			
			//================================================================
			// CUSTOM LIGHTING
			
			//Lighting-related variables
			uniform fixed4 _HColor;
			uniform fixed4 _SColor;
			uniform fixed _RampThreshold;
			uniform fixed _RampSmooth;
			
			//Custom SurfaceOutput
			struct SurfaceOutputCustom
			{
				fixed3 Albedo;
				fixed3 Normal;
				fixed3 Emission;
				half Specular;
				fixed Gloss;
				fixed Alpha;
			};
			
			inline half4 LightingToonyColorsCustom (SurfaceOutputCustom s, half3 lightDir, half3 viewDir, half atten)
			{
				s.Normal = normalize(s.Normal);
				fixed ndl = max(0, dot(s.Normal, lightDir)*0.5 + 0.5);
				
				fixed3 ramp = smoothstep(_RampThreshold-_RampSmooth*0.5, _RampThreshold+_RampSmooth*0.5, ndl);


				_SColor = lerp(_HColor, _SColor, _SColor.a);	//Shadows intensity through alpha
				ramp = lerp(_SColor.rgb,_HColor.rgb,ramp);
				
				//Specular
				half3 h = normalize(lightDir + viewDir);
				fixed ndh = max(0, dot (s.Normal, h));
				fixed spec = pow(ndh, s.Specular*128.0) * s.Gloss * 2.0;
				spec *= atten;
				fixed4 c;
				c.rgb = s.Albedo * _LightColor0.rgb * ramp;

				c.rgb *= 2;
				c.rgb += _LightColor0.rgb * _SpecColor.rgb * spec;
				c.a = s.Alpha;
				return c;
			}
			
			
			//================================================================
			// SURFACE FUNCTION
			
			void surf (Input IN, inout SurfaceOutputCustom o)
			{
				fixed4 mainTex = tex2D(_MainTex, IN.uv_MainTex);

				fixed3 mask1 = tex2D(_Mask1, IN.uv_MainTex);

				half3 finalColor =  mainTex.rgb * _MainColor.rgb * _BGColor.rgb + _Plus;

				//Rim
				fixed3 rimColor = 0;
								
				#if _ISRIM_ON

					fixed rim = (1.0 - saturate(dot(normalize(IN.viewDir), o.Normal)));

					rimColor = smoothstep(1 - _RimPower , 1, rim);

					rimColor *= _RimColor.rgb;

					rimColor = rimColor * pow(rim, _RimPower);
				#endif 
					rimColor;// * pow(1, _RimPower);


				//Final Ouput
				o.Albedo = finalColor;
				o.Emission = rimColor;
				o.Alpha = mainTex.a * _MainColor.a;

				//Specular
				o.Gloss = mask1.r;
				o.Specular = _Shininess;
			}
			
			ENDCG
//		}
	}


	SubShader
	{
		LOD 200
		Tags {"RenderType"="Opaque"}
		Lighting off

			CGPROGRAM
			
			#pragma surface surf ToonyColorsCustom noambient noforwardadd keepalpha
			#pragma target 3.0
			#pragma shader_feature _ISRIM_ON
			//#pragma glsl
			



			//================================================================
			// VARIABLES
			
			uniform fixed4 _MainColor;
			uniform fixed3 _BGColor;
			uniform fixed3 _Plus;
			uniform sampler2D _MainTex;

//			uniform fixed _IsRim;
			uniform fixed4 _RimColor;
			uniform fixed _RimPower;


			struct Input
			{
				half2 uv_MainTex;
				half3 viewDir;
			};
			
			//================================================================
			// CUSTOM LIGHTING

			fixed4 _HColor;

			struct SurfaceOutputCustom
			{
				fixed3 Albedo;
				fixed3 Normal;
				fixed3 Emission;
				fixed Alpha;
			};
			
			inline half4 LightingToonyColorsCustom (SurfaceOutputCustom s, half3 lightDir, half atten)
			{
				return half4(s.Albedo, s.Alpha);
			}
			
			
			//================================================================
			// SURFACE FUNCTION
			
			void surf (Input IN, inout SurfaceOutputCustom o)
			{
				fixed4 mainTex = tex2D(_MainTex, IN.uv_MainTex);

				fixed3 finalcolor = mainTex.rgb * _MainColor.rgb * _BGColor.rgb + _Plus;

//				#if _ISRIM_ON
//					fixed rim = (1.0 - saturate(dot(normalize(IN.viewDir), o.Normal)));
//					finalcolor = (mainTex.rgb * _MainColor.rgb + 1)  * (_RimColor.rgb * pow(rim*3, 1.5 - _RimPower));
//
//				#endif
//					finalcolor;

				o.Emission = finalcolor;
				o.Alpha = mainTex.a * _MainColor.a;
			}			
			ENDCG
//		}
	}
	
	Fallback "Diffuse"
}