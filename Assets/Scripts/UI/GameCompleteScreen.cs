using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Fase 3 (Prompt 02_continuacion, seccion 30): FinalActivity.FinishFinalActivity() dejaba al
// jugador con un texto de objetivo actualizado ("Recorrido completado.") y nada mas -- sin
// ninguna sensacion real de cierre despues del panel de recompensa (que si esta bien: titulo,
// diagrama de flujo, "Continuar"). Esta pantalla es el remate: aparece justo despues de cerrar
// ese panel, resume lo aprendido, y ofrece una salida clara en vez de dejar al jugador
// deambulando sin saber que mas hacer. Mismo lenguaje visual que el resto de dialogos del
// proyecto (fondo negro, borde cian, TextMeshPro) -- mismo patron singleton que SettingsUI/
// PauseMenuController (Bootstrap + DontDestroyOnLoad).
public class GameCompleteScreen : MonoBehaviour
{
    private const string MenuSceneName = "MainMenu";
    private static readonly Color AccentCyan = new Color(0.35f, 0.95f, 1f);

    public static GameCompleteScreen Instance { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        if (Instance != null) return;
        var go = new GameObject("GameCompleteScreen");
        go.AddComponent<GameCompleteScreen>();
    }

    private GameObject panelRoot;

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

    public void Show()
    {
        panelRoot.SetActive(true);
    }

    private void Hide()
    {
        panelRoot.SetActive(false);
    }

    private void VolverAlMenu()
    {
        // Mismo motivo que PauseMenuController.VolverAlMenu: guarda antes de salir para que
        // "Continuar" desde el Main Menu recupere exactamente este punto (aunque el hito ya se
        // autoguardo al completar el ultimo objetivo, esto es una red de seguridad barata).
        SaveManager.Instance.SaveGame();
        Hide();
        SceneManager.LoadScene(MenuSceneName);
    }

    private void BuildUI()
    {
        var canvasGO = new GameObject("GameCompleteCanvas");
        canvasGO.transform.SetParent(transform, false);
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 15; // por encima de GameHUD/MissionUI, por debajo de SettingsUI (20)
        var scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasGO.AddComponent<GraphicRaycaster>();

        panelRoot = new GameObject("Panel", typeof(RectTransform));
        panelRoot.transform.SetParent(canvasGO.transform, false);
        var backdropRT = panelRoot.GetComponent<RectTransform>();
        backdropRT.anchorMin = Vector2.zero;
        backdropRT.anchorMax = Vector2.one;
        backdropRT.offsetMin = Vector2.zero;
        backdropRT.offsetMax = Vector2.zero;
        panelRoot.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.85f);

        var boxGO = new GameObject("Box", typeof(RectTransform));
        boxGO.transform.SetParent(panelRoot.transform, false);
        var boxRT = boxGO.GetComponent<RectTransform>();
        boxRT.anchorMin = boxRT.anchorMax = boxRT.pivot = new Vector2(0.5f, 0.5f);
        boxRT.sizeDelta = new Vector2(820, 620);
        boxGO.AddComponent<Image>().color = new Color(0.05f, 0.07f, 0.09f, 0.97f);
        var boxOutline = boxGO.AddComponent<Outline>();
        boxOutline.effectColor = AccentCyan;
        boxOutline.effectDistance = new Vector2(2, 2);

        var titleGO = new GameObject("Title", typeof(RectTransform));
        titleGO.transform.SetParent(boxGO.transform, false);
        var titleRT = titleGO.GetComponent<RectTransform>();
        titleRT.anchorMin = titleRT.anchorMax = new Vector2(0.5f, 1f);
        titleRT.pivot = new Vector2(0.5f, 1f);
        titleRT.anchoredPosition = new Vector2(0, -40);
        titleRT.sizeDelta = new Vector2(760, 60);
        var title = titleGO.AddComponent<TextMeshProUGUI>();
        title.font = TMP_Settings.defaultFontAsset;
        title.text = "ASTROBIT COMPLETADO";
        title.fontSize = 38;
        title.fontStyle = FontStyles.Bold;
        title.alignment = TextAlignmentOptions.Center;
        title.color = AccentCyan;

        const float bodyOffsetY = -130f;
        var bodyGO = new GameObject("Body", typeof(RectTransform));
        bodyGO.transform.SetParent(boxGO.transform, false);
        var bodyRT = bodyGO.GetComponent<RectTransform>();
        bodyRT.anchorMin = bodyRT.anchorMax = new Vector2(0.5f, 1f);
        bodyRT.pivot = new Vector2(0.5f, 1f);
        bodyRT.anchoredPosition = new Vector2(0, bodyOffsetY);
        bodyRT.sizeDelta = new Vector2(720, 300);
        var body = bodyGO.AddComponent<TextMeshProUGUI>();
        body.font = TMP_Settings.defaultFontAsset;
        body.text = "Has recorrido el camino completo de un programa dentro de una computadora:\n\n" +
                     "ALMACENAMIENTO   →   RAM   →   CACHÉ   →   REGISTROS   →   ALU   →   CPU\n\n" +
                     "Comprendiste como cada componente cumple un rol especifico para que un programa se ejecute correctamente.\n\n" +
                     "Gracias por jugar AstroBit.";
        body.fontSize = 22;
        body.alignment = TextAlignmentOptions.Top;
        body.color = Color.white;

        // El texto tiene varios parrafos y su altura real varia segun el ancho disponible para
        // envolver -- un offset fijo para los botones (como se probo primero) quedaba justo
        // encima de la mitad del texto en vez de debajo de todo el parrafo. Mismo patron que
        // GameHUD.RepositionHintBelowObjective: usar la altura real en vez de un numero fijo.
        float buttonY = bodyOffsetY - body.preferredHeight - 40f;
        CreateButton(boxGO.transform, "SeguirExplorando", "SEGUIR EXPLORANDO", new Vector2(-165, buttonY), Hide, false);
        CreateButton(boxGO.transform, "VolverMenu", "VOLVER AL MENÚ", new Vector2(165, buttonY), VolverAlMenu, true);

        panelRoot.SetActive(false);
    }

    private void CreateButton(Transform parent, string name, string label, Vector2 anchoredPos, UnityEngine.Events.UnityAction onClick, bool primary)
    {
        var btnGO = new GameObject(name, typeof(RectTransform));
        btnGO.transform.SetParent(parent, false);
        var rt = btnGO.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 1f);
        rt.pivot = new Vector2(0.5f, 1f);
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = new Vector2(300, 60);

        var img = btnGO.AddComponent<Image>();
        img.color = primary ? new Color(0.15f, 0.55f, 0.65f) : new Color(0.15f, 0.18f, 0.2f);
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
        var labelTmp = labelGO.AddComponent<TextMeshProUGUI>();
        labelTmp.text = label;
        labelTmp.fontSize = 20;
        labelTmp.fontStyle = FontStyles.Bold;
        labelTmp.alignment = TextAlignmentOptions.Center;
        labelTmp.color = Color.white;
    }
}
