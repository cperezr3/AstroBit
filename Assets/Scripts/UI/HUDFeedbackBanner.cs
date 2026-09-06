using System.Collections;
using TMPro;
using UnityEngine;

// Prompt 09 (Bloque 1, arquitectura): extraido de GameHUD.cs. Responsabilidad unica: el mensaje
// flotante temporal en la parte inferior de pantalla (hallazgos, avisos, diagnosticos). Se
// suscribe directamente a ObjectiveSystem.OnObjectiveCompleted en vez de que GameHUD reenvie la
// llamada -- "conectados por eventos, no por referencias cruzadas directas" (Prompt 09).
public class HUDFeedbackBanner : MonoBehaviour
{
    // Antes [SerializeField] en GameHUD, pero GameHUD siempre se crea por codigo
    // (RuntimeInitializeOnLoadMethod), nunca como instancia de escena editable en el Inspector --
    // el valor por defecto era, en la practica, el unico valor posible. Constante equivalente.
    private const float DefaultFeedbackDuration = 3.5f;

    private TextMeshProUGUI feedbackText;
    private Coroutine feedbackRoutine;

    public float DefaultDuration => DefaultFeedbackDuration;

    public void Init(Transform parent)
    {
        feedbackText = HUDText.Create(parent, "FeedbackText", 26, TextAlignmentOptions.Bottom,
            new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0, 190), new Vector2(960, 90));

        HideImmediate();
        ObjectiveSystem.Instance.OnObjectiveCompleted.AddListener(text => Show(text));
    }

    // duration: null usa DefaultDuration (comportamiento normal para el resto de mensajes). Solo
    // el diagnostico de RAM insuficiente (StorageMission) pasa un valor explicito para quedarse
    // mas tiempo en pantalla sin afectar la duracion global de los demas feedbacks.
    public void Show(string text, float? duration = null)
    {
        if (feedbackText == null) return;
        if (feedbackRoutine != null) StopCoroutine(feedbackRoutine);
        feedbackText.text = text;
        feedbackText.enabled = true;
        feedbackRoutine = StartCoroutine(HideAfterDelay(duration ?? DefaultFeedbackDuration));
    }

    private void HideImmediate()
    {
        if (feedbackText == null) return;
        feedbackText.text = "";
        feedbackText.enabled = false;
    }

    private IEnumerator HideAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        HideImmediate();
    }
}
