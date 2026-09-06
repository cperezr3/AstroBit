using UnityEngine;

// Prompt 07 (Bloque 4): unico reproductor de SFX del juego -- interaccion/UI, transiciones,
// exito/error, pausa. Mismo patron que MusicManager (Prompt 33): el GameObject ya existe a mano
// en MainMenu.unity, con el AudioSource y los AudioClip ya asignados desde el Inspector, en vez
// de autoarrancar por RuntimeInitializeOnLoadMethod -- este script solo le da persistencia entre
// escenas (DontDestroyOnLoad) y expone un metodo por evento; no decide cuando reproducir nada por
// su cuenta, eso lo deciden los sistemas que ya reaccionan a cada evento (ver cada PlayXxx()).
//
// Los clips no se pueden asignar por Inspector si este objeto se creara por codigo (no hay
// GameObject de escena donde colgar la referencia serializada, mismo problema que
// GameInput/AstroBitControls.inputactions) -- de ahi que, igual que MusicManager, viva como objeto
// autorado a mano en vez de un singleton perezoso por codigo.
//
// Todos los clips vienen de Assets/ThirdParty/Kenney/SciFiSounds/ (CC0). El pack es de temática
// espacial/combate (laseres, motores, explosiones), no un pack de UI generico -- se eligieron los
// clips mas neutros/mecanicos disponibles para cada evento en vez de inventar sonidos nuevos; ver
// prompts/output/03_plan.md (Bloque 4) para el detalle de por que se eligio cada uno.
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance => _instance;

    [Header("Interaccion con objetos del mundo (canal Efectos)")]
    [SerializeField] private AudioClip interactOpenClip;
    [SerializeField] private AudioClip panelCloseClip;
    [SerializeField] private AudioClip successClip;
    [SerializeField] private AudioClip errorClip;
    [SerializeField] private AudioClip roomTransitionClip;

    [Header("UI (canal Interfaz)")]
    [SerializeField] private AudioClip uiClickClip;
    [SerializeField] private AudioClip pauseClip;
    [SerializeField] private AudioClip resumeClip;

    private AudioSource source;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
        source = GetComponent<AudioSource>();
    }

    public void PlayInteractOpen() => PlaySfx(interactOpenClip);
    public void PlayPanelClose() => PlaySfx(panelCloseClip);
    public void PlaySuccess() => PlaySfx(successClip);
    public void PlayError() => PlaySfx(errorClip);
    public void PlayRoomTransition() => PlaySfx(roomTransitionClip);

    public void PlayUiClick() => PlayUi(uiClickClip);
    public void PlayPause() => PlayUi(pauseClip);
    public void PlayResume() => PlayUi(resumeClip);

    // Canal "Efectos": eventos que ocurren en el mundo/la mision (interactuar, exito, error,
    // transicion de sala). PlayOneShot (no source.Play()) para que dos SFX cortos superpuestos no
    // se corten entre si -- p.ej. un "exito" que suena justo cuando ya estaba sonando un "click".
    private void PlaySfx(AudioClip clip)
    {
        if (clip == null || source == null) return;
        source.PlayOneShot(clip, SettingsManager.Instance.SfxVolume);
    }

    // Canal "Interfaz": clicks de menu/configuracion y pausa/reanudar -- deliberadamente separado
    // del canal Efectos para que el slider "Volumen de interfaz" (ya existia en Configuracion, sin
    // nada que escalar hasta este bloque) tenga un proposito real.
    private void PlayUi(AudioClip clip)
    {
        if (clip == null || source == null) return;
        source.PlayOneShot(clip, SettingsManager.Instance.UiVolume);
    }
}
