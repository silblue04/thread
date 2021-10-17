// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/Unlit/Transparent Colored GrayScale 2"
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
			fixed4 color : COLOR;
			float4 worldPos : TEXCOORD1;
			UNITY_VERTEX_OUTPUT_STEREO
		};

		float2 Rotate(float2 v, float2 rot)
		{
			float2 ret;
			ret.x = v.x * rot.y - v.y * rot.x;
			ret.y = v.x * rot.x + v.y * rot.y;
			return ret;
		}

		sampler2D _MainTex;
		float4 _MainTex_ST;
		fixed _EffectAmount;
		half _Intensity;
		float4 _ClipRange0 = float4(0.0, 0.0, 1.0, 1.0);
		float4 _ClipArgs0 = float4(1000.0, 1000.0, 0.0, 1.0);
		float4 _ClipRange1 = float4(0.0, 0.0, 1.0, 1.0);
		float4 _ClipArgs1 = float4(1000.0, 1000.0, 0.0, 1.0);


		v2f vert(appdata_t v)
		{
			v2f o;
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
			o.color = v.color;
			o.worldPos.xy = v.vertex.xy * _ClipRange0.zw + _ClipRange0.xy;
			o.worldPos.zw = Rotate(v.vertex.xy, _ClipArgs1.zw) * _ClipRange1.zw + _ClipRange1.xy;
			return o;
		}

		fixed4 frag(v2f i) : COLOR
		{
			// First clip region
			float2 factor = (float2(1.0, 1.0) - abs(i.worldPos.xy)) * _ClipArgs0.xy;
			float f = min(factor.x, factor.y);

			// Second clip region
			factor = (float2(1.0, 1.0) - abs(i.worldPos.zw)) * _ClipArgs1.xy;
			f = min(f, min(factor.x, factor.y));

			fixed4 col = tex2D(_MainTex, i.texcoord) * i.color;
			col.a *= clamp(f, 0.0, 1.0);
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