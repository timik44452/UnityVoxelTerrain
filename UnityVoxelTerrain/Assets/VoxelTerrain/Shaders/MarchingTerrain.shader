Shader "Marching/MarchingTerrain"
{
	Properties
	{

	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			Tags {"LightMode" = "ForwardBase"}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 diff : COLOR0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;

				half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));

				o.diff = nl * _LightColor0;

				return o;
			}

			sampler2D _MainTex;

			float4 frag(v2f i) : SV_Target
			{
				float r = saturate(abs(6 * i.uv.x - 3) - 1);
				float g = saturate(2 - abs(6 * i.uv.x - 2));
				float b = saturate(2 - abs(6 * i.uv.x - 4));

				float _a = saturate(i.uv.y * 2);
				float _b = saturate((i.uv.y - 0.5) * 2);

				float4 col = float4(r, g, b, 1);

				col = lerp(float4(0, 0, 0, 0), col, _a);
				col = lerp(col, float4(1, 1, 1, 1), _b);

				return col * i.diff;
			}
			ENDCG
		}
	}
}
