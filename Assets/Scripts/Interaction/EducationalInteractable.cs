using TMPro;
using UnityEngine;

public class EducationalInteractable : MonoBehaviour, IInteractable
{
    private enum State { Idle, PanelOpen, ActivityOpen, Completed }

    public enum Operation { Add, Subtract }
    public enum ActivityMode { Math, Choice }

    [Header("Identidad")]
    [SerializeField] private string title = "ALU";
    [SerializeField] private string subtitle = "Unidad Aritmetico-Logica";
    [SerializeField] private string promptText = "[E] Examinar";

    [Header("Progresion")]
    [Tooltip("Clave interna usada por ObjectiveSystem para saber si este es el paso esperado (ALU, REGISTROS, UNIDAD_CONTROL, CACHE_L1, CACHE_L2, CACHE_L3, RAM1, RAM2).")]
    [SerializeField] private string progressionKey = "";

    [Header("Proximidad")]
    [SerializeField] private float proximityRadius = 10f;
    [SerializeField] private float labelHeight = 2f;

    [Header("Panel educativo")]
    [TextArea(3, 8)]
    [SerializeField] private string description =
        "La ALU es una parte fundamental del procesador encargada de realizar operaciones aritmeticas y logicas.\n\nPuede realizar operaciones como:\n- suma\n- resta\n- comparacion\n- AND\n- OR\n- NOT";

    [Header("Actividad")]
    [SerializeField] private ActivityMode activityMode = ActivityMode.Math;
    [SerializeField] private string activityTitle = "DIAGNOSTICO DE ALU";

    [Header("Actividad - Matematica (legado/transicion)")]
    [SerializeField] private int operandA = 12;
    [SerializeField] private int operandB = 7;
    [SerializeField] private Operation operation = Operation.Add;

    [Header("Actividad - Conceptual (opcion multiple)")]
    [TextArea(2, 4)]
    [SerializeField] private string conceptQuestion = "";
    [SerializeField] private string[] conceptOptions = new string[3];
    [SerializeField] private int conceptCorrectIndex = 0;

    [Header("Recompensa")]
    [SerializeField] private string rewardTitle = "✓ ALU ANALIZADA";
    [TextArea]
    [SerializeField] private string rewardText = "Has aprendido como la Unidad Aritmetico-Logica procesa operaciones.";
    [SerializeField] private string objectiveCompletedText = "Objetivo completado: ALU analizada.";

    private State state = State.Idle;

    private GameObject labelRoot;
    private bool labelVisible;
    private Transform playerTransform;
    private EmissiveToggle glow;

    public string PromptText => promptText;
    public bool CanInteract => state == State.Idle;

    private void Awake()
    {
        BuildLabel();
        BuildGlow();
        RefreshVisualState();
    }

    // Prompt 04_implement (data flow, seccion 7): llamar esto solo desde Awake() NO alcanza para
    // el caso real de "cargar partida desde el Main Menu" -- SceneManager.LoadScene(...) corre
    // (y con el, TODOS los Awake() de la escena) antes de que MainMenuController.Continuar()
    // pueda siquiera llamar a SaveManager.Instance.LoadGame(). En un arranque nuevo del juego,
    // ObjectiveSystem todavia tiene sus valores por defecto en el momento de este Awake(); recien
    // se restaura despues. Por eso SaveManager.LoadGame() vuelve a llamar a este metodo
    // explicitamente tras restaurar ObjectiveSystem (mismo patron ya usado ahi para
    // FinalActivity.ResumeIfInProgress()). Publico e idempotente a proposito.
    public void RefreshVisualState()
    {
        if (ObjectiveSystem.Instance.IsKeyAchieved(progressionKey))
            glow?.Activate();
    }

    // Prompt 03_gran (bloque polish visual, seccion 2): las 6 piezas de la CPU (ALU, Registros,
    // Unidad de Control, Cache L1/L2/L3) son bloques de color solido sin ningun brillo -- la sala
    // se sentia como "componentes gigantes colocados en una habitacion" en vez de "el interior de
    // un procesador funcionando". Cada bloque ya tiene su propio material de instancia (no
    // compartido, ver m_Name "ALU (Instance)" etc.), asi que se enciende en SU PROPIO color base
    // en vez de un cian generico -- cada pieza "prende" con su propia identidad al comprenderse.
    private void BuildGlow()
    {
        var renderer = GetComponent<MeshRenderer>();
        if (renderer == null || renderer.sharedMaterial == null) return;

        Color baseColor = renderer.sharedMaterial.color;
        glow = gameObject.AddComponent<EmissiveToggle>();
        glow.Configure(renderer, 0, baseColor, 2f);
    }

    private void Update()
    {
        UpdateLabel();
    }

    private void OnDestroy()
    {
        if (labelRoot != null) Destroy(labelRoot);
    }

    public void Interact()
    {
        if (!CanInteract) return;

        state = State.PanelOpen;
        // Prompt 18: ya no hay actividad/pregunta individual por componente. El boton del
        // panel informativo (Panel 1) completa el componente directamente (Panel 3 = recompensa).
        // OpenActivity/SubmitChoice/SubmitMathAnswer quedan sin usar aqui (compatibilidad interna).
        GameHUD.Instance?.ShowEducationalPanel(title, subtitle, description, "Entendido", HandleCorrectAnswer, CloseWithoutCompleting);
    }

    private void OpenActivity()
    {
        state = State.ActivityOpen;

        if (activityMode == ActivityMode.Choice)
            GameHUD.Instance?.ShowChoicePanel(activityTitle, conceptQuestion, conceptOptions, SubmitChoice, CloseWithoutCompleting);
        else
            GameHUD.Instance?.ShowActivityPanel(activityTitle, operandA + OperatorSymbol() + operandB, "EJECUTAR", SubmitMathAnswer, CloseWithoutCompleting);
    }

    private void SubmitMathAnswer(int answer)
    {
        int correctAnswer = operation == Operation.Add ? operandA + operandB : operandA - operandB;
        if (answer == correctAnswer)
            HandleCorrectAnswer();
        else
            GameHUD.Instance?.ShowActivityError("✗ Resultado incorrecto. Intentalo nuevamente.");
    }

    private void SubmitChoice(int optionIndex)
    {
        if (optionIndex == conceptCorrectIndex)
            HandleCorrectAnswer();
        else
            GameHUD.Instance?.ShowActivityError("✗ No es correcto. Intentalo de nuevo.");
    }

    private void HandleCorrectAnswer()
    {
        state = State.Completed;
        glow?.Activate();
        GameHUD.Instance?.ShowReward(rewardTitle, rewardText, OnRewardContinue);
    }

    private void OnRewardContinue()
    {
        GameHUD.Instance?.HidePanel();

        bool wasExpectedStep = ObjectiveSystem.Instance.IsCurrentStep(progressionKey);
        string feedback = wasExpectedStep
            ? objectiveCompletedText
            : $"Ya conoces {title}. Retomaremos este componente en su momento dentro de la ruta.";

        ObjectiveSystem.Instance.ReportActivityCompleted(progressionKey, feedback);
    }

    private void CloseWithoutCompleting()
    {
        if (state != State.Completed) state = State.Idle;
        GameHUD.Instance?.HidePanel();
    }

    private string OperatorSymbol() => operation == Operation.Add ? " + " : " - ";

    private void BuildLabel()
    {
        labelRoot = new GameObject(name + "_Label");

        var canvas = labelRoot.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        var rt = labelRoot.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(400, 100);
        labelRoot.transform.localScale = Vector3.one * 0.012f;

        var titleGO = new GameObject("Title");
        titleGO.transform.SetParent(labelRoot.transform, false);
        var titleText = titleGO.AddComponent<TextMeshProUGUI>();
        titleText.font = TMP_Settings.defaultFontAsset;
        titleText.fontSize = 34;
        titleText.fontStyle = FontStyles.Bold;
        titleText.alignment = TextAlignmentOptions.Bottom;
        titleText.color = new Color(0.35f, 0.95f, 1f);
        titleText.text = title;
        var titleRT = titleGO.GetComponent<RectTransform>();
        titleRT.anchorMin = titleRT.anchorMax = titleRT.pivot = new Vector2(0.5f, 1f);
        titleRT.sizeDelta = new Vector2(400, 45);
        titleRT.anchoredPosition = Vector2.zero;
        var titleOutline = titleGO.AddComponent<UnityEngine.UI.Outline>();
        titleOutline.effectColor = new Color(0f, 0f, 0f, 0.9f);
        titleOutline.effectDistance = new Vector2(1.5f, -1.5f);

        var subtitleGO = new GameObject("Subtitle");
        subtitleGO.transform.SetParent(labelRoot.transform, false);
        var subtitleText = subtitleGO.AddComponent<TextMeshProUGUI>();
        subtitleText.font = TMP_Settings.defaultFontAsset;
        subtitleText.fontSize = 22;
        subtitleText.alignment = TextAlignmentOptions.Top;
        subtitleText.color = Color.white;
        subtitleText.text = subtitle;
        var subtitleRT = subtitleGO.GetComponent<RectTransform>();
        subtitleRT.anchorMin = subtitleRT.anchorMax = subtitleRT.pivot = new Vector2(0.5f, 1f);
        subtitleRT.sizeDelta = new Vector2(400, 40);
        subtitleRT.anchoredPosition = new Vector2(0, -45);
        var subtitleOutline = subtitleGO.AddComponent<UnityEngine.UI.Outline>();
        subtitleOutline.effectColor = new Color(0f, 0f, 0f, 0.9f);
        subtitleOutline.effectDistance = new Vector2(1.5f, -1.5f);

        labelRoot.SetActive(false);
    }

    private void UpdateLabel()
    {
        var cam = Camera.main;
        if (cam == null || labelRoot == null) return;

        if (playerTransform == null)
        {
            var movement = FindFirstObjectByType<MovementInput>();
            if (movement != null) playerTransform = movement.transform;
        }
        if (playerTransform == null) return;

        float dist = Vector3.Distance(playerTransform.position, transform.position);
        // Prompt 18: la etiqueta (nombre del componente) debe permanecer visible incluso
        // despues de completado; solo se oculta mientras hay un panel abierto encima.
        bool shouldShow = dist <= proximityRadius && state != State.PanelOpen && state != State.ActivityOpen;

        if (shouldShow != labelVisible)
        {
            labelVisible = shouldShow;
            labelRoot.SetActive(labelVisible);
        }

        if (labelVisible)
        {
            Vector3 labelPos = transform.position + Vector3.up * labelHeight;
            labelRoot.transform.position = labelPos;

            Vector3 direction = labelPos - cam.transform.position;
            if (direction.sqrMagnitude > 0.0001f)
                labelRoot.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }
}
