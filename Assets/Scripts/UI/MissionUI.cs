using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Panel de mision compacto (Prompt 23, Parte 1). Solo representa visualmente el estado real de
// MissionNavigation -- no decide progresion ni mantiene su propia lista de objetivos. Ubicado en
// el lateral izquierdo (no en la esquina superior izquierda, para no chocar con
// ObjectiveText/HintText de GameHUD).
//
// Prompt 29: layout dinamico de arriba hacia abajo. Antes cada fila tenia una posicion Y fija
// (pensada para 4 filas de historial siempre reservadas), asi que con pocas o ninguna mision
// completada quedaba un hueco vacio grande arriba y todo se veia "hundido". Ahora cada Refresh()
// recalcula las posiciones segun cuantas filas hay realmente que mostrar, empezando siempre
// justo debajo del header.
public class MissionUI : MonoBehaviour
{
    private const int MaxCompletedRows = 4;
    private const float RefreshInterval = 0.2f;

    private const float PanelWidth = 380f;
    // Prompt 01_maestro (seccion 34): 300 se quedaba corto en el estado estable del juego medio
    // (una vez completadas mas de 4 fases, el resumen "+N fases anteriores" y las 4 filas de
    // historial siempre se muestran) -- el contenido real superaba el fondo del panel en unos
    // 18-40px. 360 cubre ese caso con margen.
    private const float PanelHeight = 360f;
    private const float MarginX = 16f;
    private const float IndentX = 32f;
    private const float TopY = -14f;

    private const float HeaderHeight = 28f;
    private const float HeaderGap = 6f;
    private const float SummaryHeight = 20f;
    private const float SummaryGap = 4f;
    private const float RowHeight = 24f;
    private const float RowGap = 2f;
    private const float GapBeforeCurrent = 8f;
    private const float CurrentHeight = 30f;
    private const float CurrentGap = 6f;
    private const float DescriptionHeight = 46f;
    private const float DescriptionGap = 6f;

    private TextMeshProUGUI headerText;
    private TextMeshProUGUI completedSummaryText;
    private TextMeshProUGUI[] completedRows;
    private TextMeshProUGUI currentRowText;
    private TextMeshProUGUI descriptionText;
    private TextMeshProUGUI subProgressText;

    private MissionNavigation.Phase lastPhase = (MissionNavigation.Phase)(-1);
    private string lastSub;
    private float nextRefresh;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        var go = new GameObject("MissionUI");
        go.AddComponent<MissionUI>();
        DontDestroyOnLoad(go);
    }

    private void Awake()
    {
        BuildUI();
    }

    private void BuildUI()
    {
        var canvasGO = new GameObject("MissionUICanvas");
        canvasGO.transform.SetParent(transform, false);
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = -1; // por debajo del backdrop modal de GameHUD, para no flotar sobre los paneles
        var scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasGO.AddComponent<GraphicRaycaster>();

        var panelGO = new GameObject("MissionPanel");
        panelGO.transform.SetParent(canvasGO.transform, false);
        var panelRT = panelGO.AddComponent<RectTransform>();
        panelRT.anchorMin = panelRT.anchorMax = new Vector2(0f, 0.5f);
        panelRT.pivot = new Vector2(0f, 0.5f);
        panelRT.sizeDelta = new Vector2(PanelWidth, PanelHeight);
        panelRT.anchoredPosition = new Vector2(20, 0);
        var bg = panelGO.AddComponent<Image>();
        bg.color = new Color(0.04f, 0.07f, 0.1f, 0.72f);

        float rowWidth = PanelWidth - MarginX * 2f;
        float indentWidth = PanelWidth - IndentX - MarginX;

        headerText = CreateText(panelGO.transform, "Header", 22, TextAlignmentOptions.TopLeft,
            new Vector2(MarginX, TopY), new Vector2(rowWidth, HeaderHeight), new Color(0.35f, 0.95f, 1f), FontStyles.Bold);
        headerText.text = "MISIÓN";

        completedSummaryText = CreateText(panelGO.transform, "CompletedSummary", 15, TextAlignmentOptions.TopLeft,
            Vector2.zero, new Vector2(rowWidth, SummaryHeight), new Color(0.6f, 0.85f, 0.65f), FontStyles.Italic);

        completedRows = new TextMeshProUGUI[MaxCompletedRows];
        for (int i = 0; i < MaxCompletedRows; i++)
        {
            completedRows[i] = CreateText(panelGO.transform, "Completed" + i, 16, TextAlignmentOptions.TopLeft,
                Vector2.zero, new Vector2(rowWidth, RowHeight), new Color(0.55f, 0.8f, 0.6f), FontStyles.Normal);
        }

        currentRowText = CreateText(panelGO.transform, "Current", 19, TextAlignmentOptions.TopLeft,
            Vector2.zero, new Vector2(rowWidth, CurrentHeight), new Color(1f, 0.85f, 0.1f, 1f), FontStyles.Bold);

        descriptionText = CreateText(panelGO.transform, "Description", 16, TextAlignmentOptions.TopLeft,
            Vector2.zero, new Vector2(indentWidth, DescriptionHeight), Color.white, FontStyles.Normal);
        descriptionText.overflowMode = TextOverflowModes.Overflow;

        subProgressText = CreateText(panelGO.transform, "SubProgress", 14, TextAlignmentOptions.TopLeft,
            Vector2.zero, new Vector2(indentWidth, 20), new Color(0.75f, 0.9f, 0.95f), FontStyles.Italic);
    }

    private static TextMeshProUGUI CreateText(Transform parent, string name, int size, TextAlignmentOptions anchor,
        Vector2 anchoredPos, Vector2 sizeDelta, Color color, FontStyles style)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);

        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = sizeDelta;

        var text = go.AddComponent<TextMeshProUGUI>();
        text.font = TMP_Settings.defaultFontAsset;
        text.fontSize = size;
        text.fontStyle = style;
        text.alignment = anchor;
        text.color = color;
        text.text = "";
        text.textWrappingMode = TextWrappingModes.Normal;
        text.overflowMode = TextOverflowModes.Truncate;

        var outline = go.AddComponent<Outline>();
        outline.effectColor = new Color(0f, 0f, 0f, 0.85f);
        outline.effectDistance = new Vector2(1f, -1f);

        return text;
    }

    private void Update()
    {
        if (Time.time < nextRefresh) return;
        nextRefresh = Time.time + RefreshInterval;
        Refresh();
    }

    private void Refresh()
    {
        var nav = MissionNavigation.Instance;
        var phase = nav.CurrentPhase;
        string sub = nav.CurrentSubProgress;

        if (phase == lastPhase && sub == lastSub) return;
        lastPhase = phase;
        lastSub = sub;

        int index = MissionNavigation.PhaseIndex(phase);
        int completedCount = Mathf.Max(0, index); // toda fase anterior en el orden ya esta completada
        int hiddenCount = Mathf.Max(0, completedCount - MaxCompletedRows);
        int shownCompleted = Mathf.Min(completedCount, MaxCompletedRows);

        float y = TopY;
        SetRowPosition(headerText, y);
        y -= HeaderHeight + HeaderGap;

        bool showSummary = hiddenCount > 0;
        completedSummaryText.enabled = showSummary;
        if (showSummary)
        {
            completedSummaryText.text = "+ " + hiddenCount + " fases anteriores";
            SetRowPosition(completedSummaryText, y);
            y -= SummaryHeight + SummaryGap;
        }

        for (int i = 0; i < MaxCompletedRows; i++)
        {
            bool show = i < shownCompleted;
            completedRows[i].enabled = show;
            if (!show) continue;

            int phaseIdx = index - shownCompleted + i;
            var p = MissionNavigation.Order[phaseIdx];
            completedRows[i].text = "✓ " + MissionNavigation.GetPhaseTitle(p);
            SetRowPosition(completedRows[i], y);
            y -= RowHeight + RowGap;
        }

        y -= GapBeforeCurrent;
        currentRowText.text = "● " + nav.CurrentTitle;
        SetRowPosition(currentRowText, y);
        y -= CurrentHeight + CurrentGap;

        descriptionText.text = nav.CurrentDescription;
        SetRowPosition(descriptionText, y, IndentX);
        // La descripcion puede envolver a 2-3 lineas segun su longitud (overflowMode.Overflow no
        // la trunca pero tampoco reserva espacio extra por si sola); usar la altura real evita que
        // se superponga con subProgressText, igual que GameHUD.RepositionHintBelowObjective ya
        // hace para Objetivo/Pista.
        float descHeight = Mathf.Max(DescriptionHeight, descriptionText.preferredHeight);
        y -= descHeight + DescriptionGap;

        subProgressText.enabled = !string.IsNullOrEmpty(sub);
        subProgressText.text = sub;
        SetRowPosition(subProgressText, y, IndentX);
    }

    private static void SetRowPosition(TextMeshProUGUI text, float y, float x = MarginX)
    {
        text.rectTransform.anchoredPosition = new Vector2(x, y);
    }
}
