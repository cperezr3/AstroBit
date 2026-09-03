using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Controlador del Main Menu (Prompt 26): equivalente limpio de NewMonoBehaviourScript del
// proyecto AstroBitMenu, adaptado para cargar la escena de gameplay por nombre en vez de por
// build index (evita depender del orden accidental en Build Settings).
//
// Prompt 28: agrega el botón "Continuar" -- habilitado solo si GameSession.HasActiveGame es
// true (ver GameSession). El wiring de su OnClick se hace en Awake con AddListener normal (no
// persistente) porque, a diferencia de "Nueva Partida"/"Salir", su disponibilidad depende de
// estado que solo se conoce en tiempo de ejecucion.
//
// Fase 2 (Prompt 35, 9.1): "Continuar" ahora tambien se habilita si hay una partida guardada en
// disco (SaveManager.HasSave), no solo si ya se jugo en esta misma sesion. "Nueva Partida" pide
// confirmacion si ya existe un guardado, para no perderlo por accidente. El dialogo de
// confirmacion y el boton "Opciones" se construyen aqui en codigo (mismo lenguaje visual que ya
// tenian los botones autorados a mano en la escena: fondo negro, borde cian, TextMeshPro).
public class MainMenuController : MonoBehaviour
{
    private const string GameplaySceneName = "SampleScene";
    private static readonly Color AccentCyan = new Color(0f, 1f, 1f, 1f);

    private Button continuarButton;
    private Button opcionesButton;
    private GameObject confirmDialog;
    private GameObject creditsDialog;

    private void Awake()
    {
        continuarButton = transform.Find("Continuar")?.GetComponent<Button>();
        if (continuarButton != null)
        {
            RefreshContinuarInteractable();
            continuarButton.onClick.AddListener(Continuar);
        }

        opcionesButton = transform.Find("Opciones")?.GetComponent<Button>();
        if (opcionesButton != null)
        {
            opcionesButton.interactable = true;
            opcionesButton.onClick.AddListener(() => SettingsUI.Instance.Open());
        }

        // Prompt 01_maestro (seccion 7/34): "Salir" y "Creditos" no tenian ningun listener --
        // ni persistente en la escena ni cableado por codigo -- pese a que Salir() ya existia.
        // Dos de los cinco botones del menu principal no hacian absolutamente nada al hacer clic.
        var salirButton = transform.Find("Salir")?.GetComponent<Button>();
        salirButton?.onClick.AddListener(Salir);

        var creditosButton = transform.Find("Creditos")?.GetComponent<Button>();
        creditosButton?.onClick.AddListener(ShowCredits);

        // Mismo motivo por el que se corrigio SettingsUI: consistencia visual. Los 5 botones
        // autorados a mano en la escena traian el "selectedColor" por defecto de Unity (gris
        // casi blanco) en vez del acento cian del resto de la identidad visual -- se notaba al
        // navegar el menu con teclado/mando (Selectable.Selected).
        foreach (var name in new[] { "NuevaPartida", "Continuar", "Opciones", "Creditos", "Salir" })
        {
            var button = transform.Find(name)?.GetComponent<Button>();
            if (button == null) continue;
            var colors = button.colors;
            colors.selectedColor = AccentCyan;
            button.colors = colors;
        }

        BuildConfirmDialog();
        BuildCreditsDialog();
    }

    private void RefreshContinuarInteractable()
    {
        if (continuarButton == null) return;
        continuarButton.interactable = GameSession.HasActiveGame || SaveManager.Instance.HasSave;
    }

    public void Jugar()
    {
        if (SaveManager.Instance.HasSave)
        {
            ShowConfirmDialog();
            return;
        }

        StartNewGame();
    }

    private void StartNewGame()
    {
        GameSession.ResetAll();
        SaveManager.Instance.DeleteSave();
        SceneManager.LoadScene(GameplaySceneName);
    }

    public void Continuar()
    {
        if (!GameSession.HasActiveGame && !SaveManager.Instance.HasSave) return;

        SceneManager.LoadScene(GameplaySceneName);

        // Si ya habia partida activa en esta misma sesion (p.ej. Pausa -> Volver al Menu ->
        // Continuar), el estado en memoria ya es el correcto: cargar el guardado de disco aqui
        // lo pisaria con una version mas vieja. Solo se restaura desde disco en un arranque
        // fresco del juego, donde GameSession.HasActiveGame todavia es false.
        if (!GameSession.HasActiveGame)
        {
            SaveManager.Instance.LoadGame();
            GameSession.MarkActiveGame();
        }
    }

    public void Salir()
    {
        Debug.Log("Saliendo...");
        Application.Quit();
    }

    // ---- Dialogo de confirmacion para "Nueva Partida" cuando ya existe un guardado ----

    private void ShowConfirmDialog()
    {
        confirmDialog.SetActive(true);
    }

    private void HideConfirmDialog()
    {
        confirmDialog.SetActive(false);
    }

    // ---- Creditos: mismo lenguaje visual que el dialogo de confirmacion ----

    private void ShowCredits()
    {
        creditsDialog.SetActive(true);
    }

    private void HideCredits()
    {
        creditsDialog.SetActive(false);
    }

    private void BuildCreditsDialog()
    {
        var canvas = GetComponent<Canvas>();

        creditsDialog = new GameObject("CreditosDialog", typeof(RectTransform));
        creditsDialog.transform.SetParent(canvas.transform, false);
        var rootRT = creditsDialog.GetComponent<RectTransform>();
        rootRT.anchorMin = Vector2.zero;
        rootRT.anchorMax = Vector2.one;
        rootRT.offsetMin = Vector2.zero;
        rootRT.offsetMax = Vector2.zero;

        var backdropGO = new GameObject("Backdrop", typeof(RectTransform));
        backdropGO.transform.SetParent(creditsDialog.transform, false);
        var backdropRT = backdropGO.GetComponent<RectTransform>();
        backdropRT.anchorMin = Vector2.zero;
        backdropRT.anchorMax = Vector2.one;
        backdropRT.offsetMin = Vector2.zero;
        backdropRT.offsetMax = Vector2.zero;
        backdropGO.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.8f);

        var boxGO = new GameObject("Box", typeof(RectTransform));
        boxGO.transform.SetParent(creditsDialog.transform, false);
        var boxRT = boxGO.GetComponent<RectTransform>();
        boxRT.anchorMin = boxRT.anchorMax = boxRT.pivot = new Vector2(0.5f, 0.5f);
        boxRT.sizeDelta = new Vector2(720, 440);
        boxGO.AddComponent<Image>().color = new Color(0.05f, 0.05f, 0.05f, 0.97f);
        var boxOutline = boxGO.AddComponent<Outline>();
        boxOutline.effectColor = AccentCyan;
        boxOutline.effectDistance = new Vector2(2, 2);

        var titleGO = new GameObject("Title", typeof(RectTransform));
        titleGO.transform.SetParent(boxGO.transform, false);
        var titleRT = titleGO.GetComponent<RectTransform>();
        titleRT.anchorMin = titleRT.anchorMax = new Vector2(0.5f, 1f);
        titleRT.pivot = new Vector2(0.5f, 1f);
        titleRT.anchoredPosition = new Vector2(0, -30);
        titleRT.sizeDelta = new Vector2(660, 50);
        var title = titleGO.AddComponent<TextMeshProUGUI>();
        title.text = "ASTROBIT";
        title.fontSize = 34;
        title.fontStyle = FontStyles.Bold;
        title.alignment = TextAlignmentOptions.Center;
        title.color = AccentCyan;

        var bodyGO = new GameObject("Body", typeof(RectTransform));
        bodyGO.transform.SetParent(boxGO.transform, false);
        var bodyRT = bodyGO.GetComponent<RectTransform>();
        bodyRT.anchorMin = bodyRT.anchorMax = new Vector2(0.5f, 1f);
        bodyRT.pivot = new Vector2(0.5f, 1f);
        bodyRT.anchoredPosition = new Vector2(0, -100);
        bodyRT.sizeDelta = new Vector2(640, 260);
        var body = bodyGO.AddComponent<TextMeshProUGUI>();
        body.text = "Juego educativo interactivo sobre arquitectura de computadoras.\n\n" +
                     "Desarrollado con Unity.\n\n" +
                     "Assets de terceros:\n" +
                     "SciFi Warehouse Kit · ScifiOffice Lite · Jammo Character\n" +
                     "GoldenFrame Terminal · iPoly3D";
        body.fontSize = 20;
        body.alignment = TextAlignmentOptions.Top;
        body.color = Color.white;

        CreateDialogButton(boxGO.transform, "Cerrar", "CERRAR", new Vector2(0, -190), HideCredits);

        creditsDialog.SetActive(false);
    }

    private void BuildConfirmDialog()
    {
        var canvas = GetComponent<Canvas>();

        confirmDialog = new GameObject("ConfirmNuevaPartida", typeof(RectTransform));
        confirmDialog.transform.SetParent(canvas.transform, false);
        var rootRT = confirmDialog.GetComponent<RectTransform>();
        rootRT.anchorMin = Vector2.zero;
        rootRT.anchorMax = Vector2.one;
        rootRT.offsetMin = Vector2.zero;
        rootRT.offsetMax = Vector2.zero;

        var backdropGO = new GameObject("Backdrop", typeof(RectTransform));
        backdropGO.transform.SetParent(confirmDialog.transform, false);
        var backdropRT = backdropGO.GetComponent<RectTransform>();
        backdropRT.anchorMin = Vector2.zero;
        backdropRT.anchorMax = Vector2.one;
        backdropRT.offsetMin = Vector2.zero;
        backdropRT.offsetMax = Vector2.zero;
        backdropGO.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.75f);

        var boxGO = new GameObject("Box", typeof(RectTransform));
        boxGO.transform.SetParent(confirmDialog.transform, false);
        var boxRT = boxGO.GetComponent<RectTransform>();
        boxRT.anchorMin = boxRT.anchorMax = boxRT.pivot = new Vector2(0.5f, 0.5f);
        boxRT.sizeDelta = new Vector2(640, 260);
        boxGO.AddComponent<Image>().color = new Color(0.05f, 0.05f, 0.05f, 0.97f);
        var boxOutline = boxGO.AddComponent<Outline>();
        boxOutline.effectColor = AccentCyan;
        boxOutline.effectDistance = new Vector2(2, 2);

        var titleGO = new GameObject("Title", typeof(RectTransform));
        titleGO.transform.SetParent(boxGO.transform, false);
        var titleRT = titleGO.GetComponent<RectTransform>();
        titleRT.anchorMin = titleRT.anchorMax = new Vector2(0.5f, 1f);
        titleRT.pivot = new Vector2(0.5f, 1f);
        titleRT.anchoredPosition = new Vector2(0, -30);
        titleRT.sizeDelta = new Vector2(580, 100);
        var title = titleGO.AddComponent<TextMeshProUGUI>();
        title.text = "Ya existe una partida guardada.\n¿Deseas empezar una nueva y perder ese progreso?";
        title.fontSize = 26;
        title.alignment = TextAlignmentOptions.Center;
        title.color = Color.white;

        CreateDialogButton(boxGO.transform, "Si", "SI, EMPEZAR DE NUEVO", new Vector2(-165, -95), () =>
        {
            HideConfirmDialog();
            StartNewGame();
        });
        CreateDialogButton(boxGO.transform, "No", "CANCELAR", new Vector2(165, -95), HideConfirmDialog);

        confirmDialog.SetActive(false);
    }

    private void CreateDialogButton(Transform parent, string name, string label, Vector2 anchoredPos, UnityEngine.Events.UnityAction onClick)
    {
        var btnGO = new GameObject(name, typeof(RectTransform));
        btnGO.transform.SetParent(parent, false);
        var rt = btnGO.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = new Vector2(280, 64);

        var img = btnGO.AddComponent<Image>();
        img.color = Color.black;
        var button = btnGO.AddComponent<Button>();
        button.targetGraphic = img;
        var colors = button.colors;
        colors.highlightedColor = AccentCyan;
        colors.pressedColor = new Color(0, 0.545f, 0.545f, 1);
        button.colors = colors;
        button.onClick.AddListener(onClick);

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
        var labelText = labelGO.AddComponent<TextMeshProUGUI>();
        labelText.text = label;
        labelText.fontSize = 22;
        labelText.fontStyle = FontStyles.Bold;
        labelText.alignment = TextAlignmentOptions.Center;
        labelText.color = Color.white;
    }
}
