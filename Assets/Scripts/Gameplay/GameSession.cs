// Prompt 28: estado minimo de "hay una partida en curso durante esta sesion". NO es un
// sistema de guardado -- no serializa nada a disco ni sobrevive a cerrar el juego.
//
// El progreso real (ObjectiveSystem, StorageMission, Inventory, FinalActivity) YA persiste
// solo mientras dure la sesion, porque esos sistemas son DontDestroyOnLoad y no se destruyen
// al ir y volver entre MainMenu y SampleScene -- esta clase no duplica ese estado, solo agrega
// el unico dato que faltaba: si el Main Menu debe ofrecer "Continuar", y el punto unico donde
// "Nueva Partida"/"Reiniciar" piden reiniciar esos sistemas.
//
// Clase estatica simple (sin GameObject ni DontDestroyOnLoad): un campo static ya sobrevive a
// cualquier cambio de escena durante la ejecucion del proceso, que es exactamente lo que hace
// falta aqui -- evita agregar un singleton persistente mas de lo estrictamente necesario.
public static class GameSession
{
    public static bool HasActiveGame { get; private set; }

    // Llamado por "Nueva Partida" (Main Menu) y "Reiniciar" (menu de pausa): repone los
    // sistemas persistentes a su estado inicial y marca que ahora si existe una partida activa.
    public static void ResetAll()
    {
        ObjectiveSystem.Instance.ResetState();
        StorageMission.Instance.ResetState();
        Inventory.Instance.ResetState();
        if (FinalActivity.Instance != null) FinalActivity.Instance.ResetState();
        GameHUD.Instance?.SetInventoryText("");

        HasActiveGame = true;
    }
}
