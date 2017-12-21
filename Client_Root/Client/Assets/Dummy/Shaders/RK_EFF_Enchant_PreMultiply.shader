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

Shader "Shaders/TA/Effects/Expand/RK_EFF_Enchant_PreMultiply" {
    Properties {
    	[MaterialToggle]_ZWriteOff ("ZWrite On", Float ) = 0
    	[Enum(UnityEngine.Rendering.CullMode)]_Cull ("Cull", Float ) = 0
		[Header(Main)]
        _MainTex ("MainTex", 2D) = "white" {}
        _MainTexMask ("-MainTex Mask", 2D) = "white" {}
        _MainTexMaskAnimationSpeed ("-MainTexMaskAnimationSpeed", float) = 0.2
        _MainTexColorIntensity ("-MainTex Color Intensity", Vector) = (1,1,1,0)
        _MainTexAlphaIntensity ("-MainTex Alpha Intensity", float) = 1
        _MainTexUScrollSpeed ("-MainTex [U] Scroll Speed", float) = 0
        _MainTexVScrollSpeed ("-MainTex [V] Scroll Speed", float) = 0.05
		
		[Space(20)]
		[Header(Sub)]
        _SubTex ("SubTex", 2D) = "white" {}
        _SubTexMask ("-SubTex Mask", 2D) = "white" {}
        _SubTexColorIntensity ("-SubTex Color Intensity", Vector) = (1,1,1,0)
		_SubTexAlphaIntensity ("-SubTex Alpha Intensity", float) = 1
        _SubTexUScrollSpeed ("-SubTex [U] Scroll Speed", float) = 0
        _SubTexVScrollSpeed ("-SubTex [V] Scroll Speed", float) = -0.1
		
		[Space(20)]
		[Header(Distortion)]
        _DistortionTex ("DistortionTex", 2D) = "white" {}
        _DistortionValue ("-Distortion Value", float ) = 0.1
        _DistortionTexUScrollSpeed ("-DistortionTex [U] Scroll Speed", float) = -0.05
        _DistortionTexVScrollSpeed ("-DistortionTex [V] Scroll Speed", float) = -0.1

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
            Blend One OneMinusSrcAlpha
            Cull [_Cull]
            ZWrite [_ZWriteOff]
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
			
            uniform sampler2D _MainTex; uniform fixed4 _MainTex_ST;
            uniform sampler2D _MainTexMask; uniform fixed4 _MainTexMask_ST;
            uniform fixed _MainTexMaskAnimationSpeed;
			uniform fixed4 _MainTexColorIntensity;
			uniform fixed _MainTexAlphaIntensity;
			uniform fixed _MainTexUScrollSpeed;
			uniform fixed _MainTexVScrollSpeed;
			
            uniform sampler2D _SubTex; uniform fixed4 _SubTex_ST;
            uniform sampler2D _SubTexMask; uniform fixed4 _SubTexMask_ST;
			uniform fixed4 _SubTexColorIntensity;
			uniform fixed _SubTexAlphaIntensity;
			uniform fixed _SubTexUScrollSpeed;
			uniform fixed _SubTexVScrollSpeed;
			
            uniform sampler2D _DistortionTex; uniform fixed4 _DistortionTex_ST;
			uniform fixed _DistortionValue;
            uniform fixed _DistortionTexUScrollSpeed;
            uniform fixed _DistortionTexVScrollSpeed;
			
            struct VertexInput {
                fixed4 vertex : POSITION;
                fixed2 texcoord0 : TEXCOORD0;
				float3 normal : NORMAL;
				fixed4 vertexColor : COLOR;
            };
            struct VertexOutput {
                fixed4 pos : SV_POSITION;
                fixed2 uv0 : TEXCOORD0;
                fixed2 uv1 : TEXCOORD1;
                fixed2 uv2 : TEXCOORD2;
                fixed2 uv3 : TEXCOORD3;
                fixed2 uv4 : TEXCOORD4;
				//float3 normalDir : TEXCOORD5;
				float4 posWorld : TEXCOORD6;
				fixed4 vertexColor : COLOR;

            };
			
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
				o.vertexColor = v.vertexColor;
                o.uv0 = TRANSFORM_TEX(v.texcoord0,_SubTexMask);
                o.uv3 = TRANSFORM_TEX(v.texcoord0,_MainTex);
                o.uv4 = TRANSFORM_TEX(v.texcoord0,_MainTexMask);

                #if UNITY_VERSION >= 540
                o.pos = UnityObjectToClipPos(v.vertex);
                #else
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
                #endif

				//o.normalDir = UnityObjectToWorldNormal(v.normal);
                
                #if UNITY_VERSION >= 540
                    o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                #else
                    o.posWorld = mul(_Object2World, v.vertex); // UNITY_SHADER_NO_UPGRADE
                #endif

				fixed2 node_8437 = (o.uv0+(fixed2(_DistortionTexUScrollSpeed,_DistortionTexVScrollSpeed)*_Time.g));
				o.uv1 = TRANSFORM_TEX(node_8437, _SubTex);
				
				fixed2 node_7977 = (o.uv0+(fixed2(_SubTexUScrollSpeed,_SubTexVScrollSpeed)*_Time.g));
				o.uv2 = TRANSFORM_TEX(node_7977, _DistortionTex);

                return o;
            }
			
			
            fixed4 frag(VertexOutput i) : COLOR {
				//float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
			
                //fixed VdotN = abs(dot(viewDirection,i.normalDir));
				fixed4 _SubTexMask_var = tex2D(_SubTexMask,i.uv0);
				
                fixed4 _ScrollTex_01_var = tex2D(_SubTex,i.uv1);
				fixed4 _SubTex_var = tex2D(_SubTex,i.uv1);

                fixed4 _ScrollTex_var = tex2D(_SubTex,i.uv2);				
                fixed4 _DistortionTex_var = tex2D(_DistortionTex,i.uv2);	
				
				fixed3 subScroll = _SubTexMask_var.r*_ScrollTex_01_var.r*_ScrollTex_var.r*_SubTexColorIntensity.rgb;
				fixed subScrollAlpha = _SubTexMask_var.r*_ScrollTex_01_var.r*_ScrollTex_var.r*_SubTexAlphaIntensity;
				
                fixed node_8454 = ((_SubTex_var.r*_DistortionTex_var.r)*_DistortionValue);
                fixed2 node_7671 = (i.uv3+(fixed2(_MainTexUScrollSpeed,_MainTexVScrollSpeed)*_Time.g)+node_8454);
				fixed2 node_9496 = (i.uv4+node_8454);
				
                fixed4 _MainTex_var = tex2D(_MainTex,node_7671);
                fixed4 _MainTexMask_var = tex2D(_MainTexMask,node_9496);
				
				fixed absMainTexMask = (abs(((frac(((_Time.g*_MainTexMaskAnimationSpeed)+_MainTexMask_var.r))*2.0)+(-1.0)))*_MainTexMask_var.r);
				fixed3 mainScroll = _MainTex_var.rgb*_MainTexColorIntensity.rgb*absMainTexMask;
				fixed3 mainScrollAlpha = _MainTex_var.r*absMainTexMask*_MainTexAlphaIntensity;
				
                fixed3 emissive = (subScroll+mainScroll)*i.vertexColor.rgb;
				fixed finalAlpha = (subScrollAlpha+mainScrollAlpha)*i.vertexColor.a;
				
                return fixed4(emissive,finalAlpha);
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
            Blend One OneMinusSrcAlpha
            Cull [_Cull]
            ZWrite [_ZWriteOff]
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
			
            uniform sampler2D _MainTex; uniform fixed4 _MainTex_ST;
            uniform sampler2D _MainTexMask; uniform fixed4 _MainTexMask_ST;
			uniform fixed4 _MainTexColorIntensity;
			uniform fixed _MainTexAlphaIntensity;
			uniform fixed _MainTexUScrollSpeed;
			uniform fixed _MainTexVScrollSpeed;
			
            uniform sampler2D _SubTex; uniform fixed4 _SubTex_ST;
            uniform sampler2D _SubTexMask; uniform fixed4 _SubTexMask_ST;
			uniform fixed4 _SubTexColorIntensity;
			uniform fixed _SubTexAlphaIntensity;
			uniform fixed _SubTexUScrollSpeed;
			uniform fixed _SubTexVScrollSpeed;
			
            uniform sampler2D _DistortionTex; uniform fixed4 _DistortionTex_ST;
			uniform fixed _DistortionValue;
            uniform fixed _DistortionTexUScrollSpeed;
            uniform fixed _DistortionTexVScrollSpeed;
			
            struct VertexInput {
                fixed4 vertex : POSITION;
                fixed2 texcoord0 : TEXCOORD0;
				float3 normal : NORMAL;
				fixed4 vertexColor : COLOR;
            };
            struct VertexOutput {
                fixed4 pos : SV_POSITION;
                fixed2 uv0 : TEXCOORD0;
                fixed2 uv1 : TEXCOORD1;
                fixed2 uv2 : TEXCOORD2;

				float4 posWorld : TEXCOORD6;
				fixed4 vertexColor : COLOR;

            };
			
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
				o.vertexColor = v.vertexColor;
                o.uv0 = TRANSFORM_TEX(v.texcoord0,_MainTexMask);

                #if UNITY_VERSION >= 540
                o.pos = UnityObjectToClipPos(v.vertex);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                #else
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
                o.posWorld = mul(_Object2World, v.vertex); // UNITY_SHADER_NO_UPGRADE
                #endif
				
				fixed2 node_8437 = (o.uv0+(fixed2(_DistortionTexUScrollSpeed,_DistortionTexVScrollSpeed)*_Time.g));
				o.uv1 = TRANSFORM_TEX(node_8437, _SubTex);
				
				fixed2 node_7977 = (o.uv0+(fixed2(_SubTexUScrollSpeed,_SubTexVScrollSpeed)*_Time.g));
				o.uv2 = TRANSFORM_TEX(node_7977, _DistortionTex);

                return o;
            }
			
			
            fixed4 frag(VertexOutput i) : COLOR {

				fixed4 _ScrollTex_var = tex2D(_SubTex,i.uv0);	
				fixed4 _SubTex_var = tex2D(_SubTex,i.uv1);
				
				fixed3 subScroll = _SubTex_var.r*_ScrollTex_var.r*_SubTexColorIntensity.rgb;
				fixed subScrollAlpha = _SubTex_var.r*_ScrollTex_var.r*_SubTexAlphaIntensity;
				
				fixed4 _ScrollTex_01_var = tex2D(_MainTex,i.uv1);
				fixed4 _MainTex_var = tex2D(_MainTex,i.uv2);
                fixed4 _MainTexMask_var = tex2D(_MainTexMask,i.uv0);
				
				fixed3 mainScroll = _MainTex_var.rgb*_MainTexColorIntensity.rgb*_ScrollTex_01_var.rgb;
				fixed3 mainScrollAlpha = _MainTex_var.r*_ScrollTex_01_var.r*_MainTexAlphaIntensity;
				
                fixed3 emissive = (subScroll+mainScroll)*_MainTexMask_var.r*i.vertexColor.rgb;
				fixed finalAlpha = (subScrollAlpha+mainScrollAlpha)*_MainTexMask_var.r*i.vertexColor.a;
				
                return fixed4(emissive,finalAlpha);
            }
            ENDCG
        }
    }
    Fallback "Diffuse"

}
