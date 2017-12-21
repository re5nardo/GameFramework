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

Shader "Shaders/TA/ImageEffect/BloomAndFlare/SeparableBlurPlus"
{
	Properties {
		_MainTex ("Base (RGB)", 2D) = "" {}
	}

//	ENDCG
	
Subshader 
{

//LOD 600
 Pass {
	  ZTest Always Cull Off ZWrite Off

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag

       #include "UnityCG.cginc"

      	struct v2f {
		half4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
		half4 uv01 : TEXCOORD1;
		half4 uv23 : TEXCOORD2;
		half4 uv45 : TEXCOORD3;
		half4 uv67 : TEXCOORD4;
	};


	struct appdata_t {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD;
	};
	
	half4 offsets;
	
	sampler2D _MainTex;

	float4 _MainTex_ST;
		
	v2f vert (appdata_t v) {
		v2f o;

		#if UNITY_VERSION >= 540
		o.pos = UnityObjectToClipPos(v.vertex);
		#else
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
		#endif

		o.uv.xy = v.texcoord.xy;

		o.uv01 =  v.texcoord.xyxy + offsets.xyxy * half4(1,1, -1,-1);
		o.uv23 =  v.texcoord.xyxy + offsets.xyxy * half4(1,1, -1,-1) * 2.0;
		o.uv45 =  v.texcoord.xyxy + offsets.xyxy * half4(1,1, -1,-1) * 3.0;
		o.uv67 =  v.texcoord.xyxy + offsets.xyxy * half4(1,1, -1,-1) * 4.5;
		o.uv67 =  v.texcoord.xyxy + offsets.xyxy * half4(1,1, -1,-1) * 6.5;

		return o;  
	}
		
	half4 frag (v2f i) : COLOR {
		half4 color = half4 (0,0,0,0);

		color += 0.225 * tex2D (_MainTex, i.uv);
		color += 0.150 * tex2D (_MainTex, i.uv01.xy);
		color += 0.150 * tex2D (_MainTex, i.uv01.zw);
		color += 0.110 * tex2D (_MainTex, i.uv23.xy);
		color += 0.110 * tex2D (_MainTex, i.uv23.zw);
		color += 0.075 * tex2D (_MainTex, i.uv45.xy);
		color += 0.075 * tex2D (_MainTex, i.uv45.zw);	
		color += 0.0525 * tex2D (_MainTex, i.uv67.xy);
		color += 0.0525 * tex2D (_MainTex, i.uv67.zw);
		
		return color;
	} 
     ENDCG
  }
}
	
Fallback off
	
} // shader
