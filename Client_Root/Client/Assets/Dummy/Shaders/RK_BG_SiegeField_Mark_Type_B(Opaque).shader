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

Shader "Shaders/TA/BackGround/RK_BG_SiegeField_Mark_Type_B(Opaque)"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MaskTex("Mask Texture", 2D) = "white" {}
		_BGColor("BG Color(Client Only)", Color) = (1,1,1,1)
     	_GlowColor("Glow Color", Color) = (1.0,1.0,1.0,1.0)
		_MainColor("Main Color", Color) = (1.0,1.0,1.0,1.0)
		_Rim("Rim", Float) = 1.0
		_Speed("Scroll Speed", Float) = 1.0
	}
	SubShader
	{
		LOD 600
	
		Tags{ "Queue" = "Transparent" "Render Type" = "Opaque" }

		Lighting Off
		ZWrite Off

		
		//Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			ColorMask 0
		}
		Pass
		{
			Blend One One
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			fixed4 _MainTex_ST;
			fixed4 _GlowColor;
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
				o.uv2 = TRANSFORM_TEX(v.uv2, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 mask = tex2D(_MainTex, float2(i.uv2.x + _Time.y * _Speed,i.uv2.y));
				return col*_GlowColor * mask * _GlowColor * _Rim * _BGColor;
			}
			ENDCG
		}
			
		Pass
		{

			Lighting Off
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			fixed4 _MainTex_ST;
			sampler2D _MaskTex;
			fixed4 _MaskTex_ST;
			fixed4 _BGColor;
			fixed4 _MainColor;


			uniform fixed _Rim;
			fixed _Speed;


			v2f vert(appdata v)
			{
				v2f o;

				#if UNITY_VERSION >= 540
				o.vertex = UnityObjectToClipPos(v.vertex);
				#else
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
				#endif
				
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv2 = TRANSFORM_TEX(v.uv2, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MaskTex, i.uv);
				fixed4 mask = tex2D(_MaskTex, float2(i.uv2.x + _Time.y * _Speed,i.uv2.y));

				return col*_MainColor * mask * _MainColor * _Rim * _BGColor;
			}
				ENDCG
		}
		
	}
	
	SubShader
	{
		LOD 400
		Tags{ "Queue" = "Transparent" "Render Type" = "Opaque" }

		Lighting Off
		ZWrite Off

			Pass
		{
			ColorMask 0
		}
		//Blend SrcAlpha OneMinusSrcAlpha
		Pass
		{
			Blend One One
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"

		struct appdata
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
			float2 uv2 : TEXCOORD1;
		};

		struct v2f
		{
			float2 uv : TEXCOORD0;
			float2 uv2 : TEXCOORD1;
			float4 vertex : SV_POSITION;
		};

		sampler2D _MainTex;
		fixed4 _MainTex_ST;
		fixed4 _GlowColor;
		fixed4 _BGColor;

		uniform fixed _Rim;
		fixed _Speed;

		v2f vert(appdata v)
		{
			v2f o;
                
			#if UNITY_VERSION >= 540
			o.vertex = UnityObjectToClipPos(v.vertex);
			#else
			o.vertex = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
			#endif

			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			o.uv2 = TRANSFORM_TEX(v.uv2, _MainTex);
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			fixed4 col = tex2D(_MainTex, i.uv);
			fixed4 mask = tex2D(_MainTex, float2(_Time.y * _Speed,i.uv2.y));
			return col*_GlowColor * mask * _GlowColor * _Rim * _BGColor;
		}
			ENDCG
		}

		//Pass2
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

		struct appdata
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
			float2 uv2 : TEXCOORD1;
		};

		struct v2f
		{
			float2 uv : TEXCOORD0;
			float2 uv2 : TEXCOORD1;
			float4 vertex : SV_POSITION;
		};

		sampler2D _MainTex;
		fixed4 _MainTex_ST;
		sampler2D _MaskTex;
		fixed4 _MaskTex_ST;
		fixed4 _BGColor;
		fixed4 _MainColor;


		uniform fixed _Rim;
		fixed _Speed;


		v2f vert(appdata v)
		{
			v2f o;

			#if UNITY_VERSION >= 540
			o.vertex = UnityObjectToClipPos(v.vertex);
			#else
			o.vertex = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
			#endif

			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			o.uv2 = TRANSFORM_TEX(v.uv2, _MainTex);
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			// sample the texture
			fixed4 col = tex2D(_MaskTex, i.uv);
			fixed4 mask = tex2D(_MaskTex, float2(_Time.y * _Speed,i.uv2.y));

			return col*_MainColor * mask * _MainColor * _Rim * _BGColor;
		}
			ENDCG
		}
	}
	SubShader
	{
		LOD 200
		Tags{ "Queue" = "Transparent" "Render Type" = "Opaque" }

		Lighting Off
		ZWrite Off

			Pass
		{
			ColorMask 0
		}
		//Blend SrcAlpha OneMinusSrcAlpha
		Pass
	{
		Blend One One
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

	struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
		float2 uv2 : TEXCOORD1;
	};

	struct v2f
	{
		float2 uv : TEXCOORD0;
		float2 uv2 : TEXCOORD1;
		float4 vertex : SV_POSITION;
	};

	sampler2D _MainTex;
	fixed4 _MainTex_ST;
	fixed4 _GlowColor;
	fixed4 _BGColor;

	uniform fixed _Rim;
	fixed _Speed;

	v2f vert(appdata v)
	{
		v2f o;
			
		#if UNITY_VERSION >= 540
		o.vertex = UnityObjectToClipPos(v.vertex);
		#else
		o.vertex = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
		#endif

		o.uv = TRANSFORM_TEX(v.uv, _MainTex);
		o.uv2 = TRANSFORM_TEX(v.uv2, _MainTex);
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 col = tex2D(_MainTex, i.uv);
	fixed4 mask = tex2D(_MainTex, float2(_Time.y * _Speed,i.uv2.y));
	return col*_GlowColor * mask * _GlowColor * _Rim * _BGColor;
	}
		ENDCG
	}

		//Pass2
		Pass
	{
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

	struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
		float2 uv2 : TEXCOORD1;
	};

	struct v2f
	{
		float2 uv : TEXCOORD0;
		float2 uv2 : TEXCOORD1;
		float4 vertex : SV_POSITION;
	};

	sampler2D _MainTex;
	fixed4 _MainTex_ST;
	sampler2D _MaskTex;
	fixed4 _MaskTex_ST;
	fixed4 _BGColor;
	fixed4 _MainColor;


	uniform fixed _Rim;
	fixed _Speed;


	v2f vert(appdata v)
	{
		v2f o;

		#if UNITY_VERSION >= 540
		o.vertex = UnityObjectToClipPos(v.vertex);
		#else
		o.vertex = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
		#endif
		
		o.uv = TRANSFORM_TEX(v.uv, _MainTex);
		o.uv2 = TRANSFORM_TEX(v.uv2, _MainTex);
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		// sample the texture
		fixed4 col = tex2D(_MaskTex, i.uv);
	fixed4 mask = tex2D(_MaskTex, float2(_Time.y * _Speed,i.uv2.y));

	return col*_MainColor * mask * _MainColor * _Rim * _BGColor;
	}
		ENDCG
	}
	}

	Fallback "Diffuse"
}
