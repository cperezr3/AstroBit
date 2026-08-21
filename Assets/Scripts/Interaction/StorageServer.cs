using UnityEngine;

// Representa visualmente el "almacenamiento principal" de la sala (el GameObject se llama
// "server" por el asset de terceros, pero para la narrativa educativa NO es un servidor de
// red: es simplemente donde se recupera el archivo antes de llevarlo a la computadora).
// Solo puede interactuarse una vez que el archivo fue encontrado en algun FileShelf
// (StorageMission.FileFound); antes de eso CanInteract es false, asi que PlayerInteraction
// ni siquiera lo ofrece como objetivo de mirada.
public class StorageServer : MonoBehaviour, IInteractable
{
    [Header("Identidad")]
    [SerializeField] private string labelTitle = "ALMACENAMIENTO PRINCIPAL";
    [SerializeField] private string labelSubtitle = "Representacion del disco duro";
    [SerializeField] private string promptText = "[E] Recoger archivo";

    [Header("Proximidad")]
    [SerializeField] private float proximityRadius = 10f;
    [SerializeField] private float labelHeight = 5f;

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
