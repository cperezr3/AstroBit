using UnityEngine;

// Base comun para el feedback visual opcional de MissionStepPoint. Dos implementaciones por
// ahora: EmissiveToggle (enciende un vidrio/pantalla existente, usado por el Tv) y MissionBeacon
// (indicador de luz para puntos de mision sin representacion visual propia). Abstracta en vez de
// una interfaz para que Unity pueda serializar la referencia como campo normal del Inspector.
public abstract class StepFeedback : MonoBehaviour
{
    public abstract void Activate();
}
