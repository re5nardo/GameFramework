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

Shader "Unlit/RK_BG_Fire(Transparent)"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BGColor("BG Color(Client Only)", Color)		= (1,1,1,1)
		_AddColor ("Add Color", Color) = (1,1,1,1)
		_MaskTex("Mask Texture", 2D) = "white" {}
		_Value("Lava Power",Range(0,1)) = 0
		_Y_Speed("Lava Y Speed", float) = 0

	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "Render Type" = "Transparent" }
	
		Lighting Off
		ZWrite Off
		Cull off
		Blend One One

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
//			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
//				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float2 uv_Mask : TEXCOORD1;
			};

			sampler2D _MainTex;
			sampler2D _MaskTex;
			fixed4 _BGColor;
			
			fixed3 _AddColor;
			fixed _Y_Speed;
			fixed _Value;
			fixed4 _MainTex_ST;
			fixed4 _MaskTex_ST;

			v2f vert (appdata v)
			{
				v2f o;
				#if UNITY_VERSION >= 540
				o.vertex = UnityObjectToClipPos(v.vertex);
				#else
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
				#endif
				
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv_Mask = TRANSFORM_TEX(v.uv, _MaskTex);

//				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 final;

				fixed4 mask = tex2D(_MaskTex, float2((i.uv_Mask.x), (i.uv_Mask.y + _Time.y *-_Y_Speed * 0.1)));

				half LavaValue = (mask.g) * _Value * mask.r;

				final = tex2D(_MainTex, i.uv+ LavaValue);
		
				final = fixed4(final.rgb * _AddColor.rgb,final.a);

				final = final * _BGColor;

				// apply fog
//				UNITY_APPLY_FOG(i.fogCoord, col);
				return final;
			}
			ENDCG
		}
	}

	FallBack "Diffuse"
}
