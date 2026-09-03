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
    private GameObject indicatorGO;
    private Renderer indicatorRenderer;

    public string PromptText => promptText;
    // Prompt 20: no responde hasta que la mision realmente arranco (CPU/RAM aprendidos) --
    // el jugador puede entrar fisicamente a la sala antes, pero los Shelves no ofrecen [E].
    public bool CanInteract => !alreadyChecked && StorageMission.Instance.MissionStarted;

    // Prompt 02_continuacion (bloque almacenamiento): con 20 shelves identicos y ningun cambio
    // visual al revisarlos, el jugador no tenia forma de recordar cuales ya habia revisado salvo
    // volviendo a acercarse y ver que el [E] ya no aparece. El indicador nace oculto -- los
    // shelves sin revisar siguen viendose exactamente igual que antes (la razon por la que Prompt
    // 19 ya habia quitado la etiqueta flotante: 20 marcadores repetidos son ruido visual) -- y
    // solo aparece, ya con su color final, en el unico shelf que el jugador efectivamente revisa.
    private void Awake()
    {
        indicatorGO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        indicatorGO.name = "CheckedIndicator";
        indicatorGO.transform.SetParent(transform, false);
        indicatorGO.transform.localPosition = new Vector3(0f, 2.5f, 0f);
        indicatorGO.transform.localScale = Vector3.one * 0.22f;
        Destroy(indicatorGO.GetComponent<Collider>());
        indicatorRenderer = indicatorGO.GetComponent<Renderer>();
        indicatorGO.SetActive(false);
    }

    public void Interact()
    {
        if (!CanInteract) return;
        alreadyChecked = true;
        StorageMission.Instance.ReportShelfChecked(containsFile);
        ShowIndicator(containsFile);
    }

    private void ShowIndicator(bool found)
    {
        indicatorGO.SetActive(true);
        var mat = indicatorRenderer.material;
        mat.EnableKeyword("_EMISSION");
        Color c = found ? new Color(0.4f, 1f, 0.5f) : new Color(0.85f, 0.25f, 0.25f);
        mat.color = c;
        mat.SetColor("_EmissionColor", c * (found ? 3.5f : 1.2f));
    }
}
