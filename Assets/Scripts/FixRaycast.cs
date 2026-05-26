using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Adjunta este script al Canvas y ejecuta "Fix All Raycasts" desde el Inspector.
/// Desactiva Raycast Target en todos los elementos que no son botones.
/// Elimina este script después de usarlo.
/// </summary>
public class FixRaycast : MonoBehaviour
{
    [ContextMenu("Fix All Raycasts")]
    public void FixAllRaycasts()
    {
        // Desactiva RaycastTarget en todos los Graphic excepto botones
        Graphic[] allGraphics = GetComponentsInChildren<Graphic>(true);
        int fixed_count = 0;

        foreach (Graphic g in allGraphics)
        {
            // Solo mantener Raycast en botones
            Button btn = g.GetComponent<Button>();
            Button btnParent = g.GetComponentInParent<Button>();

            if (btn == null && btnParent == null)
            {
                g.raycastTarget = false;
                fixed_count++;
            }
        }

        Debug.Log($"✅ Fixed {fixed_count} raycast targets. Buttons preserved.");
    }
}
