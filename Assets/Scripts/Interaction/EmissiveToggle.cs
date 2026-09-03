using UnityEngine;

// Fase 3 (Prompt 01_maestro, secciones 13/14): hace que una superficie "encienda" visualmente al
// completar un paso de la mision, en vez de que la unica reaccion sea texto en el HUD. Pensado
// para el vidrio del Tv 32 Inch (ScifiOfficeLite) al abrir el archivo, pero generico para
// cualquier renderer con un shader que soporte _EmissionColor.
//
// renderer.material (no sharedMaterial) clona una instancia propia la primera vez que se accede,
// asi que activar la emision aqui NO modifica el asset de material compartido -- otras
// superficies de vidrio del mismo pack (puertas, paneles de seguridad) no se ven afectadas.
public class EmissiveToggle : StepFeedback
{
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private int materialIndex = 0;
    [SerializeField] private Color emissionColor = new Color(0.35f, 0.95f, 1f);
    [SerializeField] private float intensity = 2.5f;

    private bool activated;

    // Configuracion programatica para casos que no tienen wiring manual en la escena (ver
    // StorageServer, que agrega este componente por codigo sobre su propio submesh de vidrio).
    // El Inspector sigue funcionando igual para el caso ya cableado a mano (Tv 32 Inch).
    public void Configure(Renderer renderer, int materialIndex, Color color, float intensity)
    {
        targetRenderer = renderer;
        this.materialIndex = materialIndex;
        emissionColor = color;
        this.intensity = intensity;
    }

    public override void Activate()
    {
        if (activated || targetRenderer == null) return;
        activated = true;

        // .materials (no .material) clona una instancia propia de CADA elemento del array la
        // primera vez que se accede, incluyendo el resto de slots -- necesario para poder tocar
        // un slot que no sea el 0 (p.ej. el vidrio del server, materialIndex 1) sin afectar el
        // asset de material compartido de ningun slot.
        var mats = targetRenderer.materials;
        if (materialIndex < 0 || materialIndex >= mats.Length) return;
        var mat = mats[materialIndex];
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", emissionColor * intensity);
    }
}
