using UnityEngine;
using UnityEngine.UI;

// Etiqueta flotante generica para interactuables fuera de CPU/RAM (mismo patron visual que
// el label de EducationalInteractable, pero como componente aparte para no tocar ese script).
// Se muestra solo por proximidad al jugador; el [E] lo sigue mostrando GameHUD por separado.
public class WorldLabel : MonoBehaviour
{
    private Transform target;
    private Transform playerTransform;
    private float proximityRadius;
    private float labelHeight;

    private GameObject labelRoot;
    private bool labelVisible;

    public void Init(Transform followTarget, string title, string subtitle, float radius, float height)
    {
        target = followTarget;
        proximityRadius = radius;
        labelHeight = height;
        BuildLabel(title, subtitle);
    }

    private void BuildLabel(string title, string subtitle)
    {
        labelRoot = new GameObject(name + "_WorldLabel");

        var canvas = labelRoot.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        var rt = labelRoot.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(400, 100);
        labelRoot.transform.localScale = Vector3.one * 0.012f;

        var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        var titleGO = new GameObject("Title");
        titleGO.transform.SetParent(labelRoot.transform, false);
        var titleText = titleGO.AddComponent<Text>();
        titleText.font = font;
        titleText.fontSize = 34;
        titleText.fontStyle = FontStyle.Bold;
        titleText.alignment = TextAnchor.LowerCenter;
        titleText.color = new Color(0.35f, 0.95f, 1f);
        titleText.text = title;
        var titleRT = titleGO.GetComponent<RectTransform>();
        titleRT.anchorMin = titleRT.anchorMax = titleRT.pivot = new Vector2(0.5f, 1f);
        titleRT.sizeDelta = new Vector2(400, 45);
        titleRT.anchoredPosition = Vector2.zero;
        var titleOutline = titleGO.AddComponent<Outline>();
        titleOutline.effectColor = new Color(0f, 0f, 0f, 0.9f);
        titleOutline.effectDistance = new Vector2(1.5f, -1.5f);

        var subtitleGO = new GameObject("Subtitle");
        subtitleGO.transform.SetParent(labelRoot.transform, false);
        var subtitleText = subtitleGO.AddComponent<Text>();
        subtitleText.font = font;
        subtitleText.fontSize = 22;
        subtitleText.alignment = TextAnchor.UpperCenter;
        subtitleText.color = Color.white;
        subtitleText.text = subtitle;
        var subtitleRT = subtitleGO.GetComponent<RectTransform>();
        subtitleRT.anchorMin = subtitleRT.anchorMax = subtitleRT.pivot = new Vector2(0.5f, 1f);
        subtitleRT.sizeDelta = new Vector2(400, 40);
        subtitleRT.anchoredPosition = new Vector2(0, -45);
        var subtitleOutline = subtitleGO.AddComponent<Outline>();
        subtitleOutline.effectColor = new Color(0f, 0f, 0f, 0.9f);
        subtitleOutline.effectDistance = new Vector2(1.5f, -1.5f);

        labelRoot.SetActive(false);
    }

    private void Update()
    {
        var cam = Camera.main;
        if (cam == null || labelRoot == null || target == null) return;

        if (playerTransform == null)
        {
            var movement = FindFirstObjectByType<MovementInput>();
            if (movement != null) playerTransform = movement.transform;
        }
        if (playerTransform == null) return;

        float dist = Vector3.Distance(playerTransform.position, target.position);
        bool shouldShow = dist <= proximityRadius;

        if (shouldShow != labelVisible)
        {
            labelVisible = shouldShow;
            labelRoot.SetActive(labelVisible);
        }

        if (labelVisible)
        {
            Vector3 labelPos = target.position + Vector3.up * labelHeight;
            labelRoot.transform.position = labelPos;

            Vector3 direction = labelPos - cam.transform.position;
            if (direction.sqrMagnitude > 0.0001f)
                labelRoot.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }

    private void OnDestroy()
    {
        if (labelRoot != null) Destroy(labelRoot);
    }
}
