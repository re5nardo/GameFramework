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

Shader "Shaders/TA/Character/RK_MeshShadow"
{
	Properties
	{
		_FakeShadowHeightDefault ("Shadow Height", float) = 0
        _FakeShadowHeightOffset ("Shadow Height Offset", float) = 0.05
		_Alpha ("Alpha", float) = 1.0
	}
	
	
	SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
	        Cull Front
	        Lighting Off
	        ZWrite Off
						
			Stencil
			{
				Ref 0
				Comp Equal
				Pass IncrWrap
				ZFail Keep
			}
			
			
			CGPROGRAM
			#pragma target 2.0
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"
			
			
			fixed4	_FakeLightDir;
			fixed4	_FakeShadowColor;
			fixed	_FakeShadowTrans;
			fixed	_FakeShadowHeightDefault;
			fixed	_FakeShadowHeightOffset;
			fixed	_Alpha;
			
			struct v2f 
			{
				fixed4	pos	: SV_POSITION;
			};

 
			v2f vert( appdata_tan v )
			{
				v2f o;				

				#if UNITY_VERSION >= 540
					fixed4 vPosWorld = mul( unity_ObjectToWorld, v.vertex);
				#else
					fixed4 vPosWorld = mul( _Object2World, v.vertex); // UNITY_SHADER_NO_UPGRADE
				#endif

//				fixed4 lightDirection = -normalize(_WorldSpaceLightPos0);
				//fixed4 lightDirection = -normalize(_FakeLightDir);
				fixed shadowHeight = _FakeShadowHeightDefault + _FakeShadowHeightOffset;
				fixed opposite = vPosWorld.y - shadowHeight;
				//fixed cosTheta = -lightDirection.y;	// = lightDirection dot (0,-1,0)
				//fixed hypotenuse = opposite / cosTheta;
				fixed3 vPos = vPosWorld.xyz + (_FakeLightDir * opposite);
				o.pos = mul (UNITY_MATRIX_VP, float4(vPos.x, shadowHeight, vPos.z ,1));
				
				return o;
			}
			
			 
			fixed4 frag( v2f i ) : COLOR
			{
				fixed4 c;
				
				c = _FakeShadowColor;
				c.a = _FakeShadowTrans * _Alpha;
				
				return c;
			}
			ENDCG
		}
	}
}