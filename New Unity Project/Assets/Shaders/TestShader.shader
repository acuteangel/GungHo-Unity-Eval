Shader "Custom/DiffuseOutline2" {
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_Color("Main Color", Color) = (1,1,1,1)
		_OutlineColor("Outline color", Color) = (1, 1, 1, 1)
		_OutlineWidth("Outline width", Range(0, 1.0)) = .25
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
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.pos += float4(_OutlineWidth, 0, -0.1, 0);
			return o;
		}

		v2f vert02(appdata v) {
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.pos += float4(-_OutlineWidth, 0, -0.1, 0);
			return o;
		}

		v2f vert03(appdata v) {
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.pos += float4(0, _OutlineWidth, -0.1, 0);
			return o;
		}

		v2f vert04(appdata v) {
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.pos += float4(0, -_OutlineWidth, -0.1, 0);
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
				ZWrite Off

				CGPROGRAM
				#pragma vertex vert02
				#pragma fragment frag

				half4 frag(v2f i) : COLOR{
					return _OutlineColor;
				}
				ENDCG
			}
			Pass
			{
				ZWrite Off

				CGPROGRAM
				#pragma vertex vert03
				#pragma fragment frag

				half4 frag(v2f i) : COLOR{
					return _OutlineColor;
				}
				ENDCG
			}
			Pass
			{
				ZWrite Off

				CGPROGRAM
				#pragma vertex vert04
				#pragma fragment frag

				half4 frag(v2f i) : COLOR{
					return _OutlineColor;
				}
				ENDCG
			}
			Pass
			{
				ZWrite On

				Material{
					Diffuse[_Color]
					Ambient[_Color]
				}

				Lighting On

				SetTexture[_MainTex]
				{
					ConstantColor[_Color]
				}

				SetTexture[_MainTex]{
					Combine previous * primary DOUBLE
				}
			}
		}

}