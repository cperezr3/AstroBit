using UnityEngine;

// Sprites generados en runtime para la capa de navegacion (Prompt 23): un punto y un triangulo
// simples, sin depender de assets externos (mismo criterio que GameHUD/WorldLabel, que
// construyen toda su UI en codigo). Se generan una sola vez y se cachean estaticamente.
public static class NavIcons
{
    private static Sprite circleSprite;
    private static Sprite triangleSprite;

    public static Sprite CircleSprite => circleSprite != null ? circleSprite : circleSprite = BuildCircle(64);

    // Triangulo apuntando hacia arriba ("▲"): usado tal cual para el icono del jugador en el
    // minimapa (rota segun hacia donde mira) y rotado 180 grados para la flecha del marcador
    // de mundo (que siempre apunta hacia abajo, hacia el objetivo).
    public static Sprite TriangleSprite => triangleSprite != null ? triangleSprite : triangleSprite = BuildTriangle(64);

    private static Sprite BuildCircle(int size)
    {
        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        float radius = size * 0.5f;
        Vector2 center = new Vector2(radius, radius);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dist = Vector2.Distance(new Vector2(x + 0.5f, y + 0.5f), center);
                float alpha = Mathf.Clamp01((radius - dist) / 2f);
                tex.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
            }
        }
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
    }

    private static Sprite BuildTriangle(int size)
    {
        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;

        for (int y = 0; y < size; y++)
        {
            float t = (float)y / (size - 1); // 0 = fila inferior, 1 = fila superior
            float halfWidth = (1f - t) * (size * 0.5f); // ancho maximo abajo, vertice arriba
            for (int x = 0; x < size; x++)
            {
                float dx = Mathf.Abs(x + 0.5f - size * 0.5f);
                float alpha = dx <= halfWidth ? 1f : 0f;
                tex.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
            }
        }
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
    }
}
