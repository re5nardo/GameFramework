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

Shader "Shaders/TA/Effects/RK_EFF_Particle_CutOut(AlphaTest)"{
Properties	{
		_TintColor ("Tint Color", Color) = (1.0,1.0,1.0,1.0)
		_MainTex ("MainTex", 2D) = "white" {}
		_Alpha ("Alpha", 2D) = "white" {}
	}

	SubShader {

		//LOD 100
        Tags {"IgnoreProjector"="True" "Queue"="Transparent" "RenderType"="Transparent"}
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
			Cull Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

	    uniform sampler2D _MainTex; 
	    uniform fixed4 _MainTex_ST;

		uniform sampler2D _Alpha; 
		uniform fixed4 _Alpha_ST;

		uniform fixed4 _TintColor;

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
	        o.uv0 = v.texcoord0;
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
	     		fixed4 _MainTex_var =  tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));;
	            fixed4 _Alpha_var = tex2D(_Alpha,TRANSFORM_TEX(i.uv0, _Alpha));

              	fixed3 node_7330 = (_MainTex_var.rgb * i.vertexColor.rgb);


              	fixed node_9936 = _Alpha_var.r;
	 
	            fixed3 emissive = node_7330;

                fixed clipping = saturate(ceil((node_9936)));


                fixed3 finalColor = emissive * clipping * _TintColor;
                return fixed4(finalColor,((i.vertexColor.a *node_9936) * _MainTex_var.a * clipping));
	        }
	        ENDCG
	        }
	}

	Fallback "Diffuse"
}
