using UnityEngine;

// Slot vacio en la Room RAM donde el jugador instala un modulo de RAM llevado desde el
// inventario. Solo disponible una vez detectada la RAM insuficiente y mientras el jugador
// tenga al menos un modulo en el inventario. Un solo uso por slot.
public class InstallRamSlot : MonoBehaviour, IInteractable
{
    [SerializeField] private string promptText = "[E] Instalar RAM";
    [SerializeField] private string labelTitle = "RAM SLOT";
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
        var label = gameObject.AddComponent<WorldLabel>();
        label.Init(transform, labelTitle, labelSubtitle, proximityRadius, labelHeight);
    }

    public void Interact()
    {
        if (!CanInteract) return;
        installed = true;
        StorageMission.Instance.ReportRamModuleInstalled(transform);
    }
}
