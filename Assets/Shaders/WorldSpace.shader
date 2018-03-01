Shader "Unlit/WorldSpace"
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
			// make fog work
			//#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			/*
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			*/
			
			struct v2f
			{
				float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
            };

			sampler2D _MainTex;
			//float4 _MainTex_ST;
			
			void vert (inout appdata_full v)
			{
				// Get the worldspace normal after transformation, and ensure it's unit length.
				float3 n = normalize(mul(unity_ObjectToWorld, v.normal).xyz);

				// Get the closest vector in the polygon's plane to world up.
				// We'll use this as the "v" direction of our texture.
				float3 vDirection = normalize(float3(0, 1, 0) - n.y * n);

				// Get the perpendicular in-plane vector to use as our "u" direction.
				float3 uDirection = normalize(cross(n, vDirection));

				// Get the position of the vertex in worldspace.
				float3 worldSpace = mul(unity_ObjectToWorld, v.vertex).xyz;

				// Project the worldspace position of the vertex into our texturing plane,
				// and use this result as the primary texture coordinate.
				v.texcoord.xy = float2(dot(worldSpace, uDirection), dot(worldSpace, vDirection));
			}
			
			fixed4 frag (in appdata_full v) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, v.texcoord.xy);
				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
