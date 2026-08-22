using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Capa de navegacion derivada (Prompt 23). NO mantiene su propio estado de progresion: relee
// periodicamente ObjectiveSystem/StorageMission (la fuente real de verdad) y calcula la fase
// de mision actual, su texto y el Transform al que debe apuntar la navegacion visual.
// MissionUI, WorldObjectiveMarker y MinimapController solo leen de aqui -- ninguno decide
// logica de juego ni mantiene una progresion paralela.
public class MissionNavigation : MonoBehaviour
{
    public enum Phase
    {
        ExploreCpu, ExploreRam, ExploreStorage, SearchFile, DeliverFile,
        OpenComputer, ProcessCpu, LoadRam, CollectRam, InstallRam, Execute, Final
    }

    // Orden real de progresion macro (coincide con el flujo de StorageMission/ObjectiveSystem).
    // El indice de una fase en este arreglo es tambien lo que MissionUI usa para saber cuantas
    // fases anteriores ya se completaron.
    public static readonly Phase[] Order =
    {
        Phase.ExploreCpu, Phase.ExploreRam, Phase.ExploreStorage, Phase.SearchFile, Phase.DeliverFile,
        Phase.OpenComputer, Phase.ProcessCpu, Phase.LoadRam, Phase.CollectRam, Phase.InstallRam,
        Phase.Execute, Phase.Final,
    };

    private static readonly string[] CpuKeys = { "ALU", "REGISTROS", "UNIDAD_CONTROL", "CACHE_L1", "CACHE_L2", "CACHE_L3" };
    private static readonly string[] RamKeys = { "RAM1", "RAM2" };

    private const float RecomputeInterval = 0.2f;

    private static MissionNavigation _instance;
    public static MissionNavigation Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("MissionNavigation");
                _instance = go.AddComponent<MissionNavigation>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        _ = Instance;
    }

    public Phase CurrentPhase { get; private set; } = Phase.ExploreCpu;
    public string CurrentTitle { get; private set; } = "";
    public string CurrentDescription { get; private set; } = "";
    public string CurrentSubProgress { get; private set; } = "";
    public Transform CurrentTarget { get; private set; }

    private readonly Dictionary<string, Transform> targetCache = new Dictionary<string, Transform>();
    private Transform playerTransform;

    public Transform PlayerTransform
    {
        get
        {
            if (playerTransform == null)
            {
                var movement = FindFirstObjectByType<MovementInput>();
                if (movement != null) playerTransform = movement.transform;
            }
            return playerTransform;
        }
    }

    private void Start()
    {
        StartCoroutine(RecomputeLoop());
    }

    private IEnumerator RecomputeLoop()
    {
        var wait = new WaitForSeconds(RecomputeInterval);
        while (true)
        {
            Recompute();
            yield return wait;
        }
    }

    private void Recompute()
    {
        var os = ObjectiveSystem.Instance;
        var sm = StorageMission.Instance;

        Phase phase;
        Transform target = null;
        string sub = "";

        if (!sm.RamModulesFullyInstalled)
        {
            if (!sm.CpuRamLearned)
            {
                string key = os.CurrentStepKey;
                bool isRam = key != null && Array.IndexOf(RamKeys, key) >= 0;
                phase = isRam ? Phase.ExploreRam : Phase.ExploreCpu;
                target = key != null ? FindTarget(SceneNameFor(key)) : FindTarget("Zone_CPU");
                sub = isRam
                    ? CountAchieved(os, RamKeys) + "/" + RamKeys.Length + " modulos conocidos"
                    : CountAchieved(os, CpuKeys) + "/" + CpuKeys.Length + " componentes conocidos";
            }
            else if (!sm.MissionStarted)
            {
                phase = Phase.ExploreStorage;
                target = FindTarget("Zone_Storage");
            }
            else if (!sm.FileFound)
            {
                phase = Phase.SearchFile;
                target = FindTarget("Zone_Storage");
            }
            else if (!sm.FileRetrieved)
            {
                phase = Phase.DeliverFile;
                target = FindTarget("server (37)");
            }
            else if (!sm.ComputerOpened)
            {
                phase = Phase.OpenComputer;
                target = FindTarget("Tv 32 Inch");
            }
            else if (!sm.CpuProcessed)
            {
                phase = Phase.ProcessCpu;
                target = FindTarget("FileMission_CpuProcess");
            }
            else if (!sm.RamInsufficientDetected)
            {
                phase = Phase.LoadRam;
                target = FindTarget("FileMission_RamLoad");
            }
            else if (Inventory.Instance.GetItemCount(StorageMission.RamItemId) < StorageMission.RequiredRamModules)
            {
                phase = Phase.CollectRam;
                target = FindNearestInteractable<CollectibleRam>();
                sub = Inventory.Instance.GetItemCount(StorageMission.RamItemId) + "/" + StorageMission.RequiredRamModules + " modulos recogidos";
            }
            else
            {
                phase = Phase.InstallRam;
                target = FindNearestInteractable<InstallRamSlot>();
            }
        }
        else if (sm.CanExecuteRam)
        {
            phase = Phase.Execute;
            target = FindTarget("FileMission_RamExecute");
        }
        else
        {
            phase = Phase.Final;
            target = null;
        }

        CurrentPhase = phase;
        CurrentTarget = target;
        CurrentSubProgress = sub;
        var info = GetPhaseInfo(phase);
        CurrentTitle = info.title;
        CurrentDescription = info.desc;
    }

    private Transform FindTarget(string objectName)
    {
        if (targetCache.TryGetValue(objectName, out var cached) && cached != null) return cached;
        var go = GameObject.Find(objectName);
        if (go == null) return null;
        targetCache[objectName] = go.transform;
        return go.transform;
    }

    // Busca, entre todos los componentes activos de tipo T presentes en la escena (CollectibleRam
    // en la bodega, InstallRamSlot en la Room RAM), el mas cercano al jugador cuyo CanInteract
    // publico ya sea true en este momento (sin leer ni duplicar el estado privado del componente).
    private Transform FindNearestInteractable<T>() where T : MonoBehaviour, IInteractable
    {
        var all = FindObjectsByType<T>(FindObjectsSortMode.None);
        var player = PlayerTransform;

        Transform best = null;
        float bestDist = float.MaxValue;
        foreach (var candidate in all)
        {
            if (!candidate.CanInteract) continue;
            if (player == null) return candidate.transform;

            float dist = Vector3.Distance(player.position, candidate.transform.position);
            if (dist >= bestDist) continue;
            best = candidate.transform;
            bestDist = dist;
        }
        return best;
    }

    private static string SceneNameFor(string key)
    {
        switch (key)
        {
            case "ALU": return "ALU";
            case "REGISTROS": return "Registros";
            case "UNIDAD_CONTROL": return "Unidad de Control";
            case "CACHE_L1": return "CacheL1";
            case "CACHE_L2": return "CacheL2";
            case "CACHE_L3": return "CacheL3";
            case "RAM1": return "RAM1";
            case "RAM2": return "RAM2";
            default: return key;
        }
    }

    private static int CountAchieved(ObjectiveSystem os, string[] keys)
    {
        int n = 0;
        foreach (var k in keys)
            if (os.IsKeyAchieved(k)) n++;
        return n;
    }

    public static int PhaseIndex(Phase phase) => Array.IndexOf(Order, phase);

    public static string GetPhaseTitle(Phase phase) => GetPhaseInfo(phase).title;

    private static (string title, string desc) GetPhaseInfo(Phase phase)
    {
        switch (phase)
        {
            case Phase.ExploreCpu: return ("EXPLORAR LA CPU", "Conoce los componentes principales del procesador.");
            case Phase.ExploreRam: return ("EXPLORAR LA MEMORIA RAM", "Conoce los modulos de memoria RAM.");
            case Phase.ExploreStorage: return ("EXPLORAR EL ALMACENAMIENTO", "Entra en la sala de almacenamiento.");
            case Phase.SearchFile: return ("BUSCAR EL ARCHIVO", "Revisa los modulos de almacenamiento de la sala.");
            case Phase.DeliverFile: return ("RECUPERAR EL ARCHIVO", "Lleva el archivo al almacenamiento principal.");
            case Phase.OpenComputer: return ("ABRIR EL ARCHIVO", "Usa la computadora para abrirlo.");
            case Phase.ProcessCpu: return ("PROCESAR EL ARCHIVO", "Envialo a la CPU para procesarlo.");
            case Phase.LoadRam: return ("CARGAR EN MEMORIA", "Intenta cargar el programa en la RAM.");
            case Phase.CollectRam: return ("BUSCAR MODULOS DE RAM", "Ve a la bodega y recoge modulos de repuesto.");
            case Phase.InstallRam: return ("INSTALAR RAM", "Instala los modulos en los slots disponibles.");
            case Phase.Execute: return ("EJECUTAR EL PROGRAMA", "Vuelve al punto de ejecucion.");
            case Phase.Final: return ("ACTIVIDAD FINAL", "Responde las preguntas finales.");
            default: return ("", "");
        }
    }
}
