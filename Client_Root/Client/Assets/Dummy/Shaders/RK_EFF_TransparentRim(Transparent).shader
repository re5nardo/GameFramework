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

Shader "Shaders/TA/Effects/RK_EFF_TransparentRim(Transparent)" {
Properties {

_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,1.0)
_MainTex ("MainTex",2D) = "white"{}
_RimColor ("Rim Color", Color) = (0.5,0.5,0.5,0.5)
_InnerColor ("Inner Color", Color) = (0.5,0.5,0.5,0.5)
_InnerColorPower ("Inner Color Power", Range(0.0,1.0)) = 0.5
_RimPower ("Rim Power", Range(0.0,5.0)) = 2.5
_AlphaPower ("Alpha Rim Power", Range(0.0,8.0)) = 4.0
_AllPower ("All Power", Range(0.0, 10.0)) = 1.0


}
SubShader {
	Tags { "Queue" = "Transparent""IgnoreProjector" = "True" "Render Type" = "Transparent" }

	LOD 600
	CGPROGRAM
	#pragma surface surf Lambert alpha

		struct Input
		{
			half2 uv_MainTex;
			float3 viewDir;
			INTERNAL_DATA
		};
			sampler2D _MainTex;
			fixed4 _RimColor;
			float _RimPower;
			fixed4 _TintColor;
			float _AlphaPower;
			float _AlphaMin;
			float _InnerColorPower;
			float _AllPower;
			fixed4 _InnerColor;

			void surf (Input IN, inout SurfaceOutput o) {

				fixed4 mainTex = tex2D(_MainTex, IN.uv_MainTex);
				half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));

				o.Emission = mainTex.rgb * _RimColor.rgb * pow (rim, _RimPower)*_AllPower+(_InnerColor.rgb*2*_InnerColorPower);
				o.Emission = o.Emission * _TintColor.rgb;
				o.Alpha = (mainTex.r * (pow (rim, _AlphaPower))*_AllPower) * _TintColor.a;
				}
		ENDCG
	}


		SubShader {
        Tags {
            "Queue"="Transparent-1"
            "RenderType"="Transparent"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull [_Cull]
            ZWrite [_ZWriteOff]
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct VertexInput {
                fixed4 vertex : POSITION;
                fixed2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                fixed4 pos : SV_POSITION;
                fixed2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;

                #if UNITY_VERSION >= 540
                o.pos = UnityObjectToClipPos(v.vertex);
                #else
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
                #endif

                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                return fixed4(0,0,0,0);
            }
            ENDCG
        }
    }
	Fallback "Diffuse"
} 