using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

/// <summary>
/// Controla la pantalla de créditos/ending de Elena's Game.
/// Reproduce CreditosElena'sGame.mp4 y al terminar (o con botón) vuelve al menú.
/// </summary>
public class EndingController : MonoBehaviour
{
    private const string MAIN_MENU_SCENE = "MainMenu";

    [Header("Video de Créditos")]
    [Tooltip("VideoClip: CreditosElena'sGame.mp4")]
    public VideoClip creditsVideoClip;

    [Tooltip("Componente VideoPlayer de la escena")]
    public VideoPlayer videoPlayer;

    [Header("Audio")]
    [Tooltip("AudioSource con Fear.ogg (opcional)")]
    public AudioSource backgroundMusic;

    [Header("UI")]
    [Tooltip("Botón 'Volver' — puedes ocultarlo hasta que termine el video")]
    public GameObject backButton;

    void Start()
    {
        // Ocultar botón hasta que termine el video (opcional)
        if (backButton != null) backButton.SetActive(false);

        if (videoPlayer != null && creditsVideoClip != null)
        {
            videoPlayer.clip      = creditsVideoClip;
            videoPlayer.isLooping = false;
            videoPlayer.Play();
            videoPlayer.loopPointReached += OnCreditsFinished;
        }

        if (backgroundMusic != null)
        {
            backgroundMusic.loop = true;
            backgroundMusic.Play();
        }
    }

    /// <summary>Se llama automáticamente cuando el video de créditos termina.</summary>
    private void OnCreditsFinished(VideoPlayer vp)
    {
        // Mostrar botón de volver cuando termina el video
        if (backButton != null) backButton.SetActive(true);
    }

    /// <summary>Botón VOLVER AL MENÚ.</summary>
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(MAIN_MENU_SCENE);
    }

    /// <summary>Botón SALIR.</summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached -= OnCreditsFinished;
    }
}
