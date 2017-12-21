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

Shader "Shaders/TA/ImageEffect/RK_MotionBlur"{
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_AccumOrig("AccumOrig", Float) = 0.65
}

    SubShader { 
		ZTest Always Cull Off ZWrite Off
		Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask RGB
		    BindChannels { 
				Bind "vertex", vertex 
				Bind "texcoord", texcoord
			} 
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
	
			#include "UnityCG.cginc"
	
			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD;
			};
	
			struct v2f {
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD;
			};
			
			float4 _MainTex_ST;
			float _AccumOrig;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				
				#if UNITY_VERSION >= 540
				o.vertex = UnityObjectToClipPos(v.vertex);
				#else
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
				#endif
				
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}
	
			sampler2D _MainTex;
			
			half4 frag (v2f i) : SV_Target
			{
				return half4(tex2D(_MainTex, i.texcoord).rgb, _AccumOrig );
			}
			ENDCG 
		} 

		Pass {
			Blend One Zero
			ColorMask A
			
		    BindChannels { 
				Bind "vertex", vertex 
				Bind "texcoord", texcoord
			} 
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
	
			#include "UnityCG.cginc"
	
			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD;
			};
	
			struct v2f {
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD;
			};
			
			float4 _MainTex_ST;
			
			v2f vert (appdata_t v)
			{
				v2f o;

				#if UNITY_VERSION >= 540
				o.vertex = UnityObjectToClipPos(v.vertex);
				#else
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
				#endif

				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}
	
			sampler2D _MainTex;
			
			half4 frag (v2f i) : SV_Target
			{
				return tex2D(_MainTex, i.texcoord);
			}
			ENDCG 
		}
		
	}

SubShader {
	ZTest Always Cull Off ZWrite Off
	Pass {
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask RGB
		SetTexture [_MainTex] {
			ConstantColor (0,0,0,[_AccumOrig])
			Combine texture, constant
		}
	}
	Pass {
		Blend One Zero
		ColorMask A
		SetTexture [_MainTex] {
			Combine texture
		}
	}
}

Fallback off

}
