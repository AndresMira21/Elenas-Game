using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    // ── Escenas ──────────────────────────────────────────────
    private const string GAME_SCENE = "SampleScene";

    // ── Videos ───────────────────────────────────────────────
    [Header("Videos de Fondo")]
    public VideoClip menuVideoClip;
    public VideoClip configVideoClip;
    public VideoPlayer videoPlayer;
    public RawImage backgroundImage;
    public RenderTexture backgroundRenderTexture;

    // ── Audio ─────────────────────────────────────────────────
    [Header("Audio")]
    public AudioSource backgroundMusic;

    // ── Paneles ───────────────────────────────────────────────
    [Header("Paneles")]
    public GameObject menuPanel;
    public GameObject settingsPanel;
    public GameObject levelsPanel;

    // ── Sliders de Audio ──────────────────────────────────────
    [Header("Sliders de Audio")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    // ── Textos de valor de sliders ────────────────────────────
    [Header("Textos de Valor (opcional)")]
    public TextMeshProUGUI masterVolumeText;
    public TextMeshProUGUI musicVolumeText;
    public TextMeshProUGUI sfxVolumeText;
    public TextMeshProUGUI sensitivityText;

    // ── Controles ─────────────────────────────────────────────
    [Header("Slider de Controles")]
    public Slider mouseSensitivitySlider;

    // ── Gráficos ──────────────────────────────────────────────
    [Header("Gráficos")]
    public TMP_Dropdown graphicsQualityDropdown;
    public Toggle fullscreenToggle;
    public Toggle vsyncToggle;

    // ── Glitch ────────────────────────────────────────────────
    [Header("Glitch")]
    public TextMeshProUGUI glitchText;

    private readonly string[] hiddenMessages = {
        "NO ESTÁS SOLO...", "ELLA TE OBSERVA", "CORRE",
        "NO MIRES ATRÁS", "ESTÁN EN LAS PAREDES",
        "AYÚDAME", "SAL DE AQUÍ", "TE VEO"
    };

    private float glitchTimer = 0f;

    // ─────────────────────────────────────────────────────────
    void Start()
    {
        ShowMenu();

        // Música
        if (backgroundMusic != null)
        {
            backgroundMusic.loop = true;
            if (!backgroundMusic.isPlaying) backgroundMusic.Play();
        }

        // Valores por defecto
        InitSliders();

        // Dropdown opciones
        if (graphicsQualityDropdown != null)
        {
            graphicsQualityDropdown.ClearOptions();
            graphicsQualityDropdown.AddOptions(new System.Collections.Generic.List<string>
                { "Baja", "Media", "Alta", "Ultra" });
            graphicsQualityDropdown.value = 2; // Alta por defecto
            graphicsQualityDropdown.onValueChanged.AddListener(OnGraphicsQualityChanged);
        }

        // Toggles
        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = Screen.fullScreen;
            fullscreenToggle.onValueChanged.AddListener(OnFullscreenChanged);
        }
        if (vsyncToggle != null)
        {
            vsyncToggle.isOn = QualitySettings.vSyncCount > 0;
            vsyncToggle.onValueChanged.AddListener(OnVSyncChanged);
        }

        // Listeners de sliders
        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        if (musicVolumeSlider != null)
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        if (sfxVolumeSlider != null)
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        if (mouseSensitivitySlider != null)
            mouseSensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
    }

    void Update()
    {
        if (menuPanel != null && menuPanel.activeSelf)
        {
            glitchTimer += Time.deltaTime;
            if (glitchTimer >= 3f)
            {
                glitchTimer = 0f;
                if (Random.value > 0.7f) TriggerGlitch();
            }
        }
    }

    // ── Navegación ────────────────────────────────────────────

    public void ShowMenu()
    {
        SetPanel(menuPanel, true);
        SetPanel(settingsPanel, false);
        PlayVideo(menuVideoClip);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(GAME_SCENE);
    }

    public void OpenLevels()
    {
        SetPanel(menuPanel, false);
        SetPanel(settingsPanel, false);
        SetPanel(levelsPanel, true);
        PlayVideo(null); // Tu compañero puede asignar el video de niveles
    }

    public void CloseLevels()
    {
        ShowMenu();
    }

    public void OpenSettings()
    {
        SetPanel(menuPanel, false);
        SetPanel(settingsPanel, true);
        PlayVideo(configVideoClip);
    }

    public void CloseSettings()
    {
        ShowMenu();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // ── Configuración ─────────────────────────────────────────

    public void RestoreDefaults()
    {
        InitSliders();
        if (graphicsQualityDropdown != null) graphicsQualityDropdown.value = 2;
        if (fullscreenToggle != null) fullscreenToggle.isOn = true;
        if (vsyncToggle != null) vsyncToggle.isOn = true;
        ApplyGraphicsQuality(2);
        Screen.fullScreen = true;
        QualitySettings.vSyncCount = 1;
    }

    private void InitSliders()
    {
        if (masterVolumeSlider != null) { masterVolumeSlider.value = 80f; UpdateText(masterVolumeText, 80f); }
        if (musicVolumeSlider != null)  { musicVolumeSlider.value  = 70f; UpdateText(musicVolumeText, 70f); }
        if (sfxVolumeSlider != null)    { sfxVolumeSlider.value    = 85f; UpdateText(sfxVolumeText, 85f); }
        if (mouseSensitivitySlider != null) { mouseSensitivitySlider.value = 50f; UpdateText(sensitivityText, 50f); }
    }

    // ── Listeners ─────────────────────────────────────────────

    private void OnMasterVolumeChanged(float value)
    {
        AudioListener.volume = value / 100f;
        UpdateText(masterVolumeText, value);
    }

    private void OnMusicVolumeChanged(float value)
    {
        if (backgroundMusic != null) backgroundMusic.volume = value / 100f;
        UpdateText(musicVolumeText, value);
    }

    private void OnSFXVolumeChanged(float value)
    {
        UpdateText(sfxVolumeText, value);
    }

    private void OnSensitivityChanged(float value)
    {
        UpdateText(sensitivityText, value);
    }

    private void OnGraphicsQualityChanged(int index)
    {
        ApplyGraphicsQuality(index);
    }

    private void OnFullscreenChanged(bool value)
    {
        Screen.fullScreen = value;
    }

    private void OnVSyncChanged(bool value)
    {
        QualitySettings.vSyncCount = value ? 1 : 0;
    }

    private void ApplyGraphicsQuality(int index)
    {
        // 0=Baja, 1=Media, 2=Alta, 3=Ultra
        int[] qualityMap = { 0, 2, 4, 5 };
        if (index >= 0 && index < qualityMap.Length)
            QualitySettings.SetQualityLevel(qualityMap[index], true);
    }

    // ── Helpers ───────────────────────────────────────────────

    private void SetPanel(GameObject panel, bool active)
    {
        if (panel != null) panel.SetActive(active);
    }

    private void PlayVideo(VideoClip clip)
    {
        if (videoPlayer == null || clip == null) return;
        videoPlayer.clip = clip;
        videoPlayer.isLooping = true;
        videoPlayer.Play();
        if (backgroundImage != null && backgroundRenderTexture != null)
            backgroundImage.texture = backgroundRenderTexture;
    }

    private void UpdateText(TextMeshProUGUI tmp, float value)
    {
        if (tmp != null) tmp.text = Mathf.RoundToInt(value) + "%";
    }

    private void TriggerGlitch()
    {
        if (glitchText == null) return;
        glitchText.text = hiddenMessages[Random.Range(0, hiddenMessages.Length)];
        glitchText.enabled = true;
        Invoke(nameof(HideGlitch), 0.8f);
    }

    private void HideGlitch()
    {
        if (glitchText != null) glitchText.enabled = false;
    }
}
