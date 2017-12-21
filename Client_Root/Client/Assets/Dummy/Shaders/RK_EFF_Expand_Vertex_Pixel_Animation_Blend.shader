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

Shader "Shaders/TA/Effects/Expand/RK_EFF_Expand_Vertex_Pixel_Animation_Blend" {
    Properties {
    	[MaterialToggle]_ZWriteOff ("ZWrite On", Float ) = 0
    	[Enum(UnityEngine.Rendering.CullMode)]_Cull ("Cull", Float ) = 0
		_Color ("Color", Color) = (1,1,1,1)
        _MainTex ("MainTex", 2D) = "white" {}
        _MainTexIntensity ("-MainTex Intensity", Float ) = 1
        [MaterialToggle] _UseAlphaChannel ("-Use AlphaChannel", Float ) = 0
        _AlphaChannelIntensity ("-AlphaChannel Intensity", Float ) = 1
		[MaterialToggle] _useTexScrollonLow ("[Use MainTex Scroll on Low]", Float ) = 0
		
		[Space(20)]
		[Header(Medium)]
        _SubTex ("SubTex (for PixelAni)", 2D) = "white" {}
        _SubTexScrollSpeed ("[SubTex Scroll Speed]", Float ) = 1
        _SubTexScrollVectorU0V1 ("-SubTex Scroll Vector[U=0/V=1]", Float ) = 0.5
        _DistortionValue ("-Pixel Ani Value", Float ) = 0.5
		
		[Space]
		[MaterialToggle] _UseFresnelEmissive ("[Use FresnelEmissive]", Float ) = 0
		[MaterialToggle] _UseFresnelAlpha ("[Use FresnelAlpha]", Float ) = 0
		_FresnelColor ("-Fresnel Color", Color) = (1,1,1,1)
		_FresnelIntensity ("-Fresnel Intensity", Float ) = 1
		_FresnelWidth ("-Fresnel Width", Float ) = 1
		[Space]
		[MaterialToggle] _UseSoftOpacity ("[Use Soft Opacity]", Float ) = 0
        _OpacitybyHeight ("-Opacity by Height", Float ) = 1
		[Space(20)]
		
		[Header(High)]
		[MaterialToggle] _UseVertexAni ("[Use Vertex Ani]", Float ) = 0
        _DisplacementSpeedHighOnly ("[Displacement Speed]", Float ) = 1
        _DisplacementLevel ("-Displacement Level", Float ) = 3
        _DisplacementValue ("-Displacement Value", Float ) = 0.05		
		

    }
    SubShader {
        Tags {
            "Queue"="Transparent"
            "RenderType"="Transparent"
			"IgnoreProjector"="True"
        }
        LOD 600
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
			//Blend SrcAlpha One
			Cull [_Cull]
            ZWrite [_ZWriteOff]

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex; uniform fixed4 _MainTex_ST;
            uniform sampler2D _SubTex; uniform fixed4 _SubTex_ST;
            uniform fixed4 _Color;
            uniform fixed _SubTexScrollVectorU0V1;
            uniform fixed _DistortionValue;
            uniform fixed _SubTexScrollSpeed;
            uniform fixed _MainTexIntensity;
            uniform fixed _UseAlphaChannel;
            uniform fixed _AlphaChannelIntensity;
            uniform fixed _OpacitybyHeight;
            uniform fixed _UseSoftOpacity;
            uniform fixed _UseVertexAni;
			uniform fixed4 _FresnelColor;
            uniform fixed _UseFresnelEmissive;
            uniform fixed _UseFresnelAlpha;
            uniform fixed _FresnelIntensity;
            uniform fixed _FresnelWidth;
            uniform fixed _DisplacementValue;
            uniform fixed _DisplacementLevel;
            uniform fixed _DisplacementSpeedHighOnly;
            struct VertexInput {
                fixed4 vertex : POSITION;
                fixed3 normal : NORMAL;
                fixed2 texcoord0 : TEXCOORD0;
                fixed4 vertexColor : COLOR;
            };
            struct VertexOutput {
                fixed4 pos : SV_POSITION;
                fixed2 uv0 : TEXCOORD0;
                fixed2 uv1 : TEXCOORD1;
                fixed4 posWorld : TEXCOORD2;
                fixed3 normalDir : TEXCOORD3;
                fixed4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = TRANSFORM_TEX(v.texcoord0, _SubTex);
                o.uv1 = TRANSFORM_TEX(v.texcoord0, _MainTex);
				
				o.vertexColor = v.vertexColor;
				o.normalDir = UnityObjectToWorldNormal(v.normal);
				
                fixed node_6898 = frac(((o.uv0*_DisplacementLevel)+(_Time.g*_DisplacementSpeedHighOnly)*fixed2(0,1)).g);
                fixed node_8248 = abs(node_6898*2-1);
				
                v.vertex.xyz += (1-_UseVertexAni)*fixed3(0,0,0)+(node_8248*v.normal*_DisplacementValue)*_UseVertexAni;

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
                i.normalDir = normalize(i.normalDir);
				fixed3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);                
				fixed finalFresnel = _FresnelIntensity * pow((1.0-abs(dot(viewDirection,i.normalDir))),_FresnelWidth);
				fixed3 fresnelEmissive = _UseFresnelEmissive * finalFresnel * i.vertexColor.rgb *_FresnelColor.rgb ;
				fixed fresnelAlpha = ((1.0 - _UseFresnelAlpha) * 1.0)+(_UseFresnelAlpha * finalFresnel);

                fixed node_7913 = (_Time.g*_SubTexScrollSpeed);
                fixed2 node_8553 = lerp((i.uv0+node_7913*fixed2(1,0)),(i.uv0+node_7913*fixed2(0,1)),_SubTexScrollVectorU0V1);
				fixed4 _SubTex_var = tex2D(_SubTex,node_8553);
                fixed2 node_4087 = (i.uv1+(_SubTex_var.r*_DistortionValue));
                fixed4 _MainTex_var = tex2D(_MainTex,node_4087);
				fixed3 finalColor = (_MainTex_var.rgb*i.vertexColor.rgb*_MainTexIntensity*_Color.rgb)+fresnelEmissive.rgb;
				
                fixed softOpacity = (1.0*(1.0 - _UseSoftOpacity))+(saturate((i.posWorld.g - _OpacitybyHeight)*4.0+0.0)*_UseSoftOpacity);
                fixed aChannel = (((1.0 - _UseAlphaChannel)*dot(_MainTex_var.rgb,fixed3(0.3,0.59,0.11)))+(_UseAlphaChannel*_MainTex_var.a));
                return fixed4(finalColor,aChannel * i.vertexColor.a * _AlphaChannelIntensity * softOpacity * fresnelAlpha);
            }
            ENDCG
        }
    }
	
	SubShader {
        Tags {
            "Queue"="Transparent"
            "RenderType"="Transparent"
			"IgnoreProjector"="True"
        }
        LOD 400
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
			//Blend SrcAlpha One
			Cull [_Cull]
            ZWrite [_ZWriteOff]

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex; uniform fixed4 _MainTex_ST;
            uniform sampler2D _SubTex; uniform fixed4 _SubTex_ST;
            uniform fixed4 _Color;
            uniform fixed _SubTexScrollVectorU0V1;
            uniform fixed _DistortionValue;
            uniform fixed _SubTexScrollSpeed;
            uniform fixed _MainTexIntensity;
            uniform fixed _UseAlphaChannel;
            uniform fixed _AlphaChannelIntensity;
            uniform fixed _OpacitybyHeight;
            uniform fixed _UseSoftOpacity;
            
			uniform fixed4 _FresnelColor;
            uniform fixed _UseFresnelEmissive;
            uniform fixed _UseFresnelAlpha;
            uniform fixed _FresnelIntensity;
            uniform fixed _FresnelWidth;
            
            struct VertexInput {
                fixed4 vertex : POSITION;
                fixed3 normal : NORMAL;
                fixed2 texcoord0 : TEXCOORD0;
                fixed4 vertexColor : COLOR;
            };
            struct VertexOutput {
                fixed4 pos : SV_POSITION;
                fixed2 uv0 : TEXCOORD0;
                fixed2 uv1 : TEXCOORD1;
                fixed4 posWorld : TEXCOORD2;
                fixed3 normalDir : TEXCOORD3;
                fixed4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = TRANSFORM_TEX(v.texcoord0, _SubTex);
                o.uv1 = TRANSFORM_TEX(v.texcoord0, _MainTex);
				
				o.vertexColor = v.vertexColor;
				o.normalDir = UnityObjectToWorldNormal(v.normal);
				
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
                i.normalDir = normalize(i.normalDir);
				fixed3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);                
				fixed finalFresnel = _FresnelIntensity * pow((1.0-abs(dot(viewDirection,i.normalDir))),_FresnelWidth);
				fixed3 fresnelEmissive = _UseFresnelEmissive * finalFresnel * i.vertexColor.rgb *_FresnelColor.rgb ;
				fixed fresnelAlpha = ((1.0 - _UseFresnelAlpha) * 1.0)+(_UseFresnelAlpha * finalFresnel);

                fixed node_7913 = (_Time.g*_SubTexScrollSpeed);
                fixed2 node_8553 = lerp((i.uv0+node_7913*fixed2(1,0)),(i.uv0+node_7913*fixed2(0,1)),_SubTexScrollVectorU0V1);
				fixed4 _SubTex_var = tex2D(_SubTex,node_8553);
                fixed2 node_4087 = (i.uv1+(_SubTex_var.r*_DistortionValue));
                fixed4 _MainTex_var = tex2D(_MainTex,node_4087);
				fixed3 finalColor = (_MainTex_var.rgb*i.vertexColor.rgb*_MainTexIntensity*_Color.rgb)+fresnelEmissive.rgb;
				
                fixed softOpacity = (1.0*(1.0 - _UseSoftOpacity))+(saturate((i.posWorld.g - _OpacitybyHeight)*4.0+0.0)*_UseSoftOpacity);
                fixed aChannel = (((1.0 - _UseAlphaChannel)*dot(_MainTex_var.rgb,fixed3(0.3,0.59,0.11)))+(_UseAlphaChannel*_MainTex_var.a));
                return fixed4(finalColor,aChannel * i.vertexColor.a * _AlphaChannelIntensity  * fresnelAlpha * softOpacity);
            }
            ENDCG
        }
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
