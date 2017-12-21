
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

Shader "Shaders/TA/Character/RK_CharacterShader_Outline" 
{
Properties
	{
		//TOONY COLORS

		_Color ("Color", Color) = (1.0,1.0,1.0,1.0)
		_Plus ("Plus", Color) = (0.0,0.0,0.0,1.0)
		_HColor ("Highlight Color", Color) = (0.36,0.36,0.36,1.0)
		_SColor ("Shadow Color", Color) = (0.1,0.06,0.06,1.0)
		_ConditionColor ("Condition Color", Color) = (1.0,1.0,1.0,1.0)

		//DIFFUSE
		_MainTex ("Main Texture (RGB)", 2D) = "white" {}
		_Mask1 ("Mask 1 (Specular)", 2D) = "black" {}
		_ConditionMask ("Condition Texture", 2D) = "black" {}
		[MaterialToggle(_ISCONDITION_ON)] _IsCondition("[_Condition_]", float) = 0

		//TOONY COLORS RAMP
		_RampThreshold ("#RAMPF# Ramp Threshold", Range(0,1)) = 0.45
		_RampSmooth ("#RAMPF# Ramp Smoothing", Range(0.001,1)) = 0.15
		
		//SPECULAR
		_SpecColor ("#SPEC# Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess ("#SPEC# Shininess", Range(0.0,2)) = 0.8

		//Glow
		[MaterialToggle(_ISUVSCROLL_ON)] _IsUvScroll("[_UvScroll_]", float) = 0
		_GlowTex("Glow UV Texture(RGB) ", 2D) = "white" {}
		_GlowMask ("Glow Mask Texture(Rim)", 2D) = "black" {}
		_GlowTexIntensity("GlowTexIntensity", Vector) = (1,1,1,0)

		//Scroll
		_ScrollXSpeed("X Scroll Speed", float) = 0
		_ScrollYSpeed("Y Scroll Speed", float) = 0

		//Rim Color
		[MaterialToggle(_ISRIM_ON)] _IsRim("[_Rim_]", float) = 0
		_RimColor ("Rim Color", Color) = (0,0,0,0)
		_RimPower ("Rim Power", Range(0,1)) = 1


		[MaterialToggle(_ISDYETEX_ON)] _IsDyeTex("[_DyeTex_]", float) = 0
		_DyeTex ("DyeTex", 2D) = "white" {}
		_DyeColor ("DyeColor", Color) = (1.0,1.0,1.0,1.0)


		_IsDeath ("[Death Color]", Range(0,1)) = 0.7
	  	_Death ("_Death", Range(-0.001,1)) = -0.001


		[MaterialToggle(_ISOUTLINE_ON)] _IsOutLine	("[_Out Line_]", Float)					= 0
	  	_OutLineColor					("Out Line Color", Color) 		= (1.0,1.0,1.0,1.0)
	  	_OutLineAmount				("Out Line Amount", float)			= 0.0015
	}


	CGINCLUDE
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma shader_feature _ISOUTLINE_ON

		uniform fixed	_IsOutLine;
		uniform fixed4	_OutLineColor;
		uniform fixed	_OutLineAmount;


		struct v2f
		{
			fixed4 pos : SV_POSITION;
			fixed4 color : COLOR;
		};

		v2f vert(appdata_full v)
		{
			

			v2f o;
			UNITY_INITIALIZE_OUTPUT(v2f,o);
            
			#if UNITY_VERSION >= 540
			o.pos = UnityObjectToClipPos(v.vertex);
			#else
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
			#endif
		
			#if _ISOUTLINE_ON
			v.normal = mul((half3x3)UNITY_MATRIX_MVP, v.normal);
			o.pos.xy += (v.normal.xy * (o.pos.z) * _OutLineAmount);
			o.pos.z += 0.01 * _OutLineAmount;
			o.color = (v.color * _OutLineColor);

			#endif
			return o;
		}

		ENDCG


	//=====================================================================================
	SubShader
	{
		LOD 600
		Tags {"Queue"="Geometry-2" "IgnoreProjector"="True" "RenderType"="Transparent"}
		Blend One OneMinusSrcAlpha


		Pass
		{
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
			Cull back
			Blend SrcColor OneMinusSrcColor
			offset 15,15
			ZTEST Always

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			fixed4 frag( v2f i ) : COLOR
			{
				fixed4 c = i.color;
				return c;
			}
			ENDCG
		}


			CGPROGRAM
			
			#pragma surface surf ToonyColorsCustom noforwardadd keepalpha
			#pragma target 3.0
			#pragma glsl
			#pragma shader_feature _ISRIM_ON
			#pragma shader_feature _ISCONDITION_ON
			#pragma shader_feature _ISUVSCROLL_ON
			#pragma shader_feature _ISDYETEX_ON

			
			//================================================================
			// VARIABLES
			
			uniform fixed4 _Color;
			uniform fixed3 _Plus;
			uniform sampler2D _MainTex;
			uniform sampler2D _Mask1;
			uniform sampler2D _ConditionMask;
			uniform sampler2D _GlowTex;
			uniform sampler2D _GlowMask;
			uniform sampler2D _DyeTex;
			uniform fixed3 _DyeColor;


			uniform fixed3 _ConditionColor;

			uniform fixed _Shininess;
			uniform fixed3 _GlowTexIntensity;

			uniform fixed3 _RimColor;
			uniform fixed _RimPower;
			uniform fixed _Death;

			uniform fixed	_IsDeath;

			uniform fixed _ScrollXSpeed;
			uniform fixed _ScrollYSpeed;


			struct Input
			{
				half2 uv_MainTex;
				half2 uv_GlowTex;
				half3 viewDir;

			};


			inline fixed3 GetGrayColor(fixed3 col)
			{
				fixed average = ((col.r + col.g + col.b) * 0.33);
				col.r = average;
				col.g = average;
				col.b = average;
				
				return col;
			}
			
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
				c.a = s.Alpha + _LightColor0.a * _SpecColor.a * spec;
				return c;
			}
			
			
			//================================================================
			// SURFACE FUNCTION
			
			void surf (Input IN, inout SurfaceOutputCustom o)
			{
				fixed4 mainTex = tex2D(_MainTex, IN.uv_MainTex);

				clip((mainTex.a * ceil((mainTex.b -_Death))) - 0.5);		     	

				fixed3 mask1 = tex2D(_Mask1, IN.uv_MainTex);

				half3 finalColor = mainTex.rgb * _Color.rgb + _Plus;


				//condition
				#if _ISCONDITION_ON
					fixed3 conditionTex = tex2D(_ConditionMask, IN.uv_MainTex);
					finalColor = ((finalColor.r + finalColor.g + finalColor.b) * 0.3 + conditionTex.r) + _ConditionColor;
				#endif
					finalColor;


				//Glow Scroll
				#if _ISUVSCROLL_ON

					fixed2 scrolledUV = IN.uv_MainTex;
					fixed xScrollValue = -_ScrollXSpeed * _Time;// * __IsUvScroll;
					fixed yScrollValue = -_ScrollYSpeed * _Time;// * __IsUvScroll;
					scrolledUV += fixed2(xScrollValue, yScrollValue); 
					fixed3 glowmaskl = tex2D(_GlowMask, IN.uv_MainTex);
					half3 glowTex = tex2D(_GlowTex, IN.uv_GlowTex + scrolledUV) * tex2D(_GlowMask, IN.uv_MainTex);

					finalColor = finalColor * _Color.rgb +_Color.rgb * (glowTex.rgb * _GlowTexIntensity.rgb);// * __IsUvScroll;

				#endif
					finalColor;

				
				//Rim
				fixed3 rimColor = 0;
								
				#if _ISRIM_ON

					fixed rim = (1.0 - saturate(dot(normalize(IN.viewDir), o.Normal)));

					rimColor = smoothstep(1 - _RimPower , 1, rim);

					rimColor *= _RimColor.rgb;

					rimColor = rimColor * pow(rim, _RimPower);
				#endif 
					rimColor;// * pow(1, _RimPower);


				#if _ISDYETEX_ON
					fixed3 DyeTex_var = tex2D(_DyeTex, IN.uv_MainTex);
					finalColor = lerp(finalColor, GetGrayColor(mainTex.rgb), DyeTex_var.r);
		     		half3 DyeColor = lerp(finalColor, _DyeColor.rgb , DyeTex_var.r);     	

                	finalColor = DyeColor;
				#endif
					finalColor;


				//Final Ouput
				o.Albedo = lerp(finalColor,(finalColor*_IsDeath),(_Death*3));;
				o.Emission = rimColor;
				o.Alpha = mainTex.a * _Color.a;

				//Specular
				o.Gloss = mask1.r;
				o.Specular = _Shininess;
			}
			
			ENDCG
//		}
	}

	SubShader
	{
		LOD 400
		Tags {"Queue"="Geometry-2" "IgnoreProjector"="True" "RenderType"="Transparent"}
		Blend One OneMinusSrcAlpha

		Pass
		{
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
			Cull back
			Blend SrcColor OneMinusSrcColor
			offset 15,15
			ZTEST Always

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			fixed4 frag( v2f i ) : COLOR
			{
				fixed4 c = i.color;
				return c;
			}
			ENDCG
		}


			CGPROGRAM
			
			#pragma surface surf ToonyColorsCustom noforwardadd keepalpha
			#pragma target 3.0
			#pragma glsl
			#pragma shader_feature _ISRIM_ON
			#pragma shader_feature _ISCONDITION_ON


			
			//================================================================
			// VARIABLES
			
			uniform fixed4 _Color;
			uniform fixed3 _Plus;
			uniform sampler2D _MainTex;
			uniform sampler2D _Mask1;
			uniform sampler2D _ConditionMask;
			uniform fixed3 _ConditionColor;

			uniform fixed _Shininess;

			uniform fixed3 _RimColor;
			uniform fixed _RimPower;
			uniform fixed _Death;
			uniform fixed	_IsDeath;



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

				clip((mainTex.a * ceil((mainTex.b -_Death))) - 0.5);		     	

                half3 finalColor = lerp(mainTex.rgb,(mainTex.rgb*_IsDeath),(_Death*3));

				fixed3 mask1 = tex2D(_Mask1, IN.uv_MainTex);

				finalColor = finalColor * _Color.rgb + _Plus;


				//condition
				#if _ISCONDITION_ON
					fixed3 conditionTex = tex2D(_ConditionMask, IN.uv_MainTex);
					finalColor = ((finalColor.r + finalColor.g + finalColor.b) * 0.3 + conditionTex.r) + _ConditionColor;
				#endif
					finalColor;
				
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
				o.Alpha = mainTex.a * _Color.a;

				//Specular
				o.Gloss = mask1.r;
				o.Specular = _Shininess;
				
			}
			
			ENDCG
	}

	SubShader
	{
		LOD 200
		Tags {"Queue"="Geometry-2" "IgnoreProjector"="True" "RenderType"="Transparent"}
		Lighting off

		Blend One OneMinusSrcAlpha


		Pass
		{
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
			Cull back
			Blend SrcColor OneMinusSrcColor
			offset 15,15
			ZTEST Always

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			fixed4 frag( v2f i ) : COLOR
			{
				fixed4 c = i.color;
				return c;
			}
			ENDCG
		}



		CGPROGRAM
		
		#pragma surface surf NoLighting noambient noforwardadd keepalpha
		#pragma shader_feature _ISRIM_ON
		#pragma shader_feature _ISCONDITION_ON


		//================================================================
		// VARIABLES
		
		uniform fixed4 _Color;
		uniform fixed3 _Plus;
		uniform sampler2D _MainTex;

		uniform fixed _Death;

		uniform sampler2D _ConditionMask;
		uniform fixed3 _ConditionColor;

		uniform fixed3 _RimColor;
		uniform fixed _RimPower;

		struct Input
		{
			half2 uv_MainTex;
			half3 viewDir;
		};

		struct SurfaceOutputCustom
		{
			fixed3 Albedo;
			fixed3 Normal;
			fixed3 Emission;
			fixed Alpha;
		} ;
		

		half4 LightingNoLighting (SurfaceOutput s, half3 lightDir, half atten)
		{
			return half4(s.Albedo, s.Alpha);
		}
		
		
		//================================================================
		// SURFACE FUNCTION
		
		void surf (Input IN, inout SurfaceOutput o)
		{
			fixed4 mainTex = tex2D(_MainTex, IN.uv_MainTex);


			clip(mainTex.a *  ceil(-_Death) - 0.5);

			fixed3 finalcolor = mainTex.rgb;

			fixed3 conditioncolor = mainTex.rgb;;

			#if _ISCONDITION_ON
				fixed3 conditionTex = tex2D(_ConditionMask, IN.uv_MainTex);
				half3 d = ((finalcolor.r + finalcolor.g + finalcolor.b) * 0.3 + conditionTex.r) + _ConditionColor;
				conditioncolor = lerp(d, finalcolor,   _ConditionColor.rgb);
			#endif
				conditioncolor;
				
				
			
			#if _ISRIM_ON
				fixed rim = (1.0 - saturate(dot(normalize(IN.viewDir), o.Normal)));
				finalcolor = (mainTex.rgb * _Color.rgb + 1)  * (_RimColor.rgb * pow(rim*3, 1.5 - _RimPower));

			#endif
				finalcolor;

			o.Emission = lerp(finalcolor.rgb + _Plus , conditioncolor.rgb + _Plus, 0.8);
			o.Alpha = mainTex.a * _Color.a;
		}
			
		ENDCG
	}	
	FallBack "Mobile/VertexLit"
}