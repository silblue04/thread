﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/Unlit/Transparent Colored GrayScale (TextureClip)"
{
	Properties
	{
		_MainTex("Base (RGB), Alpha (A)", 2D) = "black" {}
		_EffectAmount("Effect Amount", Range(0, 1)) = 0.0
		_Intensity("Intencity", float) = 1.0
	}

	SubShader
	{
		LOD 100

	Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
	}

	Cull Off
	Lighting Off
	ZWrite Off
	Fog{ Mode Off }
	Offset -1, -1
	Blend SrcAlpha OneMinusSrcAlpha

	Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

		struct appdata_t
		{
			float4 vertex : POSITION;
			float2 texcoord : TEXCOORD0;
			fixed4 color : COLOR;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct v2f
		{
			float4 vertex : SV_POSITION;
			half2 texcoord : TEXCOORD0;
			float2 clipUV : TEXCOORD1;
			fixed4 color : COLOR;
			UNITY_VERTEX_OUTPUT_STEREO
		};

		sampler2D _MainTex;
		float4 _MainTex_ST;
		sampler2D _ClipTex;
		float4 _ClipRange0 = float4(0.0, 0.0, 1.0, 1.0);
		fixed _EffectAmount;
		half _Intensity;

		v2f vert(appdata_t v)
		{
			v2f o;
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
			o.color = v.color;
			o.clipUV = (v.vertex.xy * _ClipRange0.zw + _ClipRange0.xy) * 0.5 + float2(0.5, 0.5);
			return o;
		}

		fixed4 frag(v2f i) : COLOR
		{
			fixed4 col = tex2D(_MainTex, i.texcoord) * i.color;
			col.a *= tex2D(_ClipTex, i.clipUV).a;
			col.rgb = lerp(col.rgb, dot(col.rgb, float3(0.3, 0.59, 0.11)), _EffectAmount) * _Intensity;
			return col;
		}
		ENDCG
	}
}

	SubShader
	{
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}

		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog{ Mode Off }
			Offset -1, -1
			ColorMask RGB
			AlphaTest Greater .01
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse

			SetTexture[_MainTex]
			{
				Combine Texture * Primary
			}
		}
	}
}