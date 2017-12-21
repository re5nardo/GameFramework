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

Shader "Shaders/TA/Character/Rk_Stone" {
	Properties {
		_AlbedoColor("Albedo Color", Color) = (1,1,1,1)
		_SColor("SpecColor", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Color("Cubemap Color", Color) = (1,1,1,1)
		_Cube("Cubemap", CUBE) = "" {}
		_Mask("CubeMask", 2D) = "white" {}
		_RimColor("Rim Color", Color) = (0.26,0.19,0.16,0.0)
		_RimPower("Rim Power", Range(0.5,8.0)) = 3.0

	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf SimpleSpecular

		fixed4 _SColor;
		fixed4 _AlbedoColor;

		half4 LightingSimpleSpecular(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {

			half diff = max(0, dot(s.Normal, viewDir)) * 0.5 + 0.5;

			half3 h = normalize(lightDir + viewDir);

			half nh = max(0, dot(s.Normal, h));
			half spec = pow(nh, 48.0);

			fixed4 SpecColor = spec * _SColor * s.Specular.r;

			half4 c;
			c.rgb = (s.Albedo * _LightColor0.rgb * diff + SpecColor) ;
			c.a = s.Alpha;
			//c.rgb = s.Albedo;
			return c;
		}

		
		struct Input {
			fixed2 uv_MainTex;
			fixed2 uv_Mask;
			float3 worldRefl;
			float3 viewDir;
		};

		sampler2D _MainTex;

		samplerCUBE _Cube;
		sampler2D _Mask;
		fixed4 _RimColor;
		float _RimPower;

		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutput o) {

			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _AlbedoColor;

			fixed4 m = tex2D(_Mask, IN.uv_Mask);
			float3 cube = texCUBE(_Cube, IN.worldRefl).rgb ;

			cube = cube * _Color;
			
			half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));

			o.Albedo = c.rgb;
			o.Emission = (cube * c) * m.g + (_RimColor.rgb * pow(rim, _RimPower) * m.b);
		//	o.Emission = 0.5f;
			o.Specular = m.r;

			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}
