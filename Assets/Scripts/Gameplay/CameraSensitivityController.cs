using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

// Fase 2 (Prompt 35): conecta SettingsManager.CameraSensitivity/InvertY con el CinemachineFreeLook
// de la escena, sin tocar MovementInput (Jammo-Character es un asset vendored de solo lectura) ni
// la configuracion de autor del propio FreeLook -- solo multiplica sobre los valores base que ya
// tenia el vcam en el Inspector, para no perder su tuning original a sensibilidad 1x.
//
// Mismo patron singleton que el resto (Instance perezoso + DontDestroyOnLoad); vuelve a buscar el
// FreeLook cada vez que cambia de escena porque solo existe en SampleScene, no en MainMenu.
public class CameraSensitivityController : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        if (_instance != null) return;
        var go = new GameObject("CameraSensitivityController");
        _instance = go.AddComponent<CameraSensitivityController>();
        DontDestroyOnLoad(go);
    }

    private static CameraSensitivityController _instance;

    private CinemachineFreeLook freeLook;
    private float baseXSpeed;
    private float baseYSpeed;
    private bool baseYInvert;
    private bool hasBase;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        TryAcquireTarget();
        SettingsManager.Instance.OnSettingsChanged.AddListener(Apply);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TryAcquireTarget();
    }

    private void TryAcquireTarget()
    {
        freeLook = FindFirstObjectByType<CinemachineFreeLook>();
        if (freeLook == null)
        {
            hasBase = false;
            return;
        }

        if (!hasBase)
        {
            baseXSpeed = freeLook.m_XAxis.m_MaxSpeed;
            baseYSpeed = freeLook.m_YAxis.m_MaxSpeed;
            baseYInvert = freeLook.m_YAxis.m_InvertInput;
            hasBase = true;
        }

        Apply();
    }

    private void Apply()
    {
        if (freeLook == null || !hasBase) return;

        float sensitivity = SettingsManager.Instance.CameraSensitivity;
        freeLook.m_XAxis.m_MaxSpeed = baseXSpeed * sensitivity;
        freeLook.m_YAxis.m_MaxSpeed = baseYSpeed * sensitivity;
        // El eje Y del FreeLook ya viene invertido por diseno (mirar arriba = orbitar arriba);
        // "Invertir eje Y" del jugador se aplica como un XOR sobre esa base, no como reemplazo.
        freeLook.m_YAxis.m_InvertInput = baseYInvert ^ SettingsManager.Instance.InvertY;
    }
}
