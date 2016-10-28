// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/none" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_EmissionStrength ("Emission", float) = 1
		_EmissionColor("Emission Color", Color) = (1,1,1,1)
		_Divisions("Divisions", float) = 3
		_LineThickness("lineThickness", float) = 1
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
			float3 worldNormal;
			float3 worldPos;

			half3 tangent_input;
			half3 binormal_input;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		half _EmissionStrength;
		fixed4 _EmissionColor;
		float _Divisions;
		float _LineThickness;

		void vert(inout appdata_full i, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);

			half4 p_normal = mul(float4(i.normal, 0.0f), unity_WorldToObject);
			half4 p_tangent = mul(unity_ObjectToWorld, i.tangent);

			half3 normal_input = normalize(p_normal.xyz);
			half3 tangent_input = normalize(p_tangent.xyz);
			half3 binormal_input = cross(normal_input.xyz, tangent_input.xyz) * i.tangent.w;

			o.tangent_input = tangent_input;
			o.binormal_input = binormal_input;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = _Color; //tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			half4 e = _EmissionColor * _EmissionStrength;

			float3x3 invTanSpaceMatrix = transpose(float3x3(IN.tangent_input,IN.binormal_input,IN.worldNormal));
			half3 start = mul(half3(0, 0, 0), invTanSpaceMatrix);
			half3 end = mul(half3(0, 0, 1), invTanSpaceMatrix);
			float segmentLength = distance(start, end) / _Divisions;
			float4 emission = _EmissionColor * _EmissionStrength;
			float d = distance(start, IN.worldPos);
			o.Emission = mul(half3(1, 1, 1), invTanSpaceMatrix);
			//o.Emission = step(fmod(distance(start, IN.worldPos),segmentLength), _LineThickness) *  emission;
			//o.Emission += step(segmentLength - _LineThickness, fmod(distance(start, IN.worldPos), segmentLength)) * emission;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
