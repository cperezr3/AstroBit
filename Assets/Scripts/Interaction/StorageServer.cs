using UnityEngine;

// Punto de entrega/ejecucion de la mision de almacenamiento. Solo puede interactuarse una vez
// que el archivo fue encontrado en algun FileShelf (StorageMission.FileFound); antes de eso
// CanInteract es false, asi que PlayerInteraction ni siquiera lo ofrece como objetivo de mirada.
public class StorageServer : MonoBehaviour, IInteractable
{
    [Header("Identidad")]
    [SerializeField] private string labelTitle = "SERVIDOR";
    [SerializeField] private string labelSubtitle = "Procesamiento de archivos";
    [SerializeField] private string promptText = "[E] Ejecutar archivo";

    [Header("Proximidad")]
    [SerializeField] private float proximityRadius = 10f;
    [SerializeField] private float labelHeight = 2.2f;

    private bool delivered;

    public string PromptText => promptText;
    public bool CanInteract => !delivered && StorageMission.Instance.FileFound;

    private void Awake()
    {
        var label = gameObject.AddComponent<WorldLabel>();
        label.Init(transform, labelTitle, labelSubtitle, proximityRadius, labelHeight);
    }

    public void Interact()
    {
        if (!CanInteract) return;
        delivered = true;
        StorageMission.Instance.ReportFileDelivered();
    }
}
