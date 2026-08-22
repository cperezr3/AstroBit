using UnityEngine;
using UnityEngine.UI;

// Marcador amarillo de objetivo en el mundo (Prompt 23, Parte 2/3). Solo lee
// MissionNavigation.Instance.CurrentTarget cada frame -- no decide logica de mision, solo la
// representa. Mismo patron de billboard hacia Camera.main que WorldLabel.
public class WorldObjectiveMarker : MonoBehaviour
{
    private const float HoverHeight = 3.2f;
    private const float BobAmplitude = 0.15f;
    private const float BobSpeed = 2f;

    private GameObject markerRoot;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        var go = new GameObject("WorldObjectiveMarker");
        go.AddComponent<WorldObjectiveMarker>();
        DontDestroyOnLoad(go);
    }

    private void Awake()
    {
        BuildMarker();
    }

    private void BuildMarker()
    {
        markerRoot = new GameObject("ObjectiveMarker_World");

        var canvas = markerRoot.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        var rootRT = markerRoot.GetComponent<RectTransform>();
        rootRT.sizeDelta = new Vector2(200, 260);
        markerRoot.transform.localScale = Vector3.one * 0.01f;

        var dotGO = new GameObject("Dot");
        dotGO.transform.SetParent(markerRoot.transform, false);
        var dotImage = dotGO.AddComponent<Image>();
        dotImage.sprite = NavIcons.CircleSprite;
        dotImage.color = new Color(1f, 0.85f, 0.1f, 0.95f);
        var dotRT = dotGO.GetComponent<RectTransform>();
        dotRT.anchorMin = dotRT.anchorMax = dotRT.pivot = new Vector2(0.5f, 0.5f);
        dotRT.sizeDelta = new Vector2(70, 70);
        dotRT.anchoredPosition = new Vector2(0, -95);

        var arrowGO = new GameObject("Arrow");
        arrowGO.transform.SetParent(markerRoot.transform, false);
        var arrowImage = arrowGO.AddComponent<Image>();
        arrowImage.sprite = NavIcons.TriangleSprite; // apunta hacia arriba por defecto
        arrowImage.color = new Color(1f, 0.85f, 0.1f, 0.95f);
        var arrowRT = arrowGO.GetComponent<RectTransform>();
        arrowRT.anchorMin = arrowRT.anchorMax = arrowRT.pivot = new Vector2(0.5f, 0.5f);
        arrowRT.sizeDelta = new Vector2(55, 55);
        arrowRT.anchoredPosition = Vector2.zero;
        arrowRT.localRotation = Quaternion.Euler(0, 0, 180); // invertido: debe apuntar hacia abajo, hacia el punto

        markerRoot.SetActive(false);
    }

    private void Update()
    {
        var target = MissionNavigation.Instance.CurrentTarget;
        if (target == null)
        {
            if (markerRoot.activeSelf) markerRoot.SetActive(false);
            return;
        }

        var cam = Camera.main;
        if (cam == null) return;

        if (!markerRoot.activeSelf) markerRoot.SetActive(true);

        float bob = Mathf.Sin(Time.time * BobSpeed) * BobAmplitude;
        Vector3 pos = target.position + Vector3.up * (HoverHeight + bob);
        markerRoot.transform.position = pos;

        Vector3 direction = pos - cam.transform.position;
        if (direction.sqrMagnitude > 0.0001f)
            markerRoot.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }
}
