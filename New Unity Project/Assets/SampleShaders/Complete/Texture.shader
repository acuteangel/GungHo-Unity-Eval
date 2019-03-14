// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader ".ShaderTalk/Complete/Texture" {
Properties
{
	_MainTex ("Main Texture", 2D) = "white" {}
	_Color ("Main Color", Color) = (1,1,1,1)
}

SubShader
{
	Pass
	{  
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float2 texcoord : TEXCOORD0;
			};

			float4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata IN)
			{
				v2f OUT;
				OUT.pos = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = TRANSFORM_TEX(IN.texcoord, _MainTex);
				return OUT;
			}
			
			float4 frag (v2f IN) : COLOR
			{
				float4 color = _Color;
				float4 texColor = tex2D(_MainTex, IN.texcoord);
				return color * texColor;
			}
		ENDCG
	}
}

}
