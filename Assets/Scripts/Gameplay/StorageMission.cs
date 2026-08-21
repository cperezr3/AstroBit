using UnityEngine;

// Mision extendida del recorrido del archivo (Prompt 19): almacenamiento -> recuperarlo ->
// abrirlo en la "computadora" (Tv 32 Inch) -> procesarlo en la CPU -> cargarlo/ejecutarlo en
// la RAM -> Actividad Final. Reutiliza ObjectiveSystem para objetivo/pista/feedback exactamente
// igual que FinalActivity hace con su propia fase; no crea un sistema de objetivos paralelo.
//
// IMPORTANTE (Parte 3 del prompt 19): el GameObject "server" es unicamente la representacion
// visual del almacenamiento/disco duro para esta mision -- no es un servidor de red real, y el
// texto de aqui evita deliberadamente vocabulario de redes (clientes, peticiones, protocolos).
public class StorageMission : MonoBehaviour
{
    private const string SearchObjective = "Busca el archivo que necesitamos abrir.";
    private const string SearchHint = "Explora los distintos modulos de almacenamiento (Shelf) de la sala.";

    private const string FoundFeedback = "¡Archivo encontrado!";
    private const string FoundObjective = "Lleva el archivo al almacenamiento principal.";
    private const string FoundHint = "Dirigete a la unidad de almacenamiento principal de la sala.";

    private const string RetrievedFeedback = "Archivo recuperado. Ahora debemos abrirlo en la computadora.";
    private const string RetrievedObjective = "Utiliza la computadora para abrir el archivo.";
    private const string RetrievedHint = "Busca la pantalla junto a la zona de almacenamiento.";

    private const string OpenedFeedback = "Enviando archivo a la CPU...";
    private const string OpenedObjective = "El archivo llego a la CPU. Inicia su procesamiento.";
    private const string OpenedHint = "Ve a la Room CPU y busca la Unidad de Control.";

    private const string ProcessedFeedback = "El archivo ha sido procesado. Ahora debe cargarse en la memoria RAM.";
    private const string ProcessedObjective = "El programa esta listo para cargarse en memoria. Ve a la RAM.";
    private const string ProcessedHint = "Ve a la Room RAM.";

    private const string LoadedFeedback = "El programa esta ahora cargado en memoria RAM. La RAM mantiene temporalmente los datos y programas en uso mientras se ejecutan.";
    private const string LoadedObjective = "El programa esta cargado. Ejecuta el archivo.";
    private const string LoadedHint = "Busca el otro modulo de RAM para ejecutarlo.";

    private const string ExecutedFeedback = "Programa ejecutado correctamente.";

    private static readonly string[] WrongShelfFeedbacks =
    {
        "No se encuentra el archivo aqui.",
        "Solo hay datos antiguos en este modulo.",
        "Este modulo no contiene el archivo solicitado.",
    };

    private static StorageMission _instance;
    public static StorageMission Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("StorageMission");
                _instance = go.AddComponent<StorageMission>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    public bool FileFound { get; private set; }
    public bool FileRetrieved { get; private set; }
    public bool ComputerOpened { get; private set; }
    public bool CpuProcessed { get; private set; }
    public bool RamLoaded { get; private set; }

    public bool CanOpenOnComputer => FileRetrieved && !ComputerOpened;
    public bool CanProcessAtCpu => ComputerOpened && !CpuProcessed;
    public bool CanLoadRam => CpuProcessed && !RamLoaded;
    public bool CanExecuteRam => RamLoaded && !ramExecuted;

    private bool started;
    private bool ramExecuted;
    private int wrongShelfIndex;

    // Se llama al entrar a Zone_Storage (via StorageZoneTrigger). Idempotente.
    public void BeginIfNeeded()
    {
        if (started) return;
        started = true;
        ObjectiveSystem.Instance.SetObjective(SearchObjective);
        ObjectiveSystem.Instance.SetHint(SearchHint);
    }

    // Llamado por cada FileShelf al interactuar. wasCorrect indica si ese shelf en particular
    // es el que contiene el archivo (configurado en su Inspector).
    public void ReportShelfChecked(bool wasCorrect)
    {
        if (!wasCorrect || FileFound)
        {
            string msg = WrongShelfFeedbacks[wrongShelfIndex % WrongShelfFeedbacks.Length];
            wrongShelfIndex++;
            GameHUD.Instance?.ShowFeedback(msg);
            return;
        }

        FileFound = true;
        ObjectiveSystem.Instance.CompleteObjective(FoundFeedback);
        ObjectiveSystem.Instance.SetObjective(FoundObjective);
        ObjectiveSystem.Instance.SetHint(FoundHint);
    }

    // Llamado por StorageServer (la representacion visual del almacenamiento principal).
    public void ReportFileDelivered()
    {
        if (!FileFound || FileRetrieved) return;
        FileRetrieved = true;
        ObjectiveSystem.Instance.CompleteObjective(RetrievedFeedback);
        ObjectiveSystem.Instance.SetObjective(RetrievedObjective);
        ObjectiveSystem.Instance.SetHint(RetrievedHint);
    }

    // Llamado por el MissionStepPoint de la Tv 32 Inch.
    public void ReportOpenedOnComputer()
    {
        if (!CanOpenOnComputer) return;
        ComputerOpened = true;
        ObjectiveSystem.Instance.CompleteObjective(OpenedFeedback);
        ObjectiveSystem.Instance.SetObjective(OpenedObjective);
        ObjectiveSystem.Instance.SetHint(OpenedHint);
    }

    // Llamado por el MissionStepPoint junto a la Unidad de Control.
    public void ReportCpuProcessed()
    {
        if (!CanProcessAtCpu) return;
        CpuProcessed = true;
        ObjectiveSystem.Instance.CompleteObjective(ProcessedFeedback);
        ObjectiveSystem.Instance.SetObjective(ProcessedObjective);
        ObjectiveSystem.Instance.SetHint(ProcessedHint);
    }

    // Llamado por el MissionStepPoint junto a RAM1.
    public void ReportRamLoaded()
    {
        if (!CanLoadRam) return;
        RamLoaded = true;
        ObjectiveSystem.Instance.CompleteObjective(LoadedFeedback);
        ObjectiveSystem.Instance.SetObjective(LoadedObjective);
        ObjectiveSystem.Instance.SetHint(LoadedHint);
    }

    // Llamado por el MissionStepPoint junto a RAM2. Dispara la Actividad Final existente
    // reutilizando el mismo evento que ya usa el recorrido original de 8 piezas CPU/RAM.
    public void ReportRamExecuted()
    {
        if (!CanExecuteRam) return;
        ramExecuted = true;
        ObjectiveSystem.Instance.CompleteObjective(ExecutedFeedback);
        ObjectiveSystem.Instance.OnAllStepsCompleted.Invoke();
    }
}
