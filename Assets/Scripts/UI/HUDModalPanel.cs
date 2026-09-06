using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Prompt 09 (Bloque 1, arquitectura): extraido de GameHUD.cs -- era, con diferencia, la parte
// mas grande del God Object original (el sistema completo de panel modal de 4 modos). Sin
// estado ni logica de mision propia: EducationalInteractable/FinalActivity/StorageMission le
// pasan textos/callbacks a traves de GameHUD (fachada) y este componente solo los presenta.
public class HUDModalPanel : MonoBehaviour
{
    private enum PanelMode { Info, Activity, Choice, Reward }

    private const int ChoiceOptionCount = 3;

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

    public void Init(Transform parent)
    {
        panelRoot = new GameObject("EducationPanel", typeof(RectTransform));
        panelRoot.transform.SetParent(parent, false);
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
        // Prompt 07 (Bloque 4): unico punto de "abrir panel" -- el momento real en que el
        // jugador interactua con un objeto por primera vez (ver EducationalInteractable.Interact).
        // ShowActivityPanel/ShowChoicePanel/ShowReward son continuaciones de una misma secuencia
        // ya abierta, no una interaccion nueva -- reproducir el sonido ahi tambien lo dispararia
        // 2-3 veces por interaccion.
        AudioManager.Instance?.PlayInteractOpen();

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

    // Bugfix (Esc jerarquico): expone si el panel esta visible para que PauseMenuController
    // pueda cerrarlo con Esc antes de considerar alternar Pausa/Reanudar -- ver
    // PauseMenuController.Update().
    public bool IsPanelOpen => panelRoot != null && panelRoot.activeSelf;
}
