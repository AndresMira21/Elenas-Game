using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [TextArea]
    public string message;

    [Header("Disponible desde el día")]
    public int activeFromDay = 1;

    [Header("¿Solo una vez?")]
    public bool oneUseOnly = false;

    public float messageTime = 2f;

    bool used = false;

    bool CanUse()
    {
        return GameManager.Instance.currentDay >= activeFromDay;
    }

    public void Interact()
    {
        // 🚫 Aún no disponible
        if (!CanUse())
        {
            return;
        }

        // 🚫 Ya usado
        if (oneUseOnly && used)
        {
            return;
        }

        used = true;

        GameManager.Instance.RegisterExploration();

        // ✅ MENSAJE EN PANTALLA
        DialogueManager.Instance.ShowThought(message, messageTime);

        Debug.Log(message);
    }
}