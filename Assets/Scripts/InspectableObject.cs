using UnityEngine;

public class InspectableObject : MonoBehaviour
{
    [TextArea]
    public string message;

    public void Inspect()
    {
        InspectionSystem.Instance.StartInspection(message);
    }
}