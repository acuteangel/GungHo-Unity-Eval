// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader ".ShaderTalk/Complete/Diffuse" {
Properties
{
	_MainTex ("Main Texture", 2D) = "white" {}
	_Color ("Main Color", Color) = (1,1,1,1)
}

SubShader
{
	Pass
	{  
		Tags { "LightMode" = "ForwardBase" }
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

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

			float4 _LightColor0;

			float4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata IN)
			{
				v2f OUT;
				OUT.pos = UnityObjectToClipPos(IN.vertex);
				OUT.normal = mul(float4(IN.normal, 0.0), unity_ObjectToWorld).xyz;
				OUT.texcoord = TRANSFORM_TEX(IN.texcoord, _MainTex);
				return OUT;
			}
			
			float4 frag (v2f IN) : COLOR
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