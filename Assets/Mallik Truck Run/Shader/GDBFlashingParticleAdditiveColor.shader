Shader "GDB/FlashingParticleAdditiveColor" {
	Properties {
		_Color ("MainColor", Vector) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_Freq ("Freq min | max | freq | Intensity ", Vector) = (0.95,1,10,0)
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha One, SrcAlpha One
			ZWrite Off
			Cull Off
			Fog {
				Mode Off
			}
			GpuProgramID 15233
			Program "vp" {
				SubProgram "gles hw_tier00 " {
					"!!GLES
					#ifdef VERTEX
					#version 100
					
					uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	vec4 _MainTex_ST;
					attribute highp vec4 in_POSITION0;
					attribute highp vec2 in_TEXCOORD0;
					varying highp vec2 vs_TEXCOORD0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					void main()
					{
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + hlslcc_mtx4x4unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    gl_Position = hlslcc_mtx4x4unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 100
					
					#ifdef GL_FRAGMENT_PRECISION_HIGH
					    precision highp float;
					#else
					    precision mediump float;
					#endif
					precision highp int;
					uniform 	vec4 _Time;
					uniform 	mediump vec4 _Freq;
					uniform 	mediump vec4 _Color;
					uniform lowp sampler2D _MainTex;
					varying highp vec2 vs_TEXCOORD0;
					#define SV_Target0 gl_FragData[0]
					float u_xlat0;
					mediump float u_xlat16_1;
					mediump float u_xlat16_2;
					lowp float u_xlat10_2;
					void main()
					{
					    u_xlat0 = _Time.y * _Freq.z;
					    u_xlat0 = sin(u_xlat0);
					    u_xlat0 = u_xlat0 * 0.5 + 0.5;
					    u_xlat16_2 = (-_Freq.x) + _Freq.y;
					    u_xlat0 = u_xlat0 * u_xlat16_2 + _Freq.x;
					    u_xlat10_2 = texture2D(_MainTex, vs_TEXCOORD0.xy).x;
					    u_xlat0 = u_xlat0 * u_xlat10_2;
					    u_xlat16_1 = _Freq.w;
					    u_xlat16_1 = clamp(u_xlat16_1, 0.0, 1.0);
					    u_xlat16_1 = u_xlat0 * u_xlat16_1;
					    SV_Target0 = vec4(u_xlat16_1) * _Color;
					    return;
					}
					
					#endif"
				}
				SubProgram "gles hw_tier01 " {
					"!!GLES
					#ifdef VERTEX
					#version 100
					
					uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	vec4 _MainTex_ST;
					attribute highp vec4 in_POSITION0;
					attribute highp vec2 in_TEXCOORD0;
					varying highp vec2 vs_TEXCOORD0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					void main()
					{
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + hlslcc_mtx4x4unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    gl_Position = hlslcc_mtx4x4unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 100
					
					#ifdef GL_FRAGMENT_PRECISION_HIGH
					    precision highp float;
					#else
					    precision mediump float;
					#endif
					precision highp int;
					uniform 	vec4 _Time;
					uniform 	mediump vec4 _Freq;
					uniform 	mediump vec4 _Color;
					uniform lowp sampler2D _MainTex;
					varying highp vec2 vs_TEXCOORD0;
					#define SV_Target0 gl_FragData[0]
					float u_xlat0;
					mediump float u_xlat16_1;
					mediump float u_xlat16_2;
					lowp float u_xlat10_2;
					void main()
					{
					    u_xlat0 = _Time.y * _Freq.z;
					    u_xlat0 = sin(u_xlat0);
					    u_xlat0 = u_xlat0 * 0.5 + 0.5;
					    u_xlat16_2 = (-_Freq.x) + _Freq.y;
					    u_xlat0 = u_xlat0 * u_xlat16_2 + _Freq.x;
					    u_xlat10_2 = texture2D(_MainTex, vs_TEXCOORD0.xy).x;
					    u_xlat0 = u_xlat0 * u_xlat10_2;
					    u_xlat16_1 = _Freq.w;
					    u_xlat16_1 = clamp(u_xlat16_1, 0.0, 1.0);
					    u_xlat16_1 = u_xlat0 * u_xlat16_1;
					    SV_Target0 = vec4(u_xlat16_1) * _Color;
					    return;
					}
					
					#endif"
				}
				SubProgram "gles hw_tier02 " {
					"!!GLES
					#ifdef VERTEX
					#version 100
					
					uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	vec4 _MainTex_ST;
					attribute highp vec4 in_POSITION0;
					attribute highp vec2 in_TEXCOORD0;
					varying highp vec2 vs_TEXCOORD0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					void main()
					{
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + hlslcc_mtx4x4unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    gl_Position = hlslcc_mtx4x4unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 100
					
					#ifdef GL_FRAGMENT_PRECISION_HIGH
					    precision highp float;
					#else
					    precision mediump float;
					#endif
					precision highp int;
					uniform 	vec4 _Time;
					uniform 	mediump vec4 _Freq;
					uniform 	mediump vec4 _Color;
					uniform lowp sampler2D _MainTex;
					varying highp vec2 vs_TEXCOORD0;
					#define SV_Target0 gl_FragData[0]
					float u_xlat0;
					mediump float u_xlat16_1;
					mediump float u_xlat16_2;
					lowp float u_xlat10_2;
					void main()
					{
					    u_xlat0 = _Time.y * _Freq.z;
					    u_xlat0 = sin(u_xlat0);
					    u_xlat0 = u_xlat0 * 0.5 + 0.5;
					    u_xlat16_2 = (-_Freq.x) + _Freq.y;
					    u_xlat0 = u_xlat0 * u_xlat16_2 + _Freq.x;
					    u_xlat10_2 = texture2D(_MainTex, vs_TEXCOORD0.xy).x;
					    u_xlat0 = u_xlat0 * u_xlat10_2;
					    u_xlat16_1 = _Freq.w;
					    u_xlat16_1 = clamp(u_xlat16_1, 0.0, 1.0);
					    u_xlat16_1 = u_xlat0 * u_xlat16_1;
					    SV_Target0 = vec4(u_xlat16_1) * _Color;
					    return;
					}
					
					#endif"
				}
				SubProgram "gles3 hw_tier00 " {
					"!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	vec4 _MainTex_ST;
					in highp vec4 in_POSITION0;
					in highp vec2 in_TEXCOORD0;
					out highp vec2 vs_TEXCOORD0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					void main()
					{
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + hlslcc_mtx4x4unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    gl_Position = hlslcc_mtx4x4unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp float;
					precision highp int;
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					uniform 	vec4 _Time;
					uniform 	mediump vec4 _Freq;
					uniform 	mediump vec4 _Color;
					UNITY_LOCATION(0) uniform mediump sampler2D _MainTex;
					in highp vec2 vs_TEXCOORD0;
					layout(location = 0) out mediump vec4 SV_Target0;
					float u_xlat0;
					mediump float u_xlat16_1;
					mediump float u_xlat16_2;
					void main()
					{
					    u_xlat0 = _Time.y * _Freq.z;
					    u_xlat0 = sin(u_xlat0);
					    u_xlat0 = u_xlat0 * 0.5 + 0.5;
					    u_xlat16_2 = (-_Freq.x) + _Freq.y;
					    u_xlat0 = u_xlat0 * u_xlat16_2 + _Freq.x;
					    u_xlat16_2 = texture(_MainTex, vs_TEXCOORD0.xy).x;
					    u_xlat0 = u_xlat0 * u_xlat16_2;
					    u_xlat16_1 = _Freq.w;
					#ifdef UNITY_ADRENO_ES3
					    u_xlat16_1 = min(max(u_xlat16_1, 0.0), 1.0);
					#else
					    u_xlat16_1 = clamp(u_xlat16_1, 0.0, 1.0);
					#endif
					    u_xlat16_1 = u_xlat0 * u_xlat16_1;
					    SV_Target0 = vec4(u_xlat16_1) * _Color;
					    return;
					}
					
					#endif"
				}
				SubProgram "gles3 hw_tier01 " {
					"!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	vec4 _MainTex_ST;
					in highp vec4 in_POSITION0;
					in highp vec2 in_TEXCOORD0;
					out highp vec2 vs_TEXCOORD0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					void main()
					{
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + hlslcc_mtx4x4unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    gl_Position = hlslcc_mtx4x4unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp float;
					precision highp int;
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					uniform 	vec4 _Time;
					uniform 	mediump vec4 _Freq;
					uniform 	mediump vec4 _Color;
					UNITY_LOCATION(0) uniform mediump sampler2D _MainTex;
					in highp vec2 vs_TEXCOORD0;
					layout(location = 0) out mediump vec4 SV_Target0;
					float u_xlat0;
					mediump float u_xlat16_1;
					mediump float u_xlat16_2;
					void main()
					{
					    u_xlat0 = _Time.y * _Freq.z;
					    u_xlat0 = sin(u_xlat0);
					    u_xlat0 = u_xlat0 * 0.5 + 0.5;
					    u_xlat16_2 = (-_Freq.x) + _Freq.y;
					    u_xlat0 = u_xlat0 * u_xlat16_2 + _Freq.x;
					    u_xlat16_2 = texture(_MainTex, vs_TEXCOORD0.xy).x;
					    u_xlat0 = u_xlat0 * u_xlat16_2;
					    u_xlat16_1 = _Freq.w;
					#ifdef UNITY_ADRENO_ES3
					    u_xlat16_1 = min(max(u_xlat16_1, 0.0), 1.0);
					#else
					    u_xlat16_1 = clamp(u_xlat16_1, 0.0, 1.0);
					#endif
					    u_xlat16_1 = u_xlat0 * u_xlat16_1;
					    SV_Target0 = vec4(u_xlat16_1) * _Color;
					    return;
					}
					
					#endif"
				}
				SubProgram "gles3 hw_tier02 " {
					"!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	vec4 _MainTex_ST;
					in highp vec4 in_POSITION0;
					in highp vec2 in_TEXCOORD0;
					out highp vec2 vs_TEXCOORD0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					void main()
					{
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + hlslcc_mtx4x4unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    gl_Position = hlslcc_mtx4x4unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp float;
					precision highp int;
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					uniform 	vec4 _Time;
					uniform 	mediump vec4 _Freq;
					uniform 	mediump vec4 _Color;
					UNITY_LOCATION(0) uniform mediump sampler2D _MainTex;
					in highp vec2 vs_TEXCOORD0;
					layout(location = 0) out mediump vec4 SV_Target0;
					float u_xlat0;
					mediump float u_xlat16_1;
					mediump float u_xlat16_2;
					void main()
					{
					    u_xlat0 = _Time.y * _Freq.z;
					    u_xlat0 = sin(u_xlat0);
					    u_xlat0 = u_xlat0 * 0.5 + 0.5;
					    u_xlat16_2 = (-_Freq.x) + _Freq.y;
					    u_xlat0 = u_xlat0 * u_xlat16_2 + _Freq.x;
					    u_xlat16_2 = texture(_MainTex, vs_TEXCOORD0.xy).x;
					    u_xlat0 = u_xlat0 * u_xlat16_2;
					    u_xlat16_1 = _Freq.w;
					#ifdef UNITY_ADRENO_ES3
					    u_xlat16_1 = min(max(u_xlat16_1, 0.0), 1.0);
					#else
					    u_xlat16_1 = clamp(u_xlat16_1, 0.0, 1.0);
					#endif
					    u_xlat16_1 = u_xlat0 * u_xlat16_1;
					    SV_Target0 = vec4(u_xlat16_1) * _Color;
					    return;
					}
					
					#endif"
				}
				SubProgram "vulkan hw_tier00 " {
					"spirv
					
					; SPIR-V
					; Version: 1.0
					; Generator: Khronos Glslang Reference Front End; 6
					; Bound: 102
					; Schema: 0
					                                                     OpCapability Shader 
					                                              %1 = OpExtInstImport "GLSL.std.450" 
					                                                     OpMemoryModel Logical GLSL450 
					                                                     OpEntryPoint Vertex %4 "main" %9 %11 %35 %86 
					                                                     OpName vs_TEXCOORD0 "vs_TEXCOORD0" 
					                                                     OpDecorate vs_TEXCOORD0 Location 9 
					                                                     OpDecorate %11 Location 11 
					                                                     OpDecorate %16 ArrayStride 16 
					                                                     OpDecorate %17 ArrayStride 17 
					                                                     OpMemberDecorate %18 0 Offset 18 
					                                                     OpMemberDecorate %18 1 Offset 18 
					                                                     OpMemberDecorate %18 2 Offset 18 
					                                                     OpDecorate %18 Block 
					                                                     OpDecorate %20 DescriptorSet 20 
					                                                     OpDecorate %20 Binding 20 
					                                                     OpDecorate %35 Location 35 
					                                                     OpMemberDecorate %84 0 BuiltIn 84 
					                                                     OpMemberDecorate %84 1 BuiltIn 84 
					                                                     OpMemberDecorate %84 2 BuiltIn 84 
					                                                     OpDecorate %84 Block 
					                                              %2 = OpTypeVoid 
					                                              %3 = OpTypeFunction %2 
					                                              %6 = OpTypeFloat 32 
					                                              %7 = OpTypeVector %6 2 
					                                              %8 = OpTypePointer Output %7 
					                      Output f32_2* vs_TEXCOORD0 = OpVariable Output 
					                                             %10 = OpTypePointer Input %7 
					                                Input f32_2* %11 = OpVariable Input 
					                                             %13 = OpTypeVector %6 4 
					                                             %14 = OpTypeInt 32 0 
					                                         u32 %15 = OpConstant 4 
					                                             %16 = OpTypeArray %13 %15 
					                                             %17 = OpTypeArray %13 %15 
					                                             %18 = OpTypeStruct %16 %17 %13 
					                                             %19 = OpTypePointer Uniform %18 
					Uniform struct {f32_4[4]; f32_4[4]; f32_4;}* %20 = OpVariable Uniform 
					                                             %21 = OpTypeInt 32 1 
					                                         i32 %22 = OpConstant 2 
					                                             %23 = OpTypePointer Uniform %13 
					                                             %32 = OpTypePointer Private %13 
					                              Private f32_4* %33 = OpVariable Private 
					                                             %34 = OpTypePointer Input %13 
					                                Input f32_4* %35 = OpVariable Input 
					                                         i32 %38 = OpConstant 0 
					                                         i32 %39 = OpConstant 1 
					                                         i32 %58 = OpConstant 3 
					                              Private f32_4* %62 = OpVariable Private 
					                                         u32 %82 = OpConstant 1 
					                                             %83 = OpTypeArray %6 %82 
					                                             %84 = OpTypeStruct %13 %6 %83 
					                                             %85 = OpTypePointer Output %84 
					        Output struct {f32_4; f32; f32[1];}* %86 = OpVariable Output 
					                                             %94 = OpTypePointer Output %13 
					                                             %96 = OpTypePointer Output %6 
					                                         void %4 = OpFunction None %3 
					                                              %5 = OpLabel 
					                                       f32_2 %12 = OpLoad %11 
					                              Uniform f32_4* %24 = OpAccessChain %20 %22 
					                                       f32_4 %25 = OpLoad %24 
					                                       f32_2 %26 = OpVectorShuffle %25 %25 0 1 
					                                       f32_2 %27 = OpFMul %12 %26 
					                              Uniform f32_4* %28 = OpAccessChain %20 %22 
					                                       f32_4 %29 = OpLoad %28 
					                                       f32_2 %30 = OpVectorShuffle %29 %29 2 3 
					                                       f32_2 %31 = OpFAdd %27 %30 
					                                                     OpStore vs_TEXCOORD0 %31 
					                                       f32_4 %36 = OpLoad %35 
					                                       f32_4 %37 = OpVectorShuffle %36 %36 1 1 1 1 
					                              Uniform f32_4* %40 = OpAccessChain %20 %38 %39 
					                                       f32_4 %41 = OpLoad %40 
					                                       f32_4 %42 = OpFMul %37 %41 
					                                                     OpStore %33 %42 
					                              Uniform f32_4* %43 = OpAccessChain %20 %38 %38 
					                                       f32_4 %44 = OpLoad %43 
					                                       f32_4 %45 = OpLoad %35 
					                                       f32_4 %46 = OpVectorShuffle %45 %45 0 0 0 0 
					                                       f32_4 %47 = OpFMul %44 %46 
					                                       f32_4 %48 = OpLoad %33 
					                                       f32_4 %49 = OpFAdd %47 %48 
					                                                     OpStore %33 %49 
					                              Uniform f32_4* %50 = OpAccessChain %20 %38 %22 
					                                       f32_4 %51 = OpLoad %50 
					                                       f32_4 %52 = OpLoad %35 
					                                       f32_4 %53 = OpVectorShuffle %52 %52 2 2 2 2 
					                                       f32_4 %54 = OpFMul %51 %53 
					                                       f32_4 %55 = OpLoad %33 
					                                       f32_4 %56 = OpFAdd %54 %55 
					                                                     OpStore %33 %56 
					                                       f32_4 %57 = OpLoad %33 
					                              Uniform f32_4* %59 = OpAccessChain %20 %38 %58 
					                                       f32_4 %60 = OpLoad %59 
					                                       f32_4 %61 = OpFAdd %57 %60 
					                                                     OpStore %33 %61 
					                                       f32_4 %63 = OpLoad %33 
					                                       f32_4 %64 = OpVectorShuffle %63 %63 1 1 1 1 
					                              Uniform f32_4* %65 = OpAccessChain %20 %39 %39 
					                                       f32_4 %66 = OpLoad %65 
					                                       f32_4 %67 = OpFMul %64 %66 
					                                                     OpStore %62 %67 
					                              Uniform f32_4* %68 = OpAccessChain %20 %39 %38 
					                                       f32_4 %69 = OpLoad %68 
					                                       f32_4 %70 = OpLoad %33 
					                                       f32_4 %71 = OpVectorShuffle %70 %70 0 0 0 0 
					                                       f32_4 %72 = OpFMul %69 %71 
					                                       f32_4 %73 = OpLoad %62 
					                                       f32_4 %74 = OpFAdd %72 %73 
					                                                     OpStore %62 %74 
					                              Uniform f32_4* %75 = OpAccessChain %20 %39 %22 
					                                       f32_4 %76 = OpLoad %75 
					                                       f32_4 %77 = OpLoad %33 
					                                       f32_4 %78 = OpVectorShuffle %77 %77 2 2 2 2 
					                                       f32_4 %79 = OpFMul %76 %78 
					                                       f32_4 %80 = OpLoad %62 
					                                       f32_4 %81 = OpFAdd %79 %80 
					                                                     OpStore %62 %81 
					                              Uniform f32_4* %87 = OpAccessChain %20 %39 %58 
					                                       f32_4 %88 = OpLoad %87 
					                                       f32_4 %89 = OpLoad %33 
					                                       f32_4 %90 = OpVectorShuffle %89 %89 3 3 3 3 
					                                       f32_4 %91 = OpFMul %88 %90 
					                                       f32_4 %92 = OpLoad %62 
					                                       f32_4 %93 = OpFAdd %91 %92 
					                               Output f32_4* %95 = OpAccessChain %86 %38 
					                                                     OpStore %95 %93 
					                                 Output f32* %97 = OpAccessChain %86 %38 %82 
					                                         f32 %98 = OpLoad %97 
					                                         f32 %99 = OpFNegate %98 
					                                Output f32* %100 = OpAccessChain %86 %38 %82 
					                                                     OpStore %100 %99 
					                                                     OpReturn
					                                                     OpFunctionEnd
					; SPIR-V
					; Version: 1.0
					; Generator: Khronos Glslang Reference Front End; 6
					; Bound: 86
					; Schema: 0
					                                               OpCapability Shader 
					                                        %1 = OpExtInstImport "GLSL.std.450" 
					                                               OpMemoryModel Logical GLSL450 
					                                               OpEntryPoint Fragment %4 "main" %58 %77 
					                                               OpExecutionMode %4 OriginUpperLeft 
					                                               OpName vs_TEXCOORD0 "vs_TEXCOORD0" 
					                                               OpMemberDecorate %10 0 Offset 10 
					                                               OpMemberDecorate %10 1 RelaxedPrecision 
					                                               OpMemberDecorate %10 1 Offset 10 
					                                               OpMemberDecorate %10 2 RelaxedPrecision 
					                                               OpMemberDecorate %10 2 Offset 10 
					                                               OpDecorate %10 Block 
					                                               OpDecorate %12 DescriptorSet 12 
					                                               OpDecorate %12 Binding 12 
					                                               OpDecorate %23 RelaxedPrecision 
					                                               OpDecorate %31 RelaxedPrecision 
					                                               OpDecorate %34 RelaxedPrecision 
					                                               OpDecorate %35 RelaxedPrecision 
					                                               OpDecorate %37 RelaxedPrecision 
					                                               OpDecorate %38 RelaxedPrecision 
					                                               OpDecorate %40 RelaxedPrecision 
					                                               OpDecorate %43 RelaxedPrecision 
					                                               OpDecorate %45 RelaxedPrecision 
					                                               OpDecorate %48 RelaxedPrecision 
					                                               OpDecorate %48 DescriptorSet 48 
					                                               OpDecorate %48 Binding 48 
					                                               OpDecorate %49 RelaxedPrecision 
					                                               OpDecorate %52 RelaxedPrecision 
					                                               OpDecorate %52 DescriptorSet 52 
					                                               OpDecorate %52 Binding 52 
					                                               OpDecorate %53 RelaxedPrecision 
					                                               OpDecorate vs_TEXCOORD0 Location 58 
					                                               OpDecorate %61 RelaxedPrecision 
					                                               OpDecorate %63 RelaxedPrecision 
					                                               OpDecorate %65 RelaxedPrecision 
					                                               OpDecorate %68 RelaxedPrecision 
					                                               OpDecorate %69 RelaxedPrecision 
					                                               OpDecorate %72 RelaxedPrecision 
					                                               OpDecorate %74 RelaxedPrecision 
					                                               OpDecorate %77 RelaxedPrecision 
					                                               OpDecorate %77 Location 77 
					                                               OpDecorate %78 RelaxedPrecision 
					                                               OpDecorate %79 RelaxedPrecision 
					                                               OpDecorate %83 RelaxedPrecision 
					                                               OpDecorate %84 RelaxedPrecision 
					                                        %2 = OpTypeVoid 
					                                        %3 = OpTypeFunction %2 
					                                        %6 = OpTypeFloat 32 
					                                        %7 = OpTypePointer Private %6 
					                           Private f32* %8 = OpVariable Private 
					                                        %9 = OpTypeVector %6 4 
					                                       %10 = OpTypeStruct %9 %9 %9 
					                                       %11 = OpTypePointer Uniform %10 
					Uniform struct {f32_4; f32_4; f32_4;}* %12 = OpVariable Uniform 
					                                       %13 = OpTypeInt 32 1 
					                                   i32 %14 = OpConstant 0 
					                                       %15 = OpTypeInt 32 0 
					                                   u32 %16 = OpConstant 1 
					                                       %17 = OpTypePointer Uniform %6 
					                                   i32 %20 = OpConstant 1 
					                                   u32 %21 = OpConstant 2 
					                                   f32 %28 = OpConstant 3.674022E-40 
					                          Private f32* %31 = OpVariable Private 
					                                   u32 %32 = OpConstant 0 
					                          Private f32* %45 = OpVariable Private 
					                                       %46 = OpTypeImage %6 Dim2D 0 0 0 1 Unknown 
					                                       %47 = OpTypePointer UniformConstant %46 
					  UniformConstant read_only Texture2D* %48 = OpVariable UniformConstant 
					                                       %50 = OpTypeSampler 
					                                       %51 = OpTypePointer UniformConstant %50 
					              UniformConstant sampler* %52 = OpVariable UniformConstant 
					                                       %54 = OpTypeSampledImage %46 
					                                       %56 = OpTypeVector %6 2 
					                                       %57 = OpTypePointer Input %56 
					                 Input f32_2* vs_TEXCOORD0 = OpVariable Input 
					                          Private f32* %65 = OpVariable Private 
					                                   u32 %66 = OpConstant 3 
					                                   f32 %70 = OpConstant 3.674022E-40 
					                                   f32 %71 = OpConstant 3.674022E-40 
					                                       %76 = OpTypePointer Output %9 
					                         Output f32_4* %77 = OpVariable Output 
					                                   i32 %80 = OpConstant 2 
					                                       %81 = OpTypePointer Uniform %9 
					                                   void %4 = OpFunction None %3 
					                                        %5 = OpLabel 
					                          Uniform f32* %18 = OpAccessChain %12 %14 %16 
					                                   f32 %19 = OpLoad %18 
					                          Uniform f32* %22 = OpAccessChain %12 %20 %21 
					                                   f32 %23 = OpLoad %22 
					                                   f32 %24 = OpFMul %19 %23 
					                                               OpStore %8 %24 
					                                   f32 %25 = OpLoad %8 
					                                   f32 %26 = OpExtInst %1 13 %25 
					                                               OpStore %8 %26 
					                                   f32 %27 = OpLoad %8 
					                                   f32 %29 = OpFMul %27 %28 
					                                   f32 %30 = OpFAdd %29 %28 
					                                               OpStore %8 %30 
					                          Uniform f32* %33 = OpAccessChain %12 %20 %32 
					                                   f32 %34 = OpLoad %33 
					                                   f32 %35 = OpFNegate %34 
					                          Uniform f32* %36 = OpAccessChain %12 %20 %16 
					                                   f32 %37 = OpLoad %36 
					                                   f32 %38 = OpFAdd %35 %37 
					                                               OpStore %31 %38 
					                                   f32 %39 = OpLoad %8 
					                                   f32 %40 = OpLoad %31 
					                                   f32 %41 = OpFMul %39 %40 
					                          Uniform f32* %42 = OpAccessChain %12 %20 %32 
					                                   f32 %43 = OpLoad %42 
					                                   f32 %44 = OpFAdd %41 %43 
					                                               OpStore %8 %44 
					                   read_only Texture2D %49 = OpLoad %48 
					                               sampler %53 = OpLoad %52 
					            read_only Texture2DSampled %55 = OpSampledImage %49 %53 
					                                 f32_2 %59 = OpLoad vs_TEXCOORD0 
					                                 f32_4 %60 = OpImageSampleImplicitLod %55 %59 
					                                   f32 %61 = OpCompositeExtract %60 0 
					                                               OpStore %45 %61 
					                                   f32 %62 = OpLoad %8 
					                                   f32 %63 = OpLoad %45 
					                                   f32 %64 = OpFMul %62 %63 
					                                               OpStore %8 %64 
					                          Uniform f32* %67 = OpAccessChain %12 %20 %66 
					                                   f32 %68 = OpLoad %67 
					                                               OpStore %65 %68 
					                                   f32 %69 = OpLoad %65 
					                                   f32 %72 = OpExtInst %1 43 %69 %70 %71 
					                                               OpStore %65 %72 
					                                   f32 %73 = OpLoad %8 
					                                   f32 %74 = OpLoad %65 
					                                   f32 %75 = OpFMul %73 %74 
					                                               OpStore %65 %75 
					                                   f32 %78 = OpLoad %65 
					                                 f32_4 %79 = OpCompositeConstruct %78 %78 %78 %78 
					                        Uniform f32_4* %82 = OpAccessChain %12 %80 
					                                 f32_4 %83 = OpLoad %82 
					                                 f32_4 %84 = OpFMul %79 %83 
					                                               OpStore %77 %84 
					                                               OpReturn
					                                               OpFunctionEnd"
				}
				SubProgram "vulkan hw_tier01 " {
					"spirv
					
					; SPIR-V
					; Version: 1.0
					; Generator: Khronos Glslang Reference Front End; 6
					; Bound: 102
					; Schema: 0
					                                                     OpCapability Shader 
					                                              %1 = OpExtInstImport "GLSL.std.450" 
					                                                     OpMemoryModel Logical GLSL450 
					                                                     OpEntryPoint Vertex %4 "main" %9 %11 %35 %86 
					                                                     OpName vs_TEXCOORD0 "vs_TEXCOORD0" 
					                                                     OpDecorate vs_TEXCOORD0 Location 9 
					                                                     OpDecorate %11 Location 11 
					                                                     OpDecorate %16 ArrayStride 16 
					                                                     OpDecorate %17 ArrayStride 17 
					                                                     OpMemberDecorate %18 0 Offset 18 
					                                                     OpMemberDecorate %18 1 Offset 18 
					                                                     OpMemberDecorate %18 2 Offset 18 
					                                                     OpDecorate %18 Block 
					                                                     OpDecorate %20 DescriptorSet 20 
					                                                     OpDecorate %20 Binding 20 
					                                                     OpDecorate %35 Location 35 
					                                                     OpMemberDecorate %84 0 BuiltIn 84 
					                                                     OpMemberDecorate %84 1 BuiltIn 84 
					                                                     OpMemberDecorate %84 2 BuiltIn 84 
					                                                     OpDecorate %84 Block 
					                                              %2 = OpTypeVoid 
					                                              %3 = OpTypeFunction %2 
					                                              %6 = OpTypeFloat 32 
					                                              %7 = OpTypeVector %6 2 
					                                              %8 = OpTypePointer Output %7 
					                      Output f32_2* vs_TEXCOORD0 = OpVariable Output 
					                                             %10 = OpTypePointer Input %7 
					                                Input f32_2* %11 = OpVariable Input 
					                                             %13 = OpTypeVector %6 4 
					                                             %14 = OpTypeInt 32 0 
					                                         u32 %15 = OpConstant 4 
					                                             %16 = OpTypeArray %13 %15 
					                                             %17 = OpTypeArray %13 %15 
					                                             %18 = OpTypeStruct %16 %17 %13 
					                                             %19 = OpTypePointer Uniform %18 
					Uniform struct {f32_4[4]; f32_4[4]; f32_4;}* %20 = OpVariable Uniform 
					                                             %21 = OpTypeInt 32 1 
					                                         i32 %22 = OpConstant 2 
					                                             %23 = OpTypePointer Uniform %13 
					                                             %32 = OpTypePointer Private %13 
					                              Private f32_4* %33 = OpVariable Private 
					                                             %34 = OpTypePointer Input %13 
					                                Input f32_4* %35 = OpVariable Input 
					                                         i32 %38 = OpConstant 0 
					                                         i32 %39 = OpConstant 1 
					                                         i32 %58 = OpConstant 3 
					                              Private f32_4* %62 = OpVariable Private 
					                                         u32 %82 = OpConstant 1 
					                                             %83 = OpTypeArray %6 %82 
					                                             %84 = OpTypeStruct %13 %6 %83 
					                                             %85 = OpTypePointer Output %84 
					        Output struct {f32_4; f32; f32[1];}* %86 = OpVariable Output 
					                                             %94 = OpTypePointer Output %13 
					                                             %96 = OpTypePointer Output %6 
					                                         void %4 = OpFunction None %3 
					                                              %5 = OpLabel 
					                                       f32_2 %12 = OpLoad %11 
					                              Uniform f32_4* %24 = OpAccessChain %20 %22 
					                                       f32_4 %25 = OpLoad %24 
					                                       f32_2 %26 = OpVectorShuffle %25 %25 0 1 
					                                       f32_2 %27 = OpFMul %12 %26 
					                              Uniform f32_4* %28 = OpAccessChain %20 %22 
					                                       f32_4 %29 = OpLoad %28 
					                                       f32_2 %30 = OpVectorShuffle %29 %29 2 3 
					                                       f32_2 %31 = OpFAdd %27 %30 
					                                                     OpStore vs_TEXCOORD0 %31 
					                                       f32_4 %36 = OpLoad %35 
					                                       f32_4 %37 = OpVectorShuffle %36 %36 1 1 1 1 
					                              Uniform f32_4* %40 = OpAccessChain %20 %38 %39 
					                                       f32_4 %41 = OpLoad %40 
					                                       f32_4 %42 = OpFMul %37 %41 
					                                                     OpStore %33 %42 
					                              Uniform f32_4* %43 = OpAccessChain %20 %38 %38 
					                                       f32_4 %44 = OpLoad %43 
					                                       f32_4 %45 = OpLoad %35 
					                                       f32_4 %46 = OpVectorShuffle %45 %45 0 0 0 0 
					                                       f32_4 %47 = OpFMul %44 %46 
					                                       f32_4 %48 = OpLoad %33 
					                                       f32_4 %49 = OpFAdd %47 %48 
					                                                     OpStore %33 %49 
					                              Uniform f32_4* %50 = OpAccessChain %20 %38 %22 
					                                       f32_4 %51 = OpLoad %50 
					                                       f32_4 %52 = OpLoad %35 
					                                       f32_4 %53 = OpVectorShuffle %52 %52 2 2 2 2 
					                                       f32_4 %54 = OpFMul %51 %53 
					                                       f32_4 %55 = OpLoad %33 
					                                       f32_4 %56 = OpFAdd %54 %55 
					                                                     OpStore %33 %56 
					                                       f32_4 %57 = OpLoad %33 
					                              Uniform f32_4* %59 = OpAccessChain %20 %38 %58 
					                                       f32_4 %60 = OpLoad %59 
					                                       f32_4 %61 = OpFAdd %57 %60 
					                                                     OpStore %33 %61 
					                                       f32_4 %63 = OpLoad %33 
					                                       f32_4 %64 = OpVectorShuffle %63 %63 1 1 1 1 
					                              Uniform f32_4* %65 = OpAccessChain %20 %39 %39 
					                                       f32_4 %66 = OpLoad %65 
					                                       f32_4 %67 = OpFMul %64 %66 
					                                                     OpStore %62 %67 
					                              Uniform f32_4* %68 = OpAccessChain %20 %39 %38 
					                                       f32_4 %69 = OpLoad %68 
					                                       f32_4 %70 = OpLoad %33 
					                                       f32_4 %71 = OpVectorShuffle %70 %70 0 0 0 0 
					                                       f32_4 %72 = OpFMul %69 %71 
					                                       f32_4 %73 = OpLoad %62 
					                                       f32_4 %74 = OpFAdd %72 %73 
					                                                     OpStore %62 %74 
					                              Uniform f32_4* %75 = OpAccessChain %20 %39 %22 
					                                       f32_4 %76 = OpLoad %75 
					                                       f32_4 %77 = OpLoad %33 
					                                       f32_4 %78 = OpVectorShuffle %77 %77 2 2 2 2 
					                                       f32_4 %79 = OpFMul %76 %78 
					                                       f32_4 %80 = OpLoad %62 
					                                       f32_4 %81 = OpFAdd %79 %80 
					                                                     OpStore %62 %81 
					                              Uniform f32_4* %87 = OpAccessChain %20 %39 %58 
					                                       f32_4 %88 = OpLoad %87 
					                                       f32_4 %89 = OpLoad %33 
					                                       f32_4 %90 = OpVectorShuffle %89 %89 3 3 3 3 
					                                       f32_4 %91 = OpFMul %88 %90 
					                                       f32_4 %92 = OpLoad %62 
					                                       f32_4 %93 = OpFAdd %91 %92 
					                               Output f32_4* %95 = OpAccessChain %86 %38 
					                                                     OpStore %95 %93 
					                                 Output f32* %97 = OpAccessChain %86 %38 %82 
					                                         f32 %98 = OpLoad %97 
					                                         f32 %99 = OpFNegate %98 
					                                Output f32* %100 = OpAccessChain %86 %38 %82 
					                                                     OpStore %100 %99 
					                                                     OpReturn
					                                                     OpFunctionEnd
					; SPIR-V
					; Version: 1.0
					; Generator: Khronos Glslang Reference Front End; 6
					; Bound: 86
					; Schema: 0
					                                               OpCapability Shader 
					                                        %1 = OpExtInstImport "GLSL.std.450" 
					                                               OpMemoryModel Logical GLSL450 
					                                               OpEntryPoint Fragment %4 "main" %58 %77 
					                                               OpExecutionMode %4 OriginUpperLeft 
					                                               OpName vs_TEXCOORD0 "vs_TEXCOORD0" 
					                                               OpMemberDecorate %10 0 Offset 10 
					                                               OpMemberDecorate %10 1 RelaxedPrecision 
					                                               OpMemberDecorate %10 1 Offset 10 
					                                               OpMemberDecorate %10 2 RelaxedPrecision 
					                                               OpMemberDecorate %10 2 Offset 10 
					                                               OpDecorate %10 Block 
					                                               OpDecorate %12 DescriptorSet 12 
					                                               OpDecorate %12 Binding 12 
					                                               OpDecorate %23 RelaxedPrecision 
					                                               OpDecorate %31 RelaxedPrecision 
					                                               OpDecorate %34 RelaxedPrecision 
					                                               OpDecorate %35 RelaxedPrecision 
					                                               OpDecorate %37 RelaxedPrecision 
					                                               OpDecorate %38 RelaxedPrecision 
					                                               OpDecorate %40 RelaxedPrecision 
					                                               OpDecorate %43 RelaxedPrecision 
					                                               OpDecorate %45 RelaxedPrecision 
					                                               OpDecorate %48 RelaxedPrecision 
					                                               OpDecorate %48 DescriptorSet 48 
					                                               OpDecorate %48 Binding 48 
					                                               OpDecorate %49 RelaxedPrecision 
					                                               OpDecorate %52 RelaxedPrecision 
					                                               OpDecorate %52 DescriptorSet 52 
					                                               OpDecorate %52 Binding 52 
					                                               OpDecorate %53 RelaxedPrecision 
					                                               OpDecorate vs_TEXCOORD0 Location 58 
					                                               OpDecorate %61 RelaxedPrecision 
					                                               OpDecorate %63 RelaxedPrecision 
					                                               OpDecorate %65 RelaxedPrecision 
					                                               OpDecorate %68 RelaxedPrecision 
					                                               OpDecorate %69 RelaxedPrecision 
					                                               OpDecorate %72 RelaxedPrecision 
					                                               OpDecorate %74 RelaxedPrecision 
					                                               OpDecorate %77 RelaxedPrecision 
					                                               OpDecorate %77 Location 77 
					                                               OpDecorate %78 RelaxedPrecision 
					                                               OpDecorate %79 RelaxedPrecision 
					                                               OpDecorate %83 RelaxedPrecision 
					                                               OpDecorate %84 RelaxedPrecision 
					                                        %2 = OpTypeVoid 
					                                        %3 = OpTypeFunction %2 
					                                        %6 = OpTypeFloat 32 
					                                        %7 = OpTypePointer Private %6 
					                           Private f32* %8 = OpVariable Private 
					                                        %9 = OpTypeVector %6 4 
					                                       %10 = OpTypeStruct %9 %9 %9 
					                                       %11 = OpTypePointer Uniform %10 
					Uniform struct {f32_4; f32_4; f32_4;}* %12 = OpVariable Uniform 
					                                       %13 = OpTypeInt 32 1 
					                                   i32 %14 = OpConstant 0 
					                                       %15 = OpTypeInt 32 0 
					                                   u32 %16 = OpConstant 1 
					                                       %17 = OpTypePointer Uniform %6 
					                                   i32 %20 = OpConstant 1 
					                                   u32 %21 = OpConstant 2 
					                                   f32 %28 = OpConstant 3.674022E-40 
					                          Private f32* %31 = OpVariable Private 
					                                   u32 %32 = OpConstant 0 
					                          Private f32* %45 = OpVariable Private 
					                                       %46 = OpTypeImage %6 Dim2D 0 0 0 1 Unknown 
					                                       %47 = OpTypePointer UniformConstant %46 
					  UniformConstant read_only Texture2D* %48 = OpVariable UniformConstant 
					                                       %50 = OpTypeSampler 
					                                       %51 = OpTypePointer UniformConstant %50 
					              UniformConstant sampler* %52 = OpVariable UniformConstant 
					                                       %54 = OpTypeSampledImage %46 
					                                       %56 = OpTypeVector %6 2 
					                                       %57 = OpTypePointer Input %56 
					                 Input f32_2* vs_TEXCOORD0 = OpVariable Input 
					                          Private f32* %65 = OpVariable Private 
					                                   u32 %66 = OpConstant 3 
					                                   f32 %70 = OpConstant 3.674022E-40 
					                                   f32 %71 = OpConstant 3.674022E-40 
					                                       %76 = OpTypePointer Output %9 
					                         Output f32_4* %77 = OpVariable Output 
					                                   i32 %80 = OpConstant 2 
					                                       %81 = OpTypePointer Uniform %9 
					                                   void %4 = OpFunction None %3 
					                                        %5 = OpLabel 
					                          Uniform f32* %18 = OpAccessChain %12 %14 %16 
					                                   f32 %19 = OpLoad %18 
					                          Uniform f32* %22 = OpAccessChain %12 %20 %21 
					                                   f32 %23 = OpLoad %22 
					                                   f32 %24 = OpFMul %19 %23 
					                                               OpStore %8 %24 
					                                   f32 %25 = OpLoad %8 
					                                   f32 %26 = OpExtInst %1 13 %25 
					                                               OpStore %8 %26 
					                                   f32 %27 = OpLoad %8 
					                                   f32 %29 = OpFMul %27 %28 
					                                   f32 %30 = OpFAdd %29 %28 
					                                               OpStore %8 %30 
					                          Uniform f32* %33 = OpAccessChain %12 %20 %32 
					                                   f32 %34 = OpLoad %33 
					                                   f32 %35 = OpFNegate %34 
					                          Uniform f32* %36 = OpAccessChain %12 %20 %16 
					                                   f32 %37 = OpLoad %36 
					                                   f32 %38 = OpFAdd %35 %37 
					                                               OpStore %31 %38 
					                                   f32 %39 = OpLoad %8 
					                                   f32 %40 = OpLoad %31 
					                                   f32 %41 = OpFMul %39 %40 
					                          Uniform f32* %42 = OpAccessChain %12 %20 %32 
					                                   f32 %43 = OpLoad %42 
					                                   f32 %44 = OpFAdd %41 %43 
					                                               OpStore %8 %44 
					                   read_only Texture2D %49 = OpLoad %48 
					                               sampler %53 = OpLoad %52 
					            read_only Texture2DSampled %55 = OpSampledImage %49 %53 
					                                 f32_2 %59 = OpLoad vs_TEXCOORD0 
					                                 f32_4 %60 = OpImageSampleImplicitLod %55 %59 
					                                   f32 %61 = OpCompositeExtract %60 0 
					                                               OpStore %45 %61 
					                                   f32 %62 = OpLoad %8 
					                                   f32 %63 = OpLoad %45 
					                                   f32 %64 = OpFMul %62 %63 
					                                               OpStore %8 %64 
					                          Uniform f32* %67 = OpAccessChain %12 %20 %66 
					                                   f32 %68 = OpLoad %67 
					                                               OpStore %65 %68 
					                                   f32 %69 = OpLoad %65 
					                                   f32 %72 = OpExtInst %1 43 %69 %70 %71 
					                                               OpStore %65 %72 
					                                   f32 %73 = OpLoad %8 
					                                   f32 %74 = OpLoad %65 
					                                   f32 %75 = OpFMul %73 %74 
					                                               OpStore %65 %75 
					                                   f32 %78 = OpLoad %65 
					                                 f32_4 %79 = OpCompositeConstruct %78 %78 %78 %78 
					                        Uniform f32_4* %82 = OpAccessChain %12 %80 
					                                 f32_4 %83 = OpLoad %82 
					                                 f32_4 %84 = OpFMul %79 %83 
					                                               OpStore %77 %84 
					                                               OpReturn
					                                               OpFunctionEnd"
				}
				SubProgram "vulkan hw_tier02 " {
					"spirv
					
					; SPIR-V
					; Version: 1.0
					; Generator: Khronos Glslang Reference Front End; 6
					; Bound: 102
					; Schema: 0
					                                                     OpCapability Shader 
					                                              %1 = OpExtInstImport "GLSL.std.450" 
					                                                     OpMemoryModel Logical GLSL450 
					                                                     OpEntryPoint Vertex %4 "main" %9 %11 %35 %86 
					                                                     OpName vs_TEXCOORD0 "vs_TEXCOORD0" 
					                                                     OpDecorate vs_TEXCOORD0 Location 9 
					                                                     OpDecorate %11 Location 11 
					                                                     OpDecorate %16 ArrayStride 16 
					                                                     OpDecorate %17 ArrayStride 17 
					                                                     OpMemberDecorate %18 0 Offset 18 
					                                                     OpMemberDecorate %18 1 Offset 18 
					                                                     OpMemberDecorate %18 2 Offset 18 
					                                                     OpDecorate %18 Block 
					                                                     OpDecorate %20 DescriptorSet 20 
					                                                     OpDecorate %20 Binding 20 
					                                                     OpDecorate %35 Location 35 
					                                                     OpMemberDecorate %84 0 BuiltIn 84 
					                                                     OpMemberDecorate %84 1 BuiltIn 84 
					                                                     OpMemberDecorate %84 2 BuiltIn 84 
					                                                     OpDecorate %84 Block 
					                                              %2 = OpTypeVoid 
					                                              %3 = OpTypeFunction %2 
					                                              %6 = OpTypeFloat 32 
					                                              %7 = OpTypeVector %6 2 
					                                              %8 = OpTypePointer Output %7 
					                      Output f32_2* vs_TEXCOORD0 = OpVariable Output 
					                                             %10 = OpTypePointer Input %7 
					                                Input f32_2* %11 = OpVariable Input 
					                                             %13 = OpTypeVector %6 4 
					                                             %14 = OpTypeInt 32 0 
					                                         u32 %15 = OpConstant 4 
					                                             %16 = OpTypeArray %13 %15 
					                                             %17 = OpTypeArray %13 %15 
					                                             %18 = OpTypeStruct %16 %17 %13 
					                                             %19 = OpTypePointer Uniform %18 
					Uniform struct {f32_4[4]; f32_4[4]; f32_4;}* %20 = OpVariable Uniform 
					                                             %21 = OpTypeInt 32 1 
					                                         i32 %22 = OpConstant 2 
					                                             %23 = OpTypePointer Uniform %13 
					                                             %32 = OpTypePointer Private %13 
					                              Private f32_4* %33 = OpVariable Private 
					                                             %34 = OpTypePointer Input %13 
					                                Input f32_4* %35 = OpVariable Input 
					                                         i32 %38 = OpConstant 0 
					                                         i32 %39 = OpConstant 1 
					                                         i32 %58 = OpConstant 3 
					                              Private f32_4* %62 = OpVariable Private 
					                                         u32 %82 = OpConstant 1 
					                                             %83 = OpTypeArray %6 %82 
					                                             %84 = OpTypeStruct %13 %6 %83 
					                                             %85 = OpTypePointer Output %84 
					        Output struct {f32_4; f32; f32[1];}* %86 = OpVariable Output 
					                                             %94 = OpTypePointer Output %13 
					                                             %96 = OpTypePointer Output %6 
					                                         void %4 = OpFunction None %3 
					                                              %5 = OpLabel 
					                                       f32_2 %12 = OpLoad %11 
					                              Uniform f32_4* %24 = OpAccessChain %20 %22 
					                                       f32_4 %25 = OpLoad %24 
					                                       f32_2 %26 = OpVectorShuffle %25 %25 0 1 
					                                       f32_2 %27 = OpFMul %12 %26 
					                              Uniform f32_4* %28 = OpAccessChain %20 %22 
					                                       f32_4 %29 = OpLoad %28 
					                                       f32_2 %30 = OpVectorShuffle %29 %29 2 3 
					                                       f32_2 %31 = OpFAdd %27 %30 
					                                                     OpStore vs_TEXCOORD0 %31 
					                                       f32_4 %36 = OpLoad %35 
					                                       f32_4 %37 = OpVectorShuffle %36 %36 1 1 1 1 
					                              Uniform f32_4* %40 = OpAccessChain %20 %38 %39 
					                                       f32_4 %41 = OpLoad %40 
					                                       f32_4 %42 = OpFMul %37 %41 
					                                                     OpStore %33 %42 
					                              Uniform f32_4* %43 = OpAccessChain %20 %38 %38 
					                                       f32_4 %44 = OpLoad %43 
					                                       f32_4 %45 = OpLoad %35 
					                                       f32_4 %46 = OpVectorShuffle %45 %45 0 0 0 0 
					                                       f32_4 %47 = OpFMul %44 %46 
					                                       f32_4 %48 = OpLoad %33 
					                                       f32_4 %49 = OpFAdd %47 %48 
					                                                     OpStore %33 %49 
					                              Uniform f32_4* %50 = OpAccessChain %20 %38 %22 
					                                       f32_4 %51 = OpLoad %50 
					                                       f32_4 %52 = OpLoad %35 
					                                       f32_4 %53 = OpVectorShuffle %52 %52 2 2 2 2 
					                                       f32_4 %54 = OpFMul %51 %53 
					                                       f32_4 %55 = OpLoad %33 
					                                       f32_4 %56 = OpFAdd %54 %55 
					                                                     OpStore %33 %56 
					                                       f32_4 %57 = OpLoad %33 
					                              Uniform f32_4* %59 = OpAccessChain %20 %38 %58 
					                                       f32_4 %60 = OpLoad %59 
					                                       f32_4 %61 = OpFAdd %57 %60 
					                                                     OpStore %33 %61 
					                                       f32_4 %63 = OpLoad %33 
					                                       f32_4 %64 = OpVectorShuffle %63 %63 1 1 1 1 
					                              Uniform f32_4* %65 = OpAccessChain %20 %39 %39 
					                                       f32_4 %66 = OpLoad %65 
					                                       f32_4 %67 = OpFMul %64 %66 
					                                                     OpStore %62 %67 
					                              Uniform f32_4* %68 = OpAccessChain %20 %39 %38 
					                                       f32_4 %69 = OpLoad %68 
					                                       f32_4 %70 = OpLoad %33 
					                                       f32_4 %71 = OpVectorShuffle %70 %70 0 0 0 0 
					                                       f32_4 %72 = OpFMul %69 %71 
					                                       f32_4 %73 = OpLoad %62 
					                                       f32_4 %74 = OpFAdd %72 %73 
					                                                     OpStore %62 %74 
					                              Uniform f32_4* %75 = OpAccessChain %20 %39 %22 
					                                       f32_4 %76 = OpLoad %75 
					                                       f32_4 %77 = OpLoad %33 
					                                       f32_4 %78 = OpVectorShuffle %77 %77 2 2 2 2 
					                                       f32_4 %79 = OpFMul %76 %78 
					                                       f32_4 %80 = OpLoad %62 
					                                       f32_4 %81 = OpFAdd %79 %80 
					                                                     OpStore %62 %81 
					                              Uniform f32_4* %87 = OpAccessChain %20 %39 %58 
					                                       f32_4 %88 = OpLoad %87 
					                                       f32_4 %89 = OpLoad %33 
					                                       f32_4 %90 = OpVectorShuffle %89 %89 3 3 3 3 
					                                       f32_4 %91 = OpFMul %88 %90 
					                                       f32_4 %92 = OpLoad %62 
					                                       f32_4 %93 = OpFAdd %91 %92 
					                               Output f32_4* %95 = OpAccessChain %86 %38 
					                                                     OpStore %95 %93 
					                                 Output f32* %97 = OpAccessChain %86 %38 %82 
					                                         f32 %98 = OpLoad %97 
					                                         f32 %99 = OpFNegate %98 
					                                Output f32* %100 = OpAccessChain %86 %38 %82 
					                                                     OpStore %100 %99 
					                                                     OpReturn
					                                                     OpFunctionEnd
					; SPIR-V
					; Version: 1.0
					; Generator: Khronos Glslang Reference Front End; 6
					; Bound: 86
					; Schema: 0
					                                               OpCapability Shader 
					                                        %1 = OpExtInstImport "GLSL.std.450" 
					                                               OpMemoryModel Logical GLSL450 
					                                               OpEntryPoint Fragment %4 "main" %58 %77 
					                                               OpExecutionMode %4 OriginUpperLeft 
					                                               OpName vs_TEXCOORD0 "vs_TEXCOORD0" 
					                                               OpMemberDecorate %10 0 Offset 10 
					                                               OpMemberDecorate %10 1 RelaxedPrecision 
					                                               OpMemberDecorate %10 1 Offset 10 
					                                               OpMemberDecorate %10 2 RelaxedPrecision 
					                                               OpMemberDecorate %10 2 Offset 10 
					                                               OpDecorate %10 Block 
					                                               OpDecorate %12 DescriptorSet 12 
					                                               OpDecorate %12 Binding 12 
					                                               OpDecorate %23 RelaxedPrecision 
					                                               OpDecorate %31 RelaxedPrecision 
					                                               OpDecorate %34 RelaxedPrecision 
					                                               OpDecorate %35 RelaxedPrecision 
					                                               OpDecorate %37 RelaxedPrecision 
					                                               OpDecorate %38 RelaxedPrecision 
					                                               OpDecorate %40 RelaxedPrecision 
					                                               OpDecorate %43 RelaxedPrecision 
					                                               OpDecorate %45 RelaxedPrecision 
					                                               OpDecorate %48 RelaxedPrecision 
					                                               OpDecorate %48 DescriptorSet 48 
					                                               OpDecorate %48 Binding 48 
					                                               OpDecorate %49 RelaxedPrecision 
					                                               OpDecorate %52 RelaxedPrecision 
					                                               OpDecorate %52 DescriptorSet 52 
					                                               OpDecorate %52 Binding 52 
					                                               OpDecorate %53 RelaxedPrecision 
					                                               OpDecorate vs_TEXCOORD0 Location 58 
					                                               OpDecorate %61 RelaxedPrecision 
					                                               OpDecorate %63 RelaxedPrecision 
					                                               OpDecorate %65 RelaxedPrecision 
					                                               OpDecorate %68 RelaxedPrecision 
					                                               OpDecorate %69 RelaxedPrecision 
					                                               OpDecorate %72 RelaxedPrecision 
					                                               OpDecorate %74 RelaxedPrecision 
					                                               OpDecorate %77 RelaxedPrecision 
					                                               OpDecorate %77 Location 77 
					                                               OpDecorate %78 RelaxedPrecision 
					                                               OpDecorate %79 RelaxedPrecision 
					                                               OpDecorate %83 RelaxedPrecision 
					                                               OpDecorate %84 RelaxedPrecision 
					                                        %2 = OpTypeVoid 
					                                        %3 = OpTypeFunction %2 
					                                        %6 = OpTypeFloat 32 
					                                        %7 = OpTypePointer Private %6 
					                           Private f32* %8 = OpVariable Private 
					                                        %9 = OpTypeVector %6 4 
					                                       %10 = OpTypeStruct %9 %9 %9 
					                                       %11 = OpTypePointer Uniform %10 
					Uniform struct {f32_4; f32_4; f32_4;}* %12 = OpVariable Uniform 
					                                       %13 = OpTypeInt 32 1 
					                                   i32 %14 = OpConstant 0 
					                                       %15 = OpTypeInt 32 0 
					                                   u32 %16 = OpConstant 1 
					                                       %17 = OpTypePointer Uniform %6 
					                                   i32 %20 = OpConstant 1 
					                                   u32 %21 = OpConstant 2 
					                                   f32 %28 = OpConstant 3.674022E-40 
					                          Private f32* %31 = OpVariable Private 
					                                   u32 %32 = OpConstant 0 
					                          Private f32* %45 = OpVariable Private 
					                                       %46 = OpTypeImage %6 Dim2D 0 0 0 1 Unknown 
					                                       %47 = OpTypePointer UniformConstant %46 
					  UniformConstant read_only Texture2D* %48 = OpVariable UniformConstant 
					                                       %50 = OpTypeSampler 
					                                       %51 = OpTypePointer UniformConstant %50 
					              UniformConstant sampler* %52 = OpVariable UniformConstant 
					                                       %54 = OpTypeSampledImage %46 
					                                       %56 = OpTypeVector %6 2 
					                                       %57 = OpTypePointer Input %56 
					                 Input f32_2* vs_TEXCOORD0 = OpVariable Input 
					                          Private f32* %65 = OpVariable Private 
					                                   u32 %66 = OpConstant 3 
					                                   f32 %70 = OpConstant 3.674022E-40 
					                                   f32 %71 = OpConstant 3.674022E-40 
					                                       %76 = OpTypePointer Output %9 
					                         Output f32_4* %77 = OpVariable Output 
					                                   i32 %80 = OpConstant 2 
					                                       %81 = OpTypePointer Uniform %9 
					                                   void %4 = OpFunction None %3 
					                                        %5 = OpLabel 
					                          Uniform f32* %18 = OpAccessChain %12 %14 %16 
					                                   f32 %19 = OpLoad %18 
					                          Uniform f32* %22 = OpAccessChain %12 %20 %21 
					                                   f32 %23 = OpLoad %22 
					                                   f32 %24 = OpFMul %19 %23 
					                                               OpStore %8 %24 
					                                   f32 %25 = OpLoad %8 
					                                   f32 %26 = OpExtInst %1 13 %25 
					                                               OpStore %8 %26 
					                                   f32 %27 = OpLoad %8 
					                                   f32 %29 = OpFMul %27 %28 
					                                   f32 %30 = OpFAdd %29 %28 
					                                               OpStore %8 %30 
					                          Uniform f32* %33 = OpAccessChain %12 %20 %32 
					                                   f32 %34 = OpLoad %33 
					                                   f32 %35 = OpFNegate %34 
					                          Uniform f32* %36 = OpAccessChain %12 %20 %16 
					                                   f32 %37 = OpLoad %36 
					                                   f32 %38 = OpFAdd %35 %37 
					                                               OpStore %31 %38 
					                                   f32 %39 = OpLoad %8 
					                                   f32 %40 = OpLoad %31 
					                                   f32 %41 = OpFMul %39 %40 
					                          Uniform f32* %42 = OpAccessChain %12 %20 %32 
					                                   f32 %43 = OpLoad %42 
					                                   f32 %44 = OpFAdd %41 %43 
					                                               OpStore %8 %44 
					                   read_only Texture2D %49 = OpLoad %48 
					                               sampler %53 = OpLoad %52 
					            read_only Texture2DSampled %55 = OpSampledImage %49 %53 
					                                 f32_2 %59 = OpLoad vs_TEXCOORD0 
					                                 f32_4 %60 = OpImageSampleImplicitLod %55 %59 
					                                   f32 %61 = OpCompositeExtract %60 0 
					                                               OpStore %45 %61 
					                                   f32 %62 = OpLoad %8 
					                                   f32 %63 = OpLoad %45 
					                                   f32 %64 = OpFMul %62 %63 
					                                               OpStore %8 %64 
					                          Uniform f32* %67 = OpAccessChain %12 %20 %66 
					                                   f32 %68 = OpLoad %67 
					                                               OpStore %65 %68 
					                                   f32 %69 = OpLoad %65 
					                                   f32 %72 = OpExtInst %1 43 %69 %70 %71 
					                                               OpStore %65 %72 
					                                   f32 %73 = OpLoad %8 
					                                   f32 %74 = OpLoad %65 
					                                   f32 %75 = OpFMul %73 %74 
					                                               OpStore %65 %75 
					                                   f32 %78 = OpLoad %65 
					                                 f32_4 %79 = OpCompositeConstruct %78 %78 %78 %78 
					                        Uniform f32_4* %82 = OpAccessChain %12 %80 
					                                 f32_4 %83 = OpLoad %82 
					                                 f32_4 %84 = OpFMul %79 %83 
					                                               OpStore %77 %84 
					                                               OpReturn
					                                               OpFunctionEnd"
				}
			}
			Program "fp" {
				SubProgram "gles hw_tier00 " {
					"!!GLES"
				}
				SubProgram "gles hw_tier01 " {
					"!!GLES"
				}
				SubProgram "gles hw_tier02 " {
					"!!GLES"
				}
				SubProgram "gles3 hw_tier00 " {
					"!!GLES3"
				}
				SubProgram "gles3 hw_tier01 " {
					"!!GLES3"
				}
				SubProgram "gles3 hw_tier02 " {
					"!!GLES3"
				}
				SubProgram "vulkan hw_tier00 " {
					"spirv"
				}
				SubProgram "vulkan hw_tier01 " {
					"spirv"
				}
				SubProgram "vulkan hw_tier02 " {
					"spirv"
				}
			}
		}
	}
}