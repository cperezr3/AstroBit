using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Fase 2 (Prompt 35, secciones 6-8): panel de "Configuración" real, accesible tanto desde el
// Main Menu ("Opciones") como desde el menu de pausa ("Configuración") -- por eso vive en su
// propio Canvas DontDestroyOnLoad en vez de dentro de GameHUD (oculto en MainMenu) o de
// PauseMenuController (oculto fuera de SampleScene). Mismo patron singleton que el resto de la
// UI (Bootstrap + DontDestroyOnLoad).
//
// Los cambios se aplican de inmediato via SettingsManager (persistido en PlayerPrefs); este
// script solo construye los controles y los conecta, no guarda ni decide valores por su cuenta.
//
// Fase 3 (Prompt 01_maestro, seccion 5): el layout original anclaba cada fila con
// anchorMin/anchorMax = (0,1) (esquina superior-izquierda del panel) pero les aplicaba un offset
// pensado para un anclaje centrado (x = -320), lo que empujaba cada slider/label/toggle cientos
// de unidades fuera del panel hacia la izquierda. Reconstruido con un layout de dos columnas
// (label a la izquierda, control a la derecha) anclado siempre a la esquina superior-izquierda
// con offsets positivos, para que todo quede garantizado dentro del cuadro.
//
// Prompt 06 (Bloque 3): reorganizado en pestañas (Audio/Controles/Gráficos) -- agregar la UI de
// remapeo de controles como una sección mas dentro de la unica columna vertical anterior no
// entraba en los 1010px de alto del panel junto con lo que ya habia. El contenido de cada
// pestaña vive en su propio contenedor, mostrado/ocultado con SetActive; los helpers de
// construccion (CreateSlider/CreateToggle/CreateCycleSelector/etc.) no cambiaron de forma.
public class SettingsUI : MonoBehaviour
{
    private static readonly Color AccentCyan = new Color(0.35f, 0.95f, 1f);
    private static readonly Color SectionColor = new Color(0.7f, 0.85f, 0.9f);

    private const float BoxWidth = 820f;
    private const float BoxHeight = 1010f;
    private const float LabelX = 40f;
    private const float LabelWidth = 300f;
    private const float ControlX = 360f;
    private const float ControlWidth = 420f;
    private const float RowHeight = 56f;
    private const float TabContentTopY = -170f;

    public static SettingsUI Instance { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        if (Instance != null) return;
        var go = new GameObject("SettingsUI");
        go.AddComponent<SettingsUI>();
    }

    private GameObject panelRoot;
    private Slider masterSlider;
    private Slider musicSlider;
    private Slider sfxSlider;
    private Slider uiSlider;
    private Slider sensitivitySlider;
    private Toggle invertYToggle;
    private Toggle fullscreenToggle;
    private Toggle vsyncToggle;
    private CycleSelector resolutionSelector;
    private CycleSelector qualitySelector;
    private ControlsRebindingPanel rebindingPanel;

    private GameObject[] tabContents;
    private Image[] tabButtonBackgrounds;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        BuildUI();
    }

    public void Open()
    {
        RefreshFromSettings();
        rebindingPanel.RefreshLabels();
        panelRoot.SetActive(true);
    }

    public void Close()
    {
        panelRoot.SetActive(false);
    }

    // Bugfix (Esc jerarquico): expuestos para que PauseMenuController decida que hace Esc segun
    // que este abierto -- ver PauseMenuController.Update().
    public bool IsOpen => panelRoot != null && panelRoot.activeSelf;
    public bool IsRebindingActive => rebindingPanel != null && rebindingPanel.IsRebinding;

    private void RefreshFromSettings()
    {
        var s = SettingsManager.Instance;
        masterSlider.SetValueWithoutNotify(s.MasterVolume);
        musicSlider.SetValueWithoutNotify(s.MusicVolume);
        sfxSlider.SetValueWithoutNotify(s.SfxVolume);
        uiSlider.SetValueWithoutNotify(s.UiVolume);
        sensitivitySlider.SetValueWithoutNotify(s.CameraSensitivity);
        invertYToggle.SetIsOnWithoutNotify(s.InvertY);
        fullscreenToggle.SetIsOnWithoutNotify(s.Fullscreen);
        vsyncToggle.SetIsOnWithoutNotify(s.VSync);
        resolutionSelector.SetIndexWithoutNotify(s.ResolutionIndex);
        qualitySelector.SetIndexWithoutNotify(s.QualityLevel);
    }

    private void BuildUI()
    {
        var canvasGO = new GameObject("SettingsCanvas");
        canvasGO.transform.SetParent(transform, false);
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 20; // por encima de PauseMenuController (10), puede abrirse desde ahi
        var scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasGO.AddComponent<GraphicRaycaster>();

        panelRoot = new GameObject("SettingsPanel", typeof(RectTransform));
        panelRoot.transform.SetParent(canvasGO.transform, false);
        var backdropRT = panelRoot.GetComponent<RectTransform>();
        backdropRT.anchorMin = Vector2.zero;
        backdropRT.anchorMax = Vector2.one;
        backdropRT.offsetMin = Vector2.zero;
        backdropRT.offsetMax = Vector2.zero;
        panelRoot.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.8f);

        var boxGO = new GameObject("Box", typeof(RectTransform));
        boxGO.transform.SetParent(panelRoot.transform, false);
        var boxRT = boxGO.GetComponent<RectTransform>();
        boxRT.anchorMin = boxRT.anchorMax = boxRT.pivot = new Vector2(0.5f, 0.5f);
        boxRT.sizeDelta = new Vector2(BoxWidth, BoxHeight);
        boxGO.AddComponent<Image>().color = new Color(0.05f, 0.07f, 0.09f, 0.97f);
        var boxOutline = boxGO.AddComponent<Outline>();
        boxOutline.effectColor = AccentCyan;
        boxOutline.effectDistance = new Vector2(2, 2);

        CreateTitle(boxGO.transform, "CONFIGURACIÓN");
        BuildTabs(boxGO.transform);

        var audioTab = BuildAudioTab(boxGO.transform);
        var controlsTab = BuildControlsTab(boxGO.transform);
        var graphicsTab = BuildGraphicsTab(boxGO.transform);
        tabContents = new[] { audioTab, controlsTab, graphicsTab };

        CreateCloseButton(boxGO.transform);

        SelectTab(0);
        panelRoot.SetActive(false);
    }

    // ---- Pestañas ----

    private void BuildTabs(Transform parent)
    {
        string[] names = { "AUDIO", "CONTROLES", "GRÁFICOS" };
        tabButtonBackgrounds = new Image[names.Length];

        const float tabWidth = 250f;
        const float tabHeight = 46f;
        const float gap = 15f;
        float totalWidth = tabWidth * names.Length + gap * (names.Length - 1);
        float startX = -totalWidth / 2f + tabWidth / 2f;

        for (int i = 0; i < names.Length; i++)
        {
            int index = i; // captura por valor para el closure del listener
            var btnGO = new GameObject("Tab_" + names[i], typeof(RectTransform));
            btnGO.transform.SetParent(parent, false);
            var rt = btnGO.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 1f);
            rt.pivot = new Vector2(0.5f, 1f);
            rt.anchoredPosition = new Vector2(startX + i * (tabWidth + gap), -100f);
            rt.sizeDelta = new Vector2(tabWidth, tabHeight);

            var img = btnGO.AddComponent<Image>();
            img.color = new Color(1f, 1f, 1f, 0.08f);
            tabButtonBackgrounds[i] = img;

            var button = btnGO.AddComponent<Button>();
            button.targetGraphic = img;
            var colors = button.colors;
            colors.highlightedColor = new Color(1f, 1f, 1f, 0.2f);
            colors.pressedColor = new Color(0f, 0.545f, 0.545f, 1f);
            button.colors = colors;
            button.onClick.AddListener(() => SelectTab(index));

            var labelGO = new GameObject("Label", typeof(RectTransform));
            labelGO.transform.SetParent(btnGO.transform, false);
            var labelRT = labelGO.GetComponent<RectTransform>();
            labelRT.anchorMin = Vector2.zero;
            labelRT.anchorMax = Vector2.one;
            labelRT.offsetMin = Vector2.zero;
            labelRT.offsetMax = Vector2.zero;
            var labelTmp = labelGO.AddComponent<TextMeshProUGUI>();
            labelTmp.text = names[i];
            labelTmp.fontSize = 18;
            labelTmp.fontStyle = FontStyles.Bold;
            labelTmp.alignment = TextAlignmentOptions.Center;
            labelTmp.color = Color.white;
        }
    }

    private void SelectTab(int index)
    {
        for (int i = 0; i < tabContents.Length; i++)
        {
            tabContents[i].SetActive(i == index);
            tabButtonBackgrounds[i].color = i == index
                ? new Color(AccentCyan.r, AccentCyan.g, AccentCyan.b, 0.35f)
                : new Color(1f, 1f, 1f, 0.08f);
        }
    }

    // ---- Contenido de cada pestaña ----

    private GameObject BuildAudioTab(Transform parent)
    {
        var content = new GameObject("Tab_Audio_Content", typeof(RectTransform));
        content.transform.SetParent(parent, false);
        SetFullRect(content.GetComponent<RectTransform>());

        float y = TabContentTopY;
        masterSlider = CreateSlider(content.transform, "Volumen maestro", y, 0f, 1f, v => SettingsManager.Instance.SetMasterVolume(v)); y -= RowHeight;
        musicSlider = CreateSlider(content.transform, "Volumen de música", y, 0f, 1f, v => SettingsManager.Instance.SetMusicVolume(v)); y -= RowHeight;
        sfxSlider = CreateSlider(content.transform, "Volumen de efectos", y, 0f, 1f, v => SettingsManager.Instance.SetSfxVolume(v)); y -= RowHeight;
        uiSlider = CreateSlider(content.transform, "Volumen de interfaz", y, 0f, 1f, v => SettingsManager.Instance.SetUiVolume(v));

        return content;
    }

    private GameObject BuildControlsTab(Transform parent)
    {
        var content = new GameObject("Tab_Controls_Content", typeof(RectTransform));
        content.transform.SetParent(parent, false);
        SetFullRect(content.GetComponent<RectTransform>());

        float y = TabContentTopY;
        CreateSectionLabel(content.transform, "CÁMARA", y); y -= RowHeight;
        sensitivitySlider = CreateSlider(content.transform, "Sensibilidad de cámara", y,
            SettingsManager.MinSensitivity, SettingsManager.MaxSensitivity, v => SettingsManager.Instance.SetCameraSensitivity(v)); y -= RowHeight;
        invertYToggle = CreateToggle(content.transform, "Invertir eje Y", y, v => SettingsManager.Instance.SetInvertY(v)); y -= (RowHeight + 16f);

        var rebindGO = new GameObject("RebindingPanel", typeof(RectTransform));
        rebindGO.transform.SetParent(content.transform, false);
        SetFullRect(rebindGO.GetComponent<RectTransform>());
        rebindingPanel = rebindGO.AddComponent<ControlsRebindingPanel>();
        rebindingPanel.Init(rebindGO.transform, y, LabelX, ControlX, ControlWidth);

        return content;
    }

    private GameObject BuildGraphicsTab(Transform parent)
    {
        var content = new GameObject("Tab_Graphics_Content", typeof(RectTransform));
        content.transform.SetParent(parent, false);
        SetFullRect(content.GetComponent<RectTransform>());

        float y = TabContentTopY;
        var sm = SettingsManager.Instance;
        resolutionSelector = CreateCycleSelector(content.transform, "Resolución", y, sm.ResolutionCount, sm.GetResolutionLabel,
            i => SettingsManager.Instance.SetResolutionIndex(i)); y -= RowHeight;
        fullscreenToggle = CreateToggle(content.transform, "Pantalla completa", y, v => SettingsManager.Instance.SetFullscreen(v)); y -= RowHeight;
        vsyncToggle = CreateToggle(content.transform, "VSync", y, v => SettingsManager.Instance.SetVSync(v)); y -= RowHeight;
        qualitySelector = CreateCycleSelector(content.transform, "Calidad", y, sm.QualityNames.Length, i => sm.QualityNames[i],
            i => SettingsManager.Instance.SetQualityLevel(i));

        return content;
    }

    private static void SetFullRect(RectTransform rt)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    private void CreateTitle(Transform parent, string text)
    {
        var go = new GameObject("Title", typeof(RectTransform));
        go.transform.SetParent(parent, false);
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 1f);
        rt.pivot = new Vector2(0.5f, 1f);
        rt.anchoredPosition = new Vector2(0, -30);
        rt.sizeDelta = new Vector2(700, 60);
        var tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 36;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = AccentCyan;
    }

    private static void CreateSectionLabel(Transform parent, string text, float y)
    {
        var go = new GameObject("Section_" + text, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0f, 1f);
        rt.anchoredPosition = new Vector2(LabelX, y);
        rt.sizeDelta = new Vector2(400, 30);
        var tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 20;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Left;
        tmp.color = SectionColor;
    }

    private static RectTransform CreateRowLabel(Transform parent, string label, float y)
    {
        var labelGO = new GameObject("Label_" + label, typeof(RectTransform));
        labelGO.transform.SetParent(parent, false);
        var labelRT = labelGO.GetComponent<RectTransform>();
        labelRT.anchorMin = labelRT.anchorMax = labelRT.pivot = new Vector2(0f, 1f);
        labelRT.anchoredPosition = new Vector2(LabelX, y);
        labelRT.sizeDelta = new Vector2(LabelWidth, RowHeight - 12f);
        var labelTmp = labelGO.AddComponent<TextMeshProUGUI>();
        labelTmp.text = label;
        labelTmp.fontSize = 18;
        labelTmp.alignment = TextAlignmentOptions.Left;
        labelTmp.color = Color.white;
        return labelRT;
    }

    private static Slider CreateSlider(Transform parent, string label, float y, float min, float max, UnityEngine.Events.UnityAction<float> onChanged)
    {
        CreateRowLabel(parent, label, y);

        var sliderGO = new GameObject("Slider_" + label, typeof(RectTransform));
        sliderGO.transform.SetParent(parent, false);
        var sliderRT = sliderGO.GetComponent<RectTransform>();
        sliderRT.anchorMin = sliderRT.anchorMax = sliderRT.pivot = new Vector2(0f, 1f);
        sliderRT.anchoredPosition = new Vector2(ControlX, y - 2f);
        sliderRT.sizeDelta = new Vector2(ControlWidth, 24);

        var bgGO = new GameObject("Background", typeof(RectTransform));
        bgGO.transform.SetParent(sliderGO.transform, false);
        var bgRT = bgGO.GetComponent<RectTransform>();
        bgRT.anchorMin = new Vector2(0, 0.25f);
        bgRT.anchorMax = new Vector2(1, 0.75f);
        bgRT.offsetMin = bgRT.offsetMax = Vector2.zero;
        bgGO.AddComponent<Image>().color = new Color(1f, 1f, 1f, 0.15f);

        var fillAreaGO = new GameObject("Fill Area", typeof(RectTransform));
        fillAreaGO.transform.SetParent(sliderGO.transform, false);
        var fillAreaRT = fillAreaGO.GetComponent<RectTransform>();
        fillAreaRT.anchorMin = new Vector2(0, 0.25f);
        fillAreaRT.anchorMax = new Vector2(1, 0.75f);
        fillAreaRT.offsetMin = new Vector2(5, 0);
        fillAreaRT.offsetMax = new Vector2(-5, 0);

        var fillGO = new GameObject("Fill", typeof(RectTransform));
        fillGO.transform.SetParent(fillAreaGO.transform, false);
        var fillRT = fillGO.GetComponent<RectTransform>();
        fillRT.anchorMin = Vector2.zero;
        fillRT.anchorMax = new Vector2(0, 1);
        fillRT.sizeDelta = new Vector2(10, 0);
        fillGO.AddComponent<Image>().color = AccentCyan;

        var handleAreaGO = new GameObject("Handle Slide Area", typeof(RectTransform));
        handleAreaGO.transform.SetParent(sliderGO.transform, false);
        var handleAreaRT = handleAreaGO.GetComponent<RectTransform>();
        handleAreaRT.anchorMin = Vector2.zero;
        handleAreaRT.anchorMax = Vector2.one;
        handleAreaRT.offsetMin = new Vector2(10, 0);
        handleAreaRT.offsetMax = new Vector2(-10, 0);

        var handleGO = new GameObject("Handle", typeof(RectTransform));
        handleGO.transform.SetParent(handleAreaGO.transform, false);
        var handleRT = handleGO.GetComponent<RectTransform>();
        handleRT.sizeDelta = new Vector2(20, 20);
        var handleImage = handleGO.AddComponent<Image>();
        handleImage.color = Color.white;

        var slider = sliderGO.AddComponent<Slider>();
        slider.fillRect = fillRT;
        slider.handleRect = handleRT;
        slider.targetGraphic = handleImage;
        slider.direction = Slider.Direction.LeftToRight;
        slider.minValue = min;
        slider.maxValue = max;
        slider.onValueChanged.AddListener(onChanged);

        return slider;
    }

    private static Toggle CreateToggle(Transform parent, string label, float y, UnityEngine.Events.UnityAction<bool> onChanged)
    {
        CreateRowLabel(parent, label, y);

        var toggleGO = new GameObject("Toggle_" + label, typeof(RectTransform));
        toggleGO.transform.SetParent(parent, false);
        var toggleRT = toggleGO.GetComponent<RectTransform>();
        toggleRT.anchorMin = toggleRT.anchorMax = toggleRT.pivot = new Vector2(0f, 1f);
        toggleRT.anchoredPosition = new Vector2(ControlX, y);
        toggleRT.sizeDelta = new Vector2(28, 28);

        var bgGO = new GameObject("Background", typeof(RectTransform));
        bgGO.transform.SetParent(toggleGO.transform, false);
        var bgRT = bgGO.GetComponent<RectTransform>();
        bgRT.anchorMin = Vector2.zero;
        bgRT.anchorMax = Vector2.one;
        bgRT.offsetMin = bgRT.offsetMax = Vector2.zero;
        var bgImage = bgGO.AddComponent<Image>();
        bgImage.color = new Color(1f, 1f, 1f, 0.15f);

        var checkGO = new GameObject("Checkmark", typeof(RectTransform));
        checkGO.transform.SetParent(bgGO.transform, false);
        var checkRT = checkGO.GetComponent<RectTransform>();
        checkRT.anchorMin = Vector2.zero;
        checkRT.anchorMax = Vector2.one;
        checkRT.offsetMin = new Vector2(5, 5);
        checkRT.offsetMax = new Vector2(-5, -5);
        var checkImage = checkGO.AddComponent<Image>();
        checkImage.color = AccentCyan;

        var toggle = toggleGO.AddComponent<Toggle>();
        toggle.targetGraphic = bgImage;
        toggle.graphic = checkImage;
        toggle.onValueChanged.AddListener(onChanged);

        return toggle;
    }

    // Selector "< valor >" para opciones discretas (resolucion, calidad) sin depender del
    // andamiaje completo de TMP_Dropdown (Template/Viewport/Scrollbar), dificil de construir bien
    // desde codigo sin verificacion visual interactiva. Mismo lenguaje visual que el resto.
    private static CycleSelector CreateCycleSelector(Transform parent, string label, float y, int optionCount,
        System.Func<int, string> labelForIndex, UnityEngine.Events.UnityAction<int> onChanged)
    {
        CreateRowLabel(parent, label, y);

        var rowGO = new GameObject("Selector_" + label, typeof(RectTransform));
        rowGO.transform.SetParent(parent, false);
        var rowRT = rowGO.GetComponent<RectTransform>();
        rowRT.anchorMin = rowRT.anchorMax = rowRT.pivot = new Vector2(0f, 1f);
        rowRT.anchoredPosition = new Vector2(ControlX, y);
        rowRT.sizeDelta = new Vector2(ControlWidth, RowHeight - 12f);

        var prevButton = CreateArrowButton(rowGO.transform, "<", new Vector2(0f, 0f), new Vector2(0f, 0f));
        var nextButton = CreateArrowButton(rowGO.transform, ">", new Vector2(1f, 0f), new Vector2(1f, 0f));

        var valueGO = new GameObject("Value", typeof(RectTransform));
        valueGO.transform.SetParent(rowGO.transform, false);
        var valueRT = valueGO.GetComponent<RectTransform>();
        valueRT.anchorMin = new Vector2(0f, 0f);
        valueRT.anchorMax = new Vector2(1f, 1f);
        valueRT.offsetMin = new Vector2(52, 0);
        valueRT.offsetMax = new Vector2(-52, 0);
        var valueTmp = valueGO.AddComponent<TextMeshProUGUI>();
        valueTmp.fontSize = 18;
        valueTmp.alignment = TextAlignmentOptions.Center;
        valueTmp.color = AccentCyan;

        var selector = rowGO.AddComponent<CycleSelector>();
        selector.Init(valueTmp, optionCount, labelForIndex, onChanged);
        prevButton.onClick.AddListener(selector.Previous);
        nextButton.onClick.AddListener(selector.Next);

        return selector;
    }

    private static Button CreateArrowButton(Transform parent, string label, Vector2 anchor, Vector2 pivotSide)
    {
        var btnGO = new GameObject("Arrow_" + label, typeof(RectTransform));
        btnGO.transform.SetParent(parent, false);
        var rt = btnGO.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(anchor.x, 0.5f);
        rt.pivot = new Vector2(pivotSide.x, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = new Vector2(44, 44);

        var img = btnGO.AddComponent<Image>();
        img.color = new Color(1f, 1f, 1f, 0.12f);
        var button = btnGO.AddComponent<Button>();
        button.targetGraphic = img;
        var colors = button.colors;
        colors.highlightedColor = AccentCyan;
        colors.pressedColor = new Color(0, 0.545f, 0.545f, 1);
        button.colors = colors;

        var labelGO = new GameObject("Label", typeof(RectTransform));
        labelGO.transform.SetParent(btnGO.transform, false);
        var labelRT = labelGO.GetComponent<RectTransform>();
        labelRT.anchorMin = Vector2.zero;
        labelRT.anchorMax = Vector2.one;
        labelRT.offsetMin = Vector2.zero;
        labelRT.offsetMax = Vector2.zero;
        var labelTmp = labelGO.AddComponent<TextMeshProUGUI>();
        labelTmp.text = label;
        labelTmp.fontSize = 22;
        labelTmp.fontStyle = FontStyles.Bold;
        labelTmp.alignment = TextAlignmentOptions.Center;
        labelTmp.color = Color.white;

        return button;
    }

    private void CreateCloseButton(Transform parent)
    {
        var btnGO = new GameObject("CloseButton", typeof(RectTransform));
        btnGO.transform.SetParent(parent, false);
        var rt = btnGO.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0f);
        rt.pivot = new Vector2(0.5f, 0f);
        rt.anchoredPosition = new Vector2(0, 30);
        rt.sizeDelta = new Vector2(240, 56);

        var img = btnGO.AddComponent<Image>();
        img.color = Color.black;
        var button = btnGO.AddComponent<Button>();
        button.targetGraphic = img;
        var colors = button.colors;
        colors.highlightedColor = AccentCyan;
        colors.pressedColor = new Color(0, 0.545f, 0.545f, 1);
        button.colors = colors;
        button.onClick.AddListener(Close);

        var outline = btnGO.AddComponent<Outline>();
        outline.effectColor = AccentCyan;
        outline.effectDistance = new Vector2(2, 2);

        var labelGO = new GameObject("Label", typeof(RectTransform));
        labelGO.transform.SetParent(btnGO.transform, false);
        var labelRT = labelGO.GetComponent<RectTransform>();
        labelRT.anchorMin = Vector2.zero;
        labelRT.anchorMax = Vector2.one;
        labelRT.offsetMin = Vector2.zero;
        labelRT.offsetMax = Vector2.zero;
        var labelTmp = labelGO.AddComponent<TextMeshProUGUI>();
        labelTmp.text = "CERRAR";
        labelTmp.fontSize = 22;
        labelTmp.fontStyle = FontStyles.Bold;
        labelTmp.alignment = TextAlignmentOptions.Center;
        labelTmp.color = Color.white;
    }
}
