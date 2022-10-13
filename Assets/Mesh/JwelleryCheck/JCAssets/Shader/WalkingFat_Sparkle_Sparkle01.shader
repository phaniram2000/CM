Shader "WalkingFat/Sparkle/Sparkle01" {
	Properties {
		_Tint ("Tint", Vector) = (0.5,0.5,0.5,1)
		_ShadowColor ("Shadow Color", Vector) = (0,0,0,1)
		_NoiseTex ("Noise Texture", 2D) = "white" {}
		_NoiseSize ("Noise Size", Float) = 2
		_ShiningSpeed ("Shining Speed", Float) = 0.1
		_SparkleColor ("sparkle Color", Vector) = (1,1,1,1)
		SparklePower ("sparkle Power", Float) = 10
		_Specular ("Specular", Range(0, 1)) = 0.5
		_Gloss ("Gloss", Range(0, 1)) = 0.5
		_RimColor ("Rim Color", Vector) = (0.17,0.36,0.81,0)
		_RimPower ("Rim Power", Range(0.6, 36)) = 8
		_RimIntensity ("Rim Intensity", Range(0, 100)) = 1
		_specsparkleRate ("Specular sparkle Rate", Float) = 6
		_rimsparkleRate ("Rim sparkle Rate", Float) = 10
		_diffsparkleRate ("Diffuse sparkle Rate", Float) = 1
		_ParallaxMap ("Parallax Map", 2D) = "white" {}
		_HeightFactor ("Height Scale", Range(-1, 1)) = 0.05
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = 1;
		}
		ENDCG
	}
	Fallback "VertexLit"
}