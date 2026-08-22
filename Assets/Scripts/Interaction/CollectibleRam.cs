using UnityEngine;

// Modulo de RAM de repuesto en la bodega. Solo puede recogerse una vez que la mision detecto
// RAM insuficiente, y solo mientras sigan faltando modulos por recoger (si hay mas de los
// necesarios en la escena, los sobrantes simplemente dejan de ofrecer [E] una vez completo).
// Al recogerse se desactiva aqui; StorageMission solo lo cuenta para el inventario (Prompt 21:
// ya no se reutiliza fisicamente en ningun slot).
public class CollectibleRam : MonoBehaviour, IInteractable
{
    [SerializeField] private string promptText = "[E] Recoger RAM";

    [Header("Etiqueta (opcional, vacio = sin etiqueta)")]
    [SerializeField] private string labelTitle = "MODULO RAM DE REPUESTO";
    [SerializeField] private string labelSubtitle = "Puede instalarse en la Room RAM";
    [SerializeField] private float proximityRadius = 10f;
    [SerializeField] private float labelHeight = 2.5f;

    private bool collected;
    private WorldLabel worldLabel;

    public string PromptText => promptText;

    public bool CanInteract =>
        !collected &&
        StorageMission.Instance.RamInsufficientDetected &&
        Inventory.Instance.GetItemCount(StorageMission.RamItemId) < StorageMission.RequiredRamModules;

    private void Awake()
    {
        if (string.IsNullOrEmpty(labelTitle)) return;
        worldLabel = gameObject.AddComponent<WorldLabel>();
        worldLabel.Init(transform, labelTitle, labelSubtitle, proximityRadius, labelHeight);
    }

    public void Interact()
    {
        if (!CanInteract) return;
        collected = true;

        // Prompt 22: WorldLabel crea su etiqueta como un objeto suelto (sin padre) que sigue al
        // target por posicion; desactivar este GameObject NO la oculta por si sola, quedaria
        // flotando en el lugar donde estaba la RAM. Destruir el componente limpia tambien su
        // label (ver WorldLabel.OnDestroy).
        if (worldLabel != null) Destroy(worldLabel);

        StorageMission.Instance.ReportRamModuleCollected(gameObject);
    }
}
