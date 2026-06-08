using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main: MonoBehaviour
{
    // ==========================================
    // --- NEW: MAIN MENU BUTTONS ---
    // ==========================================
    [Header("Menu Buttons")]
    public Button continueButton; 

    [Header ("UI Panels")]
    public GameObject settingsPanel;
    public GameObject recipe;

    [Header("Resep")]
    public RecipeData[] allItems; // Using the new Scriptable Object!
    
    [Header("Manual Button Setup")]
    public Button[] manualButtons;
    public Transform recipeGridParent;

    [Header("UI Ref")]
    public Text nameText;
    public Image iconImage;
    public Text descriptionText;

    [Header("Audio Settings")]
    public Slider bgmSlider; 
    public Slider sfxSlider; 

    private void Start()
    {
        int monitorWidth = Screen.currentResolution.width;
        int monitorHeight = Screen.currentResolution.height;

        Screen.SetResolution(monitorWidth, monitorHeight, FullScreenMode.FullScreenWindow);
        
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (recipe != null) recipe.SetActive(false);
        
        ClearDisplay();
        LinkManualButtons();

        // ==========================================
        // --- CHECK FOR SAVED GAME ---
        // ==========================================
        if (continueButton != null)
        {
            // HasKey checks if the secret notebook has a page for "PlayerMoney"
            if (PlayerPrefs.HasKey("PlayerMoney"))
            {
                // A save exists! Make the button clickable.
                continueButton.interactable = true;
            }
            else
            {
                // No save exists (First time playing). Gray out the button!
                continueButton.interactable = false;
            }
        }
        // ==========================================

        // --- LOAD SAVED BGM VOLUME ---
        if (bgmSlider != null)
        {
            float savedBGM = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
            bgmSlider.value = savedBGM;
            SetBGMVolume(savedBGM);
            bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        }

        // --- LOAD SAVED SFX VOLUME ---
        if (sfxSlider != null)
        {
            float savedSFX = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
            sfxSlider.value = savedSFX;
            SetSFXVolume(savedSFX);
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    private void LinkManualButtons()
    {
        for (int i = 0; i < manualButtons.Length; i++)
        {
            if (i < allItems.Length && manualButtons[i] != null)
            {
                Image slotImage = manualButtons[i].GetComponent<Image>();
                if (slotImage != null && allItems[i].itemSprite != null)
                {
                    slotImage.sprite = allItems[i].itemSprite;
                }

                int itemIndex = i; 
                manualButtons[i].onClick.AddListener(() => ShowItemDetails(itemIndex));
            }
            else if (manualButtons[i] != null)
            {
                manualButtons[i].gameObject.SetActive(false);
            }
        }
    }

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
    // SCENE & PANEL CONTROLS
    // ==========================================
    public void NewGame()
    {
        PlayerPrefs.DeleteKey("PlayerMoney");
        PlayerPrefs.Save(); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayGame() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); }
    public void MiniGame() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2); }
    public void OpenSettings() { if (settingsPanel != null) settingsPanel.SetActive(true); }
    public void CloseSettings() { if (settingsPanel != null) settingsPanel.SetActive(false); }
    public void OpenRecipe() { if(recipe != null) recipe.SetActive(true); }
    public void CloseRecipe() { if(recipe != null) recipe.SetActive(false); }
    public void QuitGame() { Debug.Log ("Quit!"); Application.Quit(); }

    public void ShowItemDetails(int itemIndex)
    {
        if (itemIndex >= 0 && itemIndex < allItems.Length)
        {
            // --- FIXED: This now properly pulls from RecipeData! ---
            RecipeData selectedItem = allItems[itemIndex];
            
            if(nameText != null) nameText.text = selectedItem.itemName;
            
            if(iconImage != null && selectedItem.itemSprite != null)
            {
                iconImage.sprite = selectedItem.itemSprite;
                iconImage.color = Color.white;
            }
            
            if(descriptionText != null) descriptionText.text = selectedItem.itemDescription;
        }
    }

    public void ClearDisplay()
    {
        if(nameText != null) nameText.text = "Items";
        if(descriptionText != null) descriptionText.text = "";
        if(iconImage != null) iconImage.color = new Color(1, 1, 1, 0);
    }
}