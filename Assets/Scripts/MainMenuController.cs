using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

/// <summary>
/// Controla el menú principal de Elena's Game.
/// Maneja las pantallas: Menu, Configuración.
/// El panel de niveles se ignora por ahora (pendiente).
/// </summary>
public class MainMenuController : MonoBehaviour
{
    // ── Escenas ──────────────────────────────────────────────
    private const string GAME_SCENE    = "SampleScene";
    private const string ENDING_SCENE  = "Ending";

    // ── Videos de fondo ──────────────────────────────────────
    [Header("Videos de Fondo")]
    [Tooltip("VideoClip: MENU.mp4")]
    public VideoClip menuVideoClip;

    [Tooltip("VideoClip: CONFIGURACION.mp4")]
    public VideoClip configVideoClip;

    [Tooltip("Componente VideoPlayer de la escena")]
    public VideoPlayer videoPlayer;

    [Tooltip("RawImage donde se renderiza el video (ocupa toda la pantalla)")]
    public RawImage backgroundImage;

    [Tooltip("RenderTexture asignada al VideoPlayer y al RawImage")]
    public RenderTexture backgroundRenderTexture;

    // ── Audio ─────────────────────────────────────────────────
    [Header("Audio")]
    [Tooltip("AudioSource con Fear.ogg — loop automático")]
    public AudioSource backgroundMusic;

    // ── Paneles UI ────────────────────────────────────────────
    [Header("Paneles")]
    [Tooltip("Panel raíz del menú principal")]
    public GameObject menuPanel;

    [Tooltip("Panel de configuración")]
    public GameObject settingsPanel;

    // ── Configuración (Settings) ──────────────────────────────
    [Header("Sliders de Audio")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider mouseSensitivitySlider;

    [Header("Toggles de Gráficos")]
    public Toggle fullscreenToggle;
    public Toggle vsyncToggle;

    [Header("Dropdown de Calidad")]
    public Dropdown graphicsQualityDropdown;

    // ── Mensajes glitch ───────────────────────────────────────
    [Header("Glitch")]
    [Tooltip("Text UI donde aparecen los mensajes ocultos")]
    public Text glitchText;

    private readonly string[] hiddenMessages = {
        "NO ESTÁS SOLO...", "ELLA TE OBSERVA", "CORRE",
        "NO MIRES ATRÁS", "ESTÁN EN LAS PAREDES",
        "AYÚDAME", "SAL DE AQUÍ", "TE VEO"
    };

    private float glitchTimer = 0f;
    private float glitchInterval = 3f;

    // ─────────────────────────────────────────────────────────
    void Start()
    {
        ShowMenu();

        // Música en loop
        if (backgroundMusic != null)
        {
            backgroundMusic.loop = true;
            if (!backgroundMusic.isPlaying) backgroundMusic.Play();
        }

        // Valores por defecto de configuración
        if (masterVolumeSlider != null)    masterVolumeSlider.value    = 80f;
        if (musicVolumeSlider != null)     musicVolumeSlider.value     = 70f;
        if (sfxVolumeSlider != null)       sfxVolumeSlider.value       = 85f;
        if (mouseSensitivitySlider != null) mouseSensitivitySlider.value = 50f;
        if (fullscreenToggle != null)      fullscreenToggle.isOn       = true;
        if (vsyncToggle != null)           vsyncToggle.isOn            = true;
    }

    void Update()
    {
        // Glitch messages solo en el menú principal
        if (menuPanel != null && menuPanel.activeSelf)
        {
            glitchTimer += Time.deltaTime;
            if (glitchTimer >= glitchInterval)
            {
                glitchTimer = 0f;
                if (Random.value > 0.7f) TriggerGlitch();
            }
        }
    }

    // ── Navegación ────────────────────────────────────────────

    /// <summary>Muestra el menú principal con su video.</summary>
    public void ShowMenu()
    {
        SetPanel(menuPanel, true);
        SetPanel(settingsPanel, false);
        PlayVideo(menuVideoClip);
    }

    /// <summary>Botón JUGAR → carga SampleScene.</summary>
    public void StartGame()
    {
        SceneManager.LoadScene(GAME_SCENE);
    }

    /// <summary>Botón CONFIGURACIÓN → muestra panel de settings.</summary>
    public void OpenSettings()
    {
        SetPanel(menuPanel, false);
        SetPanel(settingsPanel, true);
        PlayVideo(configVideoClip);
    }

    /// <summary>Botón VOLVER dentro de configuración.</summary>
    public void CloseSettings()
    {
        ShowMenu();
    }

    /// <summary>Restaura los valores por defecto de configuración.</summary>
    public void RestoreDefaults()
    {
        if (masterVolumeSlider != null)     masterVolumeSlider.value     = 80f;
        if (musicVolumeSlider != null)      musicVolumeSlider.value      = 70f;
        if (sfxVolumeSlider != null)        sfxVolumeSlider.value        = 85f;
        if (mouseSensitivitySlider != null) mouseSensitivitySlider.value = 50f;
        if (fullscreenToggle != null)       fullscreenToggle.isOn        = true;
        if (vsyncToggle != null)            vsyncToggle.isOn             = true;
        if (graphicsQualityDropdown != null) graphicsQualityDropdown.value = 2; // "high"
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

    // ── Helpers ───────────────────────────────────────────────

    private void SetPanel(GameObject panel, bool active)
    {
        if (panel != null) panel.SetActive(active);
    }

    private void PlayVideo(VideoClip clip)
    {
        if (videoPlayer == null || clip == null) return;
        videoPlayer.clip      = clip;
        videoPlayer.isLooping = true;
        videoPlayer.Play();

        // Asignar la RenderTexture al RawImage
        if (backgroundImage != null && backgroundRenderTexture != null)
            backgroundImage.texture = backgroundRenderTexture;
    }

    private void TriggerGlitch()
    {
        if (glitchText == null) return;
        glitchText.text    = hiddenMessages[Random.Range(0, hiddenMessages.Length)];
        glitchText.enabled = true;
        Invoke(nameof(HideGlitch), 0.8f);
    }

    private void HideGlitch()
    {
        if (glitchText != null) glitchText.enabled = false;
    }
}
