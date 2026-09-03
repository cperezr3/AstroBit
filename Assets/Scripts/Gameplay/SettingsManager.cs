using UnityEngine;
using UnityEngine.Events;

// Fase 2 (Prompt 35): configuracion persistente de audio/camara. Mismo patron singleton que
// el resto de managers (Instance perezoso + DontDestroyOnLoad), pero sin GameObject propio en
// escena -- se crea la primera vez que algo pide Instance, igual que ObjectiveSystem/Inventory.
//
// No hay AudioMixer en el proyecto (no existia ninguno antes de este prompt y crear uno desde
// codigo no es viable via API publica); el volumen se aplica multiplicando directamente sobre
// los AudioSource existentes (ver MusicManager) en vez de enrutar por grupos de mixer. Master
// y Musica ya tienen un origen de sonido real (MusicManager); Efectos/UI quedan aqui listos
// (persisten, tienen evento de cambio) para cuando la Fase 3 agregue sonidos de interfaz.
public class SettingsManager : MonoBehaviour
{
    private const string KeyMaster = "astrobit.audio.master";
    private const string KeyMusic = "astrobit.audio.music";
    private const string KeySfx = "astrobit.audio.sfx";
    private const string KeyUi = "astrobit.audio.ui";
    private const string KeySensitivity = "astrobit.camera.sensitivity";
    private const string KeyInvertY = "astrobit.camera.inverty";
    private const string KeyResolutionIndex = "astrobit.graphics.resolutionindex";
    private const string KeyFullscreen = "astrobit.graphics.fullscreen";
    private const string KeyVSync = "astrobit.graphics.vsync";
    private const string KeyQuality = "astrobit.graphics.quality";

    private static SettingsManager _instance;
    public static SettingsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("SettingsManager");
                _instance = go.AddComponent<SettingsManager>();
                DontDestroyOnLoad(go);
                _instance.LoadFromDisk();
            }
            return _instance;
        }
    }

    public float MasterVolume { get; private set; } = 1f;
    public float MusicVolume { get; private set; } = 1f;
    public float SfxVolume { get; private set; } = 1f;
    public float UiVolume { get; private set; } = 1f;

    public const float MinSensitivity = 0.5f;
    public const float MaxSensitivity = 3f;
    public float CameraSensitivity { get; private set; } = 1f;
    public bool InvertY { get; private set; }

    // Resoluciones soportadas por el monitor actual, sin duplicados de refresh rate (Screen.resolutions
    // suele listar la misma resolucion varias veces, una por cada tasa de refresco disponible).
    private Resolution[] _resolutions;
    public int ResolutionCount => _resolutions.Length;
    public int ResolutionIndex { get; private set; }
    public bool Fullscreen { get; private set; } = true;
    public bool VSync { get; private set; } = true;
    public string[] QualityNames => QualitySettings.names;
    public int QualityLevel { get; private set; }

    public string GetResolutionLabel(int index)
    {
        var r = _resolutions[index];
        return $"{r.width} x {r.height}";
    }

    // Un solo evento para todo: quien lo escucha (MusicManager, CameraSensitivityController,
    // SettingsUI) relee los valores actuales en vez de recibir el dato en el propio evento --
    // evita firmas distintas por cada slider y mantiene esto ampliable sin romper listeners.
    public UnityEvent OnSettingsChanged = new UnityEvent();

    private void LoadFromDisk()
    {
        MasterVolume = PlayerPrefs.GetFloat(KeyMaster, 1f);
        MusicVolume = PlayerPrefs.GetFloat(KeyMusic, 1f);
        SfxVolume = PlayerPrefs.GetFloat(KeySfx, 1f);
        UiVolume = PlayerPrefs.GetFloat(KeyUi, 1f);
        CameraSensitivity = Mathf.Clamp(PlayerPrefs.GetFloat(KeySensitivity, 1f), MinSensitivity, MaxSensitivity);
        InvertY = PlayerPrefs.GetInt(KeyInvertY, 0) == 1;

        BuildResolutionList();
        int defaultResIndex = FindResolutionIndex(Screen.width, Screen.height);
        ResolutionIndex = Mathf.Clamp(PlayerPrefs.GetInt(KeyResolutionIndex, defaultResIndex), 0, _resolutions.Length - 1);
        Fullscreen = PlayerPrefs.GetInt(KeyFullscreen, Screen.fullScreen ? 1 : 0) == 1;
        VSync = PlayerPrefs.GetInt(KeyVSync, QualitySettings.vSyncCount > 0 ? 1 : 0) == 1;
        QualityLevel = Mathf.Clamp(PlayerPrefs.GetInt(KeyQuality, QualitySettings.GetQualityLevel()), 0, QualitySettings.names.Length - 1);

        AudioListener.volume = MasterVolume;
        ApplyGraphics();
    }

    private void BuildResolutionList()
    {
        var seen = new System.Collections.Generic.List<Resolution>();
        foreach (var r in Screen.resolutions)
        {
            bool duplicate = false;
            for (int i = 0; i < seen.Count; i++)
            {
                if (seen[i].width == r.width && seen[i].height == r.height) { duplicate = true; break; }
            }
            if (!duplicate) seen.Add(r);
        }
        if (seen.Count == 0) seen.Add(new Resolution { width = Screen.width, height = Screen.height });
        _resolutions = seen.ToArray();
    }

    private int FindResolutionIndex(int width, int height)
    {
        for (int i = 0; i < _resolutions.Length; i++)
        {
            if (_resolutions[i].width == width && _resolutions[i].height == height) return i;
        }
        return _resolutions.Length - 1;
    }

    public void SetMasterVolume(float value) { MasterVolume = Mathf.Clamp01(value); Apply(); }
    public void SetMusicVolume(float value) { MusicVolume = Mathf.Clamp01(value); Apply(); }
    public void SetSfxVolume(float value) { SfxVolume = Mathf.Clamp01(value); Apply(); }
    public void SetUiVolume(float value) { UiVolume = Mathf.Clamp01(value); Apply(); }
    public void SetCameraSensitivity(float value) { CameraSensitivity = Mathf.Clamp(value, MinSensitivity, MaxSensitivity); Apply(); }
    public void SetInvertY(bool value) { InvertY = value; Apply(); }

    public void SetResolutionIndex(int index) { ResolutionIndex = Mathf.Clamp(index, 0, _resolutions.Length - 1); Apply(); }
    public void SetFullscreen(bool value) { Fullscreen = value; Apply(); }
    public void SetVSync(bool value) { VSync = value; Apply(); }
    public void SetQualityLevel(int level) { QualityLevel = Mathf.Clamp(level, 0, QualitySettings.names.Length - 1); Apply(); }

    private void ApplyGraphics()
    {
        var mode = Fullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        var r = _resolutions[ResolutionIndex];
        Screen.SetResolution(r.width, r.height, mode);
        QualitySettings.vSyncCount = VSync ? 1 : 0;
        QualitySettings.SetQualityLevel(QualityLevel, applyExpensiveChanges: true);
    }

    private void Apply()
    {
        // Volumen maestro real: AudioListener.volume es un multiplicador global sobre TODO el
        // audio del proyecto (incluye el SFX ambiental ya existente del SciFi Warehouse Kit,
        // sin tener que tocar ese script vendored). Los canales Musica/Efectos/UI son
        // multiplicadores propios encima de sus fuentes especificas (ver MusicManager) -- no se
        // vuelven a multiplicar por MasterVolume aqui para no aplicarlo dos veces.
        AudioListener.volume = MasterVolume;
        ApplyGraphics();

        PlayerPrefs.SetFloat(KeyMaster, MasterVolume);
        PlayerPrefs.SetFloat(KeyMusic, MusicVolume);
        PlayerPrefs.SetFloat(KeySfx, SfxVolume);
        PlayerPrefs.SetFloat(KeyUi, UiVolume);
        PlayerPrefs.SetFloat(KeySensitivity, CameraSensitivity);
        PlayerPrefs.SetInt(KeyInvertY, InvertY ? 1 : 0);
        PlayerPrefs.SetInt(KeyResolutionIndex, ResolutionIndex);
        PlayerPrefs.SetInt(KeyFullscreen, Fullscreen ? 1 : 0);
        PlayerPrefs.SetInt(KeyVSync, VSync ? 1 : 0);
        PlayerPrefs.SetInt(KeyQuality, QualityLevel);
        PlayerPrefs.Save();

        OnSettingsChanged.Invoke();
    }
}
