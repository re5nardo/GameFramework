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

Shader "Shaders/TA/Effects/RK_EFF_Particle_Add_SrcAlpha(Transparent)"{
Properties	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("MainTex", 2D) = "white" {}
		_Alpha ("Alpha", 2D) = "white" {}

		[Enum(SrcAlpha,5,SrcColor,3)]_Blend ("Blend", Float ) = 5
		[Enum(LEQUAL,2,NOTEQUAL,6)]_ZTEST ("Ztest", Float ) = 2
	}

	Category {

	Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "Render Type" = "Transparent" }

	Blend [_Blend] One // 소스 알파값에 의해 완전하게 곱해집니다.
	Cull Off
	Lighting Off
	ZWrite Off
	fog{mode Off}
	ZTest [_ZTEST]
	ColorMask RGB


	BindChannels{
		Bind "Color" , color
		Bind "Vertex" , vertex
		Bind "Texcoord" , texcoord
	}

	SubShader{
			pass{
				SetTexture [_Alpha] {combine texture * previous}
				SetTexture [_MainTex]{
					constantColor [_TintColor]
					combine constant * primary
				}
				SetTexture [_Alpha] {combine texture * previous}
				SetTexture [_MainTex]{
					combine texture * previous DOUBLE
				}
			}//pass
		}//subshader
	}//category

	Fallback "Diffuse"
}
