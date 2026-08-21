using UnityEngine;

// Conclusion educativa corta (4 preguntas) que se dispara automaticamente cuando
// ObjectiveSystem reporta que los 8 componentes fueron completados. Reutiliza
// GameHUD.ShowChoicePanel/ShowReward, no crea un panel ni un sistema de actividades nuevo.
public class FinalActivity : MonoBehaviour
{
    private struct Question
    {
        public readonly string Prompt;
        public readonly string[] Options;
        public readonly int CorrectIndex;

        public Question(string prompt, string[] options, int correctIndex)
        {
            Prompt = prompt;
            Options = options;
            CorrectIndex = correctIndex;
        }
    }

    private static readonly Question[] Questions =
    {
        new Question("Una instruccion necesita ejecutarse.\n\n¿Que componente coordina su ejecucion?",
            new[] { "Unidad de Control", "RAM", "Cache" }, 0),
        new Question("¿Donde puede mantenerse rapidamente un dato utilizado con frecuencia?",
            new[] { "RAM", "Cache", "Registros" }, 1),
        new Question("¿Donde se realizan operaciones aritmeticas y logicas sobre los datos?",
            new[] { "ALU", "RAM", "Unidad de Control" }, 0),
        new Question("¿Donde se mantienen temporalmente los programas y datos en uso?",
            new[] { "Cache", "RAM", "Registros" }, 1),
    };

    private const string ActivityTitle = "ACTIVIDAD FINAL";

    private static FinalActivity _instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        if (_instance != null) return;
        var go = new GameObject("FinalActivity");
        _instance = go.AddComponent<FinalActivity>();
        DontDestroyOnLoad(go);
    }

    private int questionIndex;
    private bool started;

    private void Awake()
    {
        ObjectiveSystem.Instance.OnAllStepsCompleted.AddListener(BeginFinalActivity);
    }

    // Prompt 19: OnAllStepsCompleted ahora puede dispararse desde dos rutas independientes
    // (el recorrido original de 8 piezas CPU/RAM, o la mision de almacenamiento). Idempotente
    // para no reabrir la actividad si ambas rutas se completan en la misma partida.
    private void BeginFinalActivity()
    {
        if (started) return;
        started = true;

        questionIndex = 0;
        ObjectiveSystem.Instance.SetObjective("Actividad final: repasa el flujo completo.");
        ObjectiveSystem.Instance.SetHint("Responde correctamente cada pregunta para terminar el recorrido.");
        ShowCurrentQuestion();
    }

    private void ShowCurrentQuestion()
    {
        var question = Questions[questionIndex];
        GameHUD.Instance?.ShowChoicePanel(ActivityTitle, question.Prompt, question.Options, OnAnswerSelected, CloseFinalActivity);
    }

    private void OnAnswerSelected(int optionIndex)
    {
        var question = Questions[questionIndex];
        if (optionIndex != question.CorrectIndex)
        {
            GameHUD.Instance?.ShowActivityError("✗ No es correcto. Intentalo de nuevo.");
            return;
        }

        questionIndex++;
        if (questionIndex < Questions.Length)
            ShowCurrentQuestion();
        else
            GameHUD.Instance?.ShowReward("✓ RECORRIDO COMPLETADO",
                "INSTRUCCION -> UNIDAD DE CONTROL -> REGISTROS/CACHE -> ALU -> RESULTADO -> MEMORIA\n\nAsi es como una CPU real coordina sus partes, de forma simplificada.",
                FinishFinalActivity);
    }

    private void FinishFinalActivity()
    {
        GameHUD.Instance?.HidePanel();
        ObjectiveSystem.Instance.SetObjective("Recorrido completado.");
        ObjectiveSystem.Instance.SetHint("Ya conoces como colaboran la ALU, los Registros, la Unidad de Control, la Cache y la RAM.");
    }

    private void CloseFinalActivity()
    {
        GameHUD.Instance?.HidePanel();
    }
}
