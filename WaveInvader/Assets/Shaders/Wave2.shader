Shader "Custom/Wave2" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_Intensity("Intensity", Range(0.1, 10)) = 1.0
		_NumTrack("NumTrack", Int) = 2
		_TrackWidth ("Track Width", Float) = 0.0
		_Offset ("Offset", Float) = 0.0
		_HeightMap ("Height Map", 2D) = "black" {}
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
		LOD 200

		Cull Off
		

		//// Pass to render object as a shadow caster
		//Pass {
		//	Name "ShadowCaster"
		//	Tags { "LightMode" = "ShadowCaster" }

		//	Fog { Mode Off }
		//	ZWrite On ZTest LEqual Cull Off
		//	Offset 1, 1

		//	CGPROGRAM
		//	#pragma vertex vert
		//	#pragma fragment frag
		//	#pragma multi_compile_shadowcaster
		//	#pragma fragmentoption ARB_precision_hint_fastest
		//	#include "UnityCG.cginc"

		//	struct v2f {
		//		V2F_SHADOW_CASTER;
		//	};

		//	sampler2D _HeightMap;
		//	float _Intensity;
		//	float _Offset;

		//	v2f vert(appdata_base v)
		//	{
		//		v2f o;
		//		float4 h = tex2Dlod(_HeightMap, float4(v.texcoord.x * 0.5 - _Offset, 0, 0, 0));
		//		v.vertex.y = v.vertex.y + h.x * _Intensity;

		//		TRANSFER_SHADOW_CASTER(o)
		//			return o;
		//	}

		//	float4 frag(v2f i) : COLOR
		//	{
		//		SHADOW_CASTER_FRAGMENT(i)
		//	}
		//	ENDCG

		//}

		// Pass to render object as a shadow collector
		Pass {
			Name "ShadowCollector"
			Tags { "LightMode" = "ShadowCollector" }

			Fog { Mode Off }
			ZWrite On ZTest LEqual

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_shadowcollector

			#define SHADOW_COLLECTOR_PASS
			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD;
			};

			struct v2f {
				V2F_SHADOW_COLLECTOR;
			};

			sampler2D _HeightMap;
			float _Intensity;
			float _Offset;

			v2f vert(appdata v)
			{
				v2f o;
				float4 h = tex2Dlod(_HeightMap, float4(v.texcoord.x * 0.5 - _Offset, 0, 0, 0));
				v.vertex.y = v.vertex.y + h.x * _Intensity;
				TRANSFER_SHADOW_COLLECTOR(o)
					return o;
			}

			fixed4 frag(v2f i) : COLOR
			{
				SHADOW_COLLECTOR_FRAGMENT(i)
			}
			ENDCG

		}


		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert alpha

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _HeightMap;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		float _Intensity;
		float _Offset;
		int _NumTrack;
		float _TrackWidth;
		fixed4 _Color;

		void vert(inout appdata_full v) {
			float UVStep = 1.0 / _NumTrack;
			float4 h = tex2Dlod(_HeightMap, float4(v.texcoord.x * 0.5 - _Offset, 0, 0, 0));
			float4 h_0 = tex2Dlod(_HeightMap, float4((v.texcoord.x - UVStep) * 0.5 - _Offset, 0, 0, 0));
			float4 h_1 = tex2Dlod(_HeightMap, float4((v.texcoord.x + UVStep) * 0.5 - _Offset, 0, 0, 0));

			v.vertex.y = v.vertex.y + h.x * _Intensity;
			float theta = v.texcoord.y * 2 * 3.1415926;
			float3 a = float3(cos(theta), 0, sin(theta)) * 2 * _TrackWidth;
			a.y += (h_1.x - h_0.x);
			float b = a.y / (a.x * a.x + a.z * a.z);
			float3 normal = normalize(float3(a.x * b, 1, a.z * b));
			v.normal = normal;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex).a * _Color;
			o.Albedo = c.rgb;
			//o.Albedo = float3(1, 1, 1);// float3(IN.uv_MainTex.x, IN.uv_MainTex.y, 0);
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
