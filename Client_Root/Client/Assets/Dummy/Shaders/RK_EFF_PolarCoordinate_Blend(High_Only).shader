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

Shader "Shaders/TA/Effects/Expand/RK_EFF_PolarCoordinate_Blend(High_Only)" {
    Properties {
    	[MaterialToggle]_ZWriteOff ("ZWrite On", Float ) = 0
    	[Enum(UnityEngine.Rendering.CullMode)]_Cull ("Cull", Float ) = 0
        _MainTex ("MainTex", 2D) = "white" {}
        _MainTexIntensity ("-MainTex Intensity", Float ) = 1
        [MaterialToggle] _UseAlphaChannel ("-Use AlphaChannel", Float ) = 0
        _SubTex ("SubTex", 2D) = "white" {}
        _RadialScale ("RadialScale", Range(0, 4)) = 2
        _LengthScale ("LengthScale", Range(0, 2)) = 0.5
        _Speed ("Speed", Float ) = 0.5
		[MaterialToggle] _UseSpeedbyAlpha ("[Use Speed by Alpha]", Float ) = 0
		[Space(20)]
        [MaterialToggle] _UseSoftOpacity ("[Use Soft Opacity]", Float ) = 0
        _OpacitybyHeight ("-Opacity by Height", Float ) = -1.7
        //[HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
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
            Blend SrcAlpha OneMinusSrcAlpha
            Cull [_Cull]
            ZWrite [_ZWriteOff]
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform fixed4 _TimeEditor;
            uniform fixed _LengthScale;
            uniform fixed _RadialScale;
            uniform sampler2D _MainTex; uniform fixed4 _MainTex_ST;
            uniform sampler2D _SubTex; uniform fixed4 _SubTex_ST;
            uniform fixed _MainTexIntensity;
            uniform fixed _Speed;
            uniform fixed _UseSpeedbyAlpha;
            uniform fixed _UseAlphaChannel;
            uniform fixed _OpacitybyHeight;
            uniform fixed _UseSoftOpacity;
            struct VertexInput {
                fixed4 vertex : POSITION;
                fixed2 texcoord0 : TEXCOORD0;
                fixed4 vertexColor : COLOR;
            };
            struct VertexOutput {
                fixed4 pos : SV_POSITION;
                fixed2 uv0 : TEXCOORD0;
                fixed2 uv1 : TEXCOORD1;
                fixed4 posWorld : TEXCOORD2;
                fixed4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                //o.uv0 = v.texcoord0;
                o.uv0 = TRANSFORM_TEX(v.texcoord0, _MainTex);
                o.uv1 = TRANSFORM_TEX(v.texcoord0, _SubTex);
                o.vertexColor = v.vertexColor;

                #if UNITY_VERSION >= 540
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                #else
                o.posWorld = mul(_Object2World, v.vertex); // UNITY_SHADER_NO_UPGRADE
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
                #endif
                
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {

////// Emissive:
                fixed4 node_7466 = _Time + _TimeEditor;
				fixed4 SbyA = i.vertexColor.a + _TimeEditor;
                fixed2 node_4684 = ((-0.5)+i.uv0);
                fixed node_4016 = node_4684.r;
                fixed node_3422 = (_LengthScale*node_4016);
                fixed node_6843 = node_4684.g;
                fixed node_7524 = (_LengthScale*node_6843);
				fixed UseSbyA = lerp(node_7466.g*_Speed, SbyA.g * _Speed ,_UseSpeedbyAlpha);
                fixed2 node_2561 = (fixed2(pow(((node_3422*node_3422)+(node_7524*node_7524)),0.5),(atan2(node_4016,node_6843)/(_RadialScale*3.141592654)))+UseSbyA*fixed2(-1,0));

                fixed4 _MainTex_var = tex2D(_MainTex,node_2561);
				fixed4 _SubTex_var = tex2D(_SubTex,i.uv1);

                fixed3 finalColor = (_MainTex_var.rgb*i.vertexColor.rgb);
				fixed alphaChannel = lerp(dot(_MainTex_var.rgb,fixed3(0.3,0.59,0.11)),_MainTex_var.a,_UseAlphaChannel);
				fixed finalAlpha = (alphaChannel*_MainTexIntensity*i.vertexColor.a*_SubTex_var.r*lerp(1.0,saturate(((0.0 - _OpacitybyHeight)+i.posWorld.g)*4.0+0.0),_UseSoftOpacity));
                return fixed4(finalColor,finalAlpha);
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
