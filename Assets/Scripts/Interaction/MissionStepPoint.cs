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
        if (string.IsNullOrEmpty(labelTitle)) return;
        var label = gameObject.AddComponent<WorldLabel>();

        // "Procesamiento de archivo" (CpuProcess) no debe aparecer desde el inicio: solo una
        // vez que el archivo fue enviado desde el almacenamiento (ComputerOpened), siguiendo el
        // flujo Shelf -> server -> TV -> CPU. Los demas pasos no necesitan condicion extra.
        System.Func<bool> gate = step == Step.CpuProcess
            ? () => StorageMission.Instance.ComputerOpened
            : null;

        label.Init(transform, labelTitle, labelSubtitle, proximityRadius, labelHeight, gate);
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
    }
}
