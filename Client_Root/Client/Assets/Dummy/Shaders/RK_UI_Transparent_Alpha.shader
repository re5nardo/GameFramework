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

Shader "Shaders/TA/UI/RK_UI_Transparent_Alpha"{
Properties	{
//		_MainColor ("Main Color", Color) = (1.0,1.0,1.0,1.0)
//		_BGColor		("BG Color(Client Only)", Color)		= (1,1,1,1)
//		_BackGroundColor ("BackGround Color", Color) = (1.0,1.0,1.0,1.0)
		_MainTex ("MainTex", 2D) = "white" {}
		_Alpha ("Alpha", 2D) = "white" {}
	}

	SubShader{

		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "Render Type" = "Transparent" }

		Blend SrcAlpha OneMinusSrcAlpha // 소스 알파값에 의해 곱해지며 (1-소스색상)에 의 곱해집니다.
	//	Cull Off
		Lighting Off
		ZWrite Off
		Offset -1, -1
	//	fog{mode Off}

	pass{
		CGPROGRAM
		#pragma fragmentoption ARB_precision_hint_fastest
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile_fog
		#include "UnityCG.cginc"
//		#include "TerrainEngine.cginc"

 		uniform sampler2D _MainTex; 
	    uniform fixed4 _MainTex_ST;

		uniform sampler2D _Alpha; 
		uniform fixed4 _Alpha_ST;

//		uniform fixed4 _MainColor;
//		uniform fixed4 _BGColor;

		struct v2f {
	        fixed4 vertex : SV_POSITION;
	        fixed2 texcoord : TEXCOORD0;
	        fixed2 texcoord1 : TEXCOORD1;

	        fixed4 color : COLOR;

//	        UNITY_FOG_COORDS(5)
	    };


		struct appdata_t
		{
			float4 vertex : POSITION;
			float2 texcoord : TEXCOORD0;
			fixed4 color : COLOR;
		};




//	    struct VertexOutput {
//	        fixed4 pos : SV_POSITION;
//	        fixed2 uv0 : TEXCOORD0;
//	    };


	    v2f vert (appdata_full v) {
	        v2f o;

			#if UNITY_VERSION >= 540
			o.vertex = UnityObjectToClipPos(v.vertex);
			#else
			o.vertex = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
			#endif

	        o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
	         o.texcoord1 = TRANSFORM_TEX(v.texcoord, _Alpha);

	         o.color = v.color;
//	        o.vertexColor = v.vertexColor;

//			UNITY_TRANSFER_FOG(o, o.vertex);

	        return o;
	    }

	    fixed4 frag(v2f IN) : COLOR {
	     		fixed4 _MainTex_var =  tex2D(_MainTex,IN.texcoord) * IN.color;
	            fixed4 _Alpha_var = tex2D(_Alpha,IN.texcoord1);

              	fixed node_9936 = _Alpha_var;	 
	            fixed3 emissive = _MainTex_var;
	            fixed3 finalColor = emissive;

//	            UNITY_APPLY_FOG(i.fogCoord, finalColor);
	            return fixed4(finalColor, ( _MainTex_var.a * node_9936));
	        }
	        ENDCG

			}//pass


		}//subshader

		Fallback "Diffuse"

}
