Shader "FlatKit/GradientSkybox" {
	Properties {
		_Color2 ("Top Color", Vector) = (0.97,0.67,0.51,0)
		_Color1 ("Bottom Color", Vector) = (0,0.7,0.74,0)
		[Space] _Intensity ("Intensity", Range(0, 2)) = 1
		_Exponent ("Exponent", Range(0, 3)) = 1
		[Space] _DirectionYaw ("Direction X angle", Range(0, 180)) = 0
		_DirectionPitch ("Direction Y angle", Range(0, 180)) = 0
		[HideInInspector] _Direction ("Direction", Vector) = (0,1,0,0)
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
	//CustomEditor "GradientSkyboxEditor"
}