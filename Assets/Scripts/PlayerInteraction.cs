using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 3f;

    bool canInteract = true;

    void Update()
    {
        if (!canInteract) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                InspectableObject obj = hit.collider.GetComponent<InspectableObject>();

                if (obj != null)
                {
                    obj.Inspect();
                }
            }
        }
    }

    public void SetInteract(bool value)
    {
        canInteract = value;
    }
}