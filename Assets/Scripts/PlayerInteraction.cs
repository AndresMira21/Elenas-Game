using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera cam;
    public float distance = 3f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, distance))
            {
                Debug.Log("Hit: " + hit.collider.name);

                // 🔵 Decision objects
                DecisionObject d = hit.collider.GetComponentInParent<DecisionObject>();
                if (d != null)
                {
                    d.Inspect();
                    return;
                }

                // 🟡 Inspectable objects (SIN decisiones)
                InspectableObject iObj = hit.collider.GetComponentInParent<InspectableObject>();
                if (iObj != null)
                {
                    iObj.Inspect();
                    return;
                }

                // 🛏 cama
                BedSleep bed = hit.collider.GetComponentInParent<BedSleep>();
                if (bed != null)
                {
                    bed.Interact();
                    return;
                }

                Debug.Log("Nada interactuable aquí");
            }
        }
    }
}