Shader "PolygonArsenal/PolyLitSurface" {
	Properties {
		_GlowIntensity ("Glow Intensity", Range(1, 5)) = 1
		_Smoothness ("Smoothness", Range(0, 1)) = 0
		_Metallic ("Metallic", Range(0, 1)) = 0
	}
	
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard fullforwardshadows
#pragma target 3.0
		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
		}
		ENDCG
	}
	Fallback "Diffuse"
}