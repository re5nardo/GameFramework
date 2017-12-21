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

Shader "Shaders/TA/BackGround/RK_Water_VertsColorAni(Transparent)"
{

	Properties
	{
//		_Color			("Main Color2", Color)						= (1,1,1,1)
		_MainColor		("Main Color", Color)						= (1,1,1,1)
		_BGColor		("BG Color(Client Only)", Color)		= (1,1,1,1)
		_MainTex		("Base (RGB)", 2D)							= "white" {}
		_MainTexU		("U Scroll Speed",float )					= 0
		_MainTexV		("V Scroll Speed",float )					= 0
		
		_WaveTex		("Wave (RGB)", 2D)							= "white" {}
		_WaveTexU		("U Scroll Speed",float )					= 0
		_WaveTexV		("V Scroll Speed",float )					= 0
		_WaveTex1		("Wave (RGB)", 2D)							= "white" {}
		_WaveTex1U		("U Scroll Speed",float )					= 0
		_WaveTex1V		("V Scroll Speed",float )					= 0
		_WaveTexPow		("Wave Texture Power", Range(0.0,3.0))		= 1.5
		_WaveTexSpeed	("Wave Texture Speed", Range(0.0,100.0))	= 30.0
		_MainDepth		("Main Depth", Range(0.0,1.0))				= 0.5
		
		_SprayTex		("Spray (RGB) Trans (A)", 2D)				= "white" {}
		_SprayTexU		("U Scroll Speed",float )					= 0
		_SprayTexV		("V Scroll Speed",float )					= 0
		_SprayTex1		("Spray (RGB) Trans (A)", 2D)				= "white" {}
		_SprayTex1U		("U Scroll Speed",float )					= 0
		_SprayTex1V		("V Scroll Speed",float )					= 0
		_SprayTrans		("Spray Trans", Range(0.0,1.0))				= 0.5
		
		_MaskTex		("Base (RGB) ", 2D)							= "white" {}
		
//		_WaveCycle		("Wave Cycle",Range(0.0,100.0))				= 0
		_WavePow		("Wave Power",Range(0.0,0.5))				= 0.05
		_WaveSpeed		("Wave Speed",Range(1,4))					= 1

		//Blending
		_SrcBlend ("#ALPHA# Blending Source", float) = 5
		_DstBlend ("#ALPHA# Blending Dest", float) = 10
	}



	// ======================================================================
	// LOD 600
	// ======================================================================
	SubShader
	{
		LOD 600
		Tags{"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
		Blend [_SrcBlend] [_DstBlend]
			//Blend One OneMinusSrcColor
		AlphaTest Greater .01
		//Cull Off 
		//Lighting Off
		//	ZWrite Off
		
		Pass
		{
			CGPROGRAM
			#pragma target 2.0
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_fog
			#include "UnityCG.cginc"

			
			// Main
			fixed4		_BGColor;
			fixed4		_MainColor;
			sampler2D	_MainTex;
			fixed4		_MainTex_ST;
			fixed		_MainTexU;
			fixed		_MainTexV;
			
			// Wave
			sampler2D	_WaveTex;
			fixed4		_WaveTex_ST;
			fixed		_WaveTexU;
			fixed		_WaveTexV;
			sampler2D	_WaveTex1;
			fixed4		_WaveTex1_ST;
			fixed		_WaveTex1U;
			fixed		_WaveTex1V;
			fixed		_WaveTexPow;
			fixed		_WaveTexSpeed;
			fixed		_MainDepth;
			
			// Spray
			sampler2D	_SprayTex;
			fixed4		_SprayTex_ST;				
			fixed		_SprayTexU;
			fixed		_SprayTexV;
			sampler2D	_SprayTex1;
			fixed4		_SprayTex1_ST;
			fixed		_SprayTex1U;
			fixed		_SprayTex1V;
			fixed		_SprayTrans;
			
			// Mask
			sampler2D	_MaskTex;
			fixed4		_MaskTex_ST;
			
			// Verts Wave
//			fixed		_WaveCycle;
			fixed		_WavePow;
			fixed		_WaveSpeed;
			
			struct v2f
			{
				fixed4	pos	: POSITION;
				fixed2	uv	: TEXCOORD0;
				fixed2	uv1	: TEXCOORD1;
				fixed2	uv2	: TEXCOORD2;
				fixed2	uv3	: TEXCOORD3;
				fixed2	uv4	: TEXCOORD4;
				fixed2	uv5	: TEXCOORD5;
				UNITY_FOG_COORDS(6)
			};
			

//			fixed getDeform(fixed3 p)
//			{
//				fixed time = _Time.y * _WaveSpeed;
//				fixed z = sin(time + p.y) * _WavePow;
//				
//				return z;
//			}
//			
			
			v2f vert(appdata_base v)
			{
				v2f o;

				fixed time = _Time.y * _WaveSpeed;
				fixed zzz = sin(time + v.vertex.y) * _WavePow;

				v.vertex.z += zzz;

				#if UNITY_VERSION >= 540
				o.pos = UnityObjectToClipPos(v.vertex);
				#else
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
				#endif
				
				fixed mainTexU	= _MainTexU * _Time;
				fixed mainTexV	= _MainTexV * _Time;
				fixed waveTexU	= _WaveTexU * _Time;
				fixed waveTexV	= _WaveTexV * _Time;
				fixed waveTex1U	= _WaveTex1U * _Time;
				fixed waveTex1V	= _WaveTex1V * _Time;
				o.uv	= TRANSFORM_TEX(v.texcoord.xy,_MainTex) + fixed2(mainTexU, mainTexV);
				o.uv1	= TRANSFORM_TEX(v.texcoord.xy,_WaveTex) + fixed2(waveTexU, waveTexV);
				o.uv2	= TRANSFORM_TEX(v.texcoord.xy,_WaveTex1) + fixed2(waveTex1U, waveTex1V);
				
				fixed sprayTexU		= _SprayTexU * _Time;
				fixed sprayTexV		= _SprayTexV * _Time;
				fixed sprayTex1U	= _SprayTex1U * _Time;
				fixed sprayTex1V	= _SprayTex1V * _Time;
				o.uv3	= TRANSFORM_TEX(v.texcoord.xy,_SprayTex) + fixed2(sprayTexU, sprayTexV);
				o.uv4	= TRANSFORM_TEX(v.texcoord.xy,_SprayTex1) + fixed2(sprayTex1U, sprayTex1U);

				o.uv5	= TRANSFORM_TEX(v.texcoord.xy,_MaskTex);
				
				UNITY_TRANSFER_FOG(o,o.pos);				
				
				return o;
			}

							 
			fixed4 frag (v2f i) : COLOR
			{
				fixed4 c = fixed4(0,0,0,0); // a값이 없이 fixed4 * fixed4 를 하면 하위 기종에서 오류가 남
				
				fixed4 finalWaterC	= fixed4(0,0,0,0);
				fixed4 finalWaveC	= fixed4(0,0,0,0);
				fixed4 finalSprayC	= fixed4(0,0,0,0);
				
				// Trxture
				fixed4 mainTex	= tex2D (_MainTex, i.uv);
				fixed4 waveTex	= tex2D (_WaveTex, i.uv1);
				fixed4 waveTex1	= tex2D (_WaveTex1, i.uv2);
				fixed4 sprayTex	= tex2D (_SprayTex, i.uv3);
				fixed4 sprayTex1= tex2D (_SprayTex1, i.uv4);
				fixed4 maskTex	= tex2D (_MaskTex, i.uv5);
				
				// Base Water Color
				fixed depth = 1 - (maskTex.g * _MainDepth);
				finalWaterC.rgb = _MainColor.rgb * depth * _BGColor.rgb;
				
				// Wave
				fixed sinValue	= sin(_Time.x * _WaveTexSpeed);
				fixed3 waveC	= waveTex.rgb * (sinValue * 0.5f + 0.5f) * _WaveTexPow;
				fixed3 wave1C	= waveTex1.rgb * ((0 - sinValue) * 0.5f + 0.5f) * _WaveTexPow;
				finalWaveC.rgb	= mainTex.rgb * ((waveTex.rgb * waveC) + (waveTex1.rgb * wave1C));
				
				// Spray
				finalSprayC.rgb	= (sprayTex.rgb + sprayTex1.rgb);
				finalSprayC.a	= (sprayTex.a + sprayTex1.a) * maskTex.b * _SprayTrans;
				
				// Final
				c.rgb = (((finalWaterC.rgb + finalWaveC.rgb) * (1 - finalSprayC.a)) + (finalSprayC.rgb * finalSprayC.a) ) * (1-maskTex.r);
				c.a = ((1 - (maskTex.r * _MainDepth)) * (1 - finalSprayC.a)) + finalSprayC.a;
				c.a *= _MainColor.a * maskTex.r;
				
				UNITY_APPLY_FOG(i.fogCoord, c);
				
				return c;
			}
			
			ENDCG
		}
	}
	
	

	// ======================================================================
	// LOD 400					
	// ======================================================================
	SubShader
	{
		LOD 400
		Tags{"Queue" = "Transparent" "RenderType" = "Transparent"}
		Blend[_SrcBlend][_DstBlend]
		AlphaTest Greater .01
		Cull Off 
		Lighting Off 
//		Fog {mode off}
			
			
		Pass
		{
			CGPROGRAM
			#pragma target 2.0
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_fog
			#include "UnityCG.cginc"
			
			// Main
			fixed4		_MainColor;
			fixed4		_BGColor;
			sampler2D	_MainTex;
			fixed4		_MainTex_ST;
			fixed		_MainTexU;
			fixed		_MainTexV;

			// Wave
			sampler2D	_WaveTex;
			fixed4		_WaveTex_ST;
			fixed		_WaveTexU;
			fixed		_WaveTexV;
			sampler2D	_WaveTex1;
			fixed4		_WaveTex1_ST;
			fixed		_WaveTex1U;
			fixed		_WaveTex1V;
			fixed		_WaveTexPow;
			fixed		_WaveTexSpeed;
			fixed		_MainDepth;
			
			// Spray
			sampler2D	_SprayTex;
			fixed4		_SprayTex_ST;				
			fixed		_SprayTexU;
			fixed		_SprayTexV;
			sampler2D	_SprayTex1;
			fixed4		_SprayTex1_ST;
			fixed		_SprayTex1U;
			fixed		_SprayTex1V;
			fixed		_SprayTrans;
			
			// Mask
			sampler2D	_MaskTex;
			fixed4		_MaskTex_ST;
			
			
			struct v2f
			{
				fixed4	pos	: POSITION;
				fixed2	uv	: TEXCOORD0;
				fixed2	uv1	: TEXCOORD1;
				fixed2	uv2	: TEXCOORD2;
				fixed2	uv3	: TEXCOORD3;
				fixed2	uv4	: TEXCOORD4;
				fixed2	uv5	: TEXCOORD5;
				UNITY_FOG_COORDS(6)
			};
			
			
			v2f vert(appdata_base v)
			{
				v2f o;
				
				#if UNITY_VERSION >= 540
				o.pos = UnityObjectToClipPos(v.vertex);
				#else
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
				#endif
				
				fixed mainTexU	= _MainTexU * _Time;
				fixed mainTexV	= _MainTexV * _Time;
				fixed waveTexU	= _WaveTexU * _Time;
				fixed waveTexV	= _WaveTexV * _Time;
				fixed waveTex1U	= _WaveTex1U * _Time;
				fixed waveTex1V	= _WaveTex1V * _Time;
				o.uv	= TRANSFORM_TEX(v.texcoord.xy,_MainTex) + fixed2(mainTexU, mainTexV);
				o.uv1	= TRANSFORM_TEX(v.texcoord.xy,_WaveTex) + fixed2(waveTexU, waveTexV);
				o.uv2	= TRANSFORM_TEX(v.texcoord.xy,_WaveTex1) + fixed2(waveTex1U, waveTex1V);
				
				fixed sprayTexU		= _SprayTexU * _Time;
				fixed sprayTexV		= _SprayTexV * _Time;
				fixed sprayTex1U	= _SprayTex1U * _Time;
				fixed sprayTex1V	= _SprayTex1V * _Time;
				o.uv3	= TRANSFORM_TEX(v.texcoord.xy,_SprayTex) + fixed2(sprayTexU, sprayTexV);
				o.uv4	= TRANSFORM_TEX(v.texcoord.xy,_SprayTex1) + fixed2(sprayTex1U, sprayTex1U);

				o.uv5	= TRANSFORM_TEX(v.texcoord.xy,_MaskTex);
				
				UNITY_TRANSFER_FOG(o,o.pos);
				
				return o;
			}

							 
			fixed4 frag (v2f i) : COLOR
			{
				fixed4 c = fixed4(0,0,0,0);
				
				fixed4 finalWaterC	= fixed4(0,0,0,0);
				fixed4 finalWaveC	= fixed4(0,0,0,0);
				fixed4 finalSprayC	= fixed4(0,0,0,0);
				
				// Trxture
				fixed4 mainTex	= tex2D (_MainTex, i.uv);
				fixed4 waveTex	= tex2D (_WaveTex, i.uv1);
				fixed4 waveTex1	= tex2D (_WaveTex1, i.uv2);
				fixed4 sprayTex	= tex2D (_SprayTex, i.uv3);
				fixed4 sprayTex1= tex2D (_SprayTex1, i.uv4);
				fixed4 maskTex	= tex2D (_MaskTex, i.uv5);
				
				// Base Water Color
				fixed depth = 1 - (maskTex.g * _MainDepth);
				finalWaterC.rgb = _MainColor.rgb * _BGColor.rgb * depth;
				
				// Wave
				fixed sinValue	= sin(_Time.x * _WaveTexSpeed);
				fixed3 waveC	= waveTex.rgb * (sinValue * 0.5f + 0.5f) * _WaveTexPow;
				fixed3 wave1C	= waveTex1.rgb * ((0 - sinValue) * 0.5f + 0.5f) * _WaveTexPow;
				finalWaveC.rgb	= mainTex.rgb * ((waveTex.rgb * waveC) + (waveTex1.rgb * wave1C));
				
				// Spray
				finalSprayC.rgb	= (sprayTex.rgb + sprayTex1.rgb);
				finalSprayC.a	= (sprayTex.a + sprayTex1.a) * maskTex.b * _SprayTrans;
				
				// Final
				c.rgb = (((finalWaterC.rgb + finalWaveC.rgb) * (1 - finalSprayC.a)) + (finalSprayC.rgb * finalSprayC.a)) * (1-maskTex.r);
				c.a	= ((1 - (maskTex.r * _MainDepth)) * (1 - finalSprayC.a)) + finalSprayC.a;
				c.a *= _MainColor.a;
				
				UNITY_APPLY_FOG(i.fogCoord, c);
				
				return c;
			}
			
			ENDCG
		}
	}
	
	
	// ======================================================================
	// LOD 200
	// ======================================================================
	SubShader
	{
		LOD 200
		Tags{"Queue" = "Transparent" "RenderType" = "Transparent" }
		Blend[_SrcBlend][_DstBlend]
		AlphaTest Greater .01
		Cull Off 
		//Lighting Off 
//		Fog {mode off}
			
			
		Pass
		{
			CGPROGRAM
			#pragma target 2.0
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_fog
			#include "UnityCG.cginc"
			
			
			// Main
			fixed4		_MainColor;
			fixed4		_BGColor;
			sampler2D	_MainTex;
			fixed4		_MainTex_ST;
			fixed		_MainTexU;
			fixed		_MainTexV;
			
			// Wave
			sampler2D	_WaveTex;
			fixed4		_WaveTex_ST;
			fixed		_WaveTexU;
			fixed		_WaveTexV;
			// Mask
			sampler2D	_MaskTex;
			fixed4 _MaskTex_ST;
			
			struct v2f
			{
				fixed4	pos	: POSITION;
				fixed2	uv	: TEXCOORD1;
				fixed2	uv1	: TEXCOORD2;
				fixed2	uv2	: TEXCOORD3;
				UNITY_FOG_COORDS(5)
			};
			
			
			v2f vert(appdata_base v)
			{
				v2f o;
				
				#if UNITY_VERSION >= 540
				o.pos = UnityObjectToClipPos(v.vertex);
				#else
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
				#endif
				
				fixed mainTexU	= _MainTexU * _Time;
				fixed mainTexV	= _MainTexV * _Time;
				fixed waveTexU	= _WaveTexU * _Time;
				fixed waveTexV	= _WaveTexV * _Time;
				o.uv	= TRANSFORM_TEX(v.texcoord.xy,_MainTex) + fixed2(mainTexU, mainTexV);
				o.uv1	= TRANSFORM_TEX(v.texcoord.xy,_WaveTex) + fixed2(waveTexU, waveTexV);
				o.uv2	= TRANSFORM_TEX(v.texcoord.xy,_MaskTex);
				
				UNITY_TRANSFER_FOG(o,o.pos);
				
				return o;
			}

							 
			fixed4 frag (v2f i) : COLOR
			{
				fixed4 c = fixed4(0,0,0,0);
				
				fixed4 finalWaterC	= fixed4(0,0,0,0);
				fixed4 finalWaveC	= fixed4(0,0,0,0);
				
				// Trxture
				fixed4 mainTex	= tex2D (_MainTex, i.uv);
				fixed4 waveTex	= tex2D (_WaveTex, i.uv1);
				fixed4 maskTex = tex2D(_MaskTex, i.uv2);

				// Base Water Color
				finalWaterC.rgb = _MainColor.rgb * _BGColor.rgb;
				
				// Wave
				finalWaveC.rgb	= mainTex.rgb * waveTex.rgb;
				
				// Final
				c.rgb = (finalWaterC.rgb + finalWaveC.rgb) * (1-maskTex.r);
				c.a = _MainColor.a * maskTex.r;
				
				UNITY_APPLY_FOG(i.fogCoord, c);
				
				return c;
			}
			
			ENDCG
		}
	}
	
    FallBack "Diffuse"
}
 
	 