using UnityEngine;

public class NarrationZone : MonoBehaviour
{
    [TextArea]
    public string narration;

    [Header("Duración")]
    public float duration = 4f;

    [Header("Días permitidos")]
    public int activeFromDay = 1;
    public int activeUntilDay = 999;

    [Header("Exploración")]
    public bool countAsExploration = true;

    [Header("¿Solo una vez?")]
    public bool playOnlyOnce = true;

    bool activated = false;

    bool CanActivate()
    {
        int day = GameManager.Instance.currentDay;

        return day >= activeFromDay &&
               day <= activeUntilDay;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ya se activó
        if (playOnlyOnce && activated)
            return;

        // No es el jugador
        if (!other.CompareTag("Player"))
            return;

        // Día incorrecto
        if (!CanActivate())
            return;

        activated = true;

        DialogueManager.Instance.ShowThought(
            narration,
            duration
        );

        if (countAsExploration)
        {
            GameManager.Instance.RegisterExploration();
        }
    }
}