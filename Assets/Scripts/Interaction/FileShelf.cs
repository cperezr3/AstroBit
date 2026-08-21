using UnityEngine;

// Punto de busqueda de la mision de almacenamiento: un modulo "Shelf with Crates" que puede
// contener o no el archivo buscado (configurable desde Inspector). Un solo interactuar, igual
// que el resto de interactuables; toda la logica de la mision vive en StorageMission.
public class FileShelf : MonoBehaviour, IInteractable
{
    [Header("Identidad")]
    [SerializeField] private string labelTitle = "ALMACENAMIENTO";
    [SerializeField] private string labelSubtitle = "Modulo de archivos";
    [SerializeField] private string promptText = "[E] Buscar";

    [Header("Mision")]
    [Tooltip("Marcar en un unico Shelf de toda la sala: el que contiene el archivo buscado.")]
    [SerializeField] private bool containsFile = false;

    [Header("Proximidad")]
    [SerializeField] private float proximityRadius = 10f;
    [SerializeField] private float labelHeight = 2f;

    private bool alreadyChecked;

    public string PromptText => promptText;
    public bool CanInteract => !alreadyChecked;

    private void Awake()
    {
        var label = gameObject.AddComponent<WorldLabel>();
        label.Init(transform, labelTitle, labelSubtitle, proximityRadius, labelHeight);
    }

    public void Interact()
    {
        if (!CanInteract) return;
        alreadyChecked = true;
        StorageMission.Instance.ReportShelfChecked(containsFile);
    }
}
