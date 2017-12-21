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

Shader "Shaders/TA/Effects/Expand/RK_EFF_Expand_Add(Use_Control)" {
    Properties {
    	[MaterialToggle]_ZWriteOff ("ZWrite On", Float ) = 0
    	[Enum(UnityEngine.Rendering.CullMode)]_Cull ("Cull", Float ) = 0
    	[Enum(One,1,SrcColor,3,SrcAlpha,5)]_Blend ("Blend", Float ) = 1
        _MainTex ("MainTex", 2D) = "white" {}
        _MainTexIntensity ("-MainTex Intensity", Float ) = 1
        [MaterialToggle(_USEALPHACHANNEL_ON)] _UseAlphaChannel ("-Use AlphaChannel", Float ) = 0
		
		_SubTex ("SubTex", 2D) = "white" {}
        _SubTexIntensity ("-SubTex Intensity", Float ) = 1
        [Space]
        [MaterialToggle(_FLIPHORIZONTAL_ON)] _FlipHorizontal ("[Flip Horizontal]",float) = 0
        [MaterialToggle(_FLIPVERTICAL_ON)] _FlipVertical ("[Flip Vertical]",float) = 0
		
		[Space]
		[MaterialToggle(_USEMAINTEXSCROLL_ON)] _UseMainTexScroll ("[Use MainTex Scroll]",Float ) = 0
		_ScrollSpeedbyTime ("-Scroll Speed by Time",Float ) = 0
        _ScrollSpeedbyAlpha ("-Scroll Speed by Alpha", Float ) = 0
        _ScrollVector0U1V ("-Scroll Vector [0=U/1=V]", Float ) = 1

		[Space(20)]
		[Header(Medium)]
        [MaterialToggle( _USESOFTOPACITY_ON)] _UseSoftOpacity ("[Use Soft Opacity]", Float ) = 0
        _OpacitybyHeight ("-Opacity by Height", Float ) = -1.7
		
		[Space(20)]
		[Header(High)]
		[MaterialToggle(_USEMAINTEXROTATION_ON)] _UseMainTexRotation ("[Use MainTex Rotation]", Float ) = 0
        _MainTexRotationValue ("-MainTex Rotation Value", Range(-1, 1)) = 0
		
        
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
            //Blend SrcColor One
            Blend [_Blend] One
			Cull [_Cull]
            ZWrite [_ZWriteOff]
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature _USEMAINTEXSCROLL_ON
            #pragma shader_feature _USESOFTOPACITY_ON
            #pragma shader_feature _USEALPHACHANNEL_ON
            #pragma shader_feature _USEMAINTEXROTATION_ON 
            #pragma shader_feature _FLIPVERTICAL_ON
            #pragma shader_feature _FLIPHORIZONTAL_ON
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex; uniform fixed4 _MainTex_ST;
            uniform fixed _MainTexIntensity;
            uniform fixed _OpacitybyHeight;

            uniform fixed _ScrollVector0U1V;
            uniform fixed _ScrollSpeedbyTime;
            uniform fixed _ScrollSpeedbyAlpha;
			uniform fixed _MainTexRotationValue;

            uniform sampler2D _SubTex; uniform fixed4 _SubTex_ST;
            uniform fixed _SubTexIntensity;

//            uniform fixed _FlipHorizontal;
//            uniform fixed _FlipVertical;

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

             	 o.uv0 = v.texcoord0;
                o.uv1 = TRANSFORM_TEX(v.texcoord0, _SubTex);
                o.vertexColor = v.vertexColor;

                #if UNITY_VERSION >= 540
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                #else
                o.posWorld = mul(_Object2World, v.vertex); // UNITY_SHADER_NO_UPGRADE
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
                #endif

            	 #if _FLIPHORIZONTAL_ON
               	 	o.uv0 = fixed2(1,0) + v.texcoord0 * fixed2(-1,1);
               	 #endif 
               	 	o.uv0 = o.uv0;

       	          #if _FLIPVERTICAL_ON
               	 	o.uv0 = fixed2(0,1) + v.texcoord0 * fixed2(1,-1);
               	 #endif 
               	 	o.uv0 =  o.uv0;


				#if _USEMAINTEXROTATION_ON
					fixed node_394_ang = 3.141592654 * _MainTexRotationValue;
	                fixed node_394_spd = 1.0;
	                fixed node_394_cos = cos(node_394_spd*node_394_ang);
	                fixed node_394_sin = sin(node_394_spd*node_394_ang);
	                fixed2 node_394_piv = float2(0.5,0.5);
	                fixed2 node_394 = (mul(o.uv0-node_394_piv,float2x2( node_394_cos, -node_394_sin, node_394_sin, node_394_cos))+node_394_piv);					
				 	o.uv0 = node_394;
				 #endif
				 	o.uv0 = o.uv0;


				#if _USEMAINTEXSCROLL_ON
				 	fixed scrollAlpha = lerp(0.0,(_ScrollSpeedbyTime*_Time.g)+(o.vertexColor.a*_ScrollSpeedbyAlpha),1);
					fixed2 uvVector = lerp((o.uv0+scrollAlpha*fixed2(1,0)),(o.uv0+scrollAlpha*fixed2(0,1)),_ScrollVector0U1V);
					o.uv0 = uvVector;
				#endif
					o.uv0 = o.uv0;

				o.uv0 = TRANSFORM_TEX(o.uv0, _MainTex);

				return o;  
				             
            }


            fixed4 frag(VertexOutput i) : COLOR {
////// Emissive:
                fixed4 _MainTex_var = tex2D(_MainTex,i.uv0);
				

                fixed4 _SubTex_var = tex2D(_SubTex,i.uv1);

                fixed3 finalColor = _MainTexIntensity * _MainTex_var.rgb *(_SubTex_var.rgb*_SubTexIntensity);//*i.vertexColor.rgb;


                #if _USEALPHACHANNEL_ON
                	fixed3 mtAlpha = (_MainTex_var.rgb*_MainTex_var.a);
               	  	finalColor =_MainTexIntensity * (_MainTex_var.rgb*mtAlpha) *(_SubTex_var.rgb*_SubTexIntensity);
               	#endif                		
               		finalColor;


                #if _USESOFTOPACITY_ON
					fixed softOpacity = saturate(((0.0 - _OpacitybyHeight)+i.posWorld.g)*4.0+0.0);
	                finalColor = finalColor * softOpacity;
                #endif
                	finalColor;

                return fixed4(finalColor *i.vertexColor.rgb ,1);     
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
            Blend One One
			Cull [_Cull]
            ZWrite [_ZWriteOff]
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature _USEMAINTEXSCROLL_ON
            #pragma shader_feature _USESOFTOPACITY_ON
            #pragma shader_feature _USEALPHACHANNEL_ON
            #pragma shader_feature _USEMAINTEXROTATION_ON 
            #pragma shader_feature _FLIPVERTICAL_ON
            #pragma shader_feature _FLIPHORIZONTAL_ON
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex; uniform fixed4 _MainTex_ST;
            uniform fixed _MainTexIntensity;
            uniform fixed _OpacitybyHeight;

            uniform fixed _ScrollVector0U1V;
            uniform fixed _ScrollSpeedbyTime;
            uniform fixed _ScrollSpeedbyAlpha;

            uniform sampler2D _SubTex; uniform fixed4 _SubTex_ST;
            uniform fixed _SubTexIntensity;

//            uniform fixed _FlipHorizontal;
//            uniform fixed _FlipVertical;

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

             	 o.uv0 = v.texcoord0;
                o.uv1 = TRANSFORM_TEX(v.texcoord0, _SubTex);
                o.vertexColor = v.vertexColor;

                #if UNITY_VERSION >= 540
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);                    
                o.pos = UnityObjectToClipPos(v.vertex);
                #else
                o.posWorld = mul(_Object2World, v.vertex); // UNITY_SHADER_NO_UPGRADE
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
                #endif

              	 #if _FLIPHORIZONTAL_ON
               	 	o.uv0 = fixed2(1,0) + v.texcoord0 * fixed2(-1,1);
               	 #endif 
               	 	o.uv0 = o.uv0;

       	          #if _FLIPVERTICAL_ON
               	 	o.uv0 = fixed2(0,1) + v.texcoord0 * fixed2(1,-1);
               	 #endif 
               	 	o.uv0 =  o.uv0;


//				#if _USEMAINTEXROTATION_ON
//					fixed node_394_ang = 3.141592654 * _MainTexRotationValue;
//	                fixed node_394_spd = 1.0;
//	                fixed node_394_cos = cos(node_394_spd*node_394_ang);
//	                fixed node_394_sin = sin(node_394_spd*node_394_ang);
//	                fixed2 node_394_piv = float2(0.5,0.5);
//	                fixed2 node_394 = (mul(o.uv0-node_394_piv,float2x2( node_394_cos, -node_394_sin, node_394_sin, node_394_cos))+node_394_piv);					
//				 	o.uv0 = node_394;
//				 #endif
//				 	o.uv0 = o.uv0;


				#if _USEMAINTEXSCROLL_ON
				 	fixed scrollAlpha = lerp(0.0,(_ScrollSpeedbyTime*_Time.g)+(o.vertexColor.a*_ScrollSpeedbyAlpha),1);
					fixed2 uvVector = lerp((o.uv0+scrollAlpha*fixed2(1,0)),(o.uv0+scrollAlpha*fixed2(0,1)),_ScrollVector0U1V);
					o.uv0 = uvVector;
				#endif
					o.uv0 = o.uv0;

				o.uv0 = TRANSFORM_TEX(o.uv0, _MainTex);

				return o;                 
            }


            fixed4 frag(VertexOutput i) : COLOR {
////// Emissive:
                fixed4 _MainTex_var = tex2D(_MainTex,i.uv0);
				

                fixed4 _SubTex_var = tex2D(_SubTex,i.uv1);

                fixed3 finalColor = _MainTexIntensity * _MainTex_var.rgb *(_SubTex_var.rgb*_SubTexIntensity);//*i.vertexColor.rgb;


                #if _USEALPHACHANNEL_ON
                	fixed3 mtAlpha = (_MainTex_var.rgb*_MainTex_var.a);
               	  	finalColor =_MainTexIntensity * (_MainTex_var.rgb*mtAlpha) *(_SubTex_var.rgb*_SubTexIntensity);
               	#endif                		
               		finalColor;


                #if _USESOFTOPACITY_ON
					fixed softOpacity = saturate(((0.0 - _OpacitybyHeight)+i.posWorld.g)*4.0+0.0);
	                finalColor = finalColor * softOpacity;
                #endif
                	finalColor;

                return fixed4(finalColor *i.vertexColor.rgb ,1);     
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
            #pragma shader_feature _USEMAINTEXSCROLL_ON
            #pragma shader_feature _USESOFTOPACITY_ON
            #pragma shader_feature _USEALPHACHANNEL_ON
            #pragma shader_feature _USEMAINTEXROTATION_ON 
            #pragma shader_feature _FLIPVERTICAL_ON
            #pragma shader_feature _FLIPHORIZONTAL_ON
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex; uniform fixed4 _MainTex_ST;
            uniform fixed _MainTexIntensity;
//            uniform fixed _OpacitybyHeight;

            uniform fixed _ScrollVector0U1V;
            uniform fixed _ScrollSpeedbyTime;
            uniform fixed _ScrollSpeedbyAlpha;

            uniform sampler2D _SubTex; uniform fixed4 _SubTex_ST;
            uniform fixed _SubTexIntensity;

//            uniform fixed _FlipHorizontal;
//            uniform fixed _FlipVertical;

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

             	 o.uv0 = v.texcoord0;
                o.uv1 = TRANSFORM_TEX(v.texcoord0, _SubTex);
                o.vertexColor = v.vertexColor;

                #if UNITY_VERSION >= 540
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);                    
                o.pos = UnityObjectToClipPos(v.vertex);
                #else
                o.posWorld = mul(_Object2World, v.vertex); // UNITY_SHADER_NO_UPGRADE             
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
                #endif

            	 #if _FLIPHORIZONTAL_ON
               	 	o.uv0 = fixed2(1,0) + v.texcoord0 * fixed2(-1,1);
               	 #endif 
               	 	o.uv0 = o.uv0;

       	          #if _FLIPVERTICAL_ON
               	 	o.uv0 = fixed2(0,1) + v.texcoord0 * fixed2(1,-1);
               	 #endif 
               	 	o.uv0 =  o.uv0;


//				#if _USEMAINTEXROTATION_ON
//					fixed node_394_ang = 3.141592654 * _MainTexRotationValue;
//	                fixed node_394_spd = 1.0;
//	                fixed node_394_cos = cos(node_394_spd*node_394_ang);
//	                fixed node_394_sin = sin(node_394_spd*node_394_ang);
//	                fixed2 node_394_piv = float2(0.5,0.5);
//	                fixed2 node_394 = (mul(o.uv0-node_394_piv,float2x2( node_394_cos, -node_394_sin, node_394_sin, node_394_cos))+node_394_piv);					
//				 	o.uv0 = node_394;
//				 #endif
//				 	o.uv0 = o.uv0;


//				#if _USEMAINTEXSCROLL_ON
//				 	fixed scrollAlpha = lerp(0.0,(_ScrollSpeedbyTime*_Time.g)+(o.vertexColor.a*_ScrollSpeedbyAlpha),1);
//					fixed2 uvVector = lerp((o.uv0+scrollAlpha*fixed2(1,0)),(o.uv0+scrollAlpha*fixed2(0,1)),_ScrollVector0U1V);
//					o.uv0 = uvVector;
//				#endif
//					o.uv0 = o.uv0;

				o.uv0 = TRANSFORM_TEX(o.uv0, _MainTex);

				return o;                 
            }


            fixed4 frag(VertexOutput i) : COLOR {
////// Emissive:
                fixed4 _MainTex_var = tex2D(_MainTex,i.uv0);
				

                fixed4 _SubTex_var = tex2D(_SubTex,i.uv1);

               fixed3 finalColor = _MainTex_var.rgb *(_SubTex_var.rgb*_SubTexIntensity);//*i.vertexColor.rgb;


                  #if _USEALPHACHANNEL_ON
                  	fixed3 mtAlpha = (_MainTex_var.rgb*_MainTex_var.a);
               	  	finalColor =_MainTexIntensity * (_MainTex_var.rgb*mtAlpha) *(_SubTex_var.rgb*_SubTexIntensity);
               	#endif                		
               		finalColor;


//                #if _USESOFTOPACITY_ON
//					fixed softOpacity = saturate(((0.0 - _OpacitybyHeight)+i.posWorld.g)*4.0+0.0);
//	                finalColor = finalColor * softOpacity;
//                #endif
//                	finalColor;
//
                return fixed4(finalColor *i.vertexColor.rgb ,1);     
                }
            ENDCG
        }
    }

   Fallback "Diffuse"
}
