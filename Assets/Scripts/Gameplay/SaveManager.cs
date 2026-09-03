using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Fase 2 (Prompt 35, 9.1): datos serializables de una partida guardada. Formato plano (listas
// paralelas en vez de Dictionary porque JsonUtility no serializa Dictionary) para que agregar un
// campo en una version futura sea un simple append, no una migracion de estructura.
[Serializable]
public class SaveData
{
    public int version = SaveManager.SaveFormatVersion;

    // ObjectiveSystem (progresion educativa CPU/RAM, 8 pasos)
    public int objectiveIndex;
    public bool objectiveSequenceStarted;
    public List<string> achievedKeys = new List<string>();

    // StorageMission (recorrido del archivo)
    public bool cpuRamLearned;
    public bool fileFound;
    public bool fileRetrieved;
    public bool computerOpened;
    public bool cpuProcessed;
    public bool ramInsufficientDetected;
    public bool ramModulesFullyInstalled;
    public bool storageStarted;
    public bool cpuLearnedShown;
    public bool ramLoadAttempted;
    public bool ramExecuted;
    public int wrongShelfIndex;
    public int ramModulesInstalled;

    // FinalActivity (actividad final de 4 preguntas)
    public bool finalActivityStarted;
    public int finalActivityQuestionIndex;

    // Inventory (listas paralelas: inventoryKeys[i] <-> inventoryValues[i])
    public List<string> inventoryKeys = new List<string>();
    public List<int> inventoryValues = new List<int>();
}

// Capa fina de persistencia: NO duplica el estado del juego, solo lee/escribe a traves de
// ObjectiveSystem/StorageMission/Inventory/FinalActivity (la fuente de verdad real, ya
// DontDestroyOnLoad desde antes de este prompt). JSON en Application.persistentDataPath (fuera
// de Assets/, como pide el prompt) para que sobreviva a cerrar el juego y a builds futuros.
public class SaveManager : MonoBehaviour
{
    public const int SaveFormatVersion = 1;
    private const string SaveFileName = "astrobit_save.json";

    // Fuerza la creacion temprana (mismo motivo que StorageMission/FinalActivity): si nadie mas
    // pide Instance antes de que el jugador complete el primer paso educativo, el autoguardado
    // de OnObjectiveCompleted no tendria listener registrado todavia y se perderia ese hito.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        _ = Instance;
    }

    private static SaveManager _instance;
    public static SaveManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("SaveManager");
                _instance = go.AddComponent<SaveManager>();
                DontDestroyOnLoad(go);
                // Guardado automatico al completar cualquier hito de progreso (Prompt 35:
                // "guardado al completar objetivos importantes"). Un ShowFeedback/Complete es un
                // evento raro (segundos entre uno y otro, nunca por frame), asi que escribir a
                // disco aqui no cuesta nada perceptible.
                ObjectiveSystem.Instance.OnObjectiveCompleted.AddListener(_ => _instance.SaveGame());
            }
            return _instance;
        }
    }

    private static string SavePath => Path.Combine(Application.persistentDataPath, SaveFileName);

    // Cachea el resultado de File.Exists (se llama en cada refresco del boton "Continuar" del
    // Main Menu) e invalida la cache solo cuando este mismo proceso escribe o borra el archivo --
    // evita tocar disco en cada frame en el que el Main Menu este visible.
    private bool? hasSaveCache;

    public bool HasSave
    {
        get
        {
            hasSaveCache ??= File.Exists(SavePath);
            return hasSaveCache.Value;
        }
    }

    // Guardado manual/por hito (Prompt 35: "NO quiero guardados constantes ni operaciones
    // costosas durante cada frame"). Se llama desde eventos puntuales (objetivo completado,
    // pausa, volver al menu), nunca desde Update().
    public void SaveGame()
    {
        var data = new SaveData
        {
            version = SaveFormatVersion,
            objectiveIndex = ObjectiveSystem.Instance.CurrentIndex,
            objectiveSequenceStarted = ObjectiveSystem.Instance.SequenceStarted,
            achievedKeys = new List<string>(ObjectiveSystem.Instance.AchievedKeys),

            cpuRamLearned = StorageMission.Instance.CpuRamLearned,
            fileFound = StorageMission.Instance.FileFound,
            fileRetrieved = StorageMission.Instance.FileRetrieved,
            computerOpened = StorageMission.Instance.ComputerOpened,
            cpuProcessed = StorageMission.Instance.CpuProcessed,
            ramInsufficientDetected = StorageMission.Instance.RamInsufficientDetected,
            ramModulesFullyInstalled = StorageMission.Instance.RamModulesFullyInstalled,
            storageStarted = StorageMission.Instance.Started,
            cpuLearnedShown = StorageMission.Instance.CpuLearnedShown,
            ramLoadAttempted = StorageMission.Instance.RamLoadAttempted,
            ramExecuted = StorageMission.Instance.RamExecuted,
            wrongShelfIndex = StorageMission.Instance.WrongShelfIndex,
            ramModulesInstalled = StorageMission.Instance.RamModulesInstalled,

            finalActivityStarted = FinalActivity.Instance != null && FinalActivity.Instance.Started,
            finalActivityQuestionIndex = FinalActivity.Instance != null ? FinalActivity.Instance.QuestionIndex : 0,
        };

        foreach (var kv in Inventory.Instance.GetAllCounts())
        {
            data.inventoryKeys.Add(kv.Key);
            data.inventoryValues.Add(kv.Value);
        }

        try
        {
            string json = JsonUtility.ToJson(data, true);
            string tempPath = SavePath + ".tmp";
            File.WriteAllText(tempPath, json);
            // Escritura casi-atomica: si el proceso muere entre WriteAllText y Replace, el
            // guardado anterior (o ninguno) sigue intacto en vez de un JSON a medio escribir.
            if (File.Exists(SavePath)) File.Replace(tempPath, SavePath, null);
            else File.Move(tempPath, SavePath);
            hasSaveCache = true;
        }
        catch (Exception e)
        {
            Debug.LogWarning("SaveManager: no se pudo guardar la partida. " + e.Message);
        }
    }

    // Nunca lanza: una partida guardada inexistente, corrupta o de una version futura no debe
    // impedir que AstroBit inicie (requisito explicito del Prompt 35, 9.1 "Robustez").
    public bool LoadGame()
    {
        if (!HasSave) return false;

        SaveData data;
        try
        {
            string json = File.ReadAllText(SavePath);
            data = JsonUtility.FromJson<SaveData>(json);
            if (data == null) throw new Exception("JSON vacio o invalido.");
        }
        catch (Exception e)
        {
            Debug.LogWarning("SaveManager: partida guardada corrupta o ilegible, se ignora. " + e.Message);
            return false;
        }

        if (data.version > SaveFormatVersion)
        {
            Debug.LogWarning("SaveManager: la partida guardada es de una version mas nueva que esta build; se ignora.");
            return false;
        }

        ObjectiveSystem.Instance.RestoreState(data.objectiveIndex, data.objectiveSequenceStarted, data.achievedKeys);

        // Prompt 04_implement (data flow, seccion 7): SceneManager.LoadScene(...) -- y con el,
        // todos los Awake() de la escena -- ya corrio antes de que este metodo pudiera ejecutarse
        // (ver MainMenuController.Continuar). Los componentes visuales que dependen de "ya
        // completado" (brillo de ALU/Registros/etc., beacons/tuberias de MissionStepPoint) se
        // inicializaron con los valores por defecto de ObjectiveSystem, no con los recien
        // restaurados. Se refrescan aqui explicitamente, mismo patron ya usado abajo para
        // FinalActivity.ResumeIfInProgress().
        foreach (var edu in UnityEngine.Object.FindObjectsByType<EducationalInteractable>(FindObjectsSortMode.None))
            edu.RefreshVisualState();

        var inventoryPairs = new List<KeyValuePair<string, int>>();
        int pairCount = Mathf.Min(data.inventoryKeys?.Count ?? 0, data.inventoryValues?.Count ?? 0);
        for (int i = 0; i < pairCount; i++)
            inventoryPairs.Add(new KeyValuePair<string, int>(data.inventoryKeys[i], data.inventoryValues[i]));
        Inventory.Instance.RestoreState(inventoryPairs);

        StorageMission.Instance.RestoreState(
            data.cpuRamLearned, data.fileFound, data.fileRetrieved, data.computerOpened,
            data.cpuProcessed, data.ramInsufficientDetected, data.ramModulesFullyInstalled,
            data.storageStarted, data.cpuLearnedShown, data.ramLoadAttempted, data.ramExecuted,
            data.wrongShelfIndex, data.ramModulesInstalled);

        // Mismo motivo que el refresh de EducationalInteractable de arriba: los MissionStepPoint
        // (Tv/CpuProcess/RamLoad/RamExecute) ya corrieron su Awake() con StorageMission todavia en
        // sus valores por defecto.
        foreach (var msp in UnityEngine.Object.FindObjectsByType<MissionStepPoint>(FindObjectsSortMode.None))
            msp.RefreshVisualState();

        if (FinalActivity.Instance != null)
        {
            FinalActivity.Instance.RestoreState(data.finalActivityStarted, data.finalActivityQuestionIndex);
            FinalActivity.Instance.ResumeIfInProgress();
        }

        return true;
    }

    public void DeleteSave()
    {
        try
        {
            if (File.Exists(SavePath)) File.Delete(SavePath);
        }
        catch (Exception e)
        {
            Debug.LogWarning("SaveManager: no se pudo borrar la partida guardada. " + e.Message);
        }
        hasSaveCache = false;
    }
}
