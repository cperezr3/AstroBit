using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Prompt 06 (Bloque 3): UI de remapeo de controles dentro de la pestaña "Controles" de
// SettingsUI. Solo construye filas y traduce clicks a llamadas del Input System
// (InputActionRebindingExtensions.PerformInteractiveRebinding) -- no decide ni guarda nada por su
// cuenta: GameInput es quien persiste los overrides (ver GameInput.SaveBindingOverrides).
//
// Alcance deliberado (no todo binding tiene sentido remapear):
// - Movimiento: solo la tecla primaria (WASD) es remapeable. Las flechas del teclado siguen
//   funcionando siempre como alternativa fija -- ver ObtenerIndiceBinding, que busca el binding
//   por su path ORIGINAL ("<Keyboard>/w"), no el de las flechas. El stick izquierdo del mando no
//   es remapeable (no tiene sentido reasignar "cual stick es cual" en un mando estandar).
// - Interactuar/Pausa: remapeables tanto en teclado como en boton de mando (son botones
//   digitales simples, a diferencia de un stick).
// - Mirar: informativo unicamente (stick derecho del mando). El mouse no pasa por una accion del
//   Input System (ver GameInput.GetCinemachineInputAxis) y rebindear "el mouse" no es algo que la
//   mayoria de los juegos ofrezcan de todas formas -- la sensibilidad ya es configurable aparte.
public class ControlsRebindingPanel : MonoBehaviour
{
    private static readonly Color AccentCyan = new Color(0.35f, 0.95f, 1f);
    private static readonly Color SectionColor = new Color(0.7f, 0.85f, 0.9f);
    private static readonly Color InfoColor = new Color(0.6f, 0.65f, 0.68f);

    private const float RowHeight = 42f;
    private const float InfoRowHeight = 26f;

    private struct RebindableRow
    {
        public InputAction Action;
        public int BindingIndex;
        public TextMeshProUGUI BindingLabel;
        public Button ReassignButton;
        public string DeviceMatchPath; // "<Keyboard>" o "<Gamepad>", para restringir que control puede capturar el rebind
    }

    private readonly List<RebindableRow> rows = new List<RebindableRow>();
    private InputActionRebindingExtensions.RebindingOperation activeRebind;

    private float labelX;
    private float controlX;
    private float controlWidth;

    public void Init(Transform parent, float startY, float labelXValue, float controlXValue, float controlWidthValue)
    {
        labelX = labelXValue;
        controlX = controlXValue;
        controlWidth = controlWidthValue;

        var move = GameInput.Instance.MoveAction;
        var interact = GameInput.Instance.InteractAction;
        var pause = GameInput.Instance.PauseAction;

        float y = startY;
        CreateSectionLabel(parent, "MOVIMIENTO (TECLADO)", y); y -= RowHeight;
        CreateRebindRow(parent, "Arriba", y, move, FindBindingIndexByOriginalPath(move, "<Keyboard>/w"), "<Keyboard>"); y -= RowHeight;
        CreateRebindRow(parent, "Abajo", y, move, FindBindingIndexByOriginalPath(move, "<Keyboard>/s"), "<Keyboard>"); y -= RowHeight;
        CreateRebindRow(parent, "Izquierda", y, move, FindBindingIndexByOriginalPath(move, "<Keyboard>/a"), "<Keyboard>"); y -= RowHeight;
        CreateRebindRow(parent, "Derecha", y, move, FindBindingIndexByOriginalPath(move, "<Keyboard>/d"), "<Keyboard>"); y -= RowHeight;
        CreateInfoRow(parent, "Las flechas del teclado y el stick izquierdo del mando siempre funcionan como alternativa fija.", y); y -= (InfoRowHeight + 14f);

        CreateSectionLabel(parent, "ACCIONES", y); y -= RowHeight;
        CreateRebindRow(parent, "Interactuar (teclado)", y, interact, interact.GetBindingIndex(InputBinding.MaskByGroup("Keyboard&Mouse")), "<Keyboard>"); y -= RowHeight;
        CreateRebindRow(parent, "Interactuar (mando)", y, interact, interact.GetBindingIndex(InputBinding.MaskByGroup("Gamepad")), "<Gamepad>"); y -= RowHeight;
        CreateRebindRow(parent, "Pausa (teclado)", y, pause, pause.GetBindingIndex(InputBinding.MaskByGroup("Keyboard&Mouse")), "<Keyboard>"); y -= RowHeight;
        CreateRebindRow(parent, "Pausa (mando)", y, pause, pause.GetBindingIndex(InputBinding.MaskByGroup("Gamepad")), "<Gamepad>"); y -= RowHeight;
        CreateInfoRow(parent, "Mirar: mouse (siempre activo) y stick derecho del mando (fijos, ver sensibilidad arriba).", y); y -= (InfoRowHeight + 20f);

        CreateResetButton(parent, y);

        GameInput.Instance.OnBindingsChanged.AddListener(RefreshLabels);
        RefreshLabels();
    }

    public void RefreshLabels()
    {
        foreach (var row in rows)
        {
            if (row.BindingIndex < 0) { row.BindingLabel.text = "--"; continue; }
            row.BindingLabel.text = GetBindingDisplayString(row.Action, row.BindingIndex);
        }
    }

    private static string GetBindingDisplayString(InputAction action, int bindingIndex)
    {
        return action.GetBindingDisplayString(bindingIndex, InputBinding.DisplayStringOptions.DontIncludeInteractions);
    }

    // Busca un binding por su path ORIGINAL (sin overrides) -- a diferencia de .effectivePath,
    // .path no cambia cuando el jugador ya remapeo esa fila antes, asi que esto sigue encontrando
    // "el slot de W" aunque ahora apunte a otra tecla.
    private static int FindBindingIndexByOriginalPath(InputAction action, string originalPath)
    {
        var bindings = action.bindings;
        for (int i = 0; i < bindings.Count; i++)
        {
            if (bindings[i].path == originalPath) return i;
        }
        return -1;
    }

    private void CreateRebindRow(Transform parent, string label, float y, InputAction action, int bindingIndex, string deviceMatchPath)
    {
        var labelGO = new GameObject("Label_" + label, typeof(RectTransform));
        labelGO.transform.SetParent(parent, false);
        var labelRT = labelGO.GetComponent<RectTransform>();
        labelRT.anchorMin = labelRT.anchorMax = labelRT.pivot = new Vector2(0f, 1f);
        labelRT.anchoredPosition = new Vector2(labelX, y);
        labelRT.sizeDelta = new Vector2(240, RowHeight - 8f);
        var labelTmp = labelGO.AddComponent<TextMeshProUGUI>();
        labelTmp.text = label;
        labelTmp.fontSize = 16;
        labelTmp.alignment = TextAlignmentOptions.Left;
        labelTmp.color = Color.white;

        var bindingGO = new GameObject("Binding_" + label, typeof(RectTransform));
        bindingGO.transform.SetParent(parent, false);
        var bindingRT = bindingGO.GetComponent<RectTransform>();
        bindingRT.anchorMin = bindingRT.anchorMax = bindingRT.pivot = new Vector2(0f, 1f);
        bindingRT.anchoredPosition = new Vector2(controlX, y);
        bindingRT.sizeDelta = new Vector2(controlWidth - 150f, RowHeight - 8f);
        var bindingTmp = bindingGO.AddComponent<TextMeshProUGUI>();
        bindingTmp.fontSize = 16;
        bindingTmp.fontStyle = FontStyles.Bold;
        bindingTmp.alignment = TextAlignmentOptions.Left;
        bindingTmp.color = AccentCyan;

        var buttonGO = new GameObject("Reassign_" + label, typeof(RectTransform));
        buttonGO.transform.SetParent(parent, false);
        var buttonRT = buttonGO.GetComponent<RectTransform>();
        buttonRT.anchorMin = buttonRT.anchorMax = new Vector2(0f, 1f);
        buttonRT.pivot = new Vector2(0f, 1f);
        buttonRT.anchoredPosition = new Vector2(controlX + controlWidth - 140f, y);
        buttonRT.sizeDelta = new Vector2(140f, RowHeight - 8f);
        var buttonImg = buttonGO.AddComponent<Image>();
        buttonImg.color = new Color(1f, 1f, 1f, 0.1f);
        var button = buttonGO.AddComponent<Button>();
        button.targetGraphic = buttonImg;
        var colors = button.colors;
        colors.highlightedColor = new Color(1f, 1f, 1f, 0.22f);
        colors.pressedColor = new Color(0f, 0.545f, 0.545f, 1f);
        colors.disabledColor = new Color(1f, 1f, 1f, 0.05f);
        button.colors = colors;

        var buttonLabelGO = new GameObject("Label", typeof(RectTransform));
        buttonLabelGO.transform.SetParent(buttonGO.transform, false);
        var buttonLabelRT = buttonLabelGO.GetComponent<RectTransform>();
        buttonLabelRT.anchorMin = Vector2.zero;
        buttonLabelRT.anchorMax = Vector2.one;
        buttonLabelRT.offsetMin = Vector2.zero;
        buttonLabelRT.offsetMax = Vector2.zero;
        var buttonLabelTmp = buttonLabelGO.AddComponent<TextMeshProUGUI>();
        buttonLabelTmp.text = "REASIGNAR";
        buttonLabelTmp.fontSize = 13;
        buttonLabelTmp.fontStyle = FontStyles.Bold;
        buttonLabelTmp.alignment = TextAlignmentOptions.Center;
        buttonLabelTmp.color = Color.white;

        var row = new RebindableRow
        {
            Action = action,
            BindingIndex = bindingIndex,
            BindingLabel = bindingTmp,
            ReassignButton = button,
            DeviceMatchPath = deviceMatchPath,
        };
        rows.Add(row);

        if (bindingIndex < 0)
        {
            // No deberia pasar en condiciones normales (el asset siempre trae estos bindings),
            // pero deshabilitar el boton en vez de fallar silenciosamente evita un rebind sobre
            // un indice invalido si algun dia cambia el asset y alguien olvida actualizar esto.
            button.interactable = false;
            bindingTmp.text = "--";
            return;
        }

        button.onClick.AddListener(() => AudioManager.Instance?.PlayUiClick());
        button.onClick.AddListener(() => StartRebind(row));
    }

    private void CreateInfoRow(Transform parent, string text, float y)
    {
        var go = new GameObject("Info", typeof(RectTransform));
        go.transform.SetParent(parent, false);
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0f, 1f);
        rt.anchoredPosition = new Vector2(labelX, y);
        rt.sizeDelta = new Vector2(controlX + controlWidth - labelX, InfoRowHeight);
        var tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 13;
        tmp.fontStyle = FontStyles.Italic;
        tmp.alignment = TextAlignmentOptions.Left;
        tmp.color = InfoColor;
        tmp.textWrappingMode = TextWrappingModes.Normal;
    }

    private void CreateSectionLabel(Transform parent, string text, float y)
    {
        var go = new GameObject("Section_" + text, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0f, 1f);
        rt.anchoredPosition = new Vector2(labelX, y);
        rt.sizeDelta = new Vector2(400, 26);
        var tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 20;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Left;
        tmp.color = SectionColor;
    }

    private void CreateResetButton(Transform parent, float y)
    {
        var btnGO = new GameObject("ResetBindings", typeof(RectTransform));
        btnGO.transform.SetParent(parent, false);
        var rt = btnGO.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0f, 1f);
        rt.anchoredPosition = new Vector2(labelX, y);
        rt.sizeDelta = new Vector2(320f, 44f);

        var img = btnGO.AddComponent<Image>();
        img.color = new Color(1f, 1f, 1f, 0.1f);
        var button = btnGO.AddComponent<Button>();
        button.targetGraphic = img;
        var colors = button.colors;
        colors.highlightedColor = new Color(1f, 0.4f, 0.35f, 0.6f);
        colors.pressedColor = new Color(0.6f, 0.2f, 0.15f, 1f);
        button.colors = colors;
        button.onClick.AddListener(() => AudioManager.Instance?.PlayUiClick());
        button.onClick.AddListener(() => GameInput.Instance.ResetBindingOverrides());

        var outline = btnGO.AddComponent<Outline>();
        outline.effectColor = new Color(1f, 0.5f, 0.45f, 0.8f);
        outline.effectDistance = new Vector2(1.5f, -1.5f);

        var labelGO = new GameObject("Label", typeof(RectTransform));
        labelGO.transform.SetParent(btnGO.transform, false);
        var labelRT = labelGO.GetComponent<RectTransform>();
        labelRT.anchorMin = Vector2.zero;
        labelRT.anchorMax = Vector2.one;
        labelRT.offsetMin = Vector2.zero;
        labelRT.offsetMax = Vector2.zero;
        var labelTmp = labelGO.AddComponent<TextMeshProUGUI>();
        labelTmp.text = "RESTAURAR VALORES POR DEFECTO";
        labelTmp.fontSize = 14;
        labelTmp.fontStyle = FontStyles.Bold;
        labelTmp.alignment = TextAlignmentOptions.Center;
        labelTmp.color = Color.white;
    }

    // ---- Rebind interactivo ----

    private void StartRebind(RebindableRow row)
    {
        if (activeRebind != null) return; // ya hay un rebind en curso, ignorar clicks adicionales

        row.Action.Disable();
        row.BindingLabel.text = "Presiona una tecla...";
        row.ReassignButton.interactable = false;

        var operation = row.Action.PerformInteractiveRebinding(row.BindingIndex)
            .WithControlsExcluding("Mouse")
            .WithControlsHavingToMatchPath(row.DeviceMatchPath)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnCancel(_ => FinishRebind(row))
            .OnComplete(_ => OnRebindComplete(row));

        activeRebind = operation.Start();
    }

    private void OnRebindComplete(RebindableRow row)
    {
        // Prompt 06 (Bloque 3, seccion 5): conflicto basico -- si la tecla/boton recien asignado
        // ya esta en uso por otro binding del mismo mapa, se revierte en vez de dejar dos acciones
        // respondiendo al mismo control. Comparacion via InputBinding.effectivePath/.id, provistos
        // por el propio Input System -- no se reimplementa deteccion de tecla a mano.
        if (IsDuplicateBinding(row.Action, row.BindingIndex))
        {
            row.Action.RemoveBindingOverride(row.BindingIndex);
            // Prompt 07 (Bloque 4): conflicto de rebinding -- uno de los dos ejemplos de error
            // explicitamente nombrados en el prompt (el otro es "RAM insuficiente", en StorageMission).
            AudioManager.Instance?.PlayError();
        }
        else
        {
            GameInput.Instance.SaveBindingOverrides();
        }
        FinishRebind(row);
    }

    private void FinishRebind(RebindableRow row)
    {
        activeRebind?.Dispose();
        activeRebind = null;
        row.Action.Enable();
        row.ReassignButton.interactable = true;
        row.BindingLabel.text = GetBindingDisplayString(row.Action, row.BindingIndex);
    }

    private static bool IsDuplicateBinding(InputAction action, int bindingIndex)
    {
        var newBinding = action.bindings[bindingIndex];
        foreach (var binding in action.actionMap.bindings)
        {
            if (binding.isComposite) continue;
            if (binding.id == newBinding.id) continue;
            if (binding.effectivePath == newBinding.effectivePath) return true;
        }
        return false;
    }

    private void OnDestroy()
    {
        activeRebind?.Dispose();
        if (GameInput.Instance != null) GameInput.Instance.OnBindingsChanged.RemoveListener(RefreshLabels);
    }
}
