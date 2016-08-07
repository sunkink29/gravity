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
		_GradentLength("gradent Length", float) = .23
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
		float _GradentLength;

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
			float length = i.b * _MaxLength;
			float vPos = (i.r + i.g) / 2 * length;
			
			float freqency = _SegmentLength / 2 + _LineThickness / 2;
			float4 lines = step(fmod(floor(vPos / freqency), 4), 1) * step(1, fmod(floor(vPos / freqency), 2)) * step(fmod(vPos, freqency),_SegmentLength / 2);
			lines += step(fmod(floor(vPos / freqency), 4), 1) * step(fmod(floor(vPos / freqency), 2), 0) * step(_LineThickness / 2 , fmod(vPos, freqency));

			lines += step(2, fmod(floor(vPos / freqency), 4)) * step(1, fmod(floor(vPos / freqency), 2)) * step(fmod(vPos, freqency), _SegmentLength / 2) * emission;
			lines += step(2, fmod(floor(vPos / freqency), 4)) * step(fmod(floor(vPos / freqency), 2), 0) * step(_LineThickness / 2, fmod(vPos, freqency)) * emission;
			
			float halfLineThickness = _LineThickness / 2;
			float segmentLength = halfLineThickness * 2 + _SegmentLength;

			//float4 lines = step(halfLineThickness, fmod(vPos, segmentLength));
			//lines += step(fmod(vPos, segmentLength), segmentLength - halfLineThickness);
			//lines = lines / 2;
			//lines = step(1, lines);

			//lines += step(halfLineThickness, fmod(vPos, segmentLength)) * emission;
			//lines += step(fmod(vPos, segmentLength), segmentLength - halfLineThickness) * emission;
			//lines = lines / 3;
			//lines = step(1, lines);
			//lines = fmod(vPos, _SegmentLength);

			o.Emission = lines;
			//o.Albedo = lines;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
