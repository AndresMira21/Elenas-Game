using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Menú de pausa — se activa con ESC durante el juego en SampleScene.
/// Opciones: Continuar, Reiniciar Nivel, Volver al Menú.
/// </summary>
public class PauseMenuController : MonoBehaviour
{
    private const string MAIN_MENU_SCENE = "MainMenu";

    [Header("Panel de Pausa")]
    [Tooltip("Panel UI del menú de pausa (desactivado al inicio)")]
    public GameObject pausePanel;

    private bool isPaused = false;

    void Start()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    void Update()
    {
        // Abrir/cerrar pausa con ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else          PauseGame();
        }
    }

    /// <summary>Pausa el juego y muestra el panel.</summary>
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        if (pausePanel != null) pausePanel.SetActive(true);
    }

    /// <summary>Botón CONTINUAR — reanuda el juego.</summary>
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    /// <summary>Botón REINICIAR NIVEL — recarga la escena actual.</summary>
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>Botón VOLVER AL MENÚ.</summary>
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MAIN_MENU_SCENE);
    }
}
