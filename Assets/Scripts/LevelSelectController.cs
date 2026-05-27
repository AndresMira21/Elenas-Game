using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

/// <summary>
/// Controla el panel de selección de niveles.
/// Usa PlayerPrefs para guardar qué niveles están desbloqueados.
/// </summary>
public class LevelSelectController : MonoBehaviour
{
    [Header("Panel")]
    public GameObject levelsPanel;

    [Header("Video de Fondo")]
    public VideoClip levelsPanelVideo;
    public VideoPlayer videoPlayer;

    [Header("Tarjetas de Niveles")]
    public Button[] levelButtons;        // 5 botones en orden
    public Image[] levelCardImages;      // imagen de fondo de cada tarjeta
    public TextMeshProUGUI[] levelNames; // nombre de cada nivel
    public TextMeshProUGUI[] levelDescs; // descripción de cada nivel
    public GameObject[] lockIcons;       // icono de candado de cada nivel

    [Header("Colores")]
    public Color unlockedColor  = new Color(0.05f, 0.12f, 0.25f, 0.85f);
    public Color lockedColor    = new Color(0.08f, 0.08f, 0.10f, 0.85f);
    public Color selectedColor  = new Color(0.10f, 0.25f, 0.45f, 0.95f);

    // Datos de los niveles
    private readonly string[] names = {
        "NIVEL 1",
        "NOCHE 1",
        "NOCHE 2",
        "NOCHE 3",
        "NOCHE 4"
    };

    private readonly string[] descriptions = {
        "Tutorial — Aprende los controles básicos",
        "El despertar",
        "Sombras en la oscuridad",
        "El sótano olvidado",
        "Confrontación final"
    };

    // Clave para guardar progreso
    private const string UNLOCKED_KEY = "UnlockedLevel";

    private int selectedLevel = -1;
    private int unlockedUpTo = 0;

    // ─────────────────────────────────────────────────────────

    void OnEnable()
    {
        // Cargar progreso guardado (por defecto solo Tutorial desbloqueado)
        unlockedUpTo = PlayerPrefs.GetInt(UNLOCKED_KEY, 0);
        RefreshCards();

        if (videoPlayer != null && levelsPanelVideo != null)
        {
            videoPlayer.clip = levelsPanelVideo;
            videoPlayer.isLooping = true;
            videoPlayer.Play();
        }
    }

    /// <summary>
    /// Actualiza visualmente todas las tarjetas según el progreso.
    /// </summary>
    private void RefreshCards()
    {
        for (int i = 0; i < 5; i++)
        {
            bool unlocked = i <= unlockedUpTo;

            // Botón interactuable solo si está desbloqueado
            if (levelButtons != null && i < levelButtons.Length && levelButtons[i] != null)
                levelButtons[i].interactable = unlocked;

            // Color de la tarjeta
            if (levelCardImages != null && i < levelCardImages.Length && levelCardImages[i] != null)
                levelCardImages[i].color = unlocked ? unlockedColor : lockedColor;

            // Nombre del nivel
            if (levelNames != null && i < levelNames.Length && levelNames[i] != null)
                levelNames[i].text = names[i];

            // Descripción
            if (levelDescs != null && i < levelDescs.Length && levelDescs[i] != null)
                levelDescs[i].text = unlocked ? descriptions[i] : "???";

            // Candado
            if (lockIcons != null && i < lockIcons.Length && lockIcons[i] != null)
                lockIcons[i].SetActive(!unlocked);
        }
    }

    /// <summary>
    /// Llamado por cada botón de nivel. Pasa el índice (0-4).
    /// </summary>
    public void SelectLevel(int index)
    {
        if (index > unlockedUpTo) return;
        selectedLevel = index;

        // Resaltar tarjeta seleccionada
        for (int i = 0; i < 5; i++)
        {
            if (levelCardImages != null && i < levelCardImages.Length && levelCardImages[i] != null)
            {
                bool isSelected = i == selectedLevel;
                bool unlocked   = i <= unlockedUpTo;
                levelCardImages[i].color = isSelected ? selectedColor :
                                           unlocked    ? unlockedColor : lockedColor;
            }
        }
    }

    /// <summary>
    /// Llamado por el botón "COMENZAR NIVEL".
    /// Tu compañero conecta esto a la carga de escena correspondiente.
    /// </summary>
    public void StartSelectedLevel()
    {
        if (selectedLevel < 0) return;
        Debug.Log($"Iniciando nivel {selectedLevel}: {names[selectedLevel]}");
        // Tu compañero implementa la carga de escena aquí
        // Ejemplo: SceneManager.LoadScene("Nivel_" + selectedLevel);
    }

    /// <summary>
    /// Llamado cuando el jugador completa un nivel.
    /// Desbloquea el siguiente nivel y guarda el progreso.
    /// </summary>
    public static void CompleteLevel(int levelIndex)
    {
        int current = PlayerPrefs.GetInt(UNLOCKED_KEY, 0);
        if (levelIndex >= current && levelIndex < 4)
        {
            PlayerPrefs.SetInt(UNLOCKED_KEY, levelIndex + 1);
            PlayerPrefs.Save();
            Debug.Log($"Nivel {levelIndex + 1} desbloqueado.");
        }
    }

    /// <summary>
    /// Resetea todo el progreso (para pruebas).
    /// </summary>
    [ContextMenu("Reset Progress")]
    public void ResetProgress()
    {
        PlayerPrefs.SetInt(UNLOCKED_KEY, 0);
        PlayerPrefs.Save();
        unlockedUpTo = 0;
        RefreshCards();
    }
}
