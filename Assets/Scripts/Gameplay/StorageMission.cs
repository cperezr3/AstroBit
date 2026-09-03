using UnityEngine;

// Mision extendida del recorrido del archivo (Prompts 18-20): CPU/RAM educativos ->
// almacenamiento -> recuperar archivo -> abrirlo en la "computadora" (Tv 32 Inch) ->
// procesarlo en la CPU -> intentar cargarlo en RAM -> RAM insuficiente -> bodega ->
// recoger e instalar 2 modulos de repuesto -> ejecutar -> Actividad Final.
//
// Reutiliza ObjectiveSystem para objetivo/pista/feedback (igual que FinalActivity hace con
// su propia fase) e Inventory para la cuenta de modulos de RAM recogidos; no crea un sistema
// de objetivos ni de progresion paralelo.
//
// IMPORTANTE (heredado del Prompt 19): el GameObject "server" es unicamente la representacion
// visual del almacenamiento/disco duro -- no es un servidor de red real.
public class StorageMission : MonoBehaviour
{
    public const string RamItemId = "RAM";
    public const int RequiredRamModules = 2;

    private static readonly string[] CpuKeys =
        { "ALU", "REGISTROS", "UNIDAD_CONTROL", "CACHE_L1", "CACHE_L2", "CACHE_L3" };

    private const string CpuLearnedObjective = "Has conocido los principales componentes del procesador.";
    private const string CpuLearnedHint = "Ahora aprende como funciona la memoria RAM.";

    private const string RamLearnedObjective = "Ya conoces la memoria RAM. Ahora explora el sistema de almacenamiento.";
    private const string RamLearnedHint = "Busca la sala de almacenamiento del laboratorio.";
    private const string NotReadyForStorageFeedback = "Primero debes conocer los componentes de la CPU y la memoria RAM.";

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

    private const string InsufficientFeedback =
        "DIAGNOSTICO\n\nMemoria RAM insuficiente.\n\nDisponible: 2 GB\nNecesaria: 4 GB\n\n" +
        "El archivo necesita mas memoria RAM para poder cargarse.";
    private const string InsufficientObjective = "La memoria disponible no es suficiente. Busca modulos de RAM de repuesto en la bodega.";
    private const string InsufficientHint = "Ve a la bodega y busca modulos de RAM adicionales.";

    private const string CollectedObjective = "Has encontrado los modulos de RAM. Instalalos en la computadora.";
    private const string CollectedHint = "Vuelve a la Room RAM e instalalos en los slots disponibles.";

    private const string MemoryExpandedFeedback =
        "MEMORIA AMPLIADA\n\nLos modulos de RAM fueron instalados correctamente. El sistema dispone ahora de memoria suficiente.";
    private const string ExecuteReadyObjective = "La memoria ya es suficiente. Ejecuta el programa.";
    private const string ExecuteReadyHint = "Vuelve al punto de ejecucion en la Room RAM.";

    private const string ExecutedFeedback = "Programa ejecutado correctamente.";

    private static readonly string[] WrongShelfFeedbacks =
    {
        "No se encuentra el archivo aqui.",
        "Solo hay datos antiguos en este modulo.",
        "Este modulo no contiene el archivo solicitado.",
    };

    // Fuerza la creacion temprana (mismo patron que FinalActivity/GameHUD): StorageMission debe
    // registrar su listener de OnAllStepsCompleted ANTES de que el jugador pueda completar los
    // 8 pasos CPU/RAM, o el evento se dispara sin nadie escuchando y CpuRamLearned nunca pasa a
    // true. Con el singleton puramente perezoso de antes, esto fallaba si el jugador completaba
    // CPU/RAM sin haber tocado antes ninguna interaccion de la sala de almacenamiento.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        _ = Instance;
    }

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
                ObjectiveSystem.Instance.OnObjectiveCompleted.AddListener(_instance.OnAnyPieceCompleted);
                ObjectiveSystem.Instance.OnAllStepsCompleted.AddListener(_instance.OnCpuRamLearned);
            }
            return _instance;
        }
    }

    public bool CpuRamLearned { get; private set; }
    public bool FileFound { get; private set; }
    public bool FileRetrieved { get; private set; }
    public bool ComputerOpened { get; private set; }
    public bool CpuProcessed { get; private set; }
    public bool RamInsufficientDetected { get; private set; }
    public bool RamModulesFullyInstalled { get; private set; }

    public bool MissionStarted => started;
    public bool CanOpenOnComputer => FileRetrieved && !ComputerOpened;
    public bool CanProcessAtCpu => ComputerOpened && !CpuProcessed;
    public bool CanAttemptRamLoad => CpuProcessed && !ramLoadAttempted;
    public bool CanExecuteRam => RamModulesFullyInstalled && !ramExecuted;

    private bool started;
    private bool cpuLearnedShown;
    private bool ramLoadAttempted;
    private bool ramExecuted;
    private int wrongShelfIndex;
    private int ramModulesInstalled;

    // Fase 2 (Prompt 35, 9.1): expuestos solo para SaveManager -- mismos campos que ya
    // controlan CanInteract/CanOpenOnComputer/etc, sin duplicar el estado en otro lado.
    public bool Started => started;
    public bool CpuLearnedShown => cpuLearnedShown;
    public bool RamLoadAttempted => ramLoadAttempted;
    public bool RamExecuted => ramExecuted;
    public int WrongShelfIndex => wrongShelfIndex;
    public int RamModulesInstalled => ramModulesInstalled;

    // Detecta hitos dentro de los 8 pasos educativos de CPU/RAM (sin esperar a los 8/8) para
    // avisar en cuanto se completa el subconjunto CPU. RAM completo ya lo cubre OnCpuRamLearned.
    private void OnAnyPieceCompleted(string _)
    {
        if (cpuLearnedShown || CpuRamLearned) return;

        for (int i = 0; i < CpuKeys.Length; i++)
        {
            if (!ObjectiveSystem.Instance.IsKeyAchieved(CpuKeys[i])) return;
        }

        cpuLearnedShown = true;
        ObjectiveSystem.Instance.SetObjective(CpuLearnedObjective);
        ObjectiveSystem.Instance.SetHint(CpuLearnedHint);
    }

    // Se dispara cuando los 8 pasos CPU/RAM estan completos (ya no abre FinalActivity
    // directamente -- Prompt 20 desplaza ese disparo al final del recorrido completo).
    private void OnCpuRamLearned()
    {
        CpuRamLearned = true;
        ObjectiveSystem.Instance.SetObjective(RamLearnedObjective);
        ObjectiveSystem.Instance.SetHint(RamLearnedHint);
    }

    // Se llama al entrar a Zone_Storage (via StorageZoneTrigger). No hace nada si CPU/RAM
    // todavia no se aprendieron (el jugador puede entrar fisicamente, pero la mision no arranca).
    public void BeginIfNeeded()
    {
        if (started) return;
        if (!CpuRamLearned)
        {
            GameHUD.Instance?.ShowFeedback(NotReadyForStorageFeedback);
            return;
        }
        started = true;
        ObjectiveSystem.Instance.SetObjective(SearchObjective);
        ObjectiveSystem.Instance.SetHint(SearchHint);
    }

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

    public void ReportFileDelivered()
    {
        if (!FileFound || FileRetrieved) return;
        FileRetrieved = true;
        ObjectiveSystem.Instance.CompleteObjective(RetrievedFeedback);
        ObjectiveSystem.Instance.SetObjective(RetrievedObjective);
        ObjectiveSystem.Instance.SetHint(RetrievedHint);
    }

    public void ReportOpenedOnComputer()
    {
        if (!CanOpenOnComputer) return;
        ComputerOpened = true;
        ObjectiveSystem.Instance.CompleteObjective(OpenedFeedback);
        ObjectiveSystem.Instance.SetObjective(OpenedObjective);
        ObjectiveSystem.Instance.SetHint(OpenedHint);
    }

    public void ReportCpuProcessed()
    {
        if (!CanProcessAtCpu) return;
        CpuProcessed = true;
        ObjectiveSystem.Instance.CompleteObjective(ProcessedFeedback);
        ObjectiveSystem.Instance.SetObjective(ProcessedObjective);
        ObjectiveSystem.Instance.SetHint(ProcessedHint);
    }

    // Intento de cargar el programa en RAM: siempre revela memoria insuficiente la primera vez
    // (por eso el punto queda "usado" tras esto) y desbloquea la busqueda de repuestos.
    public void ReportRamLoadAttempt()
    {
        if (!CanAttemptRamLoad) return;
        ramLoadAttempted = true;
        RamInsufficientDetected = true;
        // +2s respecto a la duracion normal de feedback: el diagnostico tiene mas texto que
        // leer que un mensaje corto y desaparecia demasiado rapido.
        float duration = (GameHUD.Instance != null ? GameHUD.Instance.FeedbackDuration : 3.5f) + 2f;
        GameHUD.Instance?.ShowFeedback(InsufficientFeedback, duration);
        ObjectiveSystem.Instance.SetObjective(InsufficientObjective);
        ObjectiveSystem.Instance.SetHint(InsufficientHint);
    }

    // Llamado por cada CollectibleRam recogido en la bodega.
    public void ReportRamModuleCollected(GameObject moduleObject)
    {
        if (!RamInsufficientDetected) return;

        // El modulo recogido solo representa haberlo obtenido; no se reutiliza visualmente
        // (la instalacion activa RAM3/RAM4, ya colocadas a mano -- ver ReportRamModuleInstalled).
        moduleObject.SetActive(false);
        Inventory.Instance.AddItem(RamItemId);
        int count = Inventory.Instance.GetItemCount(RamItemId);
        UpdateInventoryDisplay();
        GameHUD.Instance?.ShowFeedback("RAM x" + count);

        if (count >= RequiredRamModules)
        {
            ObjectiveSystem.Instance.SetObjective(CollectedObjective);
            ObjectiveSystem.Instance.SetHint(CollectedHint);
        }
    }

    // Llamado por cada InstallRamSlot en la Room RAM. Consume una RAM del inventario y activa
    // la representacion visual ya colocada a mano en la escena (RAM3/RAM4). Prompt 21: ya no
    // reubica el modulo recogido en la bodega -- ese solo se desactiva al recogerlo y no se
    // vuelve a usar visualmente.
    public void ReportRamModuleInstalled(GameObject visualObject)
    {
        if (!Inventory.Instance.RemoveItem(RamItemId)) return;

        if (visualObject != null) visualObject.SetActive(true);

        ramModulesInstalled++;
        UpdateInventoryDisplay();

        if (ramModulesInstalled >= RequiredRamModules)
        {
            RamModulesFullyInstalled = true;
            ObjectiveSystem.Instance.CompleteObjective(MemoryExpandedFeedback);
            ObjectiveSystem.Instance.SetObjective(ExecuteReadyObjective);
            ObjectiveSystem.Instance.SetHint(ExecuteReadyHint);
        }
        else
        {
            GameHUD.Instance?.ShowFeedback("RAM instalada: " + ramModulesInstalled + "/" + RequiredRamModules);
        }
    }

    // Punto de ejecucion final (RAM2). Ahora depende de RamModulesFullyInstalled en vez del
    // intento de carga original; dispara FinalActivity directamente (ver FinalActivity.cs).
    public void ReportRamExecuted()
    {
        if (!CanExecuteRam) return;
        ramExecuted = true;
        ObjectiveSystem.Instance.CompleteObjective(ExecutedFeedback);
        FinalActivity.Instance.BeginFinalActivity();
    }

    // Prompt 28: unico punto de reinicio para "Nueva Partida"/"Reiniciar" (ver GameSession).
    // Aditivo -- repone todos los campos de progresion al estado que tendria una instancia
    // recien creada; no cambia ninguna logica de las demas llamadas publicas.
    public void ResetState()
    {
        CpuRamLearned = false;
        FileFound = false;
        FileRetrieved = false;
        ComputerOpened = false;
        CpuProcessed = false;
        RamInsufficientDetected = false;
        RamModulesFullyInstalled = false;

        started = false;
        cpuLearnedShown = false;
        ramLoadAttempted = false;
        ramExecuted = false;
        wrongShelfIndex = 0;
        ramModulesInstalled = 0;
    }

    // Contraparte de RestoreState de un guardado (Fase 2, Prompt 35, 9.1). A diferencia de
    // ResetState, repone los campos exactos de la partida guardada y vuelve a fijar el
    // objetivo/pista correspondientes al punto real donde el jugador se quedo -- ObjectiveSystem
    // ya fue restaurado antes por SaveManager, pero solo sabe de los 8 pasos CPU/RAM; de aqui en
    // adelante (recorrido del archivo) esta clase es la unica fuente de verdad del objetivo
    // visible, igual que ya lo es en el flujo normal (ver cada Report*).
    public void RestoreState(bool cpuRamLearned, bool fileFound, bool fileRetrieved, bool computerOpened,
        bool cpuProcessed, bool ramInsufficientDetected, bool ramModulesFullyInstalled,
        bool started, bool cpuLearnedShown, bool ramLoadAttempted, bool ramExecuted,
        int wrongShelfIndex, int ramModulesInstalled)
    {
        CpuRamLearned = cpuRamLearned;
        FileFound = fileFound;
        FileRetrieved = fileRetrieved;
        ComputerOpened = computerOpened;
        CpuProcessed = cpuProcessed;
        RamInsufficientDetected = ramInsufficientDetected;
        RamModulesFullyInstalled = ramModulesFullyInstalled;

        this.started = started;
        this.cpuLearnedShown = cpuLearnedShown;
        this.ramLoadAttempted = ramLoadAttempted;
        this.ramExecuted = ramExecuted;
        this.wrongShelfIndex = wrongShelfIndex;
        this.ramModulesInstalled = ramModulesInstalled;

        UpdateInventoryDisplay();

        if (!started) return; // aun no entro a Zone_Storage; el objetivo de ObjectiveSystem ya es correcto.

        if (ramExecuted)
        {
            ObjectiveSystem.Instance.SetObjective("Actividad final: repasa el flujo completo.");
            ObjectiveSystem.Instance.SetHint("Responde correctamente cada pregunta para terminar el recorrido.");
        }
        else if (RamModulesFullyInstalled)
        {
            ObjectiveSystem.Instance.SetObjective(ExecuteReadyObjective);
            ObjectiveSystem.Instance.SetHint(ExecuteReadyHint);
        }
        else if (RamInsufficientDetected)
        {
            ObjectiveSystem.Instance.SetObjective(CollectedObjective);
            ObjectiveSystem.Instance.SetHint(CollectedHint);
        }
        else if (CpuProcessed)
        {
            ObjectiveSystem.Instance.SetObjective(ProcessedObjective);
            ObjectiveSystem.Instance.SetHint(ProcessedHint);
        }
        else if (ComputerOpened)
        {
            ObjectiveSystem.Instance.SetObjective(OpenedObjective);
            ObjectiveSystem.Instance.SetHint(OpenedHint);
        }
        else if (FileRetrieved)
        {
            ObjectiveSystem.Instance.SetObjective(RetrievedObjective);
            ObjectiveSystem.Instance.SetHint(RetrievedHint);
        }
        else if (FileFound)
        {
            ObjectiveSystem.Instance.SetObjective(FoundObjective);
            ObjectiveSystem.Instance.SetHint(FoundHint);
        }
        else
        {
            ObjectiveSystem.Instance.SetObjective(SearchObjective);
            ObjectiveSystem.Instance.SetHint(SearchHint);
        }
    }

    private void UpdateInventoryDisplay()
    {
        int count = Inventory.Instance.GetItemCount(RamItemId);
        GameHUD.Instance?.SetInventoryText(count > 0 ? "RAM x" + count : "");
    }
}
