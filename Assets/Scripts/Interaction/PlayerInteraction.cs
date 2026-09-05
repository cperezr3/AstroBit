using UnityEngine;

// Prompt 10 (Bloque 2): el chequeo de tecla paso de Keyboard.current.eKey directo a la accion
// "Interact" de GameInput -- mismo binding de teclado (E) mas el equivalente de mando
// (Gamepad buttonSouth) definido en Resources/AstroBitControls.inputactions, sin duplicar la
// logica de deteccion aqui.
public class PlayerInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float interactionDistance = 4f;

    private IInteractable currentTarget;
    private Transform playerTransform;

    void Update()
    {
        UpdateLookTarget();

        if (currentTarget != null && GameInput.Instance.InteractAction.WasPressedThisFrame())
        {
            currentTarget.Interact();

            if (!currentTarget.CanInteract)
                UpdateLookTarget();
        }
    }

    void UpdateLookTarget()
    {
        if (playerTransform == null)
        {
            var movement = FindFirstObjectByType<MovementInput>();
            if (movement != null) playerTransform = movement.transform;
        }
        if (playerTransform == null) return;

        IInteractable found = FindNearestInteractable();

        if (found == currentTarget) return;

        currentTarget = found;

        if (GameHUD.Instance == null) return;

        if (currentTarget != null)
            GameHUD.Instance.ShowPrompt(currentTarget.PromptText);
        else
            GameHUD.Instance.HidePrompt();
    }

    // Detecta el IInteractable valido mas cercano al JUGADOR (no a la camara),
    // conservando una comprobacion de linea de vision para no interactuar a traves de paredes.
    IInteractable FindNearestInteractable()
    {
        Collider[] hits = Physics.OverlapSphere(playerTransform.position, interactionDistance);

        IInteractable best = null;
        float bestDist = float.MaxValue;
        Vector3 origin = playerTransform.position + Vector3.up;

        foreach (var col in hits)
        {
            var interactable = col.GetComponentInParent<IInteractable>();
            if (interactable == null || !interactable.CanInteract) continue;

            Vector3 point = col.ClosestPoint(playerTransform.position);
            float dist = Vector3.Distance(playerTransform.position, point);
            if (dist >= bestDist) continue;

            Vector3 toPoint = point - origin;
            float distToPoint = toPoint.magnitude;
            if (distToPoint > 0.05f && Physics.Raycast(origin, toPoint.normalized, distToPoint - 0.05f))
                continue; // algo bloquea la linea de vision (pared u obstaculo)

            best = interactable;
            bestDist = dist;
        }

        return best;
    }
}