using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string message;
    public int activeFromDay = 1;

    bool used;

    bool CanUse()
    {
        return GameManager.Instance.currentDay >= activeFromDay;
    }

    public void Interact()
    {
        if (!CanUse())
        {
            Debug.Log("No disponible hoy");
            return;
        }

        if (used) return;

        used = true;

        GameManager.Instance.RegisterExploration();

        Debug.Log(message);
    }
}