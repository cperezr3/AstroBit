using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Prompt 09 (Bloque 1, arquitectura): extraido de GameHUD.cs (antes ~720 lineas en una sola
// clase). Responsabilidad unica: el badge contextual "[E] Accion" que aparece/desaparece cerca
// de un IInteractable. No sabe nada de objetivos, feedback ni paneles -- eso vive en
// HUDObjectiveDisplay/HUDFeedbackBanner/HUDModalPanel, hermanos de este componente bajo el mismo
// Canvas. GameHUD (fachada) es quien lo crea e inicializa; nadie mas debe instanciarlo.
public class HUDPrompt : MonoBehaviour
{
    private const float PromptShownY = 130f;
    private const float PromptSlideOffset = 14f;
    private const float PromptFadeDuration = 0.12f;

    private RectTransform promptRoot;
    private CanvasGroup promptCanvasGroup;
    private TextMeshProUGUI promptKeyLabel;
    private TextMeshProUGUI promptActionLabel;
    private Coroutine promptAnimRoutine;

    public void Init(Transform parent)
    {
        BuildUI(parent);
        HideImmediate();
    }

    // Prompt 01_maestro (seccion 18): antes era un unico TextMeshProUGUI con el texto crudo
    // "[E] Interactuar" y un SetActive instantaneo. Ahora la tecla se separa visualmente en su
    // propia "credencial" (fondo + borde) apilada sobre la accion, y aparece/desaparece con un
    // fundido + deslizamiento breve en vez de un corte seco. Reutiliza la convencion de texto
    // "[X] Accion" que ya usan los IInteractable existentes -- no hace falta tocarlos.
    private void BuildUI(Transform parent)
    {
        var rootGO = new GameObject("PromptRoot", typeof(RectTransform));
        rootGO.transform.SetParent(parent, false);
        promptRoot = rootGO.GetComponent<RectTransform>();
        promptRoot.anchorMin = promptRoot.anchorMax = promptRoot.pivot = new Vector2(0.5f, 0f);
        promptRoot.sizeDelta = new Vector2(320, 78);
        promptRoot.anchoredPosition = new Vector2(0, PromptShownY - PromptSlideOffset);
        promptCanvasGroup = rootGO.AddComponent<CanvasGroup>();
        promptCanvasGroup.alpha = 0f;
        promptCanvasGroup.blocksRaycasts = false;
        promptCanvasGroup.interactable = false;

        var badgeGO = new GameObject("KeyBadge", typeof(RectTransform));
        badgeGO.transform.SetParent(rootGO.transform, false);
        var badgeRT = badgeGO.GetComponent<RectTransform>();
        badgeRT.anchorMin = badgeRT.anchorMax = new Vector2(0.5f, 1f);
        badgeRT.pivot = new Vector2(0.5f, 1f);
        badgeRT.anchoredPosition = Vector2.zero;
        badgeRT.sizeDelta = new Vector2(40, 40);
        var badgeImg = badgeGO.AddComponent<Image>();
        badgeImg.color = new Color(0f, 0f, 0f, 0.55f);
        badgeImg.raycastTarget = false;
        var badgeOutline = badgeGO.AddComponent<Outline>();
        badgeOutline.effectColor = new Color(0.35f, 0.95f, 1f);
        badgeOutline.effectDistance = new Vector2(1.5f, -1.5f);

        var keyLabelGO = new GameObject("KeyLabel", typeof(RectTransform));
        keyLabelGO.transform.SetParent(badgeGO.transform, false);
        var keyLabelRT = keyLabelGO.GetComponent<RectTransform>();
        keyLabelRT.anchorMin = Vector2.zero;
        keyLabelRT.anchorMax = Vector2.one;
        keyLabelRT.offsetMin = Vector2.zero;
        keyLabelRT.offsetMax = Vector2.zero;
        promptKeyLabel = keyLabelGO.AddComponent<TextMeshProUGUI>();
        promptKeyLabel.font = TMP_Settings.defaultFontAsset;
        promptKeyLabel.fontSize = 24;
        promptKeyLabel.fontStyle = FontStyles.Bold;
        promptKeyLabel.alignment = TextAlignmentOptions.Center;
        promptKeyLabel.color = new Color(0.35f, 0.95f, 1f);
        promptKeyLabel.raycastTarget = false;

        var actionGO = new GameObject("ActionLabel", typeof(RectTransform));
        actionGO.transform.SetParent(rootGO.transform, false);
        var actionRT = actionGO.GetComponent<RectTransform>();
        actionRT.anchorMin = actionRT.anchorMax = new Vector2(0.5f, 0f);
        actionRT.pivot = new Vector2(0.5f, 0f);
        actionRT.anchoredPosition = Vector2.zero;
        actionRT.sizeDelta = new Vector2(320, 32);
        promptActionLabel = actionGO.AddComponent<TextMeshProUGUI>();
        promptActionLabel.font = TMP_Settings.defaultFontAsset;
        promptActionLabel.fontSize = 26;
        promptActionLabel.fontStyle = FontStyles.Bold;
        promptActionLabel.alignment = TextAlignmentOptions.Center;
        promptActionLabel.color = Color.white;
        promptActionLabel.raycastTarget = false;
        var actionOutline = actionGO.AddComponent<Outline>();
        actionOutline.effectColor = new Color(0f, 0f, 0f, 0.85f);
        actionOutline.effectDistance = new Vector2(1.5f, -1.5f);

        rootGO.SetActive(false);
    }

    public void Show(string text)
    {
        if (promptRoot == null) return;
        ApplyPromptText(text);
        promptRoot.gameObject.SetActive(true);
        Animate(show: true);
    }

    public void Hide()
    {
        if (promptRoot == null) return;
        Animate(show: false);
    }

    private void HideImmediate()
    {
        if (promptAnimRoutine != null) { StopCoroutine(promptAnimRoutine); promptAnimRoutine = null; }
        promptCanvasGroup.alpha = 0f;
        promptRoot.anchoredPosition = new Vector2(0, PromptShownY - PromptSlideOffset);
        promptRoot.gameObject.SetActive(false);
    }

    // Convencion existente en los IInteractable del proyecto: "[X] Accion" (ver FileShelf,
    // EducationalInteractable, CollectibleRam, etc.). Si algun texto no la sigue, se muestra
    // completo en la etiqueta de accion y la credencial de tecla cae de vuelta a "E".
    private void ApplyPromptText(string text)
    {
        string key = "E";
        string action = text ?? "";
        if (action.StartsWith("[", StringComparison.Ordinal))
        {
            int close = action.IndexOf(']');
            if (close > 0)
            {
                key = action.Substring(1, close - 1);
                action = action.Substring(close + 1).Trim();
            }
        }
        promptKeyLabel.text = key;
        promptActionLabel.text = action.ToUpperInvariant();
    }

    private void Animate(bool show)
    {
        if (promptAnimRoutine != null) StopCoroutine(promptAnimRoutine);
        promptAnimRoutine = StartCoroutine(AnimateRoutine(show));
    }

    private IEnumerator AnimateRoutine(bool show)
    {
        float startAlpha = promptCanvasGroup.alpha;
        float endAlpha = show ? 1f : 0f;
        float endY = show ? PromptShownY : PromptShownY - PromptSlideOffset;
        float startY = promptRoot.anchoredPosition.y;
        float t = 0f;
        while (t < PromptFadeDuration)
        {
            t += Time.deltaTime;
            float f = Mathf.Clamp01(t / PromptFadeDuration);
            promptCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, f);
            promptRoot.anchoredPosition = new Vector2(0, Mathf.Lerp(startY, endY, f));
            yield return null;
        }
        promptCanvasGroup.alpha = endAlpha;
        promptRoot.anchoredPosition = new Vector2(0, endY);
        if (!show) promptRoot.gameObject.SetActive(false);
        promptAnimRoutine = null;
    }
}
