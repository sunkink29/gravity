// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/NormalTexture"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 texcoord : TEXCOORD0;
				float4 pos : SV_POSITION;
				float3 normal : NORMAL;
			};

			sampler2D _MainTex;

			v2f vert (appdata IN)
			{
				v2f OUT;
				OUT.pos = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.normal = mul(float4(IN.normal,0.0), unity_ObjectToWorld).xyz;
				OUT.texcoord = IN.texcoord;
				return OUT;
			}
			
			fixed4 frag (v2f IN) : SV_Target
			{
				fixed4 texColor = tex2D(_MainTex, IN.texcoord);

				float3 normalDirection = normalize(IN.normal) * 0.5 + 0.5;
				texColor = float4(normalDirection, 0) * texColor;
//				texColor = fixed4(normalDirection,1);
				return texColor;
			}
			ENDCG
		}
	}
}
