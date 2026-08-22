using UnityEngine;

// Punto donde el jugador instala un modulo de RAM llevado desde el inventario. Solo disponible
// una vez detectada la RAM insuficiente y mientras el jugador tenga al menos un modulo en el
// inventario. Un solo uso por slot.
//
// Prompt 21: la representacion visual instalada es un GameObject de RAM ya colocado a mano en
// la escena (visualObject, p.ej. RAM3/RAM4) que se activa con SetActive(true) -- ya NO se
// reubica fisicamente el modulo recogido en la bodega (ese solo se desactiva al recogerlo y
// no vuelve a usarse visualmente).
public class InstallRamSlot : MonoBehaviour, IInteractable
{
    [SerializeField] private string promptText = "[E] Instalar RAM";
    [SerializeField] private GameObject visualObject;

    [Header("Etiqueta (opcional, vacio = sin etiqueta)")]
    [SerializeField] private string labelTitle = "";
    [SerializeField] private string labelSubtitle = "";
    [SerializeField] private float proximityRadius = 10f;
    [SerializeField] private float labelHeight = 1.5f;

    private bool installed;

    public string PromptText => promptText;

    public bool CanInteract =>
        !installed &&
        StorageMission.Instance.RamInsufficientDetected &&
        Inventory.Instance.HasItem(StorageMission.RamItemId);

    private void Awake()
    {
        if (string.IsNullOrEmpty(labelTitle)) return;
        var label = gameObject.AddComponent<WorldLabel>();
        label.Init(transform, labelTitle, labelSubtitle, proximityRadius, labelHeight);
    }

    public void Interact()
    {
        if (!CanInteract) return;
        installed = true;
        StorageMission.Instance.ReportRamModuleInstalled(visualObject);
    }
}
