using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// Prompt 09 (Bloque 1, arquitectura): estado formal de menu/juego/pausa, reemplazando lo que
// antes vivia repartido entre GameSession (bool estatico), PauseMenuController (bool privado +
// Time.timeScale) y comprobaciones de nombre de escena repetidas en GameHUD, MinimapController,
// MissionUI y PauseMenuController. Mismo patron singleton que el resto del proyecto (Instance
// perezoso + DontDestroyOnLoad), pero con Bootstrap temprano (como SaveManager/StorageMission)
// porque necesita estar escuchando SceneManager.activeSceneChanged desde el primer frame.
public enum GameState { MainMenu, Playing, Paused }

public class GameStateManager : MonoBehaviour
{
    private const string GameplaySceneName = "SampleScene";
    private const string MenuSceneName = "MainMenu";

    // Bootstrap + Instance con creacion perezosa en el propio getter (patron identico a
    // ObjectiveSystem/StorageMission/SaveManager): otras clases (PauseMenuController,
    // MainMenuController, GameHUD) necesitan leer/suscribirse a GameStateManager desde su propio
    // Awake(), y el orden relativo entre distintos RuntimeInitializeOnLoadMethod no esta
    // garantizado -- el patron "Instance {get; private set;}" que solo se asigna en Awake() (como
    // GameHUD/SettingsUI) no es seguro aqui porque alguien podria pedir Instance antes de que el
    // propio Bootstrap de esta clase llegue a ejecutarse.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        _ = Instance;
    }

    private static GameStateManager _instance;
    public static GameStateManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("GameStateManager");
                _instance = go.AddComponent<GameStateManager>();
                DontDestroyOnLoad(go);
                _instance.Current = SceneManager.GetActiveScene().name == MenuSceneName ? GameState.MainMenu : GameState.Playing;
                SceneManager.activeSceneChanged += _instance.OnActiveSceneChanged;
            }
            return _instance;
        }
    }

    public GameState Current { get; private set; }

    // Reemplaza GameSession.HasActiveGame: si el Main Menu debe ofrecer "Continuar".
    public bool HasActiveGame { get; private set; }

    // Un solo evento para todo, mismo criterio que SettingsManager.OnSettingsChanged: quien lo
    // escucha relee GameStateManager.Current en vez de recibirlo solo en el propio evento.
    public UnityEvent<GameState> OnStateChanged = new UnityEvent<GameState>();

    // Se invoca siempre que cambia la escena activa, incluso si el nombre resultante coincide con
    // el estado ya vigente (ej. Reiniciar recarga SampleScene sobre SampleScene) -- los
    // suscriptores (GameHUD, MinimapController, MissionUI) dependen de este evento tambien para
    // resetear estado visual propio en cada (re)carga, no solo para saber "cambio de categoria".
    private void OnActiveSceneChanged(Scene previous, Scene next)
    {
        // Salir de cualquier escena limpia siempre el timeScale -- mismo motivo por el que
        // PauseMenuController ya hacia esto manualmente en Reiniciar/VolverAlMenu: si el jugador
        // sale con la pausa abierta, el juego no debe quedar congelado para siempre.
        Time.timeScale = 1f;
        Current = next.name == MenuSceneName ? GameState.MainMenu : GameState.Playing;
        OnStateChanged.Invoke(Current);
    }

    public void Pause()
    {
        if (Current != GameState.Playing) return;
        Current = GameState.Paused;
        Time.timeScale = 0f;
        OnStateChanged.Invoke(Current);
    }

    public void Resume()
    {
        if (Current != GameState.Paused) return;
        Current = GameState.Playing;
        Time.timeScale = 1f;
        OnStateChanged.Invoke(Current);
    }

    // Reemplaza MainMenuController.StartNewGame() + GameSession.ResetAll(): repone los sistemas
    // persistentes a su estado inicial y carga la escena de juego. Llamado por "Nueva Partida" y
    // por el dialogo de confirmacion cuando ya existe un guardado.
    public void StartNewGame()
    {
        ResetProgressionState();
        HasActiveGame = true;
        SaveManager.Instance.DeleteSave();
        SceneManager.LoadScene(GameplaySceneName);
    }

    // Reemplaza MainMenuController.Continuar(): si la partida activa ya esta en memoria (ej.
    // Pausa -> Volver al Menu -> Continuar dentro de la misma sesion), no vuelve a leer el
    // guardado de disco -- lo pisaria con una version mas vieja. Solo carga desde disco en un
    // arranque fresco del juego.
    public void ContinueGame()
    {
        if (!HasActiveGame && !SaveManager.Instance.HasSave) return;

        bool needsLoadFromDisk = !HasActiveGame;
        SceneManager.LoadScene(GameplaySceneName);

        if (needsLoadFromDisk)
        {
            SaveManager.Instance.LoadGame();
            HasActiveGame = true;
        }
    }

    // Reemplaza PauseMenuController.Reiniciar(): mismo reseteo que "Nueva Partida" pero llamado
    // desde el menu de pausa en vez del Main Menu.
    public void RestartSection()
    {
        Time.timeScale = 1f;
        ResetProgressionState();
        HasActiveGame = true;
        SaveManager.Instance.DeleteSave();
        SceneManager.LoadScene(GameplaySceneName);
    }

    // Reemplaza PauseMenuController.VolverAlMenu(): guarda antes de salir para que "Continuar"
    // desde el Main Menu recupere exactamente este punto.
    public void ReturnToMenu()
    {
        SaveManager.Instance.SaveGame();
        Time.timeScale = 1f;
        SceneManager.LoadScene(MenuSceneName);
    }

    private static void ResetProgressionState()
    {
        ObjectiveSystem.Instance.ResetState();
        StorageMission.Instance.ResetState();
        Inventory.Instance.ResetState();
        if (FinalActivity.Instance != null) FinalActivity.Instance.ResetState();
        GameHUD.Instance?.SetInventoryText("");
    }
}
