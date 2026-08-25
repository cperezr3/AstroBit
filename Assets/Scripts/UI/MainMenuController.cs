using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Controlador del Main Menu (Prompt 26): equivalente limpio de NewMonoBehaviourScript del
// proyecto AstroBitMenu, adaptado para cargar la escena de gameplay por nombre en vez de por
// build index (evita depender del orden accidental en Build Settings).
//
// Prompt 28: agrega el botón "Continuar" -- habilitado solo si GameSession.HasActiveGame es
// true (ver GameSession). El wiring de su OnClick se hace en Awake con AddListener normal (no
// persistente) porque, a diferencia de "Nueva Partida"/"Salir", su disponibilidad depende de
// estado que solo se conoce en tiempo de ejecucion.
public class MainMenuController : MonoBehaviour
{
    private const string GameplaySceneName = "SampleScene";

    private void Awake()
    {
        var continuarButton = transform.Find("Continuar")?.GetComponent<Button>();
        if (continuarButton == null) return;

        continuarButton.interactable = GameSession.HasActiveGame;
        continuarButton.onClick.AddListener(Continuar);
    }

    public void Jugar()
    {
        GameSession.ResetAll();
        SceneManager.LoadScene(GameplaySceneName);
    }

    public void Continuar()
    {
        if (!GameSession.HasActiveGame) return;
        SceneManager.LoadScene(GameplaySceneName);
    }

    public void Salir()
    {
        Debug.Log("Saliendo...");
        Application.Quit();
    }
}
