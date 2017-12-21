/*******************************************************************************

Copyright (C) [2017] NCSOFT Corporation. All Rights Reserved.

This software is provided 'as-is', without any express or implied warranty.
In no event will NCSOFT Corporation (��NCSOFT��) be held liable for any damages
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

Shader "UI/AlphaMask"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
		_MaskTex("Mask Texture (RGB)", 2D) = "white" {}
		_CutOff("_CutOff", Range(0,5)) = 1
	}
	
	SubShader
	{
		LOD 200

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _MaskTex;
			float4 _MainTex_ST;
			float _CutOff;

			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				half4 color : COLOR;
			};
	
			struct v2f
			{
				float4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
				half4 color : COLOR;
			};
	
			v2f o;

			v2f vert (appdata_t v)
			{
				#if UNITY_VERSION >= 540
				o.vertex = UnityObjectToClipPos(v.vertex);
				#else
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
				#endif

				o.texcoord = v.texcoord;
				o.color = v.color;
				return o;
			}
				
			half4 frag (v2f IN) : COLOR
			{
				half4 color;
				color = tex2D(_MainTex, IN.texcoord) * IN.color;
				color.a = color.a - ((1 - tex2D(_MaskTex, TRANSFORM_TEX(IN.texcoord, _MainTex)).a) * _CutOff);
				return color;
			}
			ENDCG
		}
	}

	SubShader
	{
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse
			
			SetTexture [_MainTex]
			{
				Combine Texture * Primary
			}
		}
	}
}
