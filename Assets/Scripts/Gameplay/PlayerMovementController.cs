using UnityEngine;

// Prompt 10 (Bloque 2): reemplaza a MovementInput (Assets/Jammo-Character/, vendored, no se
// edita) como el componente que realmente mueve al jugador. Replica su misma matematica de
// movimiento -- rotar hacia la direccion de movimiento relativa a camara, CharacterController.Move,
// el mismo parametro "Blend" del Animator, la misma pseudo-gravedad -- pero leyendo del Input
// System (GameInput.MoveAction) en vez de Input.GetAxis("Horizontal"/"Vertical").
//
// MovementInput NO se elimina ni se quita de Jammo_Player: queda presente pero deshabilitado
// (enabled = false) en la escena, unicamente para que las llamadas existentes a
// FindFirstObjectByType<MovementInput>() en WorldLabel/EducationalInteractable/
// PlayerInteraction/MinimapController/MissionNavigation -- que solo necesitan el Transform del
// jugador -- lo sigan encontrando sin tener que tocar esos 5 archivos (un componente
// deshabilitado sigue siendo encontrado por FindObjectByType, solo deja de correr su propio
// Update()). La unica llamada que si hacia falta migrar es
// PauseMenuController.SetPlayerControlEnabled, que alternaba MovementInput.enabled para pausar
// el movimiento: ahora alterna PlayerMovementController.enabled -- dejar que Pausa reactive el
// MovementInput legado habria vuelto a traer Input.GetAxis Y habria movido el CharacterController
// dos veces a la vez (una por cada componente).
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovementController : MonoBehaviour
{
    // Mismos valores tuneados a mano en el Inspector de MovementInput en Jammo_Player: Velocity
    // 10, desiredRotationSpeed 0.3 (el resto coincide con el default de fabrica del asset).
    [SerializeField] private float velocity = 10f;
    [SerializeField] private float desiredRotationSpeed = 0.3f;
    [SerializeField] private float allowPlayerRotation = 0.1f;
    [SerializeField] private float startAnimTime = 0.3f;
    [SerializeField] private float stopAnimTime = 0.15f;

    // El Input System, a diferencia del Input Manager legado (Horizontal/Vertical con gravity=3,
    // sensitivity=3), no suaviza por si solo un composite WASD -- da 0/1 instantaneo por tecla.
    // Se aproxima el mismo ritmo de rampa (~1/3s para llegar a velocidad plena) suavizando aqui
    // en vez de leer botones individuales y reimplementar snap/gravity byte a byte; no es una
    // replica exacta del algoritmo legado, pero preserva la sensacion percibida.
    private const float MoveRampPerSecond = 3f;

    private Animator anim;
    private CharacterController controller;
    private Camera cam;
    private Vector2 smoothedMove;

    // Mismo valor con el que arrancaba serializado el verticalVel de MovementInput en la escena.
    private float verticalVel = -0.5f;

    private void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        cam = Camera.main;
    }

    private void Update()
    {
        Vector2 rawMove = GameInput.Instance.MoveAction != null
            ? GameInput.Instance.MoveAction.ReadValue<Vector2>()
            : Vector2.zero;
        smoothedMove = Vector2.MoveTowards(smoothedMove, rawMove, MoveRampPerSecond * Time.deltaTime);

        float inputX = smoothedMove.x;
        float inputZ = smoothedMove.y;
        float speed = smoothedMove.sqrMagnitude;

        if (speed > allowPlayerRotation)
        {
            anim.SetFloat("Blend", speed, startAnimTime, Time.deltaTime);
            MoveAndRotate(inputX, inputZ);
        }
        else
        {
            anim.SetFloat("Blend", speed, stopAnimTime, Time.deltaTime);
        }

        // Misma pseudo-gravedad que MovementInput.Update() (sin cambios: no depende de input
        // legado ni nuevo, solo de CharacterController.isGrounded).
        bool isGrounded = controller.isGrounded;
        if (!isGrounded) verticalVel -= 1f;
        controller.Move(new Vector3(0f, verticalVel * 0.2f * Time.deltaTime, 0f));
    }

    private void MoveAndRotate(float inputX, float inputZ)
    {
        if (cam == null) cam = Camera.main;
        if (cam == null) return;

        Vector3 forward = cam.transform.forward;
        Vector3 right = cam.transform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * inputZ + right * inputX;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
        controller.Move(desiredMoveDirection * Time.deltaTime * velocity);
    }
}
