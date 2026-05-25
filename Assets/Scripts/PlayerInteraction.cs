using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 3f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                Debug.Log("🎯 Hit: " + hit.collider.name);

                // 1. DECISION OBJECT
                DecisionObject decision = hit.collider.GetComponent<DecisionObject>();
                if (decision != null)
                {
                    decision.Inspect();
                    return;
                }

                // 2. CAMA
                BedSleep bed = hit.collider.GetComponent<BedSleep>();
                if (bed != null)
                {
                    bed.Interact();
                    return;
                }

                // 3. SOLO TEXTO
                InspectableObject obj = hit.collider.GetComponent<InspectableObject>();
                if (obj != null)
                {
                    obj.Inspect();
                }
            }
        }
    }
}