using UnityEngine;

// Punto de busqueda de la mision de almacenamiento: un modulo "Shelf with Crates" que puede
// contener o no el archivo buscado (configurable desde Inspector). Un solo interactuar; toda
// la logica de la mision vive en StorageMission.
//
// Prompt 19 (Parte 1): sin etiqueta flotante -- solo el [E] contextual que ya maneja
// PlayerInteraction/GameHUD. Antes se usaba WorldLabel aqui; se elimino porque tener 22
// etiquetas "ALMACENAMIENTO" repetidas por la sala no aportaba nada y resultaba visualmente
// ruidoso.
public class FileShelf : MonoBehaviour, IInteractable
{
    [Header("Identidad")]
    [SerializeField] private string promptText = "[E] Buscar";

    [Header("Mision")]
    [Tooltip("Marcar en un unico Shelf de toda la sala: el que contiene el archivo buscado.")]
    [SerializeField] private bool containsFile = false;

    private bool alreadyChecked;

    public string PromptText => promptText;
    // Prompt 20: no responde hasta que la mision realmente arranco (CPU/RAM aprendidos) --
    // el jugador puede entrar fisicamente a la sala antes, pero los Shelves no ofrecen [E].
    public bool CanInteract => !alreadyChecked && StorageMission.Instance.MissionStarted;

    public void Interact()
    {
        if (!CanInteract) return;
        alreadyChecked = true;
        StorageMission.Instance.ReportShelfChecked(containsFile);
    }
}
