using System;
using System.Collections;
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

    private Text objectiveText;
    private Text hintText;
    private Text promptText;
    private Text feedbackText;
    private Coroutine feedbackRoutine;

    private Text locationText;
    private Text locationSubtitleText;
    private Coroutine locationSubtitleRoutine;

    private Text progressText;
    private Text inventoryText;

    private GameObject panelRoot;
    private Text panelTitleText;
    private Text panelSubtitleText;
    private Text panelBodyText;
    private Text panelQuestionText;
    private Text panelResultText;
    private InputField panelInputField;
    private Button panelPrimaryButton;
    private Text panelPrimaryButtonLabel;
    private Button panelCloseButton;
    private Button[] panelOptionButtons;
    private Text[] panelOptionLabels;

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
        HidePrompt();
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

        var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        objectiveText = CreateText(canvasGO.transform, "ObjectiveText", font, 28, TextAnchor.UpperLeft,
            new Vector2(0, 1), new Vector2(0, 1), new Vector2(20, -20), new Vector2(760, 40));

        hintText = CreateText(canvasGO.transform, "HintText", font, 19, TextAnchor.UpperLeft,
            new Vector2(0, 1), new Vector2(0, 1), new Vector2(20, -58), new Vector2(760, 50));
        hintText.color = new Color(0.75f, 0.9f, 0.95f);

        promptText = CreateText(canvasGO.transform, "PromptText", font, 32, TextAnchor.MiddleCenter,
            new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0, 120), new Vector2(400, 50));

        feedbackText = CreateText(canvasGO.transform, "FeedbackText", font, 26, TextAnchor.LowerCenter,
            new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0, 190), new Vector2(960, 90));

        locationText = CreateText(canvasGO.transform, "LocationText", font, 30, TextAnchor.UpperCenter,
            new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0, -20), new Vector2(500, 40));
        locationText.fontStyle = FontStyle.Bold;
        locationText.color = new Color(0.35f, 0.95f, 1f);

        locationSubtitleText = CreateText(canvasGO.transform, "LocationSubtitleText", font, 18, TextAnchor.UpperCenter,
            new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0, -56), new Vector2(500, 30));
        locationSubtitleText.color = new Color(0.85f, 0.85f, 0.85f);

        progressText = CreateText(canvasGO.transform, "ProgressText", font, 22, TextAnchor.UpperRight,
            new Vector2(1, 1), new Vector2(1, 1), new Vector2(-20, -20), new Vector2(200, 36));
        progressText.color = new Color(0.75f, 0.9f, 0.95f);

        inventoryText = CreateText(canvasGO.transform, "InventoryText", font, 20, TextAnchor.UpperRight,
            new Vector2(1, 1), new Vector2(1, 1), new Vector2(-20, -60), new Vector2(200, 30));
        inventoryText.color = new Color(0.75f, 0.9f, 0.95f);
        inventoryText.enabled = false;
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

    private static Text CreateText(Transform parent, string name, Font font, int size, TextAnchor anchor,
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

        var text = go.AddComponent<Text>();
        text.font = font;
        text.fontSize = size;
        text.alignment = anchor;
        text.color = Color.white;
        text.text = "";
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Overflow;

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
        if (promptText == null) return;
        promptText.text = text;
        promptText.enabled = true;
    }

    public void HidePrompt()
    {
        if (promptText == null) return;
        promptText.text = "";
        promptText.enabled = false;
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

        var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        panelTitleText = CreatePanelText(boxGO.transform, "Title", font, 32, TextAnchor.UpperCenter,
            new Vector2(0, 190), new Vector2(660, 50), new Color(0.35f, 0.95f, 1f), FontStyle.Bold);
        panelSubtitleText = CreatePanelText(boxGO.transform, "Subtitle", font, 22, TextAnchor.UpperCenter,
            new Vector2(0, 145), new Vector2(660, 35), new Color(0.75f, 0.9f, 0.95f), FontStyle.Normal);
        panelBodyText = CreatePanelText(boxGO.transform, "Body", font, 24, TextAnchor.UpperLeft,
            new Vector2(0, 10), new Vector2(660, 230), Color.white, FontStyle.Normal);
        // Texto informativo (Panel 1) y de recompensa (Panel 3): igual que la pregunta del
        // Panel 2, nunca debe cortarse aunque la descripcion sea larga.
        panelBodyText.verticalOverflow = VerticalWrapMode.Overflow;
        panelQuestionText = CreatePanelText(boxGO.transform, "Question", font, 28, TextAnchor.MiddleCenter,
            new Vector2(0, 90), new Vector2(660, 130), Color.white, FontStyle.Bold);
        // Las preguntas conceptuales pueden ocupar 2-3 lineas; a diferencia del resto de textos
        // del panel (que se truncan si no caben), esta nunca debe cortarse.
        panelQuestionText.verticalOverflow = VerticalWrapMode.Overflow;
        panelResultText = CreatePanelText(boxGO.transform, "Result", font, 20, TextAnchor.MiddleCenter,
            new Vector2(0, -30), new Vector2(660, 35), new Color(1f, 0.45f, 0.45f), FontStyle.Normal);

        panelInputField = CreateInputField(boxGO.transform, font, new Vector2(0, -90), new Vector2(220, 50));

        panelOptionButtons = new Button[ChoiceOptionCount];
        panelOptionLabels = new Text[ChoiceOptionCount];
        for (int i = 0; i < ChoiceOptionCount; i++)
        {
            var optionButton = CreateButton(boxGO.transform, "Option" + i, font, new Vector2(0, -10 - i * 60),
                new Vector2(500, 50), new Color(0.15f, 0.2f, 0.24f), out Text optionLabel);
            panelOptionButtons[i] = optionButton;
            panelOptionLabels[i] = optionLabel;
        }

        panelCloseButton = CreateButton(boxGO.transform, "CloseButton", font, new Vector2(-165, -200),
            new Vector2(220, 55), new Color(0.25f, 0.28f, 0.32f), out _);
        panelCloseButton.GetComponentInChildren<Text>().text = "Cerrar";

        panelPrimaryButton = CreateButton(boxGO.transform, "PrimaryButton", font, new Vector2(165, -200),
            new Vector2(220, 55), new Color(0.15f, 0.55f, 0.65f), out panelPrimaryButtonLabel);

        panelRoot.SetActive(false);
    }

    private static Text CreatePanelText(Transform parent, string name, Font font, int size, TextAnchor anchor,
        Vector2 anchoredPos, Vector2 sizeDelta, Color color, FontStyle style)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);

        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = sizeDelta;
        rt.anchoredPosition = anchoredPos;

        var text = go.AddComponent<Text>();
        text.font = font;
        text.fontSize = size;
        text.fontStyle = style;
        text.alignment = anchor;
        text.color = color;
        text.text = "";
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Truncate;

        return text;
    }

    private static InputField CreateInputField(Transform parent, Font font, Vector2 anchoredPos, Vector2 sizeDelta)
    {
        var go = new GameObject("AnswerInput");
        go.transform.SetParent(parent, false);

        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = sizeDelta;
        rt.anchoredPosition = anchoredPos;

        var image = go.AddComponent<Image>();
        image.color = new Color(1f, 1f, 1f, 0.92f);

        var inputField = go.AddComponent<InputField>();
        inputField.contentType = InputField.ContentType.IntegerNumber;
        inputField.characterLimit = 6;
        inputField.lineType = InputField.LineType.SingleLine;

        var textGO = new GameObject("Text");
        textGO.transform.SetParent(go.transform, false);
        var textRT = textGO.AddComponent<RectTransform>();
        textRT.anchorMin = Vector2.zero;
        textRT.anchorMax = Vector2.one;
        textRT.offsetMin = new Vector2(10, 4);
        textRT.offsetMax = new Vector2(-10, -4);
        var textComp = textGO.AddComponent<Text>();
        textComp.font = font;
        textComp.fontSize = 26;
        textComp.alignment = TextAnchor.MiddleCenter;
        textComp.color = Color.black;

        inputField.textComponent = textComp;

        return inputField;
    }

    private static Button CreateButton(Transform parent, string name, Font font, Vector2 anchoredPos,
        Vector2 sizeDelta, Color color, out Text label)
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
        label = labelGO.AddComponent<Text>();
        label.font = font;
        label.fontSize = 22;
        label.alignment = TextAnchor.MiddleCenter;
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
