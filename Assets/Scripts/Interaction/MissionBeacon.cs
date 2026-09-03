using UnityEngine;

// Fase 3 (Prompt 02_continuacion): marcador visual minimo para puntos de mision sin
// representacion fisica propia (FileMission_CpuProcess/RamLoad/RamExecute -- antes solo un
// BoxCollider trigger flotando en el aire, invisible para el jugador). Se evaluo traer un prop
// realista (Cosmic_Retro_Computer_1_FREE) pero desentonaba: esas salas ya tienen una identidad
// visual propia y deliberada (bloques de PCB / modulos de RAM a escala gigante, con cables y
// lineas luminosas) y un mueble de escritorio realista rompia tanto la escala como el estilo.
// Un indicador de luz encaja con ese lenguaje visual existente sin competir con el.
//
// Pulso tenue en reposo (para que la sala no se sienta muerta, seccion 12) y encendido solido al
// completarse el paso (para que el jugador "vea" el flujo de datos avanzar, seccion 13). Misma
// tecnica que EmissiveToggle (instanciar el material, no tocar el asset compartido).
public class MissionBeacon : StepFeedback
{
    [SerializeField] private Renderer targetRenderer;
    // Prompt 04_implement (data flow): algunos tramos (p.ej. los "Pipes" del piso de la Room CPU)
    // son varios renderers que deben pulsar/encender juntos como un solo tramo del flujo de datos,
    // no un unico indicador puntual. Aditivo a proposito -- targetRenderer sigue funcionando
    // exactamente igual que antes para los 3 MissionBeacon ya cableados a mano en la escena
    // (CpuProcess/RamLoad/RamExecute), esto solo agrega renderers extra opcionales al mismo pulso.
    [SerializeField] private Renderer[] extraRenderers;
    [SerializeField] private Color idleColor = new Color(0.35f, 0.95f, 1f);
    [SerializeField] private Color activeColor = new Color(0.4f, 1f, 0.5f);
    [SerializeField] private float idleMinIntensity = 0.15f;
    [SerializeField] private float idleMaxIntensity = 0.6f;
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float activeIntensity = 3f;

    private Material[] mats;
    private bool activated;

    private void Awake()
    {
        int count = (targetRenderer != null ? 1 : 0) + (extraRenderers?.Length ?? 0);
        if (count == 0) return;

        mats = new Material[count];
        int i = 0;
        if (targetRenderer != null) mats[i++] = targetRenderer.material;
        if (extraRenderers != null)
        {
            foreach (var r in extraRenderers)
                if (r != null) mats[i++] = r.material;
        }

        foreach (var m in mats)
            m?.EnableKeyword("_EMISSION");
    }

    private void Update()
    {
        if (activated || mats == null) return;
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;
        float intensity = Mathf.Lerp(idleMinIntensity, idleMaxIntensity, t);
        SetAll(idleColor * intensity);
    }

    public override void Activate()
    {
        if (activated || mats == null) return;
        activated = true;
        SetAll(activeColor * activeIntensity);
    }

    private void SetAll(Color c)
    {
        foreach (var m in mats)
            m?.SetColor("_EmissionColor", c);
    }
}
