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

Shader "Shaders/TA/Effects/Expand/RK_EFF_Expand_Blend(Use_Control)" {
    Properties {
    	[MaterialToggle]_ZWriteOff ("ZWrite On", Float ) = 0
    	[Enum(UnityEngine.Rendering.CullMode)]_Cull ("Cull", Float ) = 0
        _MainTex ("MainTex", 2D) = "white" {}
        _MainTexIntensity ("-MainTex Intensity", Float ) = 1
        [MaterialToggle(_REMOVEBLACK_ON)] _RemoveBlack ("-Remove Black", Float ) = 0
		[Space]
        [MaterialToggle(_USEALPHATEX_ON)] _UseAlphaTex ("-Use AlphaTex", Float ) = 0
		_Alpha ("-AlphaTex", 2D) = "white" {}
        _AlphaChannelIntensity ("-AlphaTex Intensity", Float ) = 1
		[Space]
		_SubTex ("SubTex", 2D) = "white" {}
        _SubTexIntensity ("-SubTex Intensity", Float ) = 1

        [Space]
        [MaterialToggle(_FLIPHORIZONTAL_ON)] _FlipHorizontal ("[Flip Horizontal]",float) = 0
        [MaterialToggle(_FLIPVERTICAL_ON)] _FlipVertical ("[Flip Vertical]",float) = 0

		[Space(20)]
		[Header(Medium)]
		[MaterialToggle(_USEMAINTEXSCROLL_ON)] _UseMainTexScroll ("[Use MainTex Scroll]",Float ) = 0
        _ScrollSpeedbyAlpha ("-Scroll Speed by Alpha", Float ) = 0
        _ScrollVector0U1V ("-Scroll Vector [0=U/1=V]", Float ) = 1
        [MaterialToggle] _DontAffectColor ("-Don't Affect Color", Float ) = 0

        [Space]
        [MaterialToggle(_USESOFTOPACITY_ON)] _UseSoftOpacity ("[Use Soft Opacity]", Float ) = 0
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
            Blend SrcAlpha OneMinusSrcAlpha
			Cull [_Cull]
            ZWrite [_ZWriteOff]
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature _USEMAINTEXSCROLL_ON
            #pragma shader_feature _USESOFTOPACITY_ON
            #pragma shader_feature _USEALPHATEX_ON
            #pragma shader_feature _USEMAINTEXROTATION_ON 
            #pragma shader_feature _FLIPVERTICAL_ON
            #pragma shader_feature _FLIPHORIZONTAL_ON
            #pragma shader_feature _REMOVEBLACK_ON
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex; uniform fixed4 _MainTex_ST;
            uniform sampler2D _Alpha; uniform fixed4 _Alpha_ST;
            uniform fixed _MainTexIntensity;
            uniform fixed _RemoveBlack;
            uniform fixed _OpacitybyHeight;
//            uniform fixed _UseSoftOpacity;
            uniform sampler2D _SubTex; uniform fixed4 _SubTex_ST;
            uniform fixed _SubTexIntensity;
//            uniform fixed _UseAlphaTex;
//            uniform fixed _UseMainTexScroll;
            uniform fixed _AlphaChannelIntensity;
			uniform fixed _MainTexRotationValue;
//            uniform fixed _UseMainTexRotation;
            uniform fixed _ScrollVector0U1V;
            uniform fixed _ScrollSpeedbyAlpha;
            uniform fixed _DontAffectColor;
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
                fixed4 posWorld : TEXCOORD2;
                fixed4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;


                 #if _FLIPHORIZONTAL_ON
               	 	o.uv0 = fixed2(1,0) + v.texcoord0 * fixed2(-1,1);
               	 #endif 
               	 	o.uv0 = o.uv0;

       	          #if _FLIPVERTICAL_ON
               	 	o.uv0 = fixed2(0,1)  + v.texcoord0 * fixed2(1,-1);
               	 #endif 
               	 	o.uv0 =  o.uv0;

               	  #if _FLIPVERTICAL_ON && _FLIPHORIZONTAL_ON
               	  	o.uv0 = fixed2(1,1)  + v.texcoord0 * fixed2(-1,-1);
               	  #endif
               	  	o.uv0 =  o.uv0;

                o.uv1 = TRANSFORM_TEX(v.texcoord0,_SubTex);
                o.vertexColor = v.vertexColor;

                #if UNITY_VERSION >= 540
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                #else
                o.posWorld = mul(_Object2World, v.vertex); // UNITY_SHADER_NO_UPGRADE
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
                #endif

                o.uv0 = TRANSFORM_TEX(o.uv0, _MainTex);

                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {


            	//Transform 연산은 _MainTex_var선언전에 i.uv에 들어와야 함.
				#if _USEMAINTEXROTATION_ON
					float node_394_ang = (3.141592654*_MainTexRotationValue);
	                float node_394_spd = 1.0;
	                float node_394_cos = cos(node_394_spd*node_394_ang);
	                float node_394_sin = sin(node_394_spd*node_394_ang);
	                float2 node_394_piv = float2(0.5,0.5);
	                float2 node_394 = (mul(i.uv0-node_394_piv,float2x2( node_394_cos, -node_394_sin, node_394_sin, node_394_cos))+node_394_piv);
	                i.uv0 = node_394;

	             #endif
	             	i.uv0;


 	             #if _USEMAINTEXSCROLL_ON
	             	fixed scrollAlpha = i.vertexColor.a*_ScrollSpeedbyAlpha;				
	                i.uv0 = lerp((i.uv0+scrollAlpha*fixed2(1,0)),(i.uv0+scrollAlpha*fixed2(0,1)),_ScrollVector0U1V);

	             #endif
	             	i.uv0;


	        	fixed4 _MainTex_var = tex2D(_MainTex,i.uv0);
	            fixed4 _Alpha_var = tex2D(_Alpha,i.uv0);
	            fixed4 _SubTex_var = tex2D(_SubTex,i.uv1);	



				_MainTex_var.rgb =_MainTex_var.rgb * _MainTexIntensity ;
				_SubTex_var.rgb = _SubTex_var.rgb * _SubTexIntensity;

				fixed finalAlpha = dot(_MainTex_var.rgb ,fixed3(0.3,0.59,0.11));    	

	


	             #if _USEALPHATEX_ON	                
	                finalAlpha = dot(_Alpha_var.rgb,fixed3(0.3,0.59,0.11))* _AlphaChannelIntensity *_MainTex_var.a;
	                finalAlpha = finalAlpha * dot(_SubTex_var.rgb  ,fixed3(0.3,0.59,0.11));
	              #endif
	              	finalAlpha = finalAlpha * dot(_SubTex_var.rgb ,fixed3(0.3,0.59,0.11));

	             
                  #if _REMOVEBLACK_ON
	                finalAlpha = dot((_MainTex_var.rgb * finalAlpha) ,fixed3(0.3,0.59,0.11));
	              #endif
	              	finalAlpha;

             	#if _USESOFTOPACITY_ON
                	fixed softOpacity = saturate(((0.0 - _OpacitybyHeight)+i.posWorld.g)*4.0+0.0);
                	finalAlpha = finalAlpha * softOpacity;
                #endif
                	finalAlpha;

                

                return fixed4((_MainTex_var.rgb * i.vertexColor.rgb), finalAlpha* saturate(i.vertexColor.a+_DontAffectColor));
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
            #pragma shader_feature _USEMAINTEXSCROLL_ON
            #pragma shader_feature _USESOFTOPACITY_ON
            #pragma shader_feature _USEALPHATEX_ON
            #pragma shader_feature _USEMAINTEXROTATION_ON 
            #pragma shader_feature _FLIPVERTICAL_ON
            #pragma shader_feature _FLIPHORIZONTAL_ON
            #pragma shader_feature _REMOVEBLACK_ON
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex; uniform fixed4 _MainTex_ST;
            uniform sampler2D _Alpha; uniform fixed4 _Alpha_ST;
            uniform fixed _MainTexIntensity;
            uniform fixed _RemoveBlack;
            uniform fixed _OpacitybyHeight;
//            uniform fixed _UseSoftOpacity;
            uniform sampler2D _SubTex; uniform fixed4 _SubTex_ST;
            uniform fixed _SubTexIntensity;
//            uniform fixed _UseAlphaTex;
//            uniform fixed _UseMainTexScroll;
            uniform fixed _AlphaChannelIntensity;
			uniform fixed _MainTexRotationValue;
//            uniform fixed _UseMainTexRotation;
            uniform fixed _ScrollVector0U1V;
            uniform fixed _ScrollSpeedbyAlpha;
            uniform fixed _DontAffectColor;
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
                fixed4 posWorld : TEXCOORD2;
                fixed4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;


               #if _FLIPHORIZONTAL_ON
               	 	o.uv0 = fixed2(1,0) + v.texcoord0 * fixed2(-1,1);
               	 #endif 
               	 	o.uv0 = o.uv0;

       	          #if _FLIPVERTICAL_ON
               	 	o.uv0 = fixed2(0,1)  + v.texcoord0 * fixed2(1,-1);
               	 #endif 
               	 	o.uv0 =  o.uv0;

               	  #if _FLIPVERTICAL_ON && _FLIPHORIZONTAL_ON
               	  	o.uv0 = fixed2(1,1)  + v.texcoord0 * fixed2(-1,-1);
               	  #endif
               	  	o.uv0 =  o.uv0;

                o.uv1 = TRANSFORM_TEX(v.texcoord0,_SubTex);
                o.vertexColor = v.vertexColor;

                #if UNITY_VERSION >= 540
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                #else
                o.posWorld = mul(_Object2World, v.vertex); // UNITY_SHADER_NO_UPGRADE
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
                #endif

                o.uv0 = TRANSFORM_TEX(o.uv0, _MainTex);

                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {

 //Transform 연산은 _MainTex_var선언전에 i.uv에 들어와야 함.
				#if _USEMAINTEXROTATION_ON
					float node_394_ang = (3.141592654*_MainTexRotationValue);
	                float node_394_spd = 1.0;
	                float node_394_cos = cos(node_394_spd*node_394_ang);
	                float node_394_sin = sin(node_394_spd*node_394_ang);
	                float2 node_394_piv = float2(0.5,0.5);
	                float2 node_394 = (mul(i.uv0-node_394_piv,float2x2( node_394_cos, -node_394_sin, node_394_sin, node_394_cos))+node_394_piv);
	                i.uv0 = node_394;

	             #endif
	             	i.uv0;


 	             #if _USEMAINTEXSCROLL_ON
	             	fixed scrollAlpha = i.vertexColor.a*_ScrollSpeedbyAlpha;				
	                i.uv0 = lerp((i.uv0+scrollAlpha*fixed2(1,0)),(i.uv0+scrollAlpha*fixed2(0,1)),_ScrollVector0U1V);

	             #endif
	             	i.uv0;


	        	fixed4 _MainTex_var = tex2D(_MainTex,i.uv0);
	            fixed4 _Alpha_var = tex2D(_Alpha,i.uv0);
	            fixed4 _SubTex_var = tex2D(_SubTex,i.uv1);	



				_MainTex_var.rgb =_MainTex_var.rgb * _MainTexIntensity ;
				_SubTex_var.rgb = _SubTex_var.rgb * _SubTexIntensity;

				fixed finalAlpha = dot(_MainTex_var.rgb ,fixed3(0.3,0.59,0.11));    	




	             #if _USEALPHATEX_ON
//	                
	                finalAlpha = dot(_Alpha_var.rgb,fixed3(0.3,0.59,0.11))* _AlphaChannelIntensity *_MainTex_var.a;
	                finalAlpha = finalAlpha * dot(_SubTex_var.rgb  ,fixed3(0.3,0.59,0.11));
	              #endif
	              	finalAlpha = finalAlpha * dot(_SubTex_var.rgb ,fixed3(0.3,0.59,0.11));

	             #if _REMOVEBLACK_ON
	                finalAlpha = dot((_MainTex_var.rgb * finalAlpha) ,fixed3(0.3,0.59,0.11));
	              #endif
	              	finalAlpha;


//             	#if _USESOFTOPACITY_ON
//                	fixed softOpacity = saturate(((0.0 - _OpacitybyHeight)+i.posWorld.g)*4.0+0.0);
//                	finalAlpha = finalAlpha * softOpacity;
//                #endif
//                	finalAlpha;

                

                return fixed4((_MainTex_var.rgb * i.vertexColor.rgb), finalAlpha* saturate(i.vertexColor.a+_DontAffectColor));
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
            #pragma shader_feature _REMOVEBLACK_ON
            #pragma shader_feature _USEALPHATEX_ON
            #pragma shader_feature _FLIPVERTICAL_ON
            #pragma shader_feature _FLIPHORIZONTAL_ON
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex; uniform fixed4 _MainTex_ST;
            uniform sampler2D _Alpha; uniform fixed4 _Alpha_ST;
            uniform fixed _MainTexIntensity;
			uniform fixed _MainTexRotationValue;
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
                fixed4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;


                  #if _FLIPHORIZONTAL_ON
               	 	o.uv0 = fixed2(1,0) + v.texcoord0 * fixed2(-1,1);
               	 #endif 
               	 	o.uv0 = o.uv0;

       	          #if _FLIPVERTICAL_ON
               	 	o.uv0 = fixed2(0,1)  + v.texcoord0 * fixed2(1,-1);
               	 #endif 
               	 	o.uv0 =  o.uv0;

               	  #if _FLIPVERTICAL_ON && _FLIPHORIZONTAL_ON
               	  	o.uv0 = fixed2(1,1)  + v.texcoord0 * fixed2(-1,-1);
               	  #endif
               	  	o.uv0 =  o.uv0;

                o.vertexColor = v.vertexColor;

                #if UNITY_VERSION >= 540
                o.pos = UnityObjectToClipPos(v.vertex);
                #else
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
                #endif

                o.uv0 = TRANSFORM_TEX(o.uv0, _MainTex);

                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {

      	fixed4 _MainTex_var = tex2D(_MainTex,i.uv0);
                fixed4 _Alpha_var = tex2D(_Alpha,i.uv0);
            	 fixed finalAlpha = dot(_MainTex_var,fixed3(0.3,0.59,0.11));





	             #if _USEALPHATEX_ON
	                fixed node_9936 = dot(_MainTex_var.rgb * _Alpha_var.rgb,fixed3(0.3,0.59,0.11))*_MainTex_var.a;
	                finalAlpha = node_9936;
	              #endif
	              	finalAlpha;


  	             #if _REMOVEBLACK_ON
	                finalAlpha = (dot((_MainTex_var.rgb * finalAlpha) ,fixed3(0.3,0.59,0.11))) * 0.25;
	              #endif
	              	finalAlpha;


	         	fixed3 MainTex_var = (_MainTexIntensity*_MainTex_var.rgb);  

				 fixed3 finalColor = (MainTex_var *  i.vertexColor.rgb);


                return fixed4(finalColor,finalAlpha);
            }
            ENDCG
        }
    }

    Fallback "Diffuse"

}
