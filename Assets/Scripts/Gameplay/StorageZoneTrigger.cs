using UnityEngine;

// Complementa LocationZone (que solo actualiza el indicador visual "ALMACENAMIENTO") arrancando
// la mision de almacenamiento la primera vez que el jugador entra a esa sala. Vive en el mismo
// GameObject/collider que Zone_Storage; no duplica ni reemplaza LocationZone.
[RequireComponent(typeof(Collider))]
public class StorageZoneTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        StorageMission.Instance.BeginIfNeeded();
    }
}
