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

 Shader "Shaders/TA/ImageEffect/RadialBlur" 
 { 
     Properties  
     { 
         _MainTex ("Input", RECT) = "white" {} 
         _BlurStrength ("", Float) = 0.5 
         _BlurWidth ("", Float) = 0.5 
     } 
 
 
     SubShader  
     { 
         Pass  
         { 
             ZTest Always Cull Off ZWrite Off 
             Fog { Mode off } 
         
             CGPROGRAM 
     
             #pragma vertex vert
             #pragma fragment frag 
             #pragma fragmentoption ARB_precision_hint_fastest 
             //#pragma exclude_renderers gles3 d3d11_9x xbox360 xboxone ps3 ps4 psp2
   
             #include "UnityCG.cginc" 
   
             uniform sampler2D _MainTex; 

             float4 _MainTex_ST;

             uniform half4 _MainTex_TexelSize; 
             uniform half _BlurStrength; 
             uniform half _BlurWidth; 
             uniform half _imgWidth; 
             uniform half _imgHeight; 


 			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD;
			};
	
			struct v2f {
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD;
			};
			

			v2f vert (appdata_t v)
			{
				v2f o;
                #if UNITY_VERSION >= 540
                o.vertex = UnityObjectToClipPos(v.vertex);
                #else
                o.vertex = mul (UNITY_MATRIX_MVP, v.vertex); // UNITY_SHADER_NO_UPGRADE
                #endif
                
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}

             half4 frag (v2f i) : COLOR  
             { 
                 half4 color = tex2D(_MainTex, i.texcoord); 
         
                 // some sample positions 
                 half samples[10]; 
                 samples[0] = -0.08; 
                 samples[1] = -0.05; 
                 samples[2] = -0.03; 
                 samples[3] = -0.02; 
                 samples[4] = -0.01; 
                 samples[5] =  0.01; 
                 samples[6] =  0.02; 
                 samples[7] =  0.03; 
                 samples[8] =  0.05; 
                 samples[9] =  0.08; 
         
                 //vector to the middle of the screen 
                 half2 dir = 0.5 * half2(_imgHeight,_imgWidth) - i.texcoord; 
         
                 //distance to center 
                 half dist = sqrt(dir.x*dir.x + dir.y*dir.y); 
         
                 //normalize direction 
                 dir = dir/dist; 
         
                 //additional samples towards center of screen 
                 half4 sum = color; 
                 for(int n = 0; n < 10; n++) 
                 { 
                     sum += tex2D(_MainTex, i.texcoord + dir * samples[n] * _BlurWidth * _imgWidth); 
                 } 
         
                 //eleven samples... 
                 sum *= 1.0/11.0; 
         
                 //weighten blur depending on distance to screen center 
                 /* 
                 half t = dist * _BlurStrength / _imgWidth; 
                 t = clamp(t, 0.0, 1.0); 
                 */		 
                 half t = saturate(dist * _BlurStrength); 
         
                 //blend original with blur 
                 return lerp(color, sum, t); 
             } 
             ENDCG 
         } 
     }
     Fallback off 
 } 
