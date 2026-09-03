using TMPro;

// Control "< valor >" reutilizado por SettingsUI para opciones discretas (resolucion, calidad).
// No decide valores por su cuenta: solo recorre indices [0, optionCount) y avisa via onChanged.
public class CycleSelector : UnityEngine.MonoBehaviour
{
    private TextMeshProUGUI valueLabel;
    private int optionCount;
    private System.Func<int, string> labelForIndex;
    private UnityEngine.Events.UnityAction<int> onChanged;
    private int index;

    public void Init(TextMeshProUGUI valueLabel, int optionCount, System.Func<int, string> labelForIndex, UnityEngine.Events.UnityAction<int> onChanged)
    {
        this.valueLabel = valueLabel;
        this.optionCount = optionCount;
        this.labelForIndex = labelForIndex;
        this.onChanged = onChanged;
        RefreshLabel();
    }

    public void SetIndexWithoutNotify(int newIndex)
    {
        index = UnityEngine.Mathf.Clamp(newIndex, 0, optionCount - 1);
        RefreshLabel();
    }

    public void Previous()
    {
        if (optionCount <= 0) return;
        index = (index - 1 + optionCount) % optionCount;
        RefreshLabel();
        onChanged?.Invoke(index);
    }

    public void Next()
    {
        if (optionCount <= 0) return;
        index = (index + 1) % optionCount;
        RefreshLabel();
        onChanged?.Invoke(index);
    }

    private void RefreshLabel()
    {
        if (valueLabel == null || optionCount <= 0) return;
        valueLabel.text = labelForIndex(index);
    }
}
