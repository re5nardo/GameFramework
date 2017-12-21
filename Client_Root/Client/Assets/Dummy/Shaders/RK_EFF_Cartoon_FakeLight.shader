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

Shader "Shaders/TA/Effects/Expand/RK_EFF_Cartoon_FakeLight" {
    Properties {
    	[MaterialToggle]_ZWriteOff ("ZWrite On", Float ) = 0
    	[Enum(UnityEngine.Rendering.CullMode)]_Cull ("Cull", Float ) = 0
        _MainTex ("MainTex", 2D) = "white" {}
		_OpacityAffectValue ("[Opacity Affect Value]", Float ) = 1
		[Space]
        [MaterialToggle] _FlipHorizontal ("[Flip Horizontal]",float) = 0
        [MaterialToggle] _FlipVertical ("[Flip Vertical]",float) = 0
		
		[Header(High)]
		[MaterialToggle] _UseMainTexRotation ("[Use MainTex Rotation]", Float ) = 0
        _TextureRotation ("-Texture Rotation", Range(-1, 1)) = 0
        [Header(Medium)]
		[MaterialToggle] _UseSoftOpacity ("[Use Soft Opacity]", Float ) = 0
        _OpacitybyHeight ("-Opacity by Height", Float ) = -1.7
        				
		[Header(DetailShadow (Medium))]
		[MaterialToggle] _UseDetailShadow ("[Use Detail Shadow]", Float ) = 1
        _DetailShadowIntensity ("-Detail Shadow Intensity", Range(0, 1)) = 0.8
        _ShadowThreshold ("-Shadow Threshold", Range(0, 1.5)) = 1.02097
        _ShadowSmooth ("-Shadow Smooth", Range(0, 1)) = 0
		_TexSheetX ("TexSheet X", Float ) = 1
        _TexSheetY ("TexSheet Y", Float ) = 1
        _DetailShadowRotation ("-Detail Shadow Rotation (High)", Range(-1, 1)) = 0
		
		[Header(HeightShadow (High))]
		[MaterialToggle] _UseHeightShadow ("[Use Height Shadow]", Float ) = 1
        _ShadowIntensity ("-Shadow Intensity", Range(0, 1)) = 0.8
        _ShadowHeight ("-Shadow Height", Float ) = 0
        
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
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

            uniform sampler2D _MainTex; uniform fixed4 _MainTex_ST;
            uniform fixed _ShadowHeight;
            uniform fixed _OpacityAffectValue;
            uniform fixed _TextureRotation;
            uniform fixed _OpacitybyHeight;
            uniform fixed _UseSoftOpacity;
            uniform fixed _ShadowSmooth;
            uniform fixed _ShadowThreshold;
            uniform fixed _DetailShadowRotation;
            uniform fixed _TexSheetX;
            uniform fixed _TexSheetY;
            uniform fixed _ShadowIntensity;
            uniform fixed _UseDetailShadow;
            uniform fixed _UseMainTexRotation;
            uniform fixed _DetailShadowIntensity;
			uniform fixed _UseHeightShadow;
			uniform fixed _FlipHorizontal;
            uniform fixed _FlipVertical;

            struct VertexInput {
                fixed4 vertex : POSITION;
                fixed2 texcoord0 : TEXCOORD0;
                fixed4 vertexColor : COLOR;
            };
            struct VertexOutput {
                fixed4 pos : SV_POSITION;
                fixed2 uv0 : TEXCOORD0;
                fixed4 posWorld : TEXCOORD1;
                fixed4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = fixed2(_FlipHorizontal,_FlipVertical) + v.texcoord0 * fixed2(-(_FlipHorizontal-0.5)*2,-(_FlipVertical-0.5)*2);
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

                fixed node_6790_ang = (_TextureRotation*3.141592654);
                fixed node_6790_cos = cos(node_6790_ang);
                fixed node_6790_sin = sin(node_6790_ang);
                fixed2 node_6790_piv = fixed2(0.5,0.5);
                fixed2 node_6790 = (mul(i.uv0-node_6790_piv,fixed2x2( node_6790_cos, -node_6790_sin, node_6790_sin, node_6790_cos))+node_6790_piv);
                fixed2 node_1650 = (i.uv0*(1.0 - _UseMainTexRotation))+(node_6790*_UseMainTexRotation);				
                fixed4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_1650, _MainTex));				
                clip(saturate(ceil(dot(_MainTex_var.rgb,fixed3(0.3,0.59,0.11))-(1.0 - i.vertexColor.a))) - 0.5);
////// Emissive:
				fixed node_5187_ang = (_DetailShadowRotation*3.141592654);
                fixed node_5187_cos = cos(node_5187_ang);
                fixed node_5187_sin = sin(node_5187_ang);
                fixed2 node_5187_piv = fixed2(0.5,0.5);
                fixed2 node_5187 = (mul(frac(i.uv0*fixed2(_TexSheetX,_TexSheetY))-node_5187_piv,fixed2x2( node_5187_cos, -node_5187_sin, node_5187_sin, node_5187_cos))+node_5187_piv);
                				
                fixed node_6236 = smoothstep( (_ShadowThreshold-_ShadowSmooth), (_ShadowThreshold+_ShadowSmooth), dot(_MainTex_var.rgb,node_5187) );
				fixed node_4121 = ((_DetailShadowIntensity*(1.0 - node_6236))+node_6236);
				fixed3 node_3893 = ((i.vertexColor.rgb*(1.0 - _UseDetailShadow))+((i.vertexColor.rgb*node_4121)*_UseDetailShadow));				
				fixed3 node_4757 = ceil((i.vertexColor.a-(1.0 - (saturate(i.posWorld.g+((1.0 - _ShadowHeight)*i.vertexColor.a))*_MainTex_var.rgb))));				
                fixed3 node_7011 = (saturate(((1.0 - node_4757)*_ShadowIntensity)+node_4757)*node_3893);		
                fixed3 finalColor = ((node_3893*(1.0 - _UseHeightShadow))+(node_7011*_UseHeightShadow));
                fixed softOpacity = (1.0*(1.0 - _UseSoftOpacity))+(_UseSoftOpacity*saturate(((0.0 - _OpacitybyHeight)+i.posWorld.g)*4.0+0.0));				
				fixed finalAlpha = (i.vertexColor.a*_OpacityAffectValue*softOpacity);
                return fixed4(finalColor,finalAlpha);
            }
            ENDCG
        }

    }

        SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 400
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

            uniform sampler2D _MainTex; uniform fixed4 _MainTex_ST;
            uniform fixed _OpacityAffectValue;
            uniform fixed _OpacitybyHeight;
            uniform fixed _UseSoftOpacity;
            uniform fixed _ShadowSmooth;
            uniform fixed _ShadowThreshold;
            uniform fixed _UseDetailShadow;
            uniform fixed _DetailShadowIntensity;
			uniform fixed _TexSheetX;
            uniform fixed _TexSheetY;
            uniform fixed _FlipHorizontal;
            uniform fixed _FlipVertical;
            struct VertexInput {
                fixed4 vertex : POSITION;
                fixed2 texcoord0 : TEXCOORD0;
                fixed4 vertexColor : COLOR;
            };
            struct VertexOutput {
                fixed4 pos : SV_POSITION;
                fixed2 uv0 : TEXCOORD0;
                fixed4 posWorld : TEXCOORD1;
                fixed4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = fixed2(_FlipHorizontal,_FlipVertical) + v.texcoord0 * fixed2(-(_FlipHorizontal-0.5)*2,-(_FlipVertical-0.5)*2);
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
                fixed4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));				
                clip(saturate(ceil(dot(_MainTex_var.rgb,fixed3(0.3,0.59,0.11))-(1.0 - i.vertexColor.a))) - 0.5);
////// Emissive:
                fixed node_6236 = smoothstep( (_ShadowThreshold-_ShadowSmooth), (_ShadowThreshold+_ShadowSmooth), dot(_MainTex_var.rgb,frac(i.uv0*fixed2(_TexSheetX,_TexSheetY))) );
				fixed node_4121 = ((_DetailShadowIntensity*(1.0 - node_6236))+node_6236);
                fixed3 finalColor = ((i.vertexColor.rgb*(1.0 - _UseDetailShadow))+((i.vertexColor.rgb*node_4121)*_UseDetailShadow));
				fixed softOpacity = (1.0*(1.0 - _UseSoftOpacity))+(_UseSoftOpacity*saturate(((0.0 - _OpacitybyHeight)+i.posWorld.g)*4.0+0.0));
				fixed finalAlpha = (i.vertexColor.a*_OpacityAffectValue*softOpacity);
                return fixed4(finalColor,finalAlpha);
            }
            ENDCG
        }

    }

        SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
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
