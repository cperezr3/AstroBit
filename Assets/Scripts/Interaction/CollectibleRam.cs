using UnityEngine;

// Modulo de RAM de repuesto en la bodega. Solo puede recogerse una vez que la mision detecto
// RAM insuficiente, y solo mientras sigan faltando modulos por recoger (si hay mas de los
// necesarios en la escena, los sobrantes simplemente dejan de ofrecer [E] una vez completo).
// Al recogerse se desactiva aqui y StorageMission lo reubica fisicamente en un slot al instalarlo.
public class CollectibleRam : MonoBehaviour, IInteractable
{
    [SerializeField] private string promptText = "[E] Recoger RAM";
    [SerializeField] private string labelTitle = "MODULO RAM DE REPUESTO";
    [SerializeField] private string labelSubtitle = "Puede instalarse en la Room RAM";
    [SerializeField] private float proximityRadius = 10f;
    [SerializeField] private float labelHeight = 2.5f;

    private bool collected;

    public string PromptText => promptText;

    public bool CanInteract =>
        !collected &&
        StorageMission.Instance.RamInsufficientDetected &&
        Inventory.Instance.GetItemCount(StorageMission.RamItemId) < StorageMission.RequiredRamModules;

    private void Awake()
    {
        var label = gameObject.AddComponent<WorldLabel>();
        label.Init(transform, labelTitle, labelSubtitle, proximityRadius, labelHeight);
    }

    public void Interact()
    {
        if (!CanInteract) return;
        collected = true;
        StorageMission.Instance.ReportRamModuleCollected(gameObject);
    }
}
