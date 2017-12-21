Shader "RK/ColoredDiffuse" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Color ("Color", Color) = (1.0,1.0,1.0,1.0)
}
SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 150

CGPROGRAM
#pragma surface surf Lambert noforwardadd

fixed4 _Color;
sampler2D _MainTex;

struct Input {
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
	o.Albedo = c.rgb * _Color.rgb;
	o.Alpha = c.a;
}
ENDCG
}

Fallback "Mobile/VertexLit"
}
