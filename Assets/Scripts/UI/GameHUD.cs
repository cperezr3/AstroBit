using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    public static GameHUD Instance { get; private set; }

    // Prompt 26: el HUD de gameplay (objetivo, pista, prompt, feedback, progreso, inventario)
    // no debe verse en el Main Menu. GameHUD sigue creandose igual (DontDestroyOnLoad) para no
    // romper su inicializacion perezosa; solo se oculta/muestra el Canvas segun la escena activa.
    private const string MenuSceneName = "MainMenu";

    private enum PanelMode { Info, Activity, Choice, Reward }

    [SerializeField] private float feedbackDuration = 3.5f;
    [SerializeField] private float locationDescriptionHoldTime = 3f;

    private const int ChoiceOptionCount = 3;
    private const float ObjectiveHintGap = 14f;

    private Transform hudCanvasTransform;

    private TextMeshProUGUI objectiveText;
    private TextMeshProUGUI hintText;
    private RectTransform promptRoot;
    private CanvasGroup promptCanvasGroup;
    private TextMeshProUGUI promptKeyLabel;
    private TextMeshProUGUI promptActionLabel;
    private Coroutine promptAnimRoutine;
    private const float PromptShownY = 130f;
    private const float PromptSlideOffset = 14f;
    private const float PromptFadeDuration = 0.12f;
    private TextMeshProUGUI feedbackText;
    private Coroutine feedbackRoutine;

    private TextMeshProUGUI locationText;
    private TextMeshProUGUI locationSubtitleText;
    private Coroutine locationSubtitleRoutine;

    private TextMeshProUGUI progressText;
    private TextMeshProUGUI inventoryText;

    private GameObject panelRoot;
    private TextMeshProUGUI panelTitleText;
    private TextMeshProUGUI panelSubtitleText;
    private TextMeshProUGUI panelBodyText;
    private TextMeshProUGUI panelQuestionText;
    private TextMeshProUGUI panelResultText;
    private TMP_InputField panelInputField;
    private Button panelPrimaryButton;
    private TextMeshProUGUI panelPrimaryButtonLabel;
    private Button panelCloseButton;
    private Button[] panelOptionButtons;
    private TextMeshProUGUI[] panelOptionLabels;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        if (Instance != null) return;
        var go = new GameObject("GameHUD");
        go.AddComponent<GameHUD>();
    }

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
        BuildPanel();

        ObjectiveSystem.Instance.OnObjectiveChanged.AddListener(SetObjectiveText);
        ObjectiveSystem.Instance.OnHintChanged.AddListener(SetHintText);
        ObjectiveSystem.Instance.OnObjectiveCompleted.AddListener(text => ShowFeedback(text));
        ObjectiveSystem.Instance.OnObjectiveCompleted.AddListener(_ => UpdateProgressText());

        SetObjectiveText(ObjectiveSystem.Instance.CurrentObjective);
        SetHintText(ObjectiveSystem.Instance.CurrentHint);
        HidePromptImmediate();
        HideFeedbackImmediate();
        SetLocation("", "");
        UpdateProgressText();

        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        UpdateVisibilityForScene(SceneManager.GetActiveScene().name);
    }

    private void OnActiveSceneChanged(Scene previous, Scene next)
    {
        UpdateVisibilityForScene(next.name);

        // Bug (Prompt 28): LocationZone guarda la zona actual en un campo static que sobrevive
        // a un recargado de SampleScene (Continuar/Reiniciar); sin este reset, el indicador
        // superior podia quedar mostrando la ultima zona visitada de la partida anterior hasta
        // que el jugador cruzara un LocationZone de nuevo.
        if (next.name != MenuSceneName)
        {
            SetLocation("", "");
        }
    }

    private void UpdateVisibilityForScene(string sceneName)
    {
        hudCanvasTransform.gameObject.SetActive(sceneName != MenuSceneName);
    }

    private void BuildUI()
    {
        var canvasGO = new GameObject("HUDCanvas");
        canvasGO.transform.SetParent(transform, false);
        var canvas = canvasGO.AddComponent<Canvas>();
        hudCanvasTransform = canvasGO.transform; // capturado despues de AddComponent<Canvas>: Unity reemplaza Transform por RectTransform al agregar el Canvas
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasGO.AddComponent<GraphicRaycaster>();

        objectiveText = CreateText(canvasGO.transform, "ObjectiveText", 28, TextAlignmentOptions.TopLeft,
            new Vector2(0, 1), new Vector2(0, 1), new Vector2(20, -20), new Vector2(760, 40));

        hintText = CreateText(canvasGO.transform, "HintText", 19, TextAlignmentOptions.TopLeft,
            new Vector2(0, 1), new Vector2(0, 1), new Vector2(20, -58), new Vector2(760, 50));
        hintText.color = new Color(0.75f, 0.9f, 0.95f);

        CreatePromptUI(canvasGO.transform);

        feedbackText = CreateText(canvasGO.transform, "FeedbackText", 26, TextAlignmentOptions.Bottom,
            new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0, 190), new Vector2(960, 90));

        locationText = CreateText(canvasGO.transform, "LocationText", 30, TextAlignmentOptions.Top,
            new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0, -20), new Vector2(500, 40));
        locationText.fontStyle = FontStyles.Bold;
        locationText.color = new Color(0.35f, 0.95f, 1f);

        locationSubtitleText = CreateText(canvasGO.transform, "LocationSubtitleText", 18, TextAlignmentOptions.Top,
            new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0, -56), new Vector2(500, 30));
        locationSubtitleText.color = new Color(0.85f, 0.85f, 0.85f);

        progressText = CreateText(canvasGO.transform, "ProgressText", 22, TextAlignmentOptions.TopRight,
            new Vector2(1, 1), new Vector2(1, 1), new Vector2(-20, -20), new Vector2(200, 36));
        progressText.color = new Color(0.75f, 0.9f, 0.95f);

        inventoryText = CreateText(canvasGO.transform, "InventoryText", 20, TextAlignmentOptions.TopRight,
            new Vector2(1, 1), new Vector2(1, 1), new Vector2(-20, -60), new Vector2(200, 30));
        inventoryText.color = new Color(0.75f, 0.9f, 0.95f);
        inventoryText.enabled = false;
    }

    // Prompt 01_maestro (seccion 18): antes era un unico TextMeshProUGUI con el texto crudo
    // "[E] Interactuar" y un SetActive instantaneo. Ahora la tecla se separa visualmente en su
    // propia "credencial" (fondo + borde) apilada sobre la accion, y aparece/desaparece con un
    // fundido + deslizamiento breve en vez de un corte seco. Reutiliza la convencion de texto
    // "[X] Accion" que ya usan los 7 IInteractable existentes -- no hace falta tocarlos.
    private void CreatePromptUI(Transform parent)
    {
        var rootGO = new GameObject("PromptRoot", typeof(RectTransform));
        rootGO.transform.SetParent(parent, false);
        promptRoot = rootGO.GetComponent<RectTransform>();
        promptRoot.anchorMin = promptRoot.anchorMax = promptRoot.pivot = new Vector2(0.5f, 0f);
        promptRoot.sizeDelta = new Vector2(320, 78);
        promptRoot.anchoredPosition = new Vector2(0, PromptShownY - PromptSlideOffset);
        promptCanvasGroup = rootGO.AddComponent<CanvasGroup>();
        promptCanvasGroup.alpha = 0f;
        promptCanvasGroup.blocksRaycasts = false;
        promptCanvasGroup.interactable = false;

        var badgeGO = new GameObject("KeyBadge", typeof(RectTransform));
        badgeGO.transform.SetParent(rootGO.transform, false);
        var badgeRT = badgeGO.GetComponent<RectTransform>();
        badgeRT.anchorMin = badgeRT.anchorMax = new Vector2(0.5f, 1f);
        badgeRT.pivot = new Vector2(0.5f, 1f);
        badgeRT.anchoredPosition = Vector2.zero;
        badgeRT.sizeDelta = new Vector2(40, 40);
        var badgeImg = badgeGO.AddComponent<Image>();
        badgeImg.color = new Color(0f, 0f, 0f, 0.55f);
        badgeImg.raycastTarget = false;
        var badgeOutline = badgeGO.AddComponent<Outline>();
        badgeOutline.effectColor = new Color(0.35f, 0.95f, 1f);
        badgeOutline.effectDistance = new Vector2(1.5f, -1.5f);

        var keyLabelGO = new GameObject("KeyLabel", typeof(RectTransform));
        keyLabelGO.transform.SetParent(badgeGO.transform, false);
        var keyLabelRT = keyLabelGO.GetComponent<RectTransform>();
        keyLabelRT.anchorMin = Vector2.zero;
        keyLabelRT.anchorMax = Vector2.one;
        keyLabelRT.offsetMin = Vector2.zero;
        keyLabelRT.offsetMax = Vector2.zero;
        promptKeyLabel = keyLabelGO.AddComponent<TextMeshProUGUI>();
        promptKeyLabel.font = TMP_Settings.defaultFontAsset;
        promptKeyLabel.fontSize = 24;
        promptKeyLabel.fontStyle = FontStyles.Bold;
        promptKeyLabel.alignment = TextAlignmentOptions.Center;
        promptKeyLabel.color = new Color(0.35f, 0.95f, 1f);
        promptKeyLabel.raycastTarget = false;

        var actionGO = new GameObject("ActionLabel", typeof(RectTransform));
        actionGO.transform.SetParent(rootGO.transform, false);
        var actionRT = actionGO.GetComponent<RectTransform>();
        actionRT.anchorMin = actionRT.anchorMax = new Vector2(0.5f, 0f);
        actionRT.pivot = new Vector2(0.5f, 0f);
        actionRT.anchoredPosition = Vector2.zero;
        actionRT.sizeDelta = new Vector2(320, 32);
        promptActionLabel = actionGO.AddComponent<TextMeshProUGUI>();
        promptActionLabel.font = TMP_Settings.defaultFontAsset;
        promptActionLabel.fontSize = 26;
        promptActionLabel.fontStyle = FontStyles.Bold;
        promptActionLabel.alignment = TextAlignmentOptions.Center;
        promptActionLabel.color = Color.white;
        promptActionLabel.raycastTarget = false;
        var actionOutline = actionGO.AddComponent<Outline>();
        actionOutline.effectColor = new Color(0f, 0f, 0f, 0.85f);
        actionOutline.effectDistance = new Vector2(1.5f, -1.5f);

        rootGO.SetActive(false);
    }

    // Contador simple "X/8" de componentes comprendidos. Se actualiza con el mismo evento
    // que ya dispara el feedback de ObjectiveSystem, sin logica de progresion nueva.
    private void UpdateProgressText()
    {
        if (progressText == null) return;
        int completed = ObjectiveSystem.Instance.CompletedSteps;
        int total = ObjectiveSystem.Instance.TotalSteps;
        // Prompt 22: al llegar a 8/8 el contador se oculta (CompletedSteps/TotalSteps siguen
        // funcionando igual, solo se deja de mostrar el texto).
        bool allDone = completed >= total;
        progressText.text = allDone ? "" : completed + "/" + total;
        progressText.enabled = !allDone;
    }

    // Indicador de inventario minimalista (Prompt 20), p.ej. "RAM x2". Generico a proposito:
    // GameHUD no sabe nada de RAM/mision, solo muestra el texto que le pasen; se oculta con
    // texto vacio.
    public void SetInventoryText(string text)
    {
        if (inventoryText == null) return;
        bool show = !string.IsNullOrEmpty(text);
        inventoryText.text = text ?? "";
        inventoryText.enabled = show;
    }

    private static TextMeshProUGUI CreateText(Transform parent, string name, int size, TextAlignmentOptions anchor,
        Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPos, Vector2 sizeDelta)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);

        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.pivot = anchorMin;
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = sizeDelta;

        var text = go.AddComponent<TextMeshProUGUI>();
        text.font = TMP_Settings.defaultFontAsset;
        text.fontSize = size;
        text.alignment = anchor;
        text.color = Color.white;
        text.text = "";
        text.textWrappingMode = TextWrappingModes.Normal;
        text.overflowMode = TextOverflowModes.Overflow;

        var outline = go.AddComponent<Outline>();
        outline.effectColor = new Color(0f, 0f, 0f, 0.85f);
        outline.effectDistance = new Vector2(1.5f, -1.5f);

        return text;
    }

    private void SetObjectiveText(string text)
    {
        if (objectiveText == null) return;
        objectiveText.text = "OBJETIVO ACTUAL\n" + text;
        RepositionHintBelowObjective();
        // Prompt 28: el objetivo cambia tanto en progresion normal como en un reset de
        // "Nueva Partida"/"Reiniciar" (ver ObjectiveSystem.ResetState); refrescar el contador
        // aqui evita que quede mostrando un valor viejo (p.ej. "6/8") tras un reset.
        UpdateProgressText();
    }

    private void SetHintText(string text)
    {
        if (hintText == null) return;
        hintText.text = string.IsNullOrEmpty(text) ? "" : "PISTA: " + text;
    }

    // El objetivo puede ocupar 1 o varias lineas segun su longitud (wrap), asi que la
    // pista se reubica debajo usando la altura real del texto en vez de un offset fijo,
    // evitando que ambos se superpongan cuando el objetivo es largo.
    private void RepositionHintBelowObjective()
    {
        if (hintText == null) return;
        var objectiveRT = objectiveText.rectTransform;
        var hintRT = hintText.rectTransform;
        float objectiveBottom = objectiveRT.anchoredPosition.y - objectiveText.preferredHeight;
        hintRT.anchoredPosition = new Vector2(hintRT.anchoredPosition.x, objectiveBottom - ObjectiveHintGap);
    }

    // Nombre de zona en la parte superior del HUD (una unica ubicacion activa).
    // Un nombre vacio oculta el indicador (el jugador esta fuera de cualquier zona conocida).
    public void SetLocation(string locationName, string description)
    {
        if (locationText == null) return;

        if (locationSubtitleRoutine != null)
        {
            StopCoroutine(locationSubtitleRoutine);
            locationSubtitleRoutine = null;
        }

        locationText.text = locationName ?? "";
        locationText.enabled = !string.IsNullOrEmpty(locationName);

        locationSubtitleText.text = description ?? "";
        locationSubtitleText.enabled = !string.IsNullOrEmpty(description);

        if (!string.IsNullOrEmpty(description))
            locationSubtitleRoutine = StartCoroutine(HideLocationSubtitleAfterDelay());
    }

    private IEnumerator HideLocationSubtitleAfterDelay()
    {
        yield return new WaitForSeconds(locationDescriptionHoldTime);
        locationSubtitleText.enabled = false;
        locationSubtitleRoutine = null;
    }

    public void ShowPrompt(string text)
    {
        if (promptRoot == null) return;
        ApplyPromptText(text);
        promptRoot.gameObject.SetActive(true);
        AnimatePrompt(show: true);
    }

    public void HidePrompt()
    {
        if (promptRoot == null) return;
        AnimatePrompt(show: false);
    }

    private void HidePromptImmediate()
    {
        if (promptRoot == null) return;
        if (promptAnimRoutine != null) { StopCoroutine(promptAnimRoutine); promptAnimRoutine = null; }
        promptCanvasGroup.alpha = 0f;
        promptRoot.anchoredPosition = new Vector2(0, PromptShownY - PromptSlideOffset);
        promptRoot.gameObject.SetActive(false);
    }

    // Convencion existente en los IInteractable del proyecto: "[X] Accion" (ver FileShelf,
    // EducationalInteractable, CollectibleRam, etc.). Si algun texto no la sigue, se muestra
    // completo en la etiqueta de accion y la credencial de tecla cae de vuelta a "E".
    private void ApplyPromptText(string text)
    {
        string key = "E";
        string action = text ?? "";
        if (action.StartsWith("[", StringComparison.Ordinal))
        {
            int close = action.IndexOf(']');
            if (close > 0)
            {
                key = action.Substring(1, close - 1);
                action = action.Substring(close + 1).Trim();
            }
        }
        promptKeyLabel.text = key;
        promptActionLabel.text = action.ToUpperInvariant();
    }

    private void AnimatePrompt(bool show)
    {
        if (promptAnimRoutine != null) StopCoroutine(promptAnimRoutine);
        promptAnimRoutine = StartCoroutine(AnimatePromptRoutine(show));
    }

    private IEnumerator AnimatePromptRoutine(bool show)
    {
        float startAlpha = promptCanvasGroup.alpha;
        float endAlpha = show ? 1f : 0f;
        float endY = show ? PromptShownY : PromptShownY - PromptSlideOffset;
        float startY = promptRoot.anchoredPosition.y;
        float t = 0f;
        while (t < PromptFadeDuration)
        {
            t += Time.deltaTime;
            float f = Mathf.Clamp01(t / PromptFadeDuration);
            promptCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, f);
            promptRoot.anchoredPosition = new Vector2(0, Mathf.Lerp(startY, endY, f));
            yield return null;
        }
        promptCanvasGroup.alpha = endAlpha;
        promptRoot.anchoredPosition = new Vector2(0, endY);
        if (!show) promptRoot.gameObject.SetActive(false);
        promptAnimRoutine = null;
    }

    // duration: null usa feedbackDuration (comportamiento normal para el resto de mensajes).
    // Solo el diagnostico de RAM insuficiente pasa un valor explicito para quedarse mas tiempo
    // en pantalla sin afectar la duracion global de los demas feedbacks.
    public void ShowFeedback(string text, float? duration = null)
    {
        if (feedbackText == null) return;
        if (feedbackRoutine != null) StopCoroutine(feedbackRoutine);
        feedbackText.text = text;
        feedbackText.enabled = true;
        feedbackRoutine = StartCoroutine(HideFeedbackAfterDelay(duration ?? feedbackDuration));
    }

    // Duracion por defecto de un feedback, expuesta para que un llamador externo (StorageMission)
    // pueda calcular "duracion actual + N" sin duplicar el valor serializado aqui.
    public float FeedbackDuration => feedbackDuration;

    private void HideFeedbackImmediate()
    {
        if (feedbackText == null) return;
        feedbackText.text = "";
        feedbackText.enabled = false;
    }

    private IEnumerator HideFeedbackAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        HideFeedbackImmediate();
    }

    private void BuildPanel()
    {
        panelRoot = new GameObject("EducationPanel", typeof(RectTransform));
        panelRoot.transform.SetParent(hudCanvasTransform, false);
        var panelRootRT = panelRoot.GetComponent<RectTransform>();
        panelRootRT.anchorMin = Vector2.zero;
        panelRootRT.anchorMax = Vector2.one;
        panelRootRT.offsetMin = Vector2.zero;
        panelRootRT.offsetMax = Vector2.zero;

        var backdropGO = new GameObject("Backdrop");
        backdropGO.transform.SetParent(panelRoot.transform, false);
        var backdropImage = backdropGO.AddComponent<Image>();
        backdropImage.color = new Color(0f, 0f, 0f, 0.75f);
        var backdropRT = backdropGO.GetComponent<RectTransform>();
        backdropRT.anchorMin = Vector2.zero;
        backdropRT.anchorMax = Vector2.one;
        backdropRT.offsetMin = Vector2.zero;
        backdropRT.offsetMax = Vector2.zero;

        var boxGO = new GameObject("PanelBox");
        boxGO.transform.SetParent(panelRoot.transform, false);
        var boxImage = boxGO.AddComponent<Image>();
        boxImage.color = new Color(0.05f, 0.09f, 0.13f, 0.97f);
        var boxRT = boxGO.GetComponent<RectTransform>();
        boxRT.anchorMin = boxRT.anchorMax = boxRT.pivot = new Vector2(0.5f, 0.5f);
        boxRT.sizeDelta = new Vector2(720, 460);
        boxRT.anchoredPosition = Vector2.zero;

        panelTitleText = CreatePanelText(boxGO.transform, "Title", 32, TextAlignmentOptions.Top,
            new Vector2(0, 190), new Vector2(660, 50), new Color(0.35f, 0.95f, 1f), FontStyles.Bold);
        panelSubtitleText = CreatePanelText(boxGO.transform, "Subtitle", 22, TextAlignmentOptions.Top,
            new Vector2(0, 145), new Vector2(660, 35), new Color(0.75f, 0.9f, 0.95f), FontStyles.Normal);
        panelBodyText = CreatePanelText(boxGO.transform, "Body", 24, TextAlignmentOptions.TopLeft,
            new Vector2(0, 10), new Vector2(660, 230), Color.white, FontStyles.Normal);
        // Texto informativo (Panel 1) y de recompensa (Panel 3): igual que la pregunta del
        // Panel 2, nunca debe cortarse aunque la descripcion sea larga.
        panelBodyText.overflowMode = TextOverflowModes.Overflow;
        panelQuestionText = CreatePanelText(boxGO.transform, "Question", 28, TextAlignmentOptions.Center,
            new Vector2(0, 90), new Vector2(660, 130), Color.white, FontStyles.Bold);
        // Las preguntas conceptuales pueden ocupar 2-3 lineas; a diferencia del resto de textos
        // del panel (que se truncan si no caben), esta nunca debe cortarse.
        panelQuestionText.overflowMode = TextOverflowModes.Overflow;
        panelResultText = CreatePanelText(boxGO.transform, "Result", 20, TextAlignmentOptions.Center,
            new Vector2(0, -30), new Vector2(660, 35), new Color(1f, 0.45f, 0.45f), FontStyles.Normal);

        panelInputField = CreateInputField(boxGO.transform, new Vector2(0, -90), new Vector2(220, 50));

        panelOptionButtons = new Button[ChoiceOptionCount];
        panelOptionLabels = new TextMeshProUGUI[ChoiceOptionCount];
        for (int i = 0; i < ChoiceOptionCount; i++)
        {
            var optionButton = CreateButton(boxGO.transform, "Option" + i, new Vector2(0, -10 - i * 60),
                new Vector2(500, 50), new Color(0.15f, 0.2f, 0.24f), out TextMeshProUGUI optionLabel);
            panelOptionButtons[i] = optionButton;
            panelOptionLabels[i] = optionLabel;
        }

        panelCloseButton = CreateButton(boxGO.transform, "CloseButton", new Vector2(-165, -200),
            new Vector2(220, 55), new Color(0.25f, 0.28f, 0.32f), out TextMeshProUGUI closeLabel);
        closeLabel.text = "Cerrar";

        panelPrimaryButton = CreateButton(boxGO.transform, "PrimaryButton", new Vector2(165, -200),
            new Vector2(220, 55), new Color(0.15f, 0.55f, 0.65f), out panelPrimaryButtonLabel);

        panelRoot.SetActive(false);
    }

    private static TextMeshProUGUI CreatePanelText(Transform parent, string name, int size, TextAlignmentOptions anchor,
        Vector2 anchoredPos, Vector2 sizeDelta, Color color, FontStyles style)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);

        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = sizeDelta;
        rt.anchoredPosition = anchoredPos;

        var text = go.AddComponent<TextMeshProUGUI>();
        text.font = TMP_Settings.defaultFontAsset;
        text.fontSize = size;
        text.fontStyle = style;
        text.alignment = anchor;
        text.color = color;
        text.text = "";
        text.textWrappingMode = TextWrappingModes.Normal;
        text.overflowMode = TextOverflowModes.Truncate;

        return text;
    }

    private static TMP_InputField CreateInputField(Transform parent, Vector2 anchoredPos, Vector2 sizeDelta)
    {
        var go = new GameObject("AnswerInput");
        go.transform.SetParent(parent, false);

        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = sizeDelta;
        rt.anchoredPosition = anchoredPos;

        var image = go.AddComponent<Image>();
        image.color = new Color(1f, 1f, 1f, 0.92f);

        var inputField = go.AddComponent<TMP_InputField>();
        inputField.contentType = TMP_InputField.ContentType.IntegerNumber;
        inputField.characterLimit = 6;
        inputField.lineType = TMP_InputField.LineType.SingleLine;

        var viewportGO = new GameObject("TextViewport");
        viewportGO.transform.SetParent(go.transform, false);
        var viewportRT = viewportGO.AddComponent<RectTransform>();
        viewportRT.anchorMin = Vector2.zero;
        viewportRT.anchorMax = Vector2.one;
        viewportRT.offsetMin = new Vector2(10, 4);
        viewportRT.offsetMax = new Vector2(-10, -4);
        viewportGO.AddComponent<RectMask2D>();

        var textGO = new GameObject("Text");
        textGO.transform.SetParent(viewportGO.transform, false);
        var textRT = textGO.AddComponent<RectTransform>();
        textRT.anchorMin = Vector2.zero;
        textRT.anchorMax = Vector2.one;
        textRT.offsetMin = Vector2.zero;
        textRT.offsetMax = Vector2.zero;
        var textComp = textGO.AddComponent<TextMeshProUGUI>();
        textComp.font = TMP_Settings.defaultFontAsset;
        textComp.fontSize = 26;
        textComp.alignment = TextAlignmentOptions.Center;
        textComp.color = Color.black;

        inputField.textViewport = viewportRT;
        inputField.textComponent = textComp;

        return inputField;
    }

    private static Button CreateButton(Transform parent, string name, Vector2 anchoredPos,
        Vector2 sizeDelta, Color color, out TextMeshProUGUI label)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);

        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = sizeDelta;
        rt.anchoredPosition = anchoredPos;

        var image = go.AddComponent<Image>();
        image.color = color;

        var button = go.AddComponent<Button>();

        var labelGO = new GameObject("Label");
        labelGO.transform.SetParent(go.transform, false);
        var labelRT = labelGO.AddComponent<RectTransform>();
        labelRT.anchorMin = Vector2.zero;
        labelRT.anchorMax = Vector2.one;
        labelRT.offsetMin = Vector2.zero;
        labelRT.offsetMax = Vector2.zero;
        label = labelGO.AddComponent<TextMeshProUGUI>();
        label.font = TMP_Settings.defaultFontAsset;
        label.fontSize = 22;
        label.alignment = TextAlignmentOptions.Center;
        label.color = Color.white;

        return button;
    }

    private void SetPanelMode(PanelMode mode)
    {
        panelSubtitleText.gameObject.SetActive(mode == PanelMode.Info);
        panelBodyText.gameObject.SetActive(mode == PanelMode.Info || mode == PanelMode.Reward);
        panelQuestionText.gameObject.SetActive(mode == PanelMode.Activity || mode == PanelMode.Choice);
        panelResultText.gameObject.SetActive(mode == PanelMode.Activity || mode == PanelMode.Choice);
        panelInputField.gameObject.SetActive(mode == PanelMode.Activity);
        panelPrimaryButton.gameObject.SetActive(mode != PanelMode.Choice);
        panelCloseButton.gameObject.SetActive(mode != PanelMode.Reward);

        var resultRT = panelResultText.GetComponent<RectTransform>();
        resultRT.anchoredPosition = mode == PanelMode.Choice ? new Vector2(0, -175) : new Vector2(0, -30);

        foreach (var optionButton in panelOptionButtons)
            optionButton.gameObject.SetActive(mode == PanelMode.Choice);

        if (mode == PanelMode.Activity || mode == PanelMode.Choice)
            panelResultText.text = "";
    }

    public void ShowEducationalPanel(string title, string subtitle, string body, string resolveLabel, Action onResolve, Action onClose)
    {
        panelRoot.SetActive(true);
        SetPanelMode(PanelMode.Info);

        panelTitleText.text = title;
        panelSubtitleText.text = subtitle;
        panelBodyText.text = body;
        panelPrimaryButtonLabel.text = resolveLabel;

        panelPrimaryButton.onClick.RemoveAllListeners();
        panelPrimaryButton.onClick.AddListener(() => onResolve?.Invoke());
        panelCloseButton.onClick.RemoveAllListeners();
        panelCloseButton.onClick.AddListener(() => onClose?.Invoke());
    }

    public void ShowActivityPanel(string activityTitle, string questionText, string submitLabel, Action<int> onSubmit, Action onClose)
    {
        panelRoot.SetActive(true);
        SetPanelMode(PanelMode.Activity);

        panelTitleText.text = activityTitle;
        panelQuestionText.text = questionText;
        panelInputField.text = "";
        panelPrimaryButtonLabel.text = submitLabel;

        panelPrimaryButton.onClick.RemoveAllListeners();
        panelPrimaryButton.onClick.AddListener(() =>
        {
            if (int.TryParse(panelInputField.text, out int value))
                onSubmit?.Invoke(value);
            else
                panelResultText.text = "Introduce un numero valido.";
        });
        panelCloseButton.onClick.RemoveAllListeners();
        panelCloseButton.onClick.AddListener(() => onClose?.Invoke());
    }

    // Actividad conceptual de opcion multiple (2-3 opciones). El llamador (EducationalInteractable
    // o FinalActivity) decide cual opcion es correcta y que ocurre despues; este panel solo presenta
    // la pregunta y reporta el indice elegido, igual que ShowActivityPanel hace con el numero tecleado.
    public void ShowChoicePanel(string activityTitle, string questionText, string[] options, Action<int> onOptionSelected, Action onClose)
    {
        panelRoot.SetActive(true);
        SetPanelMode(PanelMode.Choice);

        panelTitleText.text = activityTitle;
        panelQuestionText.text = questionText;

        for (int i = 0; i < panelOptionButtons.Length; i++)
        {
            bool hasOption = options != null && i < options.Length;
            panelOptionButtons[i].gameObject.SetActive(hasOption);
            if (!hasOption) continue;

            panelOptionLabels[i].text = options[i];

            int optionIndex = i;
            panelOptionButtons[i].onClick.RemoveAllListeners();
            panelOptionButtons[i].onClick.AddListener(() => onOptionSelected?.Invoke(optionIndex));
        }

        panelCloseButton.onClick.RemoveAllListeners();
        panelCloseButton.onClick.AddListener(() => onClose?.Invoke());
    }

    public void ShowActivityError(string message)
    {
        panelResultText.text = message;
    }

    public void ShowReward(string title, string body, Action onContinue)
    {
        panelRoot.SetActive(true);
        SetPanelMode(PanelMode.Reward);

        panelTitleText.text = title;
        panelBodyText.text = body;
        panelPrimaryButtonLabel.text = "Continuar";

        panelPrimaryButton.onClick.RemoveAllListeners();
        panelPrimaryButton.onClick.AddListener(() => onContinue?.Invoke());
    }

    public void HidePanel()
    {
        panelRoot.SetActive(false);
    }
}
