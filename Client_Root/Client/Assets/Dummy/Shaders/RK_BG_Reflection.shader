// Credit of Michael Collins

Shader "Shaders/TA/BackGround/RK_BG_Reflection"
{
	Properties
	{
		_MainAlpha("MainAlpha", Range(0, 1)) = 1
		_WaveValue2("Reflection Wave Power", Range(0, 1)) = 1
		_TintColor ("Tint Color (RGB)", Color) = (1,1,1)
		_MainTex ("MainTex (RGBA)", 2D) = ""
		_ReflectionTex ("ReflectionTex", 2D) = "white" { TexGen ObjectLinear }

		_WaveMaskTex("WaveMask",2D) = "white" {}
		_WaveValue("Wave Power",Range(0,1)) = 0
		_Wave_X_Speed("Wave X Speed", float ) = 0
		_Wave_Y_Speed("Wave Y Speed", float) = 0
		_OverlayPow("Overlay Power",Range(1,10)) = 1
	}

	//Two texture cards: full thing
	Subshader
	{
		LOD 600
		Tags {Queue = Transparent}
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		#pragma surface surf Lambert

		uniform fixed _MainAlpha;
		uniform fixed _WaveValue2;
		half3 _TintColor;
		uniform sampler2D _MainTex;
		uniform sampler2D _ReflectionTex;

		uniform fixed _WaveValue;
		uniform fixed _Wave_X_Speed;
		uniform fixed _Wave_Y_Speed;
		uniform fixed _OverlayPow;
		uniform sampler2D _WaveMaskTex;

		struct Input
		{
			fixed2 uv_MainTex;
			fixed2 uv_WaveMaskTex;
			fixed4 screenPos;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
				fixed4 mask = tex2D(_WaveMaskTex,IN.uv_WaveMaskTex);

				fixed4 mask_1 = tex2D(_WaveMaskTex, fixed2(fmod((IN.uv_WaveMaskTex.x + _Time.x *(_Wave_X_Speed*20) * 0.01),1), fmod((IN.uv_WaveMaskTex.y + _Time.x *(_Wave_Y_Speed*20) * 0.01),1)));
					
				half LavaValue = (mask_1.g) * mask.r;

				fixed4 col = tex2D(_MainTex, IN.uv_MainTex-LavaValue*_WaveValue);

				fixed dgray = mask.b;

				col = lerp(saturate(1 - ((1 - col)*(1 - mask.b) * 2))*pow(_OverlayPow,-1), saturate(col*mask.b * 2)*_OverlayPow, dgray);

				o.Albedo = _MainAlpha * _TintColor * col;
				o.Emission = tex2D(_ReflectionTex, (IN.screenPos.xy / IN.screenPos.w) + LavaValue * _WaveValue2);
		}
		ENDCG
	}


	Subshader
	{
		LOD 400
		Tags {Queue = Transparent}
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		#pragma surface surf Lambert

		fixed _MainAlpha;
		fixed _WaveValue2;
		half3 _TintColor;
		sampler2D _MainTex;
		sampler2D _ReflectionTex;

		fixed _WaveValue;
		fixed _Wave_X_Speed;
		fixed _Wave_Y_Speed;
		fixed _OverlayPow;
		sampler2D _WaveMaskTex;

		struct Input
		{
			fixed2 uv_MainTex;
			fixed2 uv_WaveMaskTex;
			fixed4 screenPos;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
				fixed4 mask = tex2D(_WaveMaskTex,IN.uv_WaveMaskTex);

				fixed4 mask_1 = tex2D(_WaveMaskTex, fixed2(fmod((IN.uv_WaveMaskTex.x + _Time.y *_Wave_X_Speed * 0.01),1), fmod((IN.uv_WaveMaskTex.y + _Time.y *_Wave_Y_Speed * 0.01),1)));
					
				half LavaValue = (mask_1.g) * mask.r;

				fixed4 col = tex2D(_MainTex, IN.uv_MainTex-LavaValue*_WaveValue);

				fixed dgray = mask.b;

				col = lerp(saturate(1 - ((1 - col)*(1 - mask.b) * 2))*pow(_OverlayPow,-1), saturate(col*mask.b * 2)*_OverlayPow, dgray);

				o.Albedo = _MainAlpha * _TintColor * col;
				o.Emission = tex2D(_ReflectionTex, (IN.screenPos.xy / IN.screenPos.w));
		}
		ENDCG
	}


	Subshader
	{
		LOD 200
		Tags {Queue = Transparent}
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		#pragma surface surf Lambert

		fixed _MainAlpha;
		half3 _TintColor;
		sampler2D _MainTex;

		fixed _WaveValue;
		fixed _Wave_X_Speed;
		fixed _Wave_Y_Speed;
		fixed _OverlayPow;
		sampler2D _WaveMaskTex;

		struct Input
		{
			fixed2 uv_MainTex;
			fixed2 uv_WaveMaskTex;
			fixed4 screenPos;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
				fixed4 mask = tex2D(_WaveMaskTex,IN.uv_WaveMaskTex);

				fixed4 mask_1 = tex2D(_WaveMaskTex, fixed2(fmod((IN.uv_WaveMaskTex.x + _Time.y *_Wave_X_Speed * 0.01),1), fmod((IN.uv_WaveMaskTex.y + _Time.y *_Wave_Y_Speed * 0.01),1)));
					
				half LavaValue = (mask_1.g) * mask.r;

				fixed4 col = tex2D(_MainTex, IN.uv_MainTex-LavaValue*_WaveValue);

				fixed dgray = mask.b;

				col = lerp(saturate(1 - ((1 - col)*(1 - mask.b) * 2))*pow(_OverlayPow,-1), saturate(col*mask.b * 2)*_OverlayPow, dgray);
				o.Emission = (_MainAlpha * _TintColor * col) + _TintColor;
		}
		ENDCG
	}



	//Fallback: just main texture
	Subshader
	{
		Pass
		{
			SetTexture [_MainTex] { combine texture }
		}
	}
}