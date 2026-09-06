using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

// Prompt 10 (Bloque 2): punto unico de entrada al Input System, reemplazando los dos usos del
// Input Manager legado que quedaban en el flujo real de juego:
// - Movimiento del jugador (antes Input.GetAxis("Horizontal"/"Vertical") en MovementInput,
//   vendored -- ver PlayerMovementController).
// - Mira de camara (antes el override por defecto de Cinemachine, que llama a
//   Input.GetAxis("Mouse X"/"Mouse Y") internamente para cualquier CinemachineFreeLook que no
//   tenga un CinemachineInputProvider propio).
//
// Mismo patron singleton perezoso que el resto del proyecto (Instance con creacion diferida +
// Bootstrap temprano). El InputActionAsset se carga desde Resources porque este singleton se
// autoarranca por codigo sin ningun GameObject de escena donde colgar una referencia serializada
// -- mismo motivo por el que ObjectiveSystem/SaveManager/etc. tampoco dependen del Inspector.
public class GameInput : MonoBehaviour
{
    private const string AssetResourcePath = "AstroBitControls";

    // Legado (ProjectSettings/InputManager.asset): "Mouse X"/"Mouse Y" tienen sensitivity 0.1 y
    // gravity 0 (delta crudo del mouse, sin suavizado). Se replica ese mismo factor aqui para que
    // CinemachineCore.GetInputAxis devuelva el mismo orden de magnitud que devolvia
    // Input.GetAxis, y el tuning existente de m_MaxSpeed/m_AccelTime del FreeLook
    // (CameraSensitivityController) se siga sintiendo igual sin tocar esos valores.
    private const float LegacyMouseSensitivity = 0.1f;

    // Prompt 06 (Bloque 3): mismo patron de persistencia que SettingsManager (PlayerPrefs, no el
    // JSON de SaveManager) -- un binding remapeado es una preferencia del jugador, no progreso de
    // partida, y no debe borrarse al presionar "Nueva Partida"/"Reiniciar" (que si borra
    // astrobit_save.json via SaveManager.DeleteSave()).
    private const string BindingOverridesPrefsKey = "astrobit.input.bindingoverrides";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        _ = Instance;
    }

    private static GameInput _instance;
    public static GameInput Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("GameInput");
                _instance = go.AddComponent<GameInput>();
                DontDestroyOnLoad(go);
                _instance.Init();
            }
            return _instance;
        }
    }

    public InputActionAsset Actions { get; private set; }
    public InputAction MoveAction { get; private set; }
    public InputAction LookAction { get; private set; }
    public InputAction InteractAction { get; private set; }
    public InputAction PauseAction { get; private set; }

    // Se dispara despues de guardar un rebind nuevo y despues de restaurar los valores por
    // defecto -- ControlsRebindingPanel lo escucha para refrescar los textos de binding actuales
    // sin que este archivo sepa nada de UI.
    public UnityEvent OnBindingsChanged = new UnityEvent();

    private void Init()
    {
        Actions = Resources.Load<InputActionAsset>(AssetResourcePath);
        if (Actions == null)
        {
            Debug.LogError("GameInput: no se encontro 'Resources/" + AssetResourcePath + ".inputactions'.");
            return;
        }

        var player = Actions.FindActionMap("Player");
        MoveAction = player.FindAction("Move");
        LookAction = player.FindAction("Look");
        InteractAction = player.FindAction("Interact");
        PauseAction = player.FindAction("Pause");

        LoadBindingOverrides();
        player.Enable();

        // Reemplaza el override por defecto de Cinemachine (Input.GetAxis) -- unico punto de
        // integracion necesario para que "CM FreeLook1" (SampleScene) lea la mira de camara del
        // Input System sin agregar CinemachineInputProvider/InputActionReference ni tocar el vcam.
        // CameraSensitivityController sigue aplicando sensibilidad/invertir Y exactamente igual
        // que antes: m_MaxSpeed/m_InvertInput multiplican lo que sea que devuelva GetInputAxis,
        // sin que le importe la fuente.
        CinemachineCore.GetInputAxis = GetCinemachineInputAxis;
    }

    // El stick derecho del mando (accion "Look") ya llega normalizado [-1,1], que es exactamente
    // lo que CinemachineCore/AxisState.SpeedMode.MaxSpeed espera (velocidad maxima en el valor
    // 1) -- no necesita ningun factor extra. El mouse, en cambio, entrega delta crudo en pixeles
    // por frame (como el legado), asi que se escala por LegacyMouseSensitivity para igualar la
    // magnitud que ya tenia ajustado el FreeLook. Se prioriza el stick sobre el mouse solo cuando
    // el stick esta realmente inclinado, para no mezclar ambas fuentes en el mismo frame.
    private float GetCinemachineInputAxis(string axisName)
    {
        if (axisName != "Mouse X" && axisName != "Mouse Y") return 0f;

        Vector2 stick = LookAction != null ? LookAction.ReadValue<Vector2>() : Vector2.zero;
        if (stick.sqrMagnitude > 0.0001f)
            return axisName == "Mouse X" ? stick.x : stick.y;

        Vector2 mouseDelta = Mouse.current != null ? Mouse.current.delta.ReadValue() : Vector2.zero;
        float raw = axisName == "Mouse X" ? mouseDelta.x : mouseDelta.y;
        return raw * LegacyMouseSensitivity;
    }

    private void OnDestroy()
    {
        if (_instance == this) CinemachineCore.GetInputAxis = null;
    }

    // Prompt 06 (Bloque 3): carga los overrides guardados de una sesion anterior, si existen.
    // Idempotente y silenciosa -- un jugador que nunca remapeo nada simplemente no tiene la key
    // en PlayerPrefs y esto no hace nada.
    private void LoadBindingOverrides()
    {
        string json = PlayerPrefs.GetString(BindingOverridesPrefsKey, "");
        if (!string.IsNullOrEmpty(json)) Actions.LoadBindingOverridesFromJson(json);
    }

    // Unico punto de guardado de rebinds -- llamado por ControlsRebindingPanel despues de cada
    // reasignacion exitosa. SaveBindingOverridesAsJson() (del propio Input System) ya serializa
    // solo los bindings efectivamente sobreescritos, no el asset entero.
    public void SaveBindingOverrides()
    {
        string json = Actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(BindingOverridesPrefsKey, json);
        PlayerPrefs.Save();
        OnBindingsChanged.Invoke();
    }

    public void ResetBindingOverrides()
    {
        foreach (var map in Actions.actionMaps) map.RemoveAllBindingOverrides();
        PlayerPrefs.DeleteKey(BindingOverridesPrefsKey);
        PlayerPrefs.Save();
        OnBindingsChanged.Invoke();
    }
}
