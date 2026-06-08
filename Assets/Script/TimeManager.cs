using UnityEngine;
using UnityEngine.UI; // Required to change the text on the screen

public class TimeManager : MonoBehaviour
{
    [Header ("Game Over UI")]
    public UnityEngine.UI.Text finalMoneyText;
    
    // --- NEW: STATS TEXT SLOTS (Win/Lose removed) ---
    public Text servedStatsText;
    public Text leftStatsText;
    public Text rushHourBonusText;

    [Header("Time Settings")]
    [Tooltip("How long the game lasts in seconds (180 = 3 minutes)")]
    public float timeLimit = 180f; 
    
    private float currentTime;
    private bool isGameOver = false;

    [Header("UI References")]
    public Text timerText;           // The clock on the screen
    public GameObject gameOverPanel; // The screen that pops up at the end

    private void Start()
    {
        currentTime = timeLimit;
        
        // Hide the Game Over screen at the start of the game
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private void Update()
    {
        // If the game is over, stop running this code entirely
        if (isGameOver) return; 

        // Count down the time
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            currentTime = 0;
            EndGame();
        }

        // Update the visual clock on the screen
        if (timerText != null)
        {
            // Math magic to turn flat seconds into a nice MM:SS format
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            
            // Format it to always show two digits (e.g., 03:05 instead of 3:5)
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

private void EndGame()
    {
        isGameOver = true;
        currentTime = 0; 
        
        Debug.Log("Time's up! The restaurant is closed.");
        Time.timeScale = 0f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // We only declare 'bank' ONCE right here!
        MoneyManager bank = FindObjectOfType<MoneyManager>();

        if (bank != null)
        {
            // 1. SAVE THE GAME FIRST
            bank.SaveGame();

            // 2. THEN UPDATE ALL THE TEXT
            if (finalMoneyText != null)
            {
                finalMoneyText.text = "Rp " + bank.currentMoney.ToString();
            }

            if (servedStatsText != null) 
            {
                servedStatsText.text = bank.customersServed.ToString();
            }
                
            if (leftStatsText != null) 
            {
                leftStatsText.text = bank.customersLeft.ToString();
            }
                
            if (rushHourBonusText != null) 
            {
                rushHourBonusText.text = "Rp " + bank.rushHourBonusMoney.ToString();
            }
        }

        // 3. CLEAN UP THE RESTAURANT
        RushHourManager rushManager = FindObjectOfType<RushHourManager>();
        if (rushManager != null)
        {
            rushManager.StopRushHourImmediately();
        }

        Spawner spawner = FindObjectOfType<Spawner>();
        if (spawner != null)
        {
            spawner.enabled = false;
        }

        Customer[] remainingCustomers = FindObjectsOfType<Customer>();
        foreach (Customer c in remainingCustomers)
        {
            Destroy(c.gameObject);
        }
    }
}