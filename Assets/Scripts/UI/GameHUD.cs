using System;
using UnityEngine;
using UnityEngine.UI;

// Prompt 09 (Bloque 1, arquitectura): GameHUD dejo de ser el God Object de ~720 lineas que
// construia y manejaba directamente el objetivo/pista, el prompt "[E]", el feedback flotante, el
// contador de progreso, el inventario Y el sistema completo de panel modal. Ahora es una fachada
// delgada: crea el Canvas comun, instancia los 4 componentes de responsabilidad unica
// (HUDPrompt, HUDFeedbackBanner, HUDObjectiveDisplay, HUDModalPanel) y reenvia cada metodo
// publico al componente correspondiente -- la API externa es identica a la de antes a proposito,
// para que SimpleInteractable/EducationalInteractable/FinalActivity/StorageMission/
// PlayerInteraction/LocationZone no necesiten cambiar una sola linea.
public class GameHUD : MonoBehaviour
{
    public static GameHUD Instance { get; private set; }

    // Prompt 26: el HUD de gameplay (objetivo, pista, prompt, feedback, progreso, inventario)
    // no debe verse en el Main Menu. GameHUD sigue creandose igual (DontDestroyOnLoad) para no
    // romper su inicializacion perezosa; solo se oculta/muestra el Canvas segun el estado actual.
    private Transform hudCanvasTransform;

    private HUDPrompt prompt;
    private HUDFeedbackBanner feedback;
    private HUDObjectiveDisplay objectiveDisplay;
    private HUDModalPanel modalPanel;

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

        BuildCanvas();

        prompt = gameObject.AddComponent<HUDPrompt>();
        prompt.Init(hudCanvasTransform);

        feedback = gameObject.AddComponent<HUDFeedbackBanner>();
        feedback.Init(hudCanvasTransform);

        objectiveDisplay = gameObject.AddComponent<HUDObjectiveDisplay>();
        objectiveDisplay.Init(hudCanvasTransform);

        modalPanel = gameObject.AddComponent<HUDModalPanel>();
        modalPanel.Init(hudCanvasTransform);

        GameStateManager.Instance.OnStateChanged.AddListener(HandleStateChanged);
        UpdateVisibility(GameStateManager.Instance.Current);
    }

    private void HandleStateChanged(GameState state)
    {
        UpdateVisibility(state);

        // Bug (Prompt 28): LocationZone guarda la zona actual en un campo static que sobrevive
        // a un recargado de SampleScene (Continuar/Reiniciar); sin este reset, el indicador
        // superior podia quedar mostrando la ultima zona visitada de la partida anterior hasta
        // que el jugador cruzara un LocationZone de nuevo. GameStateManager notifica en cada
        // (re)carga de escena, no solo cuando cambia el nombre del estado -- ver su comentario.
        if (state != GameState.MainMenu)
        {
            objectiveDisplay.SetLocation("", "");
        }
    }

    private void UpdateVisibility(GameState state)
    {
        hudCanvasTransform.gameObject.SetActive(state != GameState.MainMenu);
    }

    private void BuildCanvas()
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
    }

    public void ShowPrompt(string text) => prompt.Show(text);
    public void HidePrompt() => prompt.Hide();

    public void ShowFeedback(string text, float? duration = null) => feedback.Show(text, duration);

    // Duracion por defecto de un feedback, expuesta para que un llamador externo (StorageMission)
    // pueda calcular "duracion actual + N" sin duplicar el valor serializado aqui.
    public float FeedbackDuration => feedback.DefaultDuration;

    public void SetLocation(string locationName, string description) => objectiveDisplay.SetLocation(locationName, description);
    public void SetInventoryText(string text) => objectiveDisplay.SetInventoryText(text);

    public void ShowEducationalPanel(string title, string subtitle, string body, string resolveLabel, Action onResolve, Action onClose)
        => modalPanel.ShowEducationalPanel(title, subtitle, body, resolveLabel, onResolve, onClose);

    public void ShowActivityPanel(string activityTitle, string questionText, string submitLabel, Action<int> onSubmit, Action onClose)
        => modalPanel.ShowActivityPanel(activityTitle, questionText, submitLabel, onSubmit, onClose);

    public void ShowChoicePanel(string activityTitle, string questionText, string[] options, Action<int> onOptionSelected, Action onClose)
        => modalPanel.ShowChoicePanel(activityTitle, questionText, options, onOptionSelected, onClose);

    public void ShowActivityError(string message) => modalPanel.ShowActivityError(message);

    public void ShowReward(string title, string body, Action onContinue) => modalPanel.ShowReward(title, body, onContinue);

    public void HidePanel() => modalPanel.HidePanel();
}
