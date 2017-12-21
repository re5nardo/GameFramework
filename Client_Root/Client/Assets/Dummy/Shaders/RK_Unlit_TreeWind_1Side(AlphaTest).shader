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

Shader "Shaders/TA/BackGround/RK_Unlit_TreeWind_1Side(AlphaTest)"
{
	Properties
	{
//		_Color 				("Color", Color)				= (1.0,1.0,1.0,1.0)
		_MainColor 			("MainColor", Color)				= (1.0,1.0,1.0,1.0)
		_BGColor			("BG Color(Client Only)", Color) = (1,1,1,1)
		_MainTex			("Base (RGBA)", 2D)				= "white"{}
		_Alpha 				("Alpha", 2D) = "white" {}
//		_TransTex			("Alpha (R)", 2D)				= "white"{}
		_Cutoff				("Alpha Cutoff", Range(0,1))	= 0.5
		_SecondaryFactor	("Factor", float)				= 2.5
	}
	
	
	SubShader
	{
		Tags {"Queue"="AlphaTest" "RenderType"="TransparentCutout" "LightMode"="ForwardBase"}
		AlphaTest Greater [_Cutoff]
		
		
		CGINCLUDE
		#include "UnityCG.cginc"
		#include "TerrainEngine.cginc"
		
		
		fixed4		_MainColor;
		fixed4 		_BGColor;
		sampler2D	_MainTex;
		fixed4		_MainTex_ST;

		uniform sampler2D _Alpha; 
		uniform fixed4 _Alpha_ST;

//		sampler2D	_TransTex;
//		fixed4		_TransTex_ST;
		fixed		_Cutoff;
		float		_SecondaryFactor;
		
		#ifndef LIGHTMAP_OFF
		//fixed4		unity_LightmapST;
		// sampler2D	unity_Lightmap;
		#endif
		
		
		struct v2f
		{
			float4 pos		: SV_POSITION;
			fixed2 uv		: TEXCOORD0;

	        fixed2 uv2 : TEXCOORD2;
			
			#ifndef LIGHTMAP_OFF
			fixed2 lmap		: TEXCOORD1;
			#endif
			
			UNITY_FOG_COORDS(5)
		};
		
		
		inline float4 AnimateVertex2(float4 pos, float3 normal, float4 animParams, float SecondaryFactor)
		{	
			// animParams stored in color
			// animParams.x = branch phase = unused
			// animParams.y = edge flutter factor = green
			// animParams.z = primary factor = blue
			// animParams.w = secondary factor = red

			float fDetailAmp = 0.1f;
			float fBranchAmp = 0.3f;
			
			// Phases (object, vertex, branch)
			#if UNITY_VERSION >= 540
				float fObjPhase = dot(unity_ObjectToWorld[3].xyz, 1);
			#else
				float fObjPhase = dot(_Object2World[3].xyz, 1); // UNITY_SHADER_NO_UPGRADE
			#endif

			float fBranchPhase = fObjPhase;// + animParams.x;
			float fVtxPhase = dot(pos.xyz, animParams.y + fBranchPhase);
			
			// x is used for edges; y is used for branches
			// use pos.xz to create some variation
			float2 vWavesIn = _Time.yy + pos.xz *.3 + float2(fVtxPhase, fBranchPhase );
			
			// 1.975, 0.793, 0.375, 0.193 are good frequencies
			float4 vWaves = (frac( vWavesIn.xxyy * float4(1.975, 0.793, 0.375, 0.193) ) * 2.0 - 1.0);
			vWaves = SmoothTriangleWave( vWaves );
			float2 vWavesSum = vWaves.xz + vWaves.yw;

			// Edge (xz) and branch bending (y)
			// sign important to match normals of both faces!!! otherwise edge fluttering will be corrupted.
			float3 bend = animParams.y * fDetailAmp * normal.xyz * sign(normal.xyz);
			
			bend.y = animParams.z * fBranchAmp * SecondaryFactor; // controlled by vertex color red
			pos.xyz += ((vWavesSum.xyx * bend) + (_Wind.xyz * vWavesSum.y * animParams.w)) * _Wind.w; 

			// Primary bending
			// Displace position
//			pos.xyz += animParams.w * _Wind.xyz * _Wind.w; // controlled by vertex color blue
			
			return pos;
		}
		
		
		v2f vert (appdata_full v)
		{
			v2f o;
			
			float4 windParams = float4(0, v.color.g, v.color.r, v.color.b);		
			// call vertex animation
			#if UNITY_VERSION >= 540
				float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
				float3 worldNormal = mul((float3x3)unity_ObjectToWorld, v.normal);			
			#else
				float4 worldPos = mul(_Object2World, v.vertex); // UNITY_SHADER_NO_UPGRADE
				float3 worldNormal = mul((float3x3)_Object2World, v.normal); // UNITY_SHADER_NO_UPGRADE
			#endif
			
			float4 mdlPos = AnimateVertex2(worldPos, worldNormal, windParams, _SecondaryFactor);
			//float4 mdlPos = AnimateVertex2(v.vertex, v.normal, windParams, _SecondaryFactor);
			o.pos = mul(UNITY_MATRIX_VP,mdlPos);
			o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
			o.uv2 = TRANSFORM_TEX(v.texcoord, _Alpha);
			
			#ifndef LIGHTMAP_OFF
			o.lmap = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
			#endif
			
			UNITY_TRANSFER_FOG(o,o.pos);			
			return o;
		}
		ENDCG
		

		Pass
		{
			CGPROGRAM
			#pragma debug
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma fragmentoption ARB_precision_hint_fastest
			
			
			fixed4 frag (v2f i) : COLOR
			{
				fixed4 c;
				
				fixed4 mainTex	= tex2D (_MainTex, i.uv);
				fixed4 alphaTex	= tex2D (_Alpha, i.uv);
				
				clip(alphaTex - _Cutoff);
				
				c = mainTex * _MainColor * _BGColor;
				c.a = alphaTex;
				
		//		#ifndef LIGHTMAP_OFF
		//		fixed3 lm = DecodeLightmap (UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lmap));
		//		c.rgb *= lm;
		//		#endif
				
				UNITY_APPLY_FOG(i.fogCoord, c);
				
				return c;
			}
			ENDCG 
		}	
	}

	FallBack "Diffuse"
}


