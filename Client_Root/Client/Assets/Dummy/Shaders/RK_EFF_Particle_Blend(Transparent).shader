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

Shader "Shaders/TA/Effects/RK_EFF_Particle_Blend(Transparent)"{
Properties	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("MainTex", 2D) = "white" {}
		_Alpha ("Alpha", 2D) = "white" {}

		[Enum(LEQUAL,2,NOTEQUAL,6)]_ZTEST ("Ztest", Float ) = 2
	}

	SubShader {

        Tags {"IgnoreProjector"="True" "Queue"="Transparent" "RenderType"="Transparent"}
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
			Cull Off
            ZWrite Off
            ZTest [_ZTEST]

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

	    uniform sampler2D _MainTex; 
	    uniform fixed4 _MainTex_ST;

		uniform sampler2D _Alpha; 
		uniform fixed4 _Alpha_ST;

		uniform fixed4 _TintColor;

	    struct v2f 
	    {
			fixed4 pos : SV_POSITION;
			fixed2 uv : TEXCOORD0;
			fixed2 uv1 : TEXCOORD1;
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

			return o;
	    }

	    fixed4 frag(v2f i) : COLOR {
	     		fixed4 _MainTex_var =  tex2D(_MainTex,i.uv);
	            fixed4 _Alpha_var = tex2D(_Alpha,i.uv1);

              	fixed3 emissive = _MainTex_var.rgb * i.color.rgb;
              	fixed node_9936 = _Alpha_var.r;	 

	            fixed3 finalColor = emissive * (_TintColor * 2);

	            return fixed4(finalColor, (i.color.a * _MainTex_var.a * node_9936));
	        }
	        ENDCG
	    }
	}//SubShader

	Fallback "Diffuse"
}
