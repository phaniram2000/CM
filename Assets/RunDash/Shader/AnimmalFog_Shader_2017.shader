Shader "Animmal/Fog_Shader_2017" {
	Properties {
		_Depth ("Depth", Float) = 0
		_Opacity ("Opacity", Range(0, 1)) = 0
		_Color ("Color ", Vector) = (0.1470588,1,0.634,0)
		[HideInInspector] __dirty ("", Float) = 1
		[Header(Forward Rendering Options)] [ToggleOff] _SpecularHighlights ("Specular Highlights", Float) = 1
		[ToggleOff] _GlossyReflections ("Reflections", Float) = 1
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
	//CustomEditor "ASEMaterialInspector"
}