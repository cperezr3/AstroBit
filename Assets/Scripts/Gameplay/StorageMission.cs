using UnityEngine;

// Mini-mision de la sala de almacenamiento (disco duro): buscar un archivo en los modulos
// Shelf with Crates, llevarlo al servidor y ejecutarlo. Reutiliza ObjectiveSystem para
// objetivo/pista/feedback exactamente igual que FinalActivity hace con su propia fase,
// sin crear un segundo sistema de objetivos paralelo.
public class StorageMission : MonoBehaviour
{
    private const string StartObjective = "Recupera el archivo de configuracion perdido.";
    private const string StartHint = "Busca en los distintos modulos de almacenamiento (Shelf) de la sala.";
    private const string FoundObjective = "Archivo encontrado. Llevalo al servidor.";
    private const string FoundHint = "Dirigete al servidor de la sala de almacenamiento.";
    private const string CompletedObjective = "Mision de almacenamiento completada.";
    private const string CompletedHint = "El archivo fue entregado y ejecutado correctamente.";

    private const string WrongShelfFeedback = "No hay nada aqui. Los datos se organizan en el almacenamiento para poder localizarse cuando el sistema los necesita.";
    private const string CorrectShelfFeedback = "Archivo encontrado.";
    private const string ServerFeedback = "Archivo ejecutado correctamente. Los servidores procesan y almacenan informacion para otros equipos.";

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

    private bool started;
    private bool completed;

    // Se llama al entrar a Zone_Storage (via StorageZoneTrigger). Idempotente.
    public void BeginIfNeeded()
    {
        if (started) return;
        started = true;
        ObjectiveSystem.Instance.SetObjective(StartObjective);
        ObjectiveSystem.Instance.SetHint(StartHint);
    }

    // Llamado por cada FileShelf al interactuar. wasCorrect indica si ese shelf en particular
    // es el que contiene el archivo (configurado en su Inspector).
    public void ReportShelfChecked(bool wasCorrect)
    {
        if (!wasCorrect || FileFound)
        {
            GameHUD.Instance?.ShowFeedback(WrongShelfFeedback);
            return;
        }

        FileFound = true;
        ObjectiveSystem.Instance.CompleteObjective(CorrectShelfFeedback);
        ObjectiveSystem.Instance.SetObjective(FoundObjective);
        ObjectiveSystem.Instance.SetHint(FoundHint);
    }

    // Llamado por StorageServer al interactuar (solo posible una vez FileFound == true).
    public void ReportFileDelivered()
    {
        if (completed) return;
        completed = true;
        ObjectiveSystem.Instance.CompleteObjective(ServerFeedback);
        ObjectiveSystem.Instance.SetObjective(CompletedObjective);
        ObjectiveSystem.Instance.SetHint(CompletedHint);
    }
}
