// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Transparent Colored GrayScale (Packed) (TextureClip)"
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
			fixed4 color : COLOR;
			float2 texcoord : TEXCOORD0;
			float2 worldPos : TEXCOORD1;
		};

		struct v2f
		{
			float4 vertex : SV_POSITION;
			fixed4 color : COLOR;
			half2 texcoord : TEXCOORD0;
			float2 worldPos : TEXCOORD1;
		};

		sampler2D _MainTex;
		float4 _MainTex_ST;
		sampler2D _ClipTex;
		fixed _EffectAmount;
		half _Intensity;

		v2f vert(appdata_t v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.color = v.color;
			o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
			o.worldPos = TRANSFORM_TEX(v.vertex.xy, _MainTex);
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			half alpha = tex2D(_ClipTex, i.worldPos * 0.5 + float2(0.5, 0.5)).a;
			half4 mask = tex2D(_MainTex, i.texcoord);
			half4 mixed = saturate(ceil(i.color - 0.5));
			fixed4 col = saturate((mixed * 0.51 - i.color) / -0.49);

			col.a *= alpha;
			mask *= mixed;
			col.a *= mask.r + mask.g + mask.b + mask.a;

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