using TMPro;
using UnityEngine;

// Prompt 09 (Bloque 1, arquitectura): pequeno helper compartido para el estilo de texto plano
// que GameHUD.cs repetia identico en 7 lugares distintos antes de dividirse en
// HUDObjectiveDisplay/HUDFeedbackBanner. Sin estado propio -- una funcion pura de construccion de
// UI, no un componente ni una referencia cruzada entre los sub-sistemas del HUD.
public static class HUDText
{
    public static TextMeshProUGUI Create(Transform parent, string name, int size, TextAlignmentOptions anchor,
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

        var outline = go.AddComponent<UnityEngine.UI.Outline>();
        outline.effectColor = new Color(0f, 0f, 0f, 0.85f);
        outline.effectDistance = new Vector2(1.5f, -1.5f);

        return text;
    }
}
