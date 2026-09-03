using UnityEngine;

// Musica global de fondo (Prompt 33). Vive en el GameObject "MusicManager" ya colocado a mano
// en MainMenu.unity, con su AudioSource configurado desde el Inspector (clip, Loop, Play On
// Awake, Volume ~0.25, Spatial Blend 0). Este script no decide que suena ni cuando -- solo le
// da persistencia entre escenas y evita que exista mas de una instancia sonando a la vez.
//
// Patron identico al resto de singletons de UI (GameHUD, MinimapController, etc.): unica
// instancia + DontDestroyOnLoad + autodestruccion de duplicados. La diferencia es que aqui el
// GameObject ya existe en la escena (autoria manual, con el AudioSource ya configurado) en vez
// de crearse por codigo via RuntimeInitializeOnLoadMethod, asi que no hace falta un Bootstrap:
// el propio Awake() al cargar MainMenu.unity ya es el unico punto de entrada.
[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    private static MusicManager _instance;

    private AudioSource source;
    private float baseVolume;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            // Ya existe una instancia persistente (p.ej. el jugador volvio al Main Menu):
            // esta copia recien cargada con la escena es la duplicada, no la original.
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        // Fase 2 (Prompt 35): volumen maestro/musica configurable. baseVolume conserva el 0.25
        // configurado a mano en el Inspector como el 100% de "Volumen musica"; el multiplicador
        // de SettingsManager se aplica encima, nunca lo reemplaza.
        source = GetComponent<AudioSource>();
        baseVolume = source.volume;
        ApplyVolume();
        SettingsManager.Instance.OnSettingsChanged.AddListener(ApplyVolume);
    }

    private void ApplyVolume()
    {
        // Volumen maestro se aplica globalmente via AudioListener.volume (ver SettingsManager);
        // aqui solo se escala por el canal de musica, para no aplicar el maestro dos veces.
        source.volume = baseVolume * SettingsManager.Instance.MusicVolume;
    }
}
