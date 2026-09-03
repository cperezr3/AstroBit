using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Menu de pausa (Prompt 28). Responsable unicamente de: abrir/cerrar la pausa, Time.timeScale,
// visibilidad de su propio Canvas, y las acciones del menu (Continuar/Configuración/Reiniciar/
// Volver al Menu). No conoce nada de mision/progresion -- eso vive en GameSession y en los
// sistemas ya existentes.
//
// Mismo patron que GameHUD/MinimapController/MissionUI: singleton unico creado una sola vez
// via RuntimeInitializeOnLoadMethod + DontDestroyOnLoad, activo solo mientras la escena actual
// sea SampleScene (igual que el resto de la UI de gameplay se oculta en MainMenu).
//
// Fase 2 (Prompt 35, secciones 6-9): migrado a TextMeshPro y agrega el boton "Configuración"
// (abre SettingsUI, el mismo panel que el Main Menu). "Volver al Menu" ahora guarda la partida
// antes de salir, para que "Continuar" desde el Main Menu la recupere igual que si se hubiera
// cerrado el juego.
public class PauseMenuController : MonoBehaviour
{
    private const string GameplaySceneName = "SampleScene";
    private const string MenuSceneName = "MainMenu";
    private static readonly Color AccentCyan = new Color(0f, 1f, 1f, 1f);

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

        // Los botones del panel nunca se destruyen, solo se ocultan -- el EventSystem sigue
        // recordando cual fue clickeado por ultima vez (p.ej. "Continuar"), asi que al
        // reactivar el panel ese boton se redibuja en estado Selected (borde celeste) sin que
        // el mouse este encima. Limpiar la seleccion (no los componentes Selectable) al abrir y
        // al cerrar deja cada apertura visualmente limpia sin afectar Highlighted/Pressed.
        if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);
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

    public void Configuracion()
    {
        SettingsUI.Instance.Open();
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;
        GameSession.ResetAll();
        SaveManager.Instance.DeleteSave();
        panelRoot.SetActive(false);
        isPaused = false;
        SceneManager.LoadScene(GameplaySceneName);
    }

    public void VolverAlMenu()
    {
        // Fase 2 (Prompt 35, 9.1): guarda antes de salir para que "Continuar" desde el Main Menu
        // recupere exactamente este punto, igual que si el jugador hubiera cerrado AstroBit aqui.
        SaveManager.Instance.SaveGame();

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

        var titleGO = new GameObject("Title");
        titleGO.transform.SetParent(panelRoot.transform, false);
        var titleRT = titleGO.AddComponent<RectTransform>();
        titleRT.anchorMin = titleRT.anchorMax = new Vector2(0.5f, 0.5f);
        titleRT.anchoredPosition = new Vector2(0, 210);
        titleRT.sizeDelta = new Vector2(600, 80);
        var titleText = titleGO.AddComponent<TextMeshProUGUI>();
        titleText.fontSize = 44;
        titleText.fontStyle = FontStyles.Bold;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.color = AccentCyan;
        titleText.text = "PAUSA";

        CreateMenuButton(panelRoot.transform, "Continuar", "CONTINUAR", 108f, Continuar);
        CreateMenuButton(panelRoot.transform, "Configuracion", "CONFIGURACIÓN", 24f, Configuracion);
        CreateMenuButton(panelRoot.transform, "Reiniciar", "REINICIAR SECCIÓN", -60f, Reiniciar);
        CreateMenuButton(panelRoot.transform, "VolverAlMenu", "VOLVER AL MENÚ", -144f, VolverAlMenu);

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

        var labelGO = new GameObject("Label");
        labelGO.transform.SetParent(btnGO.transform, false);
        var labelRT = labelGO.AddComponent<RectTransform>();
        labelRT.anchorMin = Vector2.zero;
        labelRT.anchorMax = Vector2.one;
        labelRT.offsetMin = Vector2.zero;
        labelRT.offsetMax = Vector2.zero;
        var labelText = labelGO.AddComponent<TextMeshProUGUI>();
        labelText.fontSize = 26;
        labelText.fontStyle = FontStyles.Bold;
        labelText.alignment = TextAlignmentOptions.Center;
        labelText.color = Color.white;
        labelText.text = label;
    }
}
