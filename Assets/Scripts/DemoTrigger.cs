using UnityEngine;

/// <summary>
/// DemoTrigger.cs — Elena's Game
/// Detecta cuando el día llega a 6 y activa el final de la demo
/// sin cambiar de escena.
/// Ponlo en el mismo GameObject que el GameManager.
/// </summary>
public class DemoTrigger : MonoBehaviour
{
    [Tooltip("En qué día se activa el final (6 = después de dormir la noche 5)")]
    public int diaFinal = 6;

    [Tooltip("Arrastra aquí el GameObject que tiene DemoEnding.cs")]
    public DemoEnding demoEnding;

    bool _finalActivado = false;

    /// Otros scripts pueden consultar esto para saber si la demo terminó
    public static bool DemoTerminada { get; private set; } = false;

    void Update()
    {
        if (_finalActivado) return;
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.currentDay >= diaFinal)
        {
            _finalActivado = true;
            DemoTerminada = true;

            Debug.Log("🎬 Demo terminada — mostrando pantalla final...");

            if (demoEnding != null)
                demoEnding.MostrarFinal();
            else
                Debug.LogError("❌ DemoEnding no asignado en el Inspector");
        }
    }
}