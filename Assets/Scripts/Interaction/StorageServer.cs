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
    private EmissiveToggle glow;

    public string PromptText => promptText;
    public bool CanInteract => !delivered && StorageMission.Instance.FileFound;

    private void Awake()
    {
        var label = gameObject.AddComponent<WorldLabel>();
        // Prompt 02_continuacion (bloque post-procesado/polish): este objeto y el Tv 32 Inch
        // (que abre el archivo justo despues) estan a solo ~4 unidades de distancia, ambos con
        // proximityRadius=10 -- las dos etiquetas quedaban visibles y superpuestas en pantalla
        // casi siempre. Se ocultan una vez el archivo ya fue recuperado de aqui, momento en el
        // que la etiqueta del Tv empieza a mostrarse (ver MissionStepPoint.cs) -- nunca son
        // relevantes al mismo tiempo porque la mision los usa en secuencia estricta.
        label.Init(transform, labelTitle, labelSubtitle, proximityRadius, labelHeight,
            () => !StorageMission.Instance.FileRetrieved);

        // Prompt 02_continuacion (bloque almacenamiento): el modelo "server" (iPoly3D) ya trae un
        // submesh de vidrio sin usar (segundo slot de material) -- mismo patron que el Tv 32 Inch:
        // se enciende al entregar el archivo en vez de que la unica reaccion sea texto en el HUD.
        // GetComponentInChildren<MeshRenderer>() por si sola devolveria el MeshRenderer del
        // propio "server (37)" (un solo material, wrapper del modelo) en vez del renderer real
        // del mesh anidado ("Cube.074", 2 materiales) -- se busca explicitamente el que tenga
        // mas de un material en vez de asumir cual es el primero en la jerarquia.
        MeshRenderer glassRenderer = null;
        foreach (var candidate in GetComponentsInChildren<MeshRenderer>())
        {
            if (candidate.sharedMaterials.Length > 1) { glassRenderer = candidate; break; }
        }
        if (glassRenderer != null)
        {
            glow = gameObject.AddComponent<EmissiveToggle>();
            glow.Configure(glassRenderer, 1, new Color(0.4f, 0.85f, 1f), 3f);
        }
    }

    public void Interact()
    {
        if (!CanInteract) return;
        delivered = true;
        StorageMission.Instance.ReportFileDelivered();
        glow?.Activate();
    }
}
