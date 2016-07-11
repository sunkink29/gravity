Shader "Custom/LightsShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_EmissionStrength ("Emission", float) = 1
		_EmissionColor("Emission Color", Color) = (1,1,1,1)
		_Divisions("Divisions", int) = 3
		_lineThickness("lineThickness", int) = 1
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
			float3 worldNormal;
			float3 worldPos;

			half3 tangent_input;
			half3 binormal_input;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _EmissionStrength;
		fixed4 _EmissionColor;
		int _Divisions;
		int _lineThickness;

		void vert(inout appdata_full i, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);

			half4 p_normal = mul(float4(i.normal, 0.0f), _World2Object);
			half4 p_tangent = mul(_Object2World, i.tangent);

			half3 normal_input = normalize(p_normal.xyz);
			half3 tangent_input = normalize(p_tangent.xyz);
			half3 binormal_input = cross(p_normal.xyz, tangent_input.xyz) * i.tangent.w;

			o.tangent_input = tangent_input;
			o.binormal_input = binormal_input;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			half4 e = _EmissionColor * _EmissionStrength;

			float3x3 invTanSpaceMatrix = transpose(float3x3(IN.tangent_input,IN.binormal_input,IN.worldNormal));
			half3 start = mul(half3(0, 0, 0), invTanSpaceMatrix);
			half3 end = mul(half3(1, 0, 0), invTanSpaceMatrix);
			segmentLength = distance(start, end) / _Divisions;
			float4 emission = _EmissionColor * EmissionStrength;
			o.Emission = step(distance(start, worldPos) % segmentLenght, _LineThickness) *  emission;
			o.Emission += step(segmentLength - _LineThickness, distance(start, worldPos) % segmentLenght) * emission;
		}
		}
		ENDCG
	}
	FallBack "Diffuse"
}
