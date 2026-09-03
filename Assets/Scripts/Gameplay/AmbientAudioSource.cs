using UnityEngine;

// Fase 3 (Prompt 01_maestro, secciones 12/22): el laboratorio no tenia ningun sonido ambiente --
// solo la musica de fondo (MusicManager, persistente desde MainMenu). Sin loop propio alguno, la
// sala se veia como una computadora pero sonaba completamente muerta.
//
// El clip se asigna por Inspector/tooling (referencia normal a un asset, sin Resources.Load ni
// AssetDatabase en runtime) apuntando al ventilador que ya trae SciFi Warehouse Kit
// (Demo/Audio/SFX/Fan_St.wav) -- ver Prompt 01_maestro seccion 33 (priorizar assets existentes
// antes que generar o descargar nuevos). No modifica nada dentro de esa carpeta vendored, solo
// la referencia desde este componente nuevo.
//
// A diferencia de MusicManager/SettingsManager, este componente NO es DontDestroyOnLoad a
// proposito: la ambientacion solo tiene sentido en la escena donde vive (el laboratorio), asi
// que se destruye sola al salir a Main Menu en vez de perseguir escenas que no la necesitan.
public class AmbientAudioSource : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [SerializeField, Range(0f, 1f)] private float baseVolume = 0.18f;

    private AudioSource source;

    private void Awake()
    {
        if (clip == null) return;

        source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = true;
        source.playOnAwake = false;
        source.spatialBlend = 0f;
        source.Play();

        ApplyVolume();
        SettingsManager.Instance.OnSettingsChanged.AddListener(ApplyVolume);
    }

    // Ambiente de sala = efecto, no interfaz ni musica: usa el canal "Volumen de efectos".
    private void ApplyVolume()
    {
        if (source == null) return;
        source.volume = baseVolume * SettingsManager.Instance.SfxVolume;
    }
}
