using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SimpleInteractable : MonoBehaviour, IInteractable
{
    [Header("Prompt")]
    [SerializeField] private string promptText = "[E] Interactuar";

    [Header("Feedback")]
    [TextArea]
    [SerializeField] private string feedbackText = "";
    [SerializeField] private float feedbackHoldTime = 2.5f;

    [Header("Objetivo")]
    [SerializeField] private string objectiveCompletedText = "";
    [SerializeField] private string nextObjectiveText = "";
    [SerializeField] private float nextObjectiveDelay = 2f;

    [Header("Comportamiento")]
    [SerializeField] private bool oneShot = true;

    public UnityEvent OnInteracted;

    private bool interacted;

    public string PromptText => promptText;
    public bool CanInteract => !oneShot || !interacted;

    public void Interact()
    {
        if (!CanInteract) return;
        interacted = true;

        StartCoroutine(InteractSequence());
        OnInteracted?.Invoke();
    }

    private IEnumerator InteractSequence()
    {
        if (!string.IsNullOrEmpty(feedbackText))
        {
            GameHUD.Instance?.ShowFeedback(feedbackText);
            yield return new WaitForSeconds(feedbackHoldTime);
        }

        if (!string.IsNullOrEmpty(objectiveCompletedText))
        {
            ObjectiveSystem.Instance.CompleteObjective(objectiveCompletedText);

            if (!string.IsNullOrEmpty(nextObjectiveText))
            {
                yield return new WaitForSeconds(nextObjectiveDelay);
                ObjectiveSystem.Instance.SetObjective(nextObjectiveText);
            }
        }
    }
}
