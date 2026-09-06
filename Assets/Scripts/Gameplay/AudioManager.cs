using UnityEngine;

// Prompt 07 (Bloque 4), reducido tras feedback del usuario: el set original de 8 SFX (click de UI,
// pausa/reanudar, transicion de sala, error, cerrar panel, exito generico) resulto saturado y
// repetitivo en juego real. Se redujo a los 2 eventos que sí aportan: abrir un panel al interactuar
// con un objeto, y la entrega exitosa del archivo en el almacenamiento (evento especifico de
// StorageMission, no "cualquier objetivo completado" -- ver ReportFileDelivered). Mismo patron que
// MusicManager: el GameObject ya existe a mano en MainMenu.unity, con el AudioSource y los
// AudioClip ya asignados desde el Inspector, en vez de autoarrancar por
// RuntimeInitializeOnLoadMethod -- este script solo le da persistencia entre escenas
// (DontDestroyOnLoad) y expone un metodo por evento; no decide cuando reproducir nada por su
// cuenta, eso lo deciden los sistemas que ya reaccionan a cada evento (ver cada PlayXxx()).
//
// Los clips no se pueden asignar por Inspector si este objeto se creara por codigo (no hay
// GameObject de escena donde colgar la referencia serializada, mismo problema que
// GameInput/AstroBitControls.inputactions) -- de ahi que, igual que MusicManager, viva como objeto
// autorado a mano en vez de un singleton perezoso por codigo.
//
// Ambos clips vienen de Assets/ThirdParty/Kenney/SciFiSounds/ (CC0, se queda en el proyecto aunque
// ya no se use el resto de sus clips). Ver prompts/output/03_plan.md (Bloque 4) para el detalle de
// por que se eligio cada uno.
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance => _instance;

    [Header("SFX (canal Efectos)")]
    [SerializeField] private AudioClip interactOpenClip;
    [SerializeField] private AudioClip fileDeliveredClip;

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
    public void PlayFileDelivered() => PlaySfx(fileDeliveredClip);

    // PlayOneShot (no source.Play()) para que dos SFX cortos superpuestos no se corten entre si.
    private void PlaySfx(AudioClip clip)
    {
        if (clip == null || source == null) return;
        source.PlayOneShot(clip, SettingsManager.Instance.SfxVolume);
    }
}
