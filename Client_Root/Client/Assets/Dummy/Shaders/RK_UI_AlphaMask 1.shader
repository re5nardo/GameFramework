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

Shader "UI/AlphaMask 1"
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
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _MaskTex;
			float4 _MainTex_ST;
			float _CutOff;
			float4 _ClipRange0 = float4(0.0, 0.0, 1.0, 1.0);
			float2 _ClipArgs0 = float2(1000.0, 1000.0);

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
				float2 worldPos : TEXCOORD1;
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
				o.worldPos = v.vertex.xy * _ClipRange0.zw + _ClipRange0.xy;
				return o;
			}
				
			half4 frag (v2f IN) : COLOR
			{
				// Softness factor
				float2 factor = (float2(1.0, 1.0) - abs(IN.worldPos)) * _ClipArgs0;

				half4 col;
				col = tex2D(_MainTex, IN.texcoord) * IN.color;
				col.a = col.a - ((1 - tex2D(_MaskTex, TRANSFORM_TEX(IN.texcoord, _MainTex)).a) * _CutOff);

				col.a *= clamp(min(factor.x, factor.y), 0.0, 1.0);
				return col;
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
