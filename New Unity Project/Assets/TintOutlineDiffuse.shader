﻿// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
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

		v2f vert(appdata v) {
			v.vertex.xyz *= _OutlineWidth;

			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			return o;
		}

		ENDCG

			SubShader
		{
			Pass
			{
				ZWrite Off

				CGPROGRAM
				#pragma vertex vert
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