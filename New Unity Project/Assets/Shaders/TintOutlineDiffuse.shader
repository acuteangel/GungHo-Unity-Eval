// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/DiffuseOutline" {
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_Color("Main Color", Color) = (1,1,1,1)
		_OutlineColor("Outline color", Color) = (1, 1, 1, 1)
		_OutlineWidth("Outline width", Range(1.0, 5.0)) = 1.01
	}

		CGINCLUDE
		#include "UnityCG.cginc"

		struct appdata {
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float2 texcoord : TEXCOORD0;
		};

		struct v2f {
			float4 pos : SV_POSITION;
			float3 normal : NORMAL;
			float2 texcoord : TEXCOORD0;
		};

		float4 _OutlineColor;
		float _OutlineWidth;

		v2f vert01(appdata v) {
			v.vertex.xyz *= _OutlineWidth;

			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			return o;
		}

		v2f vert02(appdata v) {
			v.vertex.xyz *= 2 - _OutlineWidth;

			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			return o;
		}

		ENDCG

			SubShader
		{
			Tags {"Queue" = "Transparent"}
			Pass
			{
				ZWrite Off

				CGPROGRAM
				#pragma vertex vert01
				#pragma fragment frag

				half4 frag(v2f i) : COLOR{
					return _OutlineColor;
				}
				ENDCG
			}
			Pass
	{
		Tags { "LightMode" = "ForwardBase" }
		CGPROGRAM
			#pragma vertex vert2
			#pragma fragment frag2

			#include "UnityCG.cginc"
			
			float4 _LightColor0;

			float4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert2(appdata IN)
			{
				v2f OUT;
				OUT.pos = UnityObjectToClipPos(IN.vertex);
				OUT.normal = mul(float4(IN.normal, 0.0), unity_ObjectToWorld).xyz;
				OUT.texcoord = TRANSFORM_TEX(IN.texcoord, _MainTex);
				return OUT;
			}

			float4 frag2(v2f IN) : COLOR
			{
				float4 texColor = tex2D(_MainTex, IN.texcoord);

				float3 normalDirection = normalize(IN.normal);
				float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				float3 diffuse = _LightColor0.rgb * _Color.rgb * max(0.0, dot(normalDirection, lightDirection));

				return float4(diffuse,1) * texColor;
			}
		ENDCG
	}
		}

}