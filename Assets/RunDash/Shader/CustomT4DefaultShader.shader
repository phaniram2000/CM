Shader "Custom/T4DefaultShader" {
	Properties {
		_MainTexture ("MainTexture", 2D) = "white" {}
		_DecalTexture ("DecalTexture", 2D) = "white" {}
		_TextureTint ("TextureTint", Vector) = (1,1,1,1)
		_Cutoff ("Mask Clip Value", Float) = 0.5
		_ShadowColor ("Shadow Color", Vector) = (0.1039961,0.3181929,0.5377358,1)
		_EmissionColor ("EmissionColor", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord ("", 2D) = "white" {}
		[HideInInspector] _texcoord2 ("", 2D) = "white" {}
		[HideInInspector] __dirty ("", Float) = 1
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
	//CustomEditor "ASEMaterialInspector"
}