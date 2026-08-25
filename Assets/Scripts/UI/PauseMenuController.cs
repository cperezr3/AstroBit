using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Menu de pausa (Prompt 28). Responsable unicamente de: abrir/cerrar la pausa, Time.timeScale,
// visibilidad de su propio Canvas, y las tres acciones del menu (Continuar/Reiniciar/Volver al
// Menu). No conoce nada de mision/progresion -- eso vive en GameSession y en los sistemas ya
// existentes.
//
// Mismo patron que GameHUD/MinimapController/MissionUI: singleton unico creado una sola vez
// via RuntimeInitializeOnLoadMethod + DontDestroyOnLoad, activo solo mientras la escena actual
// sea SampleScene (igual que el resto de la UI de gameplay se oculta en MainMenu).
public class PauseMenuController : MonoBehaviour
{
    private const string GameplaySceneName = "SampleScene";
    private const string MenuSceneName = "MainMenu";

    private GameObject panelRoot;
    private bool isPaused;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        var go = new GameObject("PauseMenuController");
        go.AddComponent<PauseMenuController>();
        DontDestroyOnLoad(go);
    }

    private void Awake()
    {
        BuildUI();
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void OnActiveSceneChanged(Scene previous, Scene next)
    {
        // Si el jugador vuelve a MainMenu (o a cualquier otra escena) mientras la pausa seguia
        // abierta, hay que soltar el timeScale y el control del jugador -- si no, "Volver al
        // Menu" dejaria el juego congelado en timeScale 0 para siempre.
        if (isPaused) SetPaused(false);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != GameplaySceneName) return;
        if (Keyboard.current == null) return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            SetPaused(!isPaused);
        }
    }

    private void SetPaused(bool paused)
    {
        isPaused = paused;
        panelRoot.SetActive(paused);
        Time.timeScale = paused ? 0f : 1f;
        SetPlayerControlEnabled(!paused);
    }

    private void SetPlayerControlEnabled(bool controlEnabled)
    {
        var interaction = FindFirstObjectByType<PlayerInteraction>();
        if (interaction != null) interaction.enabled = controlEnabled;

        var movement = FindFirstObjectByType<MovementInput>();
        if (movement != null) movement.enabled = controlEnabled;
    }

    public void Continuar()
    {
        SetPaused(false);
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;
        GameSession.ResetAll();
        panelRoot.SetActive(false);
        isPaused = false;
        SceneManager.LoadScene(GameplaySceneName);
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        panelRoot.SetActive(false);
        isPaused = false;
        SetPlayerControlEnabled(true);
        SceneManager.LoadScene(MenuSceneName);
    }

    // ---- UI: mismo lenguaje visual que MainMenuController (fondo negro, acento cian) ----

    private void BuildUI()
    {
        var canvasGO = new GameObject("PauseCanvas");
        canvasGO.transform.SetParent(transform, false);
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 10; // por encima de GameHUD/MissionUI/Minimap
        var scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasGO.AddComponent<GraphicRaycaster>();

        panelRoot = new GameObject("PausePanel");
        panelRoot.transform.SetParent(canvasGO.transform, false);
        var backdropRT = panelRoot.AddComponent<RectTransform>();
        backdropRT.anchorMin = Vector2.zero;
        backdropRT.anchorMax = Vector2.one;
        backdropRT.offsetMin = Vector2.zero;
        backdropRT.offsetMax = Vector2.zero;
        var backdrop = panelRoot.AddComponent<Image>();
        backdrop.color = new Color(0f, 0f, 0f, 0.75f);

        var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        var titleGO = new GameObject("Title");
        titleGO.transform.SetParent(panelRoot.transform, false);
        var titleRT = titleGO.AddComponent<RectTransform>();
        titleRT.anchorMin = titleRT.anchorMax = new Vector2(0.5f, 0.5f);
        titleRT.anchoredPosition = new Vector2(0, 180);
        titleRT.sizeDelta = new Vector2(600, 80);
        var titleText = titleGO.AddComponent<Text>();
        titleText.font = font;
        titleText.fontSize = 44;
        titleText.fontStyle = FontStyle.Bold;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = new Color(0.35f, 0.95f, 1f);
        titleText.text = "PAUSA";
        var titleOutline = titleGO.AddComponent<Outline>();
        titleOutline.effectColor = new Color(0f, 0f, 0f, 0.9f);
        titleOutline.effectDistance = new Vector2(2f, -2f);

        CreateMenuButton(panelRoot.transform, "Continuar", "CONTINUAR", 60f, Continuar);
        CreateMenuButton(panelRoot.transform, "Reiniciar", "REINICIAR", -32f, Reiniciar);
        CreateMenuButton(panelRoot.transform, "VolverAlMenu", "VOLVER AL MENÚ", -124f, VolverAlMenu);

        panelRoot.SetActive(false);
    }

    private void CreateMenuButton(Transform parent, string name, string label, float anchoredY, UnityEngine.Events.UnityAction onClick)
    {
        var btnGO = new GameObject(name);
        btnGO.transform.SetParent(parent, false);
        var btnRT = btnGO.AddComponent<RectTransform>();
        btnRT.anchorMin = btnRT.anchorMax = new Vector2(0.5f, 0.5f);
        btnRT.pivot = new Vector2(0.5f, 0.5f);
        btnRT.anchoredPosition = new Vector2(0, anchoredY);
        btnRT.sizeDelta = new Vector2(360, 64);

        // Sin sprite: UnityEditor.AssetDatabase.GetBuiltinExtraResource (usado para el mismo
        // boton en MainMenuController) solo existe en el Editor y rompe en build. Un Image sin
        // sprite ya rellena el rect con su color solido, visualmente equivalente para esto.
        var img = btnGO.AddComponent<Image>();
        img.color = Color.black;

        var btn = btnGO.AddComponent<Button>();
        btn.targetGraphic = img;
        var colors = btn.colors;
        colors.normalColor = new Color(0, 0, 0, 1);
        colors.highlightedColor = new Color(0, 1, 1, 1);
        colors.pressedColor = new Color(0, 0, 0.545f, 1);
        colors.selectedColor = new Color(0.96f, 0.96f, 0.96f, 1);
        colors.disabledColor = new Color(0.784f, 0.784f, 0.784f, 0.5f);
        btn.colors = colors;
        btn.onClick.AddListener(onClick);

        var outline = btnGO.AddComponent<Outline>();
        outline.effectColor = new Color(0, 1, 1, 1);
        outline.effectDistance = new Vector2(2, 2);

        var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        var labelGO = new GameObject("Label");
        labelGO.transform.SetParent(btnGO.transform, false);
        var labelRT = labelGO.AddComponent<RectTransform>();
        labelRT.anchorMin = Vector2.zero;
        labelRT.anchorMax = Vector2.one;
        labelRT.offsetMin = Vector2.zero;
        labelRT.offsetMax = Vector2.zero;
        var labelText = labelGO.AddComponent<Text>();
        labelText.font = font;
        labelText.fontSize = 28;
        labelText.fontStyle = FontStyle.Bold;
        labelText.alignment = TextAnchor.MiddleCenter;
        labelText.color = Color.white;
        labelText.text = label;
    }
}
