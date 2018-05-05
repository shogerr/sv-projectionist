Shader "Tri-Planar World" {
    Properties{
        _Color("Color", Color) = (0,0,0,1)
        _MainTex("Texture", 2D) = "black" {}
    }

    SubShader{
        Tags{
        "Queue" = "Geometry"
        "IgnoreProjector" = "False"
        "RenderType" = "Opaque"
        }

        Cull Back
        ZWrite On

        CGPROGRAM
        #pragma surface surf NoLighting
        #pragma exclude_renderers flash

        sampler2D _MainTex;
        float4 _MainTex_ST;
        fixed4 _COLOR;

        struct Input {
            float3 worldPos;
            float3 worldNormal;
        };

        void surf(Input IN, inout SurfaceOutput o) {
            float3 projNormal = saturate(pow(IN.worldNormal * 1.4, 4));
            //float3 projNormal = IN.worldNormal;

            float2 offset = IN.worldPos.zy * _MainTex_ST.xy + _MainTex_ST.zw;

            // SIDE X
            float3 x = tex2D(_MainTex, offset) * abs(IN.worldNormal.x);

            // TOP / BOTTOM
            float3 y = tex2D(_MainTex, IN.worldPos.zx * _MainTex_ST.xy + _MainTex_ST.zw) * abs(IN.worldNormal.y);
            
            // SIDE Z	
            float3 z = tex2D(_MainTex, IN.worldPos.xy * _MainTex_ST.xy + _MainTex_ST.zw) * abs(IN.worldNormal.z);

            o.Albedo = z;
            o.Albedo = lerp(o.Albedo, x, projNormal.x);
            o.Albedo = lerp(o.Albedo, y, projNormal.y);
        }

        fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
        {
            fixed4 c;
            c.rgb = s.Albedo;
            c.a = s.Alpha;
            return c;
        }
        ENDCG
    }
        Fallback "Diffuse"
}