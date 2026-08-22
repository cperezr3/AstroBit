using UnityEngine;
using UnityEngine.UI;

// Panel de mision compacto (Prompt 23, Parte 1). Solo representa visualmente el estado real de
// MissionNavigation -- no decide progresion ni mantiene su propia lista de objetivos. Ubicado en
// el lateral izquierdo (no en la esquina superior izquierda, para no chocar con
// ObjectiveText/HintText de GameHUD).
public class MissionUI : MonoBehaviour
{
    private const int MaxCompletedRows = 4;
    private const float RefreshInterval = 0.2f;

    private Text completedSummaryText;
    private Text[] completedRows;
    private Text currentRowText;
    private Text descriptionText;
    private Text subProgressText;

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
        panelRT.sizeDelta = new Vector2(320, 260);
        panelRT.anchoredPosition = new Vector2(20, 0);
        var bg = panelGO.AddComponent<Image>();
        bg.color = new Color(0.04f, 0.07f, 0.1f, 0.72f);

        var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        var headerText = CreateText(panelGO.transform, "Header", font, 20, TextAnchor.UpperLeft,
            new Vector2(14, -12), new Vector2(290, 26), new Color(0.35f, 0.95f, 1f), FontStyle.Bold);
        headerText.text = "MISIÓN";

        completedSummaryText = CreateText(panelGO.transform, "CompletedSummary", font, 14, TextAnchor.UpperLeft,
            new Vector2(14, -40), new Vector2(290, 18), new Color(0.6f, 0.85f, 0.65f), FontStyle.Italic);

        completedRows = new Text[MaxCompletedRows];
        for (int i = 0; i < MaxCompletedRows; i++)
        {
            completedRows[i] = CreateText(panelGO.transform, "Completed" + i, font, 14, TextAnchor.UpperLeft,
                new Vector2(14, -60 - i * 20), new Vector2(290, 18), new Color(0.55f, 0.8f, 0.6f), FontStyle.Normal);
        }

        currentRowText = CreateText(panelGO.transform, "Current", font, 17, TextAnchor.UpperLeft,
            new Vector2(14, -146), new Vector2(290, 26), new Color(1f, 0.85f, 0.1f, 1f), FontStyle.Bold);

        descriptionText = CreateText(panelGO.transform, "Description", font, 14, TextAnchor.UpperLeft,
            new Vector2(30, -176), new Vector2(270, 40), Color.white, FontStyle.Normal);
        descriptionText.verticalOverflow = VerticalWrapMode.Overflow;

        subProgressText = CreateText(panelGO.transform, "SubProgress", font, 13, TextAnchor.UpperLeft,
            new Vector2(30, -222), new Vector2(270, 20), new Color(0.75f, 0.9f, 0.95f), FontStyle.Italic);
    }

    private static Text CreateText(Transform parent, string name, Font font, int size, TextAnchor anchor,
        Vector2 anchoredPos, Vector2 sizeDelta, Color color, FontStyle style)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);

        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = sizeDelta;

        var text = go.AddComponent<Text>();
        text.font = font;
        text.fontSize = size;
        text.fontStyle = style;
        text.alignment = anchor;
        text.color = color;
        text.text = "";
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Truncate;

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

        completedSummaryText.enabled = hiddenCount > 0;
        completedSummaryText.text = hiddenCount > 0 ? ("+ " + hiddenCount + " fases anteriores") : "";

        for (int i = 0; i < MaxCompletedRows; i++)
        {
            bool show = i >= (MaxCompletedRows - shownCompleted);
            completedRows[i].enabled = show;
            if (!show) continue;

            int phaseIdx = index - shownCompleted + (i - (MaxCompletedRows - shownCompleted));
            var p = MissionNavigation.Order[phaseIdx];
            completedRows[i].text = "✓ " + MissionNavigation.GetPhaseTitle(p);
        }

        currentRowText.text = "● " + nav.CurrentTitle;
        descriptionText.text = nav.CurrentDescription;

        subProgressText.enabled = !string.IsNullOrEmpty(sub);
        subProgressText.text = sub;
    }
}
