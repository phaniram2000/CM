Shader "Unlit/UnlitFresnel" {
	Properties {
		_MainTexture ("Main Texture", 2D) = "white" {}
		_TextureTint ("Texture Tint", Vector) = (1,1,1,1)
		_FresnelTint ("Fresnel Tint", Vector) = (1,1,1,1)
		_FresnelScale ("Fresnel Scale", Float) = 1.12
		_PoesterisePower ("Poesterise Power", Float) = 80
		_SparkleTexture ("Sparkle Texture", 2D) = "white" {}
		_SparkleNoiseTexture ("Sparkle Noise Texture", 2D) = "gray" {}
		_Sparklepanningspeed ("Sparkle panning speed", Vector) = (-0.2,-0.2,0,0)
		_Sparklemultiplier ("Sparkle multiplier", Float) = 0
		_SparkleColor ("Sparkle Color", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord2 ("", 2D) = "white" {}
		[HideInInspector] _texcoord ("", 2D) = "white" {}
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