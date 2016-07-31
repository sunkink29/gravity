// calculate the start by making a texture that has the r value fade from 0 to 1 with 0 being the start of the quad
// the b value represents the lenght of the quad: the b value would be calculated by dividing the distance by a max distance and multipliying that with 255: this methoid has more error the larger the max distance is 
// to get the distance of the pixel you would multiply the r value with the distance from b
Shader "Custom/LightsShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		[NoScaleOffset] _MainTex ("Albedo (RGB)", 2D) = "white" {}
		[NoScaleOffset] _InfoTex("Information (Distance, lenght)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_EmissionStrength ("Emission", float) = 1
		_EmissionColor("Emission Color", Color) = (1,1,1,1)
		_SegmentLength("Segment Length", float) = 2
		_LineThickness("lineThickness", float) = .5
		_MaxLength("MaxLength", float) = 25
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		struct Input {
			float2 uv_MainTex;
			float2 uv_InfoTex;
			float3 worldPos;
			//half3 normal;
			//half4 tangent;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		half _EmissionStrength;
		fixed4 _EmissionColor;
		float _SegmentLength;
		float _LineThickness;
		float _MaxLength;
		sampler2D _MainTex;
		sampler2D _InfoTex;

		void vert(inout appdata_full i, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);

			//o.normal = i.normal;
			//o.tangent = i.tangent;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			half4 emission = _EmissionColor * _EmissionStrength;
			float4 i = tex2D(_InfoTex, IN.uv_InfoTex);
			//o.Albedo = i.r;
			//o.Emission = i.r;
			float length = i.g * _MaxLength;
			float vPos = i.r * length;
			float halfLineThickness = _LineThickness;
			_SegmentLength = halfLineThickness + _SegmentLength;

			//half3 wNormal = UnityObjectToWorldNormal(IN.normal);
			//half3 wTangent = UnityObjectToWorldDir(IN.tangent.xyz);
			// compute bitangent from cross product of normal and tangent
			//half tangentSign = IN.tangent.w * unity_WorldTransformParams.w;
			//half3 wBitangent = cross(wNormal, wTangent) * tangentSign;
			// output the tangent space matrix
			//half3 tspace0 = half3(wTangent.x, wBitangent.x, wNormal.x);
			//half3 tspace1 = half3(wTangent.y, wBitangent.y, wNormal.y);
			//half3 tspace2 = half3(wTangent.z, wBitangent.z, wNormal.z);

			//float3x3 invTanSpaceMatrix = float3x3(tspace0, tspace1, tspace2);
			//half3 start = mul(half3(0, 0, 0), invTanSpaceMatrix);
			//half3 end = mul(half3(0, 0, 1), invTanSpaceMatrix);
			//float segmentLength = distance(start, end) / _Divisions;

			//o.Emission = step(i.r, _SegmentLength);
			float4 lines = step(halfLineThickness, fmod(vPos, _SegmentLength)) *  emission;
			o.Emission = lines;
			//o.Albedo = lines;
			//o.Emission += step(fmod(vPos, _SegmentLength), _SegmentLength - halfLineThickness) * emission;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
