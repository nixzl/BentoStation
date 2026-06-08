using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI; // REQUIRED: This lets us use the UI Sliders!

public class GameOver : MonoBehaviour
{
    [Header("Scene Navigation")]
    [Tooltip("Type the EXACT name of your Main Menu scene here")]
    public string mainMenuSceneName = "MainMenu"; 
    public string NextScene;

    [Header("UI Panels")]
    [Tooltip("Drag your Settings Panel object here")]
    public GameObject settingsPanel;
    public GameObject decorationsPanel;

    // ==========================================
    // --- NEW: AUDIO SETTINGS ---
    // ==========================================
    [Header("Audio Settings")]
    public Slider bgmSlider; 
    public Slider sfxSlider; 

    private bool isSettingsOpen = false;
    private bool isDecorationsOpen = false;

    private void Start()
    {
        // 1. Always ensure time is running normally when the level starts
        Time.timeScale = 1f; 

        // 2. Hide the settings panel if we accidentally left it turned on in the editor
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        if (decorationsPanel !=null)
        {
            decorationsPanel.SetActive(false);
        }

        // ==========================================
        // --- NEW: LOAD SAVED VOLUMES ---
        // ==========================================
        if (bgmSlider != null)
        {
            float savedBGM = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
            bgmSlider.value = savedBGM;
            SetBGMVolume(savedBGM);
            bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        }

        if (sfxSlider != null)
        {
            float savedSFX = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
            sfxSlider.value = savedSFX;
            SetSFXVolume(savedSFX);
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    private void Update()
    {
        // Let the player toggle the settings menu with the Escape key!
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isSettingsOpen)
            {
                CloseSettings();
            }
            else
            {
                OpenSettings();
            }
        }
    }    

    public void ToggleSettings()
    {
        if (isSettingsOpen) CloseSettings();
        else OpenSettings();
    }

    public void ToggleDecorations()
    {
        if (isDecorationsOpen) CloseDecorations();
        else OpenDecorations();
    }

    // ==========================================
    //           SETTINGS CONTROLS
    // ==========================================

    public void OpenDecorations()
    {
        if (decorationsPanel !=null)
        {
            decorationsPanel.SetActive(true);
            isDecorationsOpen = true;
            Time.timeScale = 0;
        }
    }

    public void CloseDecorations()
    {
        if (decorationsPanel !=null)
        {
            decorationsPanel.SetActive(false);
            isDecorationsOpen = false;
            Time.timeScale = 1;
        }
    }

    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
            isSettingsOpen = true;
            Time.timeScale = 0; // Freeze the game
            Debug.Log("Game Paused.");
        }
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
            isSettingsOpen = false;
            Time.timeScale = 1; // Unfreeze the game
            Debug.Log("Game Resumed.");
        }
    }

    // ==========================================
    // --- NEW: AUDIO SLIDER FUNCTIONS ---
    // ==========================================
    public void SetBGMVolume(float volume)
    {
        PlayerPrefs.SetFloat("BGMVolume", volume); 
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        
        if (audioManager != null && audioManager.bgmSource != null) 
        {
            audioManager.bgmSource.volume = volume;
        }
    }

    public void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume", volume); 
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        
        if (audioManager != null && audioManager.sfxSource != null) 
        {
            audioManager.sfxSource.volume = volume;
        }
    }

    // ==========================================
    //           SCENE CONTROLS
    // ==========================================

    public void ReloadCurrentScene()
    {
        Time.timeScale = 1; // CRITICAL: Unfreeze the game before restarting!
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // CRITICAL: Unfreeze the game before going to menu!
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void NextDay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(NextScene);
    }
}