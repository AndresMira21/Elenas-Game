using UnityEngine;

public class NarrationZone : MonoBehaviour
{
    [TextArea]
    public string narration;

    public float duration = 4f;

    public bool countAsExploration = true;

    bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated)
            return;

        if (other.CompareTag("Player"))
        {
            activated = true;

            DialogueManager.Instance.ShowThought(narration, duration);

            if (countAsExploration)
            {
                GameManager.Instance.RegisterExploration();
            }
        }
    }
}