using UnityEngine;
using UnityEngine.UI; 

public class MoneyManager : MonoBehaviour
{
    public int currentMoney = 0;

    [Tooltip("Requirement Money")]
    public int targetMoney = 15000;
    
    [Header("UI References")]
    public Text moneyText;       
    public Text targetMoneyText; 

    [Header("Stats")]
    public int customersServed = 0;
    public int customersLeft = 0;
    public int rushHourBonusMoney = 0;

    private void Start()
    {   
        // --- NEW: Load the saved money the moment the scene starts! ---
        LoadGame();

        if (targetMoneyText != null)
        {
            targetMoneyText.text = "Rp " + targetMoney.ToString();
        }
        
        UpdateMoneyUI();
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateMoneyUI();
        Debug.Log("Collected Rp" + amount + "! Total: Rp" + currentMoney);
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = "Rp " + currentMoney.ToString();
        }
    }

    // ==========================================
    // --- NEW: SAVE & LOAD FUNCTIONS ---
    // ==========================================
    public void SaveGame()
    {
        // Write the current money into the "PlayerMoney" slot
        PlayerPrefs.SetInt("PlayerMoney", currentMoney);
        
        // Force Unity to save the notebook immediately
        PlayerPrefs.Save(); 
        Debug.Log("Game Saved! You have Rp " + currentMoney);
    }

    private void LoadGame()
    {
        // Check if the notebook has a save file for "PlayerMoney" first
        if (PlayerPrefs.HasKey("PlayerMoney"))
        {
            // If it exists, read the number and overwrite our starting money!
            currentMoney = PlayerPrefs.GetInt("PlayerMoney");
            Debug.Log("Game Loaded! Starting with Rp " + currentMoney);
        }
        else
        {
            // If no save exists (first time playing), start at 0
            currentMoney = 0; 
        }
    }
}