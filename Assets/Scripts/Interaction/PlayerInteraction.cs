using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float interactionDistance = 3f;

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Interactuar();
        }
    }

    void Interactuar()
    {
        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            Debug.Log("Estoy mirando: " + hit.collider.gameObject.name);
        }
    }
}