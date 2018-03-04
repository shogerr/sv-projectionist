Shader "Tri-Planar World" {
    Properties{
        _Color("Color", Color) = (0,0,0,1)
        _MainTex("Texture", 2D) = "black" {}
        _Side("Side", 2D) = "white" {}
        _Top("Top", 2D) = "white" {}
        _Bottom("Bottom", 2D) = "white" {}
        _SideScale("Side Scale", Float) = 2
        _TopScale("Top Scale", Float) = 2
        _BottomScale("Bottom Scale", Float) = 2
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

        sampler2D _Side, _Top, _Bottom, _MainTex;
        float _SideScale, _TopScale, _BottomScale;
        fixed4 _COLOR;

        struct Input {
            float3 worldPos;
            float3 worldNormal;
        };

        void surf(Input IN, inout SurfaceOutput o) {
            float3 projNormal = saturate(pow(IN.worldNormal * 1.4, 4));

            // SIDE X
            float3 x = tex2D(_MainTex, frac(IN.worldPos.zy * _SideScale)) * abs(IN.worldNormal.x);

            // TOP / BOTTOM
            float3 y = 0;
            if (IN.worldNormal.y > 0) {
                y = tex2D(_Top, frac(IN.worldPos.zx * _TopScale)) * abs(IN.worldNormal.y);
            }
            else {
                y = tex2D(_MainTex, frac(IN.worldPos.zx * _BottomScale)) * abs(IN.worldNormal.y);
            }

            // SIDE Z	
            float3 z = tex2D(_Side, frac(IN.worldPos.xy * _SideScale)) * abs(IN.worldNormal.z);

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