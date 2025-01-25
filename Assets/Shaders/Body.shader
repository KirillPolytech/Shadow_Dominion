Shader "Custom/Body"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _SpecularTex("Specular (RGB)", 2D) = "white" {}
        _NormalMap("Normal Map", 2D) = "bump" {}
        _Roughness("Roughness (Grayscale)", 2D) = "white" {}
        _Color("Tint Color", Color) = (1,1,1,1)

        _BulletHoleTex("Bullet Hole Texture", 2D) = "white" {}
        _HitUVs("Hit UVs", Vector) = (0,0,0,0)
        _BulletHoleCount("Bullet Hole Count", Int) = 0
        _HitColor ("Hit Color", Color) = (1,1,1,1)
        _Radius ("Radius", Float) = 0.02
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 300

        CGPROGRAM
        #pragma surface surf StandardSpecular fullforwardshadows

        // Включаем поддержку карт нормалей
        #pragma shader_feature _NORMALMAP

        sampler2D _MainTex;
        sampler2D _SpecularTex;
        sampler2D _NormalMap;
        sampler2D _Roughness;
        sampler2D _BulletHoleTex;
        fixed4 _Color;
        
        float4 _HitUVs[10];
        int _BulletHoleCount;
        fixed4 _HitColor;
        float _Radius;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_SpecularTex;
            float2 uv_NormalMap;
            float2 uv_Roughness;
        };

        void surf(Input IN, inout SurfaceOutputStandardSpecular o)
        {
            // injury
            float highlight = 0.0;
            for (int i = 0; i < _BulletHoleCount; i++)
            {
                float2 delta = IN.uv_MainTex - _HitUVs[i].xy;
                float dist = length(delta); // Евклидово расстояние от текущего пикселя до точки попадания
                highlight = max(highlight, 1.0 - step(_Radius, dist)); // Если в пределах радиуса, применяем дырку
            }
            fixed4 bulletHole = tex2D(_BulletHoleTex, IN.uv_MainTex) * _HitColor;
            
            // Albedo (основной цвет)
            fixed4 albedo = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = lerp(albedo, bulletHole.rgb, highlight);

            // Specular (отражение)
            fixed4 specular = tex2D(_SpecularTex, IN.uv_SpecularTex);
            o.Specular = specular.rgb;

            // Normal Map (нормали)
            #ifdef _NORMALMAP
            fixed3 normal = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap));
            o.Normal = normal;
            #endif

            // Roughness (грубость/шершавость)
            fixed roughness = tex2D(_Roughness, IN.uv_Roughness).r;
            o.Smoothness = 1.0 - roughness; // Обратная связь: Roughness → Smoothness
        }
        ENDCG
    }

    FallBack "Diffuse"
}