using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Minimapa discreto (Prompt 23, Parte 4): camara ortografica secundaria que sigue al jugador
// desde arriba, mostrada en un circulo pequeno en la esquina superior derecha, con el jugador
// como triangulo y el objetivo actual (MissionNavigation.CurrentTarget) como punto amarillo.
// Mapa fijo orientado al norte (no rota con el jugador) para mantenerlo simple, sin NavMesh ni
// pathfinding. Solo navegacion: no dibuja etiquetas, nombres ni texto educativo.
public class MinimapController : MonoBehaviour
{
    private const int MapPixelSize = 200;
    private const float MapWorldRadius = 35f;
    private const float CameraHeight = 120f;
    private const int RenderTextureSize = 256;

    // Prompt 26: el minimapa no debe verse en el Main Menu. Igual que GameHUD, sigue
    // creandose siempre (DontDestroyOnLoad) para no romper su inicializacion; solo se
    // oculta/muestra su Canvas segun la escena activa.
    private const string MenuSceneName = "MainMenu";

    private Camera minimapCamera;
    private RenderTexture renderTexture;
    private RectTransform playerIconRT;
    private RectTransform objectiveDotRT;
    private Transform playerTransform;
    private GameObject minimapCanvasGO;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        var go = new GameObject("MinimapController");
        go.AddComponent<MinimapController>();
        DontDestroyOnLoad(go);
    }

    private void Awake()
    {
        BuildCamera();
        BuildUI();

        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        UpdateVisibilityForScene(SceneManager.GetActiveScene().name);
    }

    private void OnActiveSceneChanged(Scene previous, Scene next)
    {
        UpdateVisibilityForScene(next.name);
    }

    private void UpdateVisibilityForScene(string sceneName)
    {
        if (minimapCanvasGO != null) minimapCanvasGO.SetActive(sceneName != MenuSceneName);
    }

    private void BuildCamera()
    {
        var camGO = new GameObject("MinimapCamera");
        camGO.transform.SetParent(transform, false);
        minimapCamera = camGO.AddComponent<Camera>();
        minimapCamera.orthographic = true;
        minimapCamera.orthographicSize = MapWorldRadius;
        minimapCamera.nearClipPlane = 1f;
        minimapCamera.farClipPlane = CameraHeight + 20f;
        minimapCamera.clearFlags = CameraClearFlags.SolidColor;
        minimapCamera.backgroundColor = new Color(0.05f, 0.08f, 0.1f, 1f);
        minimapCamera.transform.rotation = Quaternion.Euler(90f, 0f, 0f); // mirando hacia abajo, "arriba" de camara = +Z mundo (norte)
        minimapCamera.depth = -10;

        renderTexture = new RenderTexture(RenderTextureSize, RenderTextureSize, 16);
        minimapCamera.targetTexture = renderTexture;
    }

    private void BuildUI()
    {
        var canvasGO = new GameObject("MinimapCanvas");
        canvasGO.transform.SetParent(transform, false);
        minimapCanvasGO = canvasGO;
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = -1; // por debajo del backdrop modal de GameHUD, para no flotar sobre los paneles
        var scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasGO.AddComponent<GraphicRaycaster>();

        var frameGO = new GameObject("MinimapFrame");
        frameGO.transform.SetParent(canvasGO.transform, false);
        var frameRT = frameGO.AddComponent<RectTransform>();
        frameRT.anchorMin = frameRT.anchorMax = new Vector2(1, 1);
        frameRT.pivot = new Vector2(1, 1);
        // Desplazado hacia abajo (y=-110) para no superponerse con ProgressText/InventoryText,
        // que ya ocupan la esquina superior derecha del HUD.
        frameRT.anchoredPosition = new Vector2(-20, -110);
        frameRT.sizeDelta = new Vector2(MapPixelSize + 10, MapPixelSize + 10);
        var frameImage = frameGO.AddComponent<Image>();
        frameImage.sprite = NavIcons.CircleSprite;
        frameImage.color = new Color(0.35f, 0.95f, 1f, 0.35f);

        var maskGO = new GameObject("MapMask");
        maskGO.transform.SetParent(frameGO.transform, false);
        var maskRT = maskGO.AddComponent<RectTransform>();
        maskRT.anchorMin = maskRT.anchorMax = maskRT.pivot = new Vector2(0.5f, 0.5f);
        maskRT.sizeDelta = new Vector2(MapPixelSize, MapPixelSize);
        var maskImage = maskGO.AddComponent<Image>();
        maskImage.sprite = NavIcons.CircleSprite;
        var mask = maskGO.AddComponent<Mask>();
        mask.showMaskGraphic = false;

        var rawGO = new GameObject("MapTexture");
        rawGO.transform.SetParent(maskGO.transform, false);
        var rawRT = rawGO.AddComponent<RectTransform>();
        rawRT.anchorMin = Vector2.zero;
        rawRT.anchorMax = Vector2.one;
        rawRT.offsetMin = Vector2.zero;
        rawRT.offsetMax = Vector2.zero;
        var rawImage = rawGO.AddComponent<RawImage>();
        rawImage.texture = renderTexture;

        var objGO = new GameObject("ObjectiveDot");
        objGO.transform.SetParent(maskGO.transform, false);
        objectiveDotRT = objGO.AddComponent<RectTransform>();
        objectiveDotRT.anchorMin = objectiveDotRT.anchorMax = objectiveDotRT.pivot = new Vector2(0.5f, 0.5f);
        objectiveDotRT.sizeDelta = new Vector2(18, 18);
        var objImage = objGO.AddComponent<Image>();
        objImage.sprite = NavIcons.CircleSprite;
        objImage.color = new Color(1f, 0.85f, 0.1f, 1f);
        objGO.SetActive(false);

        var playerGO = new GameObject("PlayerIcon");
        playerGO.transform.SetParent(maskGO.transform, false);
        playerIconRT = playerGO.AddComponent<RectTransform>();
        playerIconRT.anchorMin = playerIconRT.anchorMax = playerIconRT.pivot = new Vector2(0.5f, 0.5f);
        playerIconRT.sizeDelta = new Vector2(20, 20);
        playerIconRT.anchoredPosition = Vector2.zero;
        var playerImage = playerGO.AddComponent<Image>();
        playerImage.sprite = NavIcons.TriangleSprite;
        playerImage.color = new Color(0.35f, 0.95f, 1f, 1f);
    }

    private void EnsurePlayer()
    {
        if (playerTransform != null) return;
        var movement = FindFirstObjectByType<MovementInput>();
        if (movement != null) playerTransform = movement.transform;
    }

    private void Update()
    {
        EnsurePlayer();
        if (playerTransform == null || minimapCamera == null) return;

        Vector3 camPos = playerTransform.position;
        camPos.y += CameraHeight;
        minimapCamera.transform.position = camPos;

        playerIconRT.localEulerAngles = new Vector3(0, 0, -playerTransform.eulerAngles.y);

        var target = MissionNavigation.Instance.CurrentTarget;
        if (target == null)
        {
            objectiveDotRT.gameObject.SetActive(false);
            return;
        }

        objectiveDotRT.gameObject.SetActive(true);

        Vector3 delta = target.position - playerTransform.position;
        Vector2 flat = new Vector2(delta.x, delta.z); // mapa fijo al norte: +Z mundo = arriba en pantalla
        float pixelsPerWorldUnit = (MapPixelSize * 0.5f) / MapWorldRadius;
        Vector2 mapPos = flat * pixelsPerWorldUnit;

        float maxRadius = MapPixelSize * 0.5f - 12f;
        if (mapPos.magnitude > maxRadius)
            mapPos = mapPos.normalized * maxRadius;

        objectiveDotRT.anchoredPosition = mapPos;
    }

    private void OnDestroy()
    {
        if (renderTexture != null) renderTexture.Release();
    }
}
