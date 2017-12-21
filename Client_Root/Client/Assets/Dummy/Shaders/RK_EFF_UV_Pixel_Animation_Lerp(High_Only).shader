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

Shader "Shaders/TA/Effects/Expand/RK_EFF_UV_Pixel_Animation_Lerp(High_Only)" {
    Properties {
    	[MaterialToggle]_ZWriteOff ("ZWrite On", Float ) = 0
    	[Enum(UnityEngine.Rendering.CullMode)]_Cull ("Cull", Float ) = 0
        _MainTex ("MainTex", 2D) = "white" {}
        _MainTexIntensity ("MainTex Intensity", Float ) = 1
        _SubTex ("SubTex", 2D) = "white" {}
        _SubTexIntensity ("SubTex Intensity", Float) = 1
        _SourceTex ("SourceTex", 2D) = "white" {}
        _DisplaceValue ("Displace Value", Float) = 0.5
        _Animation_Speed ("Animation_Speed", Float) = 1
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 600
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            Cull [_Cull]
            ZWrite [_ZWriteOff]
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
			
            uniform fixed4 _TimeEditor;
            uniform sampler2D _MainTex; uniform fixed4 _MainTex_ST;
            uniform sampler2D _SubTex; uniform fixed4 _SubTex_ST;
            uniform fixed _MainTexIntensity;
            uniform fixed _SubTexIntensity;
            uniform sampler2D _SourceTex; uniform fixed4 _SourceTex_ST;
            uniform fixed _DisplaceValue;
            uniform fixed _Animation_Speed;
            struct VertexInput {
                fixed4 vertex : POSITION;
                fixed2 texcoord0 : TEXCOORD0;
                fixed4 vertexColor : COLOR;
            };
            struct VertexOutput {
                fixed4 pos : SV_POSITION;
                fixed2 uv0 : TEXCOORD0;
                fixed4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;

                #if UNITY_VERSION >= 540
                o.pos = UnityObjectToClipPos(v.vertex);
                #else
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
                #endif

                return o;
            }
            fixed4 frag(VertexOutput i, fixed facing : VFACE) : COLOR {
                fixed isFrontFace = ( facing >= 0 ? 1 : 0 );
                fixed faceSign = ( facing >= 0 ? 1 : -1 );
////// Emissive:
                fixed4 node_5225 = _Time + _TimeEditor;
                fixed node_1117 = (node_5225.g*_Animation_Speed);
                fixed2 node_9680 = (i.uv0+node_1117*fixed2(0.5,0.5));
                fixed4 _SourceTex_var = tex2D(_SourceTex,TRANSFORM_TEX(node_9680, _SourceTex));
                fixed2 node_8443 = (i.uv0+node_1117*fixed2(0.3,-0.3));
                fixed4 _SRC_02_var = tex2D(_SourceTex,TRANSFORM_TEX(node_8443, _SourceTex));
                fixed2 node_4861 = (i.uv0+(_SourceTex_var.r*_SRC_02_var.r*_DisplaceValue));
                fixed4 _SubTex_var = tex2D(_SubTex,TRANSFORM_TEX(node_4861, _SubTex));
                fixed4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_4861, _MainTex));
                fixed3 emissive = (i.vertexColor.rgb*lerp((_SubTexIntensity*_SubTex_var.rgb),(_MainTex_var.rgb*_MainTexIntensity),i.vertexColor.a));
                fixed3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }

	
	SubShader {
        Tags {
            "RenderType"="Opaque"
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
