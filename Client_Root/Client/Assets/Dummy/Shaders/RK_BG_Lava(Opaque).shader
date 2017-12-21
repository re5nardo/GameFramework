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

Shader "Shaders/TA/BackGround/RK_BG_Lava(Opaque)"
{
	Properties
	{
		_MainTex ("MainTex", 2D) = "white" {}
		_BGColor("BG Color(Client Only)", Color)		= (1,1,1,1)
		_LavaMaskTex("Mask",2D) = "white" {}
		_LavaValue("Lava Power",Range(0,1)) = 0
		_Lava_X_Speed("Lava X Speed", float ) = 0
		_Lava_Y_Speed("Lava Y Speed", float) = 0
		_OverlayPow("Overlay Power",Range(1,10)) = 1
	}
		
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 600

		Pass
		{

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv_LavaMaskTex : TEXCOORD1;
				
				float4 vertex : SV_POSITION;
				UNITY_FOG_COORDS(5)
			};

			fixed4 _BGColor;
			fixed _LavaValue;
			fixed _Lava_X_Speed;
			fixed _Lava_Y_Speed;
			fixed _OverlayPow;

			sampler2D _MainTex;
			sampler2D _LavaMaskTex;

			fixed4 _MainTex_ST;
			fixed4 _LavaMaskTex_ST;

			v2f vert (appdata v)
			{
				v2f o;

				#if UNITY_VERSION >= 540
				o.vertex = UnityObjectToClipPos(v.vertex);
				#else
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
				#endif

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.uv_LavaMaskTex = TRANSFORM_TEX(v.uv, _LavaMaskTex);

				UNITY_TRANSFER_FOG(o,o.vertex);

				return o;
			}


			fixed4 frag (v2f i) : SV_Target

			{

				// sample the texture
				fixed4 mask = tex2D(_LavaMaskTex,i.uv_LavaMaskTex);

				fixed4 mask_1 = tex2D(_LavaMaskTex, float2((i.uv_LavaMaskTex.x + _Time.y *_Lava_X_Speed * 0.01), (i.uv_LavaMaskTex.y + _Time.y *_Lava_Y_Speed * 0.01)));
				
				half LavaValue = (mask_1.g) * _LavaValue * mask.r ;

				fixed4 col = tex2D(_MainTex, i.uv+LavaValue) ;

				col = col *  _BGColor;

				fixed dgray = mask.b;

				col = lerp(saturate(1 - ((1 - col)*(1 - mask.b) * 2))*pow(_OverlayPow,-1), saturate(col*mask.b * 2)*_OverlayPow, dgray);

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);

				return col;
			}
			ENDCG
		}
	}


	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 400

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv_LavaMaskTex : TEXCOORD1;

				float4 vertex : SV_POSITION;
				UNITY_FOG_COORDS(5)
			};

			fixed4 _BGColor;
			fixed _LavaValue;
			fixed _Lava_X_Speed;
			fixed _Lava_Y_Speed;

			sampler2D _MainTex;
			sampler2D _LavaMaskTex;

			fixed4 _MainTex_ST;
			fixed4 _LavaMaskTex_ST;

			v2f vert(appdata v)
			{
				v2f o;

				#if UNITY_VERSION >= 540
				o.vertex = UnityObjectToClipPos(v.vertex);
				#else
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
				#endif

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.uv_LavaMaskTex = TRANSFORM_TEX(v.uv, _LavaMaskTex);

				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				fixed4 mask = tex2D(_LavaMaskTex,i.uv_LavaMaskTex);

				fixed4 mask_1 = tex2D(_LavaMaskTex, float2((i.uv_LavaMaskTex.x + _Time.y *_Lava_X_Speed * 0.01), (i.uv_LavaMaskTex.y + _Time.y *_Lava_Y_Speed * 0.01)));
				
				half LavaValue = (mask_1.g) * _LavaValue * mask.r;

				fixed4 col = tex2D(_MainTex, i.uv + LavaValue);

				col = col * _BGColor;

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);

				return col;
			}
			ENDCG
		}
	}
	
	
	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv_LavaMaskTex : TEXCOORD1;

				float4 vertex : SV_POSITION;
				UNITY_FOG_COORDS(5)
			};
			sampler2D _MainTex;
			sampler2D _LavaMaskTex;

			fixed4 _BGColor;
			fixed4 _MainTex_ST;
			fixed4 _LavaMaskTex_ST;

			fixed _LavaValue;
//			fixed _Lava_X_Speed;
//			fixed _Lava_Y_Speed;

			v2f vert(appdata v)
			{
				v2f o;

				#if UNITY_VERSION >= 540
				o.vertex = UnityObjectToClipPos(v.vertex);
				#else
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
				#endif

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.uv_LavaMaskTex = TRANSFORM_TEX(v.uv, _LavaMaskTex);

				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{

				fixed4 mask = tex2D(_LavaMaskTex,i.uv_LavaMaskTex);

//				fixed4 mask_1 = tex2D(_LavaMaskTex, float2((i.uv_LavaMaskTex.x + _Time.x *_Lava_X_Speed), (i.uv_LavaMaskTex.y + _Time.x *_Lava_X_Speed)));
				
				half LavaValue = (mask.g) * _LavaValue * mask.r;

				fixed4 col = tex2D(_MainTex, i.uv + LavaValue);

				col = col*_BGColor;

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);

				return col;
			}
				ENDCG
		}
	}

	Fallback "Diffuse"
		
}
