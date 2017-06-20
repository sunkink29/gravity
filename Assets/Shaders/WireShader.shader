// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/WireShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_EmissionStrength ("Emission", float) = 1
		_EmissionColor ("Emission Color", Color) = (1,1,1,1)
		_WireStart ("Wire Start", Vector) = (0,0,0)
		_Distance ("Distance", float) = 0
		_VectorDirection ("Vector Direction", Vector) = (0,0,0)
		_FalloutDistance("Fallout distance", float) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float3 localPos;
		};

		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.localPos = v.vertex.xyz;
		}

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _EmissionStrength;
		fixed4 _EmissionColor;
		float3 _WireStart;
		float _Distance;
		float3 _VectorDirection;
		float _FalloutDistance;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			float3 pos = IN.localPos * _VectorDirection;
			float d = distance(pos, _WireStart);
			float wire = step(d, _Distance);
			float fallout = step(d, _Distance + _FalloutDistance) * !wire * (1 - (d - _Distance) / _FalloutDistance);
			half4 e = _EmissionColor * _EmissionStrength * (wire + fallout);// *((IN.worldPos - _Distance) / _FalloutDistance);
			o.Emission = e.rgb;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
