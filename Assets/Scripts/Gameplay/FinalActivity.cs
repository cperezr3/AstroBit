using UnityEngine;

// Conclusion educativa corta (4 preguntas) que se dispara al terminar el recorrido completo
// (ver BeginFinalActivity, llamado por StorageMission). Reutiliza GameHUD.ShowChoicePanel/
// ShowReward, no crea un panel ni un sistema de actividades nuevo.
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
    public static FinalActivity Instance => _instance;

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

    // Prompt 20: ya no escucha ObjectiveSystem.OnAllStepsCompleted directamente -- ese evento
    // ahora solo marca "CPU/RAM aprendidos" (ver StorageMission.OnCpuRamLearned) y desbloquea
    // la sala de almacenamiento. StorageMission llama a BeginFinalActivity() explicitamente
    // al terminar el recorrido completo (archivo ejecutado tras instalar la RAM de repuesto).
    // Idempotente por si en el futuro hubiera mas de una ruta de finalizacion.
    public void BeginFinalActivity()
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
        // Prompt 02_continuacion (seccion 30): sin esto, el jugador quedaba deambulando con solo
        // un texto de objetivo actualizado y ninguna sensacion real de cierre tras el recorrido.
        GameCompleteScreen.Instance?.Show();
    }

    private void CloseFinalActivity()
    {
        GameHUD.Instance?.HidePanel();
    }

    // Prompt 28: unico punto de reinicio para "Nueva Partida"/"Reiniciar" (ver GameSession).
    // Sin esto, "started" quedaria en true tras una primera partida completa y una segunda
    // partida en la misma sesion nunca podria volver a disparar la actividad final.
    public void ResetState()
    {
        started = false;
        questionIndex = 0;
    }

    // Fase 2 (Prompt 35, 9.1): expuestos solo para SaveManager.
    public bool Started => started;
    public int QuestionIndex => questionIndex;

    public void RestoreState(bool started, int questionIndex)
    {
        this.started = started;
        this.questionIndex = Mathf.Clamp(questionIndex, 0, Questions.Length);
    }

    // Si la partida se guardo a mitad de la actividad final, el punto de ejecucion que la abre
    // (RAM2) ya quedo consumido (ramExecuted=true) y no puede volver a interactuarse -- sin esto
    // el jugador se quedaria sin forma de reabrir el panel de preguntas tras cargar. Idempotente:
    // no hace nada si la actividad no estaba en curso o ya se completo.
    public void ResumeIfInProgress()
    {
        if (!started || questionIndex >= Questions.Length) return;
        ShowCurrentQuestion();
    }
}
