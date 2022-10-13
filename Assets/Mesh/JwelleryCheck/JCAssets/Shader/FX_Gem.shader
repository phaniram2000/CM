Shader "FX/Gem" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		_ReflectionStrength ("Reflection Strength", Range(0, 2)) = 1
		_EnvironmentLight ("Environment Light", Range(0, 2)) = 1
		_Emission ("Emission", Range(0, 2)) = 0
		[NoScaleOffset] _RefractTex ("Refraction Texture", Cube) = "" {}
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = _Color.rgb;
			o.Alpha = _Color.a;
		}
		ENDCG
	}
}