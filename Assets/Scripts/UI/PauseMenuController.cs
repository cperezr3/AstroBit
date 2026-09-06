using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Menu de pausa (Prompt 28). Responsable unicamente de: visibilidad de su propio Canvas y las
// acciones del menu (Continuar/Configuración/Reiniciar/Volver al Menu). Ya no decide nada de
// Time.timeScale ni de que es "estar pausado" -- eso ahora es responsabilidad exclusiva de
// GameStateManager (Prompt 09, Bloque 1); este controlador solo dibuja el panel en reaccion a
// GameStateManager.OnStateChanged y traduce clicks de boton en llamadas a GameStateManager.
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
    private static readonly Color AccentCyan = new Color(0f, 1f, 1f, 1f);

    private GameObject panelRoot;

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
        GameStateManager.Instance.OnStateChanged.AddListener(HandleStateChanged);
    }

    // Unica fuente de verdad de "estar pausado" ahora es GameStateManager.Current -- este
    // metodo solo refleja ese estado en el panel/control del jugador, nunca lo decide.
    private void HandleStateChanged(GameState state)
    {
        bool paused = state == GameState.Paused;
        panelRoot.SetActive(paused);
        SetPlayerControlEnabled(state == GameState.Playing);

        // Los botones del panel nunca se destruyen, solo se ocultan -- el EventSystem sigue
        // recordando cual fue clickeado por ultima vez (p.ej. "Continuar"), asi que al
        // reactivar el panel ese boton se redibuja en estado Selected (borde celeste) sin que
        // el mouse este encima. Limpiar la seleccion (no los componentes Selectable) al abrir y
        // al cerrar deja cada apertura visualmente limpia sin afectar Highlighted/Pressed.
        if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);
    }

    private void Update()
    {
        if (GameStateManager.Instance.Current == GameState.MainMenu) return;

        // Prompt 10 (Bloque 2): antes Keyboard.current.escapeKey directo -- ahora la accion
        // "Pause" de GameInput, que ademas del Escape ya trae el equivalente de mando
        // (Gamepad start) sin duplicar el binding aqui.
        //
        // Bugfix: Esc alternaba Pausa/Reanudar sin importar que panel estuviera abierto encima,
        // asi que reanudar mientras Configuracion (o un panel de HUDModalPanel) seguia visible
        // dejaba al jugador moverse con el panel superpuesto -- estado inconsistente. Ahora Esc
        // cierra primero lo que este "mas arriba" en la pila de UI, y solo alterna Pausa/Reanudar
        // si no hay ningun panel abierto. Se consulta el estado ya existente de cada panel
        // (HUDModalPanel.IsPanelOpen, SettingsUI.IsOpen/IsRebindingActive) en vez de crear una
        // pila de UI nueva -- ninguno de los paneles del proyecto se anida mas de un nivel hoy.
        if (GameInput.Instance.PauseAction.WasPressedThisFrame())
        {
            if (SettingsUI.Instance != null && SettingsUI.Instance.IsRebindingActive)
            {
                // Hay una captura de tecla en curso (pestaña Controles): el propio Input System
                // ya la cancela via WithCancelingThrough("<Keyboard>/escape") en
                // ControlsRebindingPanel.StartRebind. No hacemos nada mas con este Esc -- ni
                // cerrar Configuracion ni tocar Pausa -- para que un solo toque tenga un solo
                // efecto (cancelar el rebind). Un segundo Esc, ya sin rebind activo, cierra
                // Configuracion.
                return;
            }

            if (GameHUD.Instance != null && GameHUD.Instance.IsPanelOpen)
            {
                GameHUD.Instance.HidePanel();
                return;
            }

            if (SettingsUI.Instance != null && SettingsUI.Instance.IsOpen)
            {
                SettingsUI.Instance.Close();
                return;
            }

            if (GameStateManager.Instance.Current == GameState.Paused)
                GameStateManager.Instance.Resume();
            else
                GameStateManager.Instance.Pause();
        }
    }

    // Prompt 10 (Bloque 2): antes alternaba MovementInput.enabled -- MovementInput (vendored)
    // ahora queda permanentemente deshabilitado, reemplazado por PlayerMovementController (ver
    // ese archivo para el porque). Reactivar el MovementInput legado aqui volveria a traer
    // Input.GetAxis y produciria doble movimiento junto al componente nuevo.
    private void SetPlayerControlEnabled(bool controlEnabled)
    {
        var interaction = FindFirstObjectByType<PlayerInteraction>();
        if (interaction != null) interaction.enabled = controlEnabled;

        var movement = FindFirstObjectByType<PlayerMovementController>();
        if (movement != null) movement.enabled = controlEnabled;
    }

    public void Continuar()
    {
        GameStateManager.Instance.Resume();
    }

    public void Configuracion()
    {
        SettingsUI.Instance.Open();
    }

    public void Reiniciar()
    {
        GameStateManager.Instance.RestartSection();
    }

    public void VolverAlMenu()
    {
        GameStateManager.Instance.ReturnToMenu();
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
