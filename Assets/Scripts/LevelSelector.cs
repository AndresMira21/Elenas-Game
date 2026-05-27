using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// LevelSelector.cs — Elena's Game
/// Ponlo en el panel de niveles del MainMenu.
/// Cada botón de nivel llama a CargarDia(int dia).
/// </summary>
public class LevelSelector : MonoBehaviour
{
    [Tooltip("Nombre exacto de la escena del juego en Build Settings")]
    public string escenaJuego = "SampleScene";

    public void CargarDia(int dia)
    {
        // Guarda el día en PlayerPrefs para que SampleScene lo lea al cargar
        PlayerPrefs.SetInt("DiaInicial", dia);
        PlayerPrefs.Save();

        Debug.Log($"[LevelSelector] Cargando día {dia}...");
        SceneManager.LoadScene(escenaJuego);
    }
}