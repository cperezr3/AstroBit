using UnityEngine;

// Punto de interaccion generico para un paso del recorrido del archivo fuera de la sala de
// almacenamiento (abrir en la "computadora", procesar en CPU, cargar/ejecutar en RAM). Cada
// paso depende del anterior a traves de StorageMission (CanInteract consulta el paso previo),
// y un solo uso por punto -- mismo criterio que FileShelf/StorageServer.
//
// Vive en un GameObject propio (no en el mismo objeto que un EducationalInteractable existente)
// para que PlayerInteraction pueda encontrarlo sin ambiguedad: GetComponentInParent<IInteractable>
// solo puede devolver un componente por objeto, y los componentes CPU/RAM ya tienen el suyo.
public class MissionStepPoint : MonoBehaviour, IInteractable
{
    public enum Step { OpenOnComputer, CpuProcess, RamLoad, RamExecute }

    [SerializeField] private Step step;
    [SerializeField] private string promptText = "[E] Interactuar";

    [Header("Etiqueta (opcional)")]
    [SerializeField] private string labelTitle = "";
    [SerializeField] private string labelSubtitle = "";
    [SerializeField] private float proximityRadius = 10f;
    [SerializeField] private float labelHeight = 2.5f;

    [Header("Feedback visual (opcional)")]
    [Tooltip("Si se asigna, se activa al completar este paso -- p.ej. encender la pantalla del Tv, o un MissionBeacon en un punto sin representacion fisica propia.")]
    [SerializeField] private StepFeedback screenToActivate;

    private bool used;

    public string PromptText => promptText;

    public bool CanInteract
    {
        get
        {
            if (used) return false;
            switch (step)
            {
                case Step.OpenOnComputer: return StorageMission.Instance.CanOpenOnComputer;
                case Step.CpuProcess: return StorageMission.Instance.CanProcessAtCpu;
                case Step.RamLoad: return StorageMission.Instance.CanAttemptRamLoad;
                case Step.RamExecute: return StorageMission.Instance.CanExecuteRam;
                default: return false;
            }
        }
    }

    private void Awake()
    {
        if (!string.IsNullOrEmpty(labelTitle))
        {
            var label = gameObject.AddComponent<WorldLabel>();

            // "Procesamiento de archivo" (CpuProcess) no debe aparecer desde el inicio: solo una
            // vez que el archivo fue enviado desde el almacenamiento (ComputerOpened), siguiendo el
            // flujo Shelf -> server -> TV -> CPU. Tampoco debe quedar flotando despues de procesar
            // (CpuProcessed ya en true) -- sin el !CpuProcessed, ComputerOpened se queda en true
            // para siempre y la etiqueta nunca se ocultaba tras presionar [E].
            //
            // "Interfaz para abrir archivos" (OpenOnComputer, el Tv) tiene el mismo problema con
            // StorageServer: estan a solo ~4 unidades de distancia con proximityRadius=10 cada uno,
            // asi que ambas etiquetas quedaban visibles y superpuestas en pantalla casi siempre que
            // el jugador estuviera cerca de cualquiera de los dos. En vez de recortar el radio (que
            // seguiria solapando salvo un radio artificialmente pequeño), se usa el mismo mecanismo
            // de gate: el Tv solo tiene sentido mostrarse una vez el archivo fue recuperado del
            // almacenamiento, momento en el que StorageServer.cs ya oculta la suya (ver ese archivo)
            // -- nunca son relevantes al mismo tiempo porque la mision los usa en secuencia estricta.
            System.Func<bool> gate = step switch
            {
                Step.CpuProcess => () => StorageMission.Instance.ComputerOpened && !StorageMission.Instance.CpuProcessed,
                Step.OpenOnComputer => () => StorageMission.Instance.FileRetrieved && !StorageMission.Instance.ComputerOpened,
                _ => null
            };

            label.Init(transform, labelTitle, labelSubtitle, proximityRadius, labelHeight, gate);
        }

        RefreshVisualState();
    }

    // Prompt 04_implement (data flow, seccion 7): llamar esto solo desde Awake() NO alcanza para
    // el caso real de "cargar partida desde el Main Menu" -- SceneManager.LoadScene(...) corre (y
    // con el, TODOS los Awake() de la escena) antes de que MainMenuController.Continuar() pueda
    // siquiera llamar a SaveManager.Instance.LoadGame(). En un arranque nuevo del juego,
    // StorageMission todavia tiene sus valores por defecto en el momento de este Awake(); recien
    // se restaura despues. Por eso SaveManager.LoadGame() vuelve a llamar a este metodo
    // explicitamente tras restaurar StorageMission (mismo patron ya usado ahi para
    // FinalActivity.ResumeIfInProgress()). Publico e idempotente a proposito (screenToActivate ya
    // se protege a si mismo contra doble activacion).
    public void RefreshVisualState()
    {
        if (!IsAlreadyCompleted()) return;
        used = true;
        screenToActivate?.Activate();
    }

    private bool IsAlreadyCompleted()
    {
        switch (step)
        {
            case Step.OpenOnComputer: return StorageMission.Instance.ComputerOpened;
            case Step.CpuProcess: return StorageMission.Instance.CpuProcessed;
            case Step.RamLoad: return StorageMission.Instance.RamInsufficientDetected;
            case Step.RamExecute: return StorageMission.Instance.RamExecuted;
            default: return false;
        }
    }

    public void Interact()
    {
        if (!CanInteract) return;
        used = true;

        switch (step)
        {
            case Step.OpenOnComputer: StorageMission.Instance.ReportOpenedOnComputer(); break;
            case Step.CpuProcess: StorageMission.Instance.ReportCpuProcessed(); break;
            case Step.RamLoad: StorageMission.Instance.ReportRamLoadAttempt(); break;
            case Step.RamExecute: StorageMission.Instance.ReportRamExecuted(); break;
        }

        screenToActivate?.Activate();
    }
}
