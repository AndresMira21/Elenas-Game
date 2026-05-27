using UnityEngine;

/// <summary>
/// GameStarter.cs — Elena's Game
/// Ponlo en el mismo GameObject del GameManager en SampleScene.
/// Al cargar la escena lee el día guardado por LevelSelector y lo aplica.
/// </summary>
public class GameStarter : MonoBehaviour
{
    void Start()
    {
        // Si viene del selector de niveles, PlayerPrefs tendrá el día
        if (PlayerPrefs.HasKey("DiaInicial"))
        {
            int dia = PlayerPrefs.GetInt("DiaInicial");
            GameManager.Instance.currentDay = dia;

            // Limpia para que la próxima vez empiece normal
            PlayerPrefs.DeleteKey("DiaInicial");

            Debug.Log($"[GameStarter] Iniciando en día {dia}");
        }
        else
        {
            // Viene del botón Play normal → empieza en día 1
            GameManager.Instance.currentDay = 1;
            Debug.Log("[GameStarter] Iniciando desde el día 1");
        }
    }
}