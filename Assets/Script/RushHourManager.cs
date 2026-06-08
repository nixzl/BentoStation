using UnityEngine;
using UnityEngine.UI;

public class RushHourManager : MonoBehaviour
{
    [Header("UI Setup")]
    public Image rushHourFillBar; // Drag your Filled Image here

    [Header("Visual")]
    public Sprite normalBarSprite;
    public Sprite activeRushHourSprites;

    [Header("Settings")]
    public float timeUntilRushHour = 60f; // How many seconds until it triggers?

    public float rushHourDuration = 10f;
    private float currentTimer = 0f;
    private float activeTimer = 0f; //tracking time

    // Other scripts will look at this variable to know if they should panic!
    public bool isRushHourActive = false;

    private void Start()
    {
        if (rushHourFillBar != null)
        {
            rushHourFillBar.gameObject.SetActive(true);
            rushHourFillBar.fillAmount = 0f;
            
            // Removed the extra semicolon here!
            if (normalBarSprite != null) 
            {
                rushHourFillBar.sprite = normalBarSprite;
            }
        }
    }

    private void Update()
    {
        // Stop if the game is paused
        if (Time.timeScale == 0f) return;
        
        // ==========================================
        // PHASE 1: FILLING UP
        // ==========================================
        if (isRushHourActive == false)
        {
            // 1. Count up the timer
            currentTimer += Time.deltaTime;

            // 2. Fill the visual bar
            if (rushHourFillBar != null)
            {
                rushHourFillBar.fillAmount = currentTimer / timeUntilRushHour;
            }

            // 3. Trigger Rush Hour!
            if (currentTimer >= timeUntilRushHour)
            {
                isRushHourActive = true;
                activeTimer = 0f;
                
                if (rushHourFillBar != null && activeRushHourSprites != null)
                {
                    rushHourFillBar.sprite = activeRushHourSprites;
                }
                Debug.Log("Fever Time!");
            }
        }
        // ==========================================
        // PHASE 2: DRAINING (Moved this back inside Update!)
        // ==========================================
        else
        {
            activeTimer += Time.deltaTime;
            float safeDuration = rushHourDuration > 0 ? rushHourDuration : 1f;
            
            if (rushHourFillBar != null) //visual
            {
                // Make sure to use the safeDuration here!
                rushHourFillBar.fillAmount = 1f - (activeTimer / safeDuration);
            }
            
            //ending
            if(activeTimer >= rushHourDuration)
            {
                isRushHourActive = false;
                currentTimer = 0f; //reset
                activeTimer = 0f;

                if (rushHourFillBar != null)
                {
                    rushHourFillBar.fillAmount = 0f;

                    if (normalBarSprite != null)
                    {
                        rushHourFillBar.sprite = normalBarSprite;
                    }
                }
                
                Debug.Log("Back normal");
            }
        }
    }

    // ==========================================
    // STOPPING FUNCTION (Only runs at Game Over)
    // ==========================================
    public void StopRushHourImmediately()
    {
        isRushHourActive = false;
        currentTimer = 0f;
        activeTimer = 0f;

        if (rushHourFillBar != null)
        {
            rushHourFillBar.fillAmount = 0f; 
            
            if (normalBarSprite != null)
            {
                rushHourFillBar.sprite = normalBarSprite;
            }
            rushHourFillBar.gameObject.SetActive(false);
        }
    }
}