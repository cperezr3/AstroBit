using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectiveSystem : MonoBehaviour
{
    private struct Step
    {
        public readonly string Key;
        public readonly string ObjectiveText;
        public readonly string HintText;

        public Step(string key, string objective, string hint)
        {
            Key = key;
            ObjectiveText = objective;
            HintText = hint;
        }
    }

    // Orden de la progresion educativa real. Coincide con el progressionKey configurado
    // en cada EducationalInteractable de la escena (ALU, Registros y Unidad de Control
    // comparten sala fisica con las 3 caches; RAM1/RAM2 estan en una sala aparte).
    private static readonly Step[] Sequence =
    {
        new Step("ALU", "Conoce la ALU.", "Busca el componente marcado como ALU dentro de la CPU."),
        new Step("REGISTROS", "Conoce los Registros.", "Busca donde la CPU guarda temporalmente los datos que usa al instante."),
        new Step("UNIDAD_CONTROL", "Conoce la Unidad de Control.", "Busca el componente que coordina el resto del procesador."),
        new Step("CACHE_L1", "Conoce la Cache L1.", "Busca la memoria cache mas cercana al procesador."),
        new Step("CACHE_L2", "Conoce la Cache L2.", "Busca el segundo nivel de cache, junto a la L1."),
        new Step("CACHE_L3", "Conoce la Cache L3.", "Busca el ultimo nivel de cache antes de salir hacia la RAM."),
        new Step("RAM1", "Conoce la RAM.", "Sal de la CPU y busca la sala con los modulos de memoria RAM."),
        new Step("RAM2", "Conoce el segundo modulo de RAM.", "Hay mas de un modulo de RAM en esta sala. Busca el otro."),
    };

    private const string InitialObjective = "Explora el laboratorio y encuentra la CPU.";
    private const string InitialHint = "Sigue el pasillo y busca la sala con la ALU, los Registros y la Unidad de Control.";
    private const float AdvanceDelay = 2f;

    private static ObjectiveSystem _instance;
    public static ObjectiveSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("ObjectiveSystem");
                _instance = go.AddComponent<ObjectiveSystem>();
                DontDestroyOnLoad(go);
                _instance.SetObjective(InitialObjective);
                _instance.SetHint(InitialHint);
            }
            return _instance;
        }
    }

    public UnityEvent<string> OnObjectiveChanged = new UnityEvent<string>();
    public UnityEvent<string> OnHintChanged = new UnityEvent<string>();
    public UnityEvent<string> OnObjectiveCompleted = new UnityEvent<string>();
    public UnityEvent OnAllStepsCompleted = new UnityEvent();

    public string CurrentObjective { get; private set; } = InitialObjective;
    public string CurrentHint { get; private set; } = InitialHint;

    public int CompletedSteps => achievedKeys.Count;
    public int TotalSteps => Sequence.Length;

    private int currentIndex;
    private bool sequenceStarted;
    private readonly HashSet<string> achievedKeys = new HashSet<string>();

    // Pasa del objetivo introductorio ("encuentra la CPU") al primer paso real de la
    // secuencia. Se llama al entrar a cualquier LocationZone; no hace nada si ya se inicio.
    public void BeginSequenceIfNeeded()
    {
        if (sequenceStarted || Sequence.Length == 0) return;
        sequenceStarted = true;
        SetObjective(Sequence[currentIndex].ObjectiveText);
        SetHint(Sequence[currentIndex].HintText);
    }

    public void SetObjective(string text)
    {
        CurrentObjective = text;
        OnObjectiveChanged.Invoke(text);
    }

    public void SetHint(string text)
    {
        CurrentHint = text;
        OnHintChanged.Invoke(text);
    }

    public void CompleteObjective(string completedText)
    {
        OnObjectiveCompleted.Invoke(completedText);
    }

    // Verdadero solo si "key" es el paso que la progresion espera ahora mismo.
    public bool IsCurrentStep(string key)
    {
        return currentIndex < Sequence.Length && Sequence[currentIndex].Key == key;
    }

    // Unica forma de avanzar la progresion real: una actividad resuelta correctamente.
    // Si "key" no es el paso esperado, se registra igualmente (para no repetirlo cuando
    // la progresion llegue a el) pero NO mueve el objetivo actual ni la pista.
    public void ReportActivityCompleted(string key, string feedbackText)
    {
        bool wasCurrent = IsCurrentStep(key);
        achievedKeys.Add(key);
        OnObjectiveCompleted.Invoke(feedbackText);

        if (wasCurrent)
        {
            sequenceStarted = true;
            StartCoroutine(AdvanceAfterDelay());
        }
    }

    private IEnumerator AdvanceAfterDelay()
    {
        yield return new WaitForSeconds(AdvanceDelay);

        currentIndex++;
        while (currentIndex < Sequence.Length && achievedKeys.Contains(Sequence[currentIndex].Key))
            currentIndex++;

        if (currentIndex >= Sequence.Length)
        {
            OnAllStepsCompleted.Invoke();
        }
        else
        {
            SetObjective(Sequence[currentIndex].ObjectiveText);
            SetHint(Sequence[currentIndex].HintText);
        }
    }
}
