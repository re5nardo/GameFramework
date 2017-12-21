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


Shader "Shaders/TA/ImageEffect/RK_Mosaic" {
         Properties {
        _MainTex ("Source", 2D) = "white" {}
        _Size ("Size", Float) = 1
    }
    SubShader {
        ZTest Always
        Cull Off
        ZWrite Off
        Fog { Mode Off }

        Pass{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct v2f {
                fixed4 pos : SV_POSITION;
                fixed2 uv : TEXCOORD0;
            };

            v2f vert(appdata_img v) {
                v2f o;

                #if UNITY_VERSION >= 540
                o.pos = UnityObjectToClipPos(v.vertex);
                #else
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
                #endif

                o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
                return o;
            }

            sampler2D _MainTex;
           	uniform fixed _Size;

            fixed4 frag(v2f i) : SV_TARGET {
                fixed2 delta = _Size / _ScreenParams.xy;
                fixed2 uv = (floor(i.uv / delta) + 0.5 ) * delta;

                return tex2D(_MainTex, uv);
            }
            ENDCG
        }
    }
    FallBack Off
}