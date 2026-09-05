using System.Collections;
using TMPro;
using UnityEngine;

// Prompt 09 (Bloque 1, arquitectura): extraido de GameHUD.cs. Responsabilidad unica: objetivo
// actual, pista, indicador de ubicacion, contador de progreso e inventario -- todo el texto de
// estado permanente del HUD (a diferencia de HUDFeedbackBanner, que es temporal, y HUDPrompt, que
// es contextual por proximidad). Se suscribe directamente a ObjectiveSystem, no a traves de
// GameHUD -- "conectados por eventos, no por referencias cruzadas directas" (Prompt 09).
public class HUDObjectiveDisplay : MonoBehaviour
{
    // Antes [SerializeField] en GameHUD; ver la misma nota en HUDFeedbackBanner sobre por que un
    // const equivalente no cambia el comportamiento real (GameHUD nunca se configura desde el
    // Inspector, se crea siempre por codigo).
    private const float LocationDescriptionHoldTime = 3f;
    private const float ObjectiveHintGap = 14f;

    private TextMeshProUGUI objectiveText;
    private TextMeshProUGUI hintText;
    private TextMeshProUGUI locationText;
    private TextMeshProUGUI locationSubtitleText;
    private TextMeshProUGUI progressText;
    private TextMeshProUGUI inventoryText;
    private Coroutine locationSubtitleRoutine;

    public void Init(Transform parent)
    {
        objectiveText = HUDText.Create(parent, "ObjectiveText", 28, TextAlignmentOptions.TopLeft,
            new Vector2(0, 1), new Vector2(0, 1), new Vector2(20, -20), new Vector2(760, 40));

        hintText = HUDText.Create(parent, "HintText", 19, TextAlignmentOptions.TopLeft,
            new Vector2(0, 1), new Vector2(0, 1), new Vector2(20, -58), new Vector2(760, 50));
        hintText.color = new Color(0.75f, 0.9f, 0.95f);

        locationText = HUDText.Create(parent, "LocationText", 30, TextAlignmentOptions.Top,
            new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0, -20), new Vector2(500, 40));
        locationText.fontStyle = FontStyles.Bold;
        locationText.color = new Color(0.35f, 0.95f, 1f);

        locationSubtitleText = HUDText.Create(parent, "LocationSubtitleText", 18, TextAlignmentOptions.Top,
            new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0, -56), new Vector2(500, 30));
        locationSubtitleText.color = new Color(0.85f, 0.85f, 0.85f);

        progressText = HUDText.Create(parent, "ProgressText", 22, TextAlignmentOptions.TopRight,
            new Vector2(1, 1), new Vector2(1, 1), new Vector2(-20, -20), new Vector2(200, 36));
        progressText.color = new Color(0.75f, 0.9f, 0.95f);

        inventoryText = HUDText.Create(parent, "InventoryText", 20, TextAlignmentOptions.TopRight,
            new Vector2(1, 1), new Vector2(1, 1), new Vector2(-20, -60), new Vector2(200, 30));
        inventoryText.color = new Color(0.75f, 0.9f, 0.95f);
        inventoryText.enabled = false;

        ObjectiveSystem.Instance.OnObjectiveChanged.AddListener(SetObjectiveText);
        ObjectiveSystem.Instance.OnHintChanged.AddListener(SetHintText);
        ObjectiveSystem.Instance.OnObjectiveCompleted.AddListener(_ => UpdateProgressText());

        SetObjectiveText(ObjectiveSystem.Instance.CurrentObjective);
        SetHintText(ObjectiveSystem.Instance.CurrentHint);
        SetLocation("", "");
        UpdateProgressText();
    }

    private void SetObjectiveText(string text)
    {
        if (objectiveText == null) return;
        objectiveText.text = "OBJETIVO ACTUAL\n" + text;
        RepositionHintBelowObjective();
        // Prompt 28: el objetivo cambia tanto en progresion normal como en un reset de
        // "Nueva Partida"/"Reiniciar" (ver GameStateManager.StartNewGame/RestartSection);
        // refrescar el contador aqui evita que quede mostrando un valor viejo (p.ej. "6/8") tras
        // un reset.
        UpdateProgressText();
    }

    private void SetHintText(string text)
    {
        if (hintText == null) return;
        hintText.text = string.IsNullOrEmpty(text) ? "" : "PISTA: " + text;
    }

    // El objetivo puede ocupar 1 o varias lineas segun su longitud (wrap), asi que la
    // pista se reubica debajo usando la altura real del texto en vez de un offset fijo,
    // evitando que ambos se superpongan cuando el objetivo es largo.
    private void RepositionHintBelowObjective()
    {
        if (hintText == null) return;
        var objectiveRT = objectiveText.rectTransform;
        var hintRT = hintText.rectTransform;
        float objectiveBottom = objectiveRT.anchoredPosition.y - objectiveText.preferredHeight;
        hintRT.anchoredPosition = new Vector2(hintRT.anchoredPosition.x, objectiveBottom - ObjectiveHintGap);
    }

    // Contador simple "X/8" de componentes comprendidos. Se actualiza con el mismo evento
    // que ya dispara el feedback de ObjectiveSystem, sin logica de progresion nueva.
    private void UpdateProgressText()
    {
        if (progressText == null) return;
        int completed = ObjectiveSystem.Instance.CompletedSteps;
        int total = ObjectiveSystem.Instance.TotalSteps;
        // Prompt 22: al llegar a 8/8 el contador se oculta (CompletedSteps/TotalSteps siguen
        // funcionando igual, solo se deja de mostrar el texto).
        bool allDone = completed >= total;
        progressText.text = allDone ? "" : completed + "/" + total;
        progressText.enabled = !allDone;
    }

    // Indicador de inventario minimalista (Prompt 20), p.ej. "RAM x2". Generico a proposito: no
    // sabe nada de RAM/mision, solo muestra el texto que le pasen; se oculta con texto vacio.
    public void SetInventoryText(string text)
    {
        if (inventoryText == null) return;
        bool show = !string.IsNullOrEmpty(text);
        inventoryText.text = text ?? "";
        inventoryText.enabled = show;
    }

    // Nombre de zona en la parte superior del HUD (una unica ubicacion activa).
    // Un nombre vacio oculta el indicador (el jugador esta fuera de cualquier zona conocida).
    public void SetLocation(string locationName, string description)
    {
        if (locationText == null) return;

        if (locationSubtitleRoutine != null)
        {
            StopCoroutine(locationSubtitleRoutine);
            locationSubtitleRoutine = null;
        }

        locationText.text = locationName ?? "";
        locationText.enabled = !string.IsNullOrEmpty(locationName);

        locationSubtitleText.text = description ?? "";
        locationSubtitleText.enabled = !string.IsNullOrEmpty(description);

        if (!string.IsNullOrEmpty(description))
            locationSubtitleRoutine = StartCoroutine(HideLocationSubtitleAfterDelay());
    }

    private IEnumerator HideLocationSubtitleAfterDelay()
    {
        yield return new WaitForSeconds(LocationDescriptionHoldTime);
        locationSubtitleText.enabled = false;
        locationSubtitleRoutine = null;
    }
}
