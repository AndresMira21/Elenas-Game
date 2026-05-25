using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera cam;
    public float distance = 3f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(
                cam.transform.position,
                cam.transform.forward,
                out RaycastHit hit,
                distance))
            {
                Debug.Log("Hit: " + hit.collider.name);

                // 🔵 OBJETOS CON DECISIONES
                DecisionObject d =
                    hit.collider.GetComponentInParent<DecisionObject>();

                if (d != null)
                {
                    d.Inspect();
                    return;
                }

                // 🟡 OBJETOS SIEMPRE INSPECTABLES
                InspectableObject inspect =
                    hit.collider.GetComponentInParent<InspectableObject>();

                if (inspect != null)
                {
                    inspect.Inspect();
                    return;
                }

                // 🟠 OBJETOS POR DÍA
                InteractableObject interact =
                    hit.collider.GetComponentInParent<InteractableObject>();

                if (interact != null)
                {
                    interact.Interact();
                    return;
                }

                // 🛏 CAMA
                BedSleep bed =
                    hit.collider.GetComponentInParent<BedSleep>();

                if (bed != null)
                {
                    bed.Interact();
                    return;
                }

                Debug.Log("Nada interactuable.");
            }
        }
    }
}