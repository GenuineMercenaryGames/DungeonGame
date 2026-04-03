using Assets.Scripts.Generation;
using UnityEngine;
using UnityEngine.UI;

public class MinimapUIController : MonoBehaviour
{

    [SerializeField] private Image minimapImage;
    [SerializeField] private Transform minimapPointTransform;
    [SerializeField] private RectTransform minimapPointRectTransform;
    [SerializeField] private World world;
    [SerializeField] private NavMeshController navMeshController; // Esto no me convence mucho, quizá hay que sacar la lógica del floorplane del navMeshController e integrarla correctamente en el world generation.


    public int textureSize = 512;
    public float pixelsPerUnit = 10f;
    public int revealRadius = 50;

    private Sprite fullMapSprite;
    private Vector3 lastUpdatePosition;

    private Sprite CreateMinimapSprite(Mesh mesh, Transform meshTransform)
    {

        Texture2D texture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
        Color32[] pixels = new Color32[textureSize * textureSize];

        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = new Color32(0, 0, 0, 0);
        }

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        Vector2 ProjectToPixel(Vector3 worldPos)
        {
            float u = Mathf.InverseLerp(0, world.MaxWorldSizeInCells.x, worldPos.x);
            float v = Mathf.InverseLerp(0, world.MaxWorldSizeInCells.y, worldPos.z);

            return new Vector2(
                u * (textureSize - 1),
                v * (textureSize - 1)
            );
        }

        float Edge(Vector2 a, Vector2 b, Vector2 p)
        {
            return (p.x - a.x) * (b.y - a.y) - (p.y - a.y) * (b.x - a.x);
        }

        void FillTriangle(Vector2 a, Vector2 b, Vector2 c, Color32 color)
        {
            int minX = Mathf.Clamp(Mathf.FloorToInt(Mathf.Min(a.x, Mathf.Min(b.x, c.x))), 0, textureSize - 1);
            int maxX = Mathf.Clamp(Mathf.CeilToInt(Mathf.Max(a.x, Mathf.Max(b.x, c.x))), 0, textureSize - 1);
            int minY = Mathf.Clamp(Mathf.FloorToInt(Mathf.Min(a.y, Mathf.Min(b.y, c.y))), 0, textureSize - 1);
            int maxY = Mathf.Clamp(Mathf.CeilToInt(Mathf.Max(a.y, Mathf.Max(b.y, c.y))), 0, textureSize - 1);

            float area = Edge(a, b, c);
            if (Mathf.Approximately(area, 0f))
            {
                return;
            }

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    Vector2 p = new Vector2(x + 0.5f, y + 0.5f);

                    float w0 = Edge(b, c, p);
                    float w1 = Edge(c, a, p);
                    float w2 = Edge(a, b, p);

                    bool inside =
                        (w0 >= 0f && w1 >= 0f && w2 >= 0f) ||
                        (w0 <= 0f && w1 <= 0f && w2 <= 0f);

                    if (inside)
                    {
                        pixels[y * textureSize + x] = new Color32(255, 255, 255, 255);
                    }
                }
            }
        }

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 w0 = meshTransform.TransformPoint(vertices[triangles[i]]);
            Vector3 w1 = meshTransform.TransformPoint(vertices[triangles[i + 1]]);
            Vector3 w2 = meshTransform.TransformPoint(vertices[triangles[i + 2]]);

            Vector2 p0 = ProjectToPixel(w0);
            Vector2 p1 = ProjectToPixel(w1);
            Vector2 p2 = ProjectToPixel(w2);

            FillTriangle(p0, p1, p2, new Color32(255, 255, 255, 255));
        }

        texture.SetPixels32(pixels);
        texture.Apply();

        return Sprite.Create(
            texture,
            new UnityEngine.Rect(0f, 0f, texture.width, texture.height),
            new Vector2(0.5f, 0.5f),
            pixelsPerUnit
        );
    }

    private void UpdateMinimapPoint()
    {
        RectTransform minimapRect = minimapImage.rectTransform;

        float u = Mathf.InverseLerp(0f, world.MaxWorldSizeInCells.x, minimapPointTransform.position.x);
        float v = Mathf.InverseLerp(0f, world.MaxWorldSizeInCells.y, minimapPointTransform.position.z);

        minimapPointRectTransform.anchoredPosition = new Vector2(
            (u - 0.5f) * minimapRect.rect.width,
            (v - 0.5f) * minimapRect.rect.height
        );
    }

    void UpdateMinimapDiscovery()
    {
        float u = Mathf.InverseLerp(0f, world.MaxWorldSizeInCells.x, minimapPointTransform.position.x);
        float v = Mathf.InverseLerp(0f, world.MaxWorldSizeInCells.y, minimapPointTransform.position.z);

        Texture2D minimap_texture = minimapImage.sprite.texture;
        Texture2D full_map_texture = fullMapSprite.texture;

        int center_x = Mathf.RoundToInt(u * textureSize);
        int center_y = Mathf.RoundToInt(v * textureSize);

        int from_x = Mathf.Clamp(center_x - revealRadius, 0, textureSize);
        int to_x = Mathf.Clamp(center_x + revealRadius, 0, textureSize);
        int from_y = Mathf.Clamp(center_y - revealRadius, 0, textureSize);
        int to_y = Mathf.Clamp(center_y + revealRadius, 0, textureSize);

        int radius_squared = revealRadius * revealRadius;

        for (int i = from_x; i < to_x; ++i)
        {
            for (int j = from_y; j < to_y; ++j)
            {

                int dx = i - center_x;
                int dy = j - center_y;
                int ds = dx * dx + dy * dy;

                if (ds <= radius_squared)
                {
                    Color pixel = full_map_texture.GetPixel(i, j);
                    minimap_texture.SetPixel(i, j, pixel);
                }
            }
        }

        minimap_texture.Apply();
    }

    void InitMinimap()
    {
        fullMapSprite = CreateMinimapSprite(navMeshController.fullWalkablePlane, navMeshController.fullWalkablePlaneTransform);


        Texture2D texture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
        minimapImage.sprite = Sprite.Create(
            texture,
            new UnityEngine.Rect(0f, 0f, texture.width, texture.height),
            new Vector2(0.5f, 0.5f),
            pixelsPerUnit
        );

        Color32[] pixels = new Color32[textureSize * textureSize];

        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = new Color32(0, 0, 0, 0);
        }

        texture.SetPixels32(pixels);
        texture.Apply();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitMinimap();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMinimapPoint();
        if (Vector3.Distance(lastUpdatePosition, minimapPointTransform.position) > 10)
        {

            lastUpdatePosition = minimapPointTransform.position;
            UpdateMinimapDiscovery();
        }
    }
}
