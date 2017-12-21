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

Shader "Shaders/TA/BackGround/RK_BG_SimpleLight(frag)"{
Properties	{
		_MainColor ("Main Color", Color) = (1.0,1.0,1.0,1.0)
		_BGColor	("BG Color(Client Only)", Color)		= (1,1,1,1)
		_MainTex ("MainTex", 2D) = "white" {}
		_Rim ("Rim", Float) = 1.0
	}

	SubShader {

        Tags {"IgnoreProjector"="True" "Queue"="Transparent" "RenderType"="Transparent"}
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
//            Name "FORWARD"
//            Tags {"LightMode"="ForwardBase"}
//            Blend SrcAlpha OneMinusSrcAlpha
//			Cull Off
//            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

	    uniform sampler2D _MainTex; 
	    uniform fixed4 _MainTex_ST;


		uniform fixed4 _MainColor;
		uniform fixed4 _BGColor;
		uniform fixed4 _LightColor0;
		uniform fixed _Rim;

	    struct v2f 
	    {
			fixed4 pos : SV_POSITION;
			fixed2 uv : TEXCOORD0;
			fixed4 color : COLOR;
	    };



	    v2f vert (appdata_full v) 
	    {
			v2f o;

			float4 normal = float4(v.normal, 0.0);

			#if UNITY_VERSION >= 540
			float3 n = normalize(mul(normal, unity_WorldToObject));
			#else
			float3 n = normalize(mul(normal, _World2Object)); // UNITY_SHADER_NO_UPGRADE
			#endif
			
			float3 l = normalize(_WorldSpaceLightPos0);

			#if UNITY_VERSION >= 540
			o.pos = UnityObjectToClipPos(v.vertex);
			#else
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
			#endif

			o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

			float3 NdotL = (max(0.5, dot(n,l)));

			float3 d = NdotL * (_LightColor0 + 1) * _MainColor;
			o.color = float4(d, 1.0) + UNITY_LIGHTMODEL_AMBIENT;

			return o;
	    }

	    fixed4 frag(v2f i) : COLOR {

	    		fixed4 _MainTex_var =  tex2D(_MainTex,i.uv);

	    		fixed3 emissive = _MainTex_var.rgb * i.color.rgb * _Rim * _BGColor;
	    		
				return fixed4(emissive,1);

	        }
	        ENDCG
	    }
	}//SubShader

	Fallback "Diffuse"
}
