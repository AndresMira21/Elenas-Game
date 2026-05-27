using UnityEngine;

public class TestSkip : MonoBehaviour
{
    void Update()
    {
        // Saltar de nivel
        if (Input.GetKeyDown(KeyCode.T))
            GameManager.Instance.Sleep();

        // Forzar estados
        if (Input.GetKeyDown(KeyCode.D))
        {
            GameManager.Instance.duda += 5;
            Debug.Log("Forzado: Duda +5 — Total: " + GameManager.Instance.duda);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            GameManager.Instance.negacion += 5;
            Debug.Log("Forzado: Negacion +5 — Total: " + GameManager.Instance.negacion);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            GameManager.Instance.aceptacion += 5;
            Debug.Log("Forzado: Aceptacion +5 — Total: " + GameManager.Instance.aceptacion);
        }

        // Reset estados
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.Instance.negacion = 0;
            GameManager.Instance.duda = 0;
            GameManager.Instance.aceptacion = 0;
            Debug.Log("Estados reseteados");
        }
    }
}