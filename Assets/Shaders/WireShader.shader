Shader "Custom/WireShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_EmissionStrength ("Emission", float) = 1
		_EmissionColor ("Emission Color", Color) = (1,1,1,1)
		_WireStart ("Wire Start", Vector) = (0,0,0,0)
		_Distance ("Distance", float) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		//void vert(inout appdata_full v, out Input o) {
			//UNITY_INITIALIZE_OUTPUT(Input, o);
			//o.objPos = v.vertex;
		//}

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _EmissionStrength;
		fixed4 _EmissionColor;
		float3 _WireStart;
		float _Distance;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			// _WireStart = mul(_WireStart , _Object2World);
			// _WireStart.y = mul(float4(IN.worldPos,0.0), _Object2World).y;
			// _WireStart = mul(_WireStart, _World2Object);

			// provided direction -1 < 0 = 1 means there is a value
			// provided direction * above result + defalt * ((above result -1)*-1) = value to use 
			//half4 center = mul(half4(0, 0, 0, 0), _Object2World);
			//half4 edge = mul(half4(1, 1, 1, 1), _Object2World);
			// y > x = 1
			// z > y = 2
			// x > z = 3
			// x: 3 || 5
			// y: 1 || 4
			// z: 2 || 3
			// (x>y * x>z, y>x * y>z, z>x * z>y)
			//half4 ed = edge - center;
			//half4 ddirection = half4(step(ed.y, ed.x) * step(ed.z, ed.x), step(ed.x, ed.y) * step(ed.z, ed.y), step(ed.x, ed.z) * step(ed.y, ed.z), 0);
			//_WireStart = mul(ddirection, _Object2World);
			float d = distance(_WireStart, IN.worldPos); // mul(float4(IN.WorldPos, 0), _World2Object).xyz);
			half4 e = _EmissionColor * _EmissionStrength * step(d,_Distance);
			o.Emission = e.rgb;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
