using UnityEngine;

// Zona simple de ubicacion: un Collider (Is Trigger) que, al ser cruzado por el jugador,
// actualiza el indicador superior de GameHUD. Solo puede haber una ubicacion activa a la vez.
[RequireComponent(typeof(Collider))]
public class LocationZone : MonoBehaviour
{
    [SerializeField] private string locationName = "ZONA";
    [TextArea(1, 2)]
    [SerializeField] private string locationDescription = "";

    private static LocationZone current;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsPlayer(other)) return;
        current = this;
        ObjectiveSystem.Instance.BeginSequenceIfNeeded();
        GameHUD.Instance?.SetLocation(locationName, locationDescription);
        // Prompt 07 (Bloque 4): unico punto de "entro a una zona nombrada" -- cubre las
        // transiciones CPU -> RAM -> Almacenamiento sin duplicar deteccion de zona.
        AudioManager.Instance?.PlayRoomTransition();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsPlayer(other)) return;
        if (current != this) return;

        current = null;
        GameHUD.Instance?.SetLocation("", "");
    }

    private static bool IsPlayer(Collider other)
    {
        return other.gameObject.layer == LayerMask.NameToLayer("Player");
    }
}
