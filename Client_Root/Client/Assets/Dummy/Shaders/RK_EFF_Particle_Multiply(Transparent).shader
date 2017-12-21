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

Shader "Shaders/TA/Effects/RK_EFF_Particle_Multiply(Transparent)"{
Properties	{
		[MaterialToggle]_ZWriteOff ("ZWrite On", Float ) = 0
    	[Enum(UnityEngine.Rendering.CullMode)]_Cull ("Cull", Float ) = 0
    	[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("#ALPHA# Blending Source", float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("#ALPHA# Blending Dest", float) = 10
		_TintColor ("Tint Color", Color) = (1.0,1.0,1.0,1.0)
		_MainTex ("MainTex", 2D) = "white" {}
		_Alpha ("Alpha", 2D) = "white" {}
		_Intensity ("-Intensity", Float ) = 1

		[Space(20)]
		[Header(Medium)]
        [MaterialToggle] _UseSoftOpacity ("[Use Soft Opacity]", Float ) = 0
        _OpacitybyHeight ("-Opacity by Height", Float ) = -1.7

	}

	SubShader {
	LOD 600
	Tags { "IgnoreProjector"="True" "Queue"="Transparent" "RenderType"="Transparent"}
	Pass{
//		Name "FORWARD"
//        Tags {"LightMode"="ForwardBase"}

	Blend [_SrcBlend] [_DstBlend]
	Cull [_Cull]
	//Lighting Off
	ZWrite [_ZWriteOff]
	//fog{mode Off}

	 CGPROGRAM
	 	#pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"

	    uniform sampler2D _MainTex; 
	    uniform fixed4 _MainTex_ST;

		uniform sampler2D _Alpha; 
		uniform fixed4 _Alpha_ST;

		uniform fixed4 _TintColor;
		uniform fixed _Intensity;

		uniform fixed _OpacitybyHeight;
        uniform fixed _UseSoftOpacity;

		struct v2f 
	    {
			fixed4 pos : SV_POSITION;
			fixed2 uv : TEXCOORD0;
			fixed2 uv1 : TEXCOORD1;
			fixed4 posWorld : TEXCOORD2;
			fixed4 color : COLOR;
	    };

	    v2f vert (appdata_full v) 
	    {
			v2f o;
			o.color = v.color;

			#if UNITY_VERSION >= 540
			o.pos = UnityObjectToClipPos(v.vertex);
			#else
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
			#endif

			o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
			o.uv1 = TRANSFORM_TEX(v.texcoord, _Alpha);

			#if UNITY_VERSION >= 540
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
			#else
				o.posWorld = mul(_Object2World, v.vertex); // UNITY_SHADER_NO_UPGRADE
			#endif

			return o;
	    }

	    fixed4 frag(v2f i) : COLOR {
	     		fixed4 _MainTex_var =  tex2D(_MainTex,i.uv);
	            fixed4 _Alpha_var = tex2D(_Alpha,i.uv1);
              	fixed3 emissive = _MainTex_var.rgb * i.color.rgb;
              	fixed alpha = (1.0 - i.color.a) * _Intensity ;
	            fixed3 finalColor = emissive * (_TintColor * 2);
	            fixed softOpacity = (1.0*(1.0 - _UseSoftOpacity))+(_UseSoftOpacity*saturate(((0.0 - _OpacitybyHeight)+i.posWorld.g)*4.0+0.0));
	            return fixed4(alpha * finalColor * softOpacity,  _Alpha_var.r);
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
