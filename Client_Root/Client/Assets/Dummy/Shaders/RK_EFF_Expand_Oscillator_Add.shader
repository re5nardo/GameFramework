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

Shader "Shaders/TA/Effects/Expand/RK_EFF_Expand_Oscillator_Add" {
    Properties {
    	_Color ("Color(forScriptAnim)", Color) = (1,1,1,1)
    	[MaterialToggle]_ZWriteOff ("ZWrite On", Float ) = 0
    	[Enum(UnityEngine.Rendering.CullMode)]_Cull ("Cull", Float ) = 0
        _MainTex ("MainTex", 2D) = "white" {}
        _MainTexIntensity ("-MainTex Intensity", Float ) = 1
        [MaterialToggle] _UseAlphaChannel ("-Use AlphaChannel", Float ) = 0
		
		_SubTex ("SubTex", 2D) = "white" {}
        _SubTexIntensity ("-SubTex Intensity", Float ) = 1
        [Space]
        [MaterialToggle] _FlipHorizontal ("[Flip Horizontal]",float) = 0
        [MaterialToggle] _FlipVertical ("[Flip Vertical]",float) = 0
		
		[Space]
		[MaterialToggle] _UseMainTexScroll ("[Use MainTex Scroll]",Float ) = 1
		_ScrollSpeedbyTime ("-Scroll Speed by Time",Float ) = 0
        _ScrollSpeedbyAlpha ("-Scroll Speed by Alpha", Float ) = 0
        _ScrollVector0U1V ("-Scroll Vector [0=U/1=V]", Float ) = 1
        		
		[Space(20)]
		[Header(Medium)]
		[Space]
		[MaterialToggle] _UseFresnelEmissive ("[Use Fresnel Emissive]", Float ) = 0
		[MaterialToggle] _UseFresnelAlpha ("[Use Fresnel Alpha]", Float ) = 0
		_FresnelColor ("-Fresnel Color", Color) = (1,1,1,1)
		_FresnelIntensity ("-Fresnel Intensity", Float ) = 1
		_FresnelWidth ("-Fresnel Width", Float ) = 1
		
		[Space]
		[MaterialToggle] _UseOscillator ("[Use Oscillator]", Float ) = 0
		_OscillatorSpeed ("-Oscillator Speed", Float ) = 0
		_RangeMin ("-Oscillator Range Min", Float ) = 0
		_RangeMax ("-Oscillator Range Max", Float ) = 1
		[MaterialToggle] _FresnelOnly ("-Oscillate Fresnel Only", Float ) = 0
		
		//[Space(20)]
		//[Header(High)]
		//[MaterialToggle] _UseMainTexRotation ("[Use MainTex Rotation]", Float ) = 0
        //_MainTexRotationValue ("-MainTex Rotation Value", Range(-1, 1)) = 0
		
		[Space]
        [MaterialToggle] _UseSoftOpacity ("[Use Soft Opacity]", Float ) = 0
        _OpacitybyHeight ("-Opacity by Height", Float ) = -1.7
        
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
            Blend One One
			Cull [_Cull]
            ZWrite [_ZWriteOff]
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform fixed4 _Color;
            uniform sampler2D _MainTex; uniform fixed4 _MainTex_ST;
            uniform fixed _MainTexIntensity;
			
			uniform fixed _OpacitybyHeight;
            uniform fixed _UseSoftOpacity;

            uniform fixed _ScrollVector0U1V;
            uniform fixed _ScrollSpeedbyAlpha;
            uniform sampler2D _SubTex; uniform fixed4 _SubTex_ST;
            uniform fixed _SubTexIntensity;
            uniform fixed _UseAlphaChannel;
            uniform fixed _UseMainTexScroll;
			uniform fixed _ScrollSpeedbyTime;
			
			uniform fixed4 _FresnelColor;
			uniform fixed _UseFresnelEmissive;
            uniform fixed _UseFresnelAlpha;
            uniform fixed _FresnelIntensity;
            uniform fixed _FresnelWidth;
			uniform fixed _UseOscillator;
            uniform fixed _OscillatorSpeed;
            uniform fixed _RangeMin;
            uniform fixed _RangeMax;
            uniform fixed _FresnelOnly;
            uniform fixed _FlipHorizontal;
            uniform fixed _FlipVertical;
			
			
			
            struct VertexInput {
                fixed4 vertex : POSITION;
                fixed2 texcoord0 : TEXCOORD0;
                fixed3 normal : NORMAL;
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
                o.uv0 = fixed2(_FlipHorizontal,_FlipVertical) + v.texcoord0 * fixed2(-(_FlipHorizontal-0.5)*2,-(_FlipVertical-0.5)*2);
                o.uv1 = TRANSFORM_TEX(v.texcoord0, _SubTex);
				o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.vertexColor = v.vertexColor;

                #if UNITY_VERSION >= 540
                o.pos = UnityObjectToClipPos(v.vertex);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                #else
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
                o.posWorld = mul(_Object2World, v.vertex); // UNITY_SHADER_NO_UPGRADE
                #endif
				
				fixed scrollAlpha = lerp(0.0,(_ScrollSpeedbyTime*_Time.g)+(o.vertexColor.a*_ScrollSpeedbyAlpha),_UseMainTexScroll);
                fixed2 uvVector = lerp((o.uv0+scrollAlpha*fixed2(1,0)),(o.uv0+scrollAlpha*fixed2(0,1)),_ScrollVector0U1V);
				
				o.uv0 = TRANSFORM_TEX(uvVector, _MainTex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {

                i.normalDir = normalize(i.normalDir);
                fixed3 normalDirection = i.normalDir;
				fixed3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);   				
				fixed finalFresnel = _FresnelIntensity * pow((1.0-abs(dot(viewDirection,i.normalDir))),_FresnelWidth);
				fixed3 fresnelEmissive = _UseFresnelEmissive * finalFresnel * i.vertexColor.rgb *_FresnelColor.rgb ;
				fixed fresnelAlpha = ((1.0 - _UseFresnelAlpha) * 1.0)+(_UseFresnelAlpha * finalFresnel);
				fixed oscillator = (1.0-_UseOscillator)+_UseOscillator*((abs((frac(_Time.g*_OscillatorSpeed)*2.0)-1.0)*abs((frac(_Time.g*_OscillatorSpeed)*2.0)-1.0))*(_RangeMax-_RangeMin)+_RangeMin);
				
                fixed4 _MainTex_var = tex2D(_MainTex,i.uv0);
                fixed3 mtAlpha = (_MainTex_var.rgb*_MainTex_var.a);
                fixed4 _SubTex_var = tex2D(_SubTex,i.uv1);

                fixed softOpacity = (1.0*(1.0 - _UseSoftOpacity))+(_UseSoftOpacity*saturate(((0.0 - _OpacitybyHeight)+i.posWorld.g)*4.0+0.0));
				fixed3 emissive = _MainTexIntensity*((_MainTex_var.rgb*(1.0 - _UseAlphaChannel))+(mtAlpha*_UseAlphaChannel))*_SubTex_var.rgb*_SubTexIntensity*i.vertexColor.rgb*softOpacity;
                fixed3 fresnelOscillater = ((1.0-_FresnelOnly)*(emissive*(fresnelAlpha*oscillator)+(fresnelEmissive*oscillator)))+(_FresnelOnly*(emissive*fresnelAlpha+(fresnelEmissive*oscillator)));
				fixed3 finalColor = fresnelOscillater*_Color.rgb;
                return fixed4(finalColor,1);
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
            Blend One One
			Cull [_Cull]
            ZWrite [_ZWriteOff]
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform fixed4 _Color;
            uniform sampler2D _MainTex; uniform fixed4 _MainTex_ST;
            uniform fixed _MainTexIntensity;

            uniform fixed _ScrollVector0U1V;
            uniform fixed _ScrollSpeedbyAlpha;
            uniform sampler2D _SubTex; uniform fixed4 _SubTex_ST;
            uniform fixed _SubTexIntensity;
            uniform fixed _UseAlphaChannel;
            uniform fixed _UseMainTexScroll;

            uniform fixed _UseOscillator;
            uniform fixed _OscillatorSpeed;
            uniform fixed _RangeMin;
            uniform fixed _RangeMax;

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
                fixed2 uv1 : TEXCOORD1;
                fixed4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = fixed2(_FlipHorizontal,_FlipVertical) + v.texcoord0 * fixed2(-(_FlipHorizontal-0.5)*2,-(_FlipVertical-0.5)*2);
                o.uv1 = TRANSFORM_TEX(v.texcoord0, _SubTex);
                o.vertexColor = v.vertexColor;

                #if UNITY_VERSION >= 540
                o.pos = UnityObjectToClipPos(v.vertex);
                #else
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
                #endif
				
				fixed scrollAlpha = o.vertexColor.a*lerp(0.0,_ScrollSpeedbyAlpha,_UseMainTexScroll);
                fixed2 uvVector = lerp((o.uv0+scrollAlpha*fixed2(1,0)),(o.uv0+scrollAlpha*fixed2(0,1)),_ScrollVector0U1V);
				o.uv0 = TRANSFORM_TEX(uvVector, _MainTex);
				
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {

            	fixed oscillator = (1.0-_UseOscillator)+_UseOscillator*((abs((frac(_Time.g*_OscillatorSpeed)*2.0)-1.0)*abs((frac(_Time.g*_OscillatorSpeed)*2.0)-1.0))*(_RangeMax-_RangeMin)+_RangeMin);

                fixed4 _MainTex_var = tex2D(_MainTex,i.uv0);
                fixed3 mtAlpha = (_MainTex_var.rgb*_MainTex_var.a);
                fixed4 _SubTex_var = tex2D(_SubTex,i.uv1);

                fixed3 emissive = (((_MainTexIntensity*((_MainTex_var.rgb*(1.0 - _UseAlphaChannel))+(mtAlpha*_UseAlphaChannel)))*_SubTex_var.rgb*_SubTexIntensity)*i.vertexColor.rgb);

                fixed3 finalColor = emissive*oscillator*_Color.rgb;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
	
	Fallback "Diffuse"
}
