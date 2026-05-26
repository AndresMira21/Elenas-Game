using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [TextArea]
    public string message;

    [Header("Días activos")]
    public int activeFromDay = 1;
    public int activeUntilDay = 999;

    [Header("¿Solo una vez?")]
    public bool oneUseOnly = false;

    public float messageTime = 2f;

    bool used = false;

    bool CanUse()
    {
        int day = GameManager.Instance.currentDay;

        return day >= activeFromDay &&
               day <= activeUntilDay;
    }

    public void Interact()
    {
        if (!CanUse())
            return;

        if (oneUseOnly && used)
            return;

        used = true;

        GameManager.Instance.RegisterExploration();

        DialogueManager.Instance.ShowThought(
            message,
            messageTime
        );

        Debug.Log(message);
    }
}