using UnityEngine;

public class BodyInjuryService
{
    private static readonly int BulletHoleCount = Shader.PropertyToID("_BulletHoleCount");

    public static void DrawHole(Renderer renderer, Vector3 dir)
    {
        Material material = renderer.material;

        // Получаем UV-координаты в точке попадания
        Vector2 hitUV = dir;

        // Получаем текущее количество дырок
        int bulletHoleCount = material.GetInt(BulletHoleCount);

        // Устанавливаем новые UV-координаты попадания
        material.SetVector("_HitUVs" + bulletHoleCount, new Vector4(hitUV.x, hitUV.y, 0, 0));

        // Увеличиваем количество дырок
        material.SetInt(BulletHoleCount, bulletHoleCount + 1);
    }
}
