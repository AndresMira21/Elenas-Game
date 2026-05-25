using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIRaycastDebugger : MonoBehaviour
{
    void Update()
    {
        // Solo cuando presionas click izquierdo
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("---- UI RAYCAST DEBUG ----");

            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();

            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count == 0)
            {
                Debug.Log("❌ No UI detectada bajo el mouse");
                return;
            }

            Debug.Log("✔ Objetos detectados en orden (de arriba a abajo):");

            foreach (var r in results)
            {
                Debug.Log("→ " + r.gameObject.name + " | layer: " + LayerMask.LayerToName(r.gameObject.layer));
            }

            Debug.Log("---------------------------");
        }
    }
}