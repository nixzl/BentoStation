using UnityEngine;
using UnityEngine.UI; 

public class Customer : MonoBehaviour
{
    public enum CustomerState
    {
        Happy,      
        Impatient,  
        Angry       
    }

    [Header("Customer Needs")]
    public Sprite desiredDish;
    public float maxWaitTime = 30f; 
    public float currentWaitTime;

    [Header("Rewards")]
    public GameObject coinPrefab;
    
    // ==========================================
    // --- NEW: EXACT SPAWN POINT ---
    // ==========================================
    [Tooltip("Drag an Empty GameObject here to set the exact coin drop spot!")]
    public Transform coinSpawnPoint; 

    [Tooltip("Tip")]
    public int happyTip = 3000;
    public int impatientTip = 1000;
    public int angryTip = 500;

    [Header("State Visuals")]
    public Image customerImage; 
    
    public Sprite happySprite;
    public Sprite impatientSprite;
    public Sprite angrySprite;

    [Header("UI Waiting Bar")]
    public Image waitBarFill; 
    public Color happyColor = Color.green;
    public Color impatientColor = Color.yellow;
    public Color angryColor = Color.red;

    private CustomerState currentState;

    private void Start()
    {
        RushHourManager rushManager = FindObjectOfType<RushHourManager>();
        if(rushManager != null && rushManager.isRushHourActive)
        {
            maxWaitTime = 13f; 
        }

        currentWaitTime = maxWaitTime;
        ChangeState(CustomerState.Happy);
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;

        currentWaitTime -= Time.deltaTime;

        if (currentWaitTime <= 0)
        {
            Debug.Log("Pelanggan kabur!");

            MoneyManager bank  = FindObjectOfType<MoneyManager>();
            if (bank !=null) bank.customersLeft++;
            Destroy(gameObject);
            return;
        }

        float timePercentage = currentWaitTime / maxWaitTime;

        if (waitBarFill != null)
        {
            waitBarFill.fillAmount = timePercentage;

            if (currentState == CustomerState.Happy) 
                waitBarFill.color = happyColor;
            else if (currentState == CustomerState.Impatient) 
                waitBarFill.color = impatientColor;
            else if (currentState == CustomerState.Angry) 
                waitBarFill.color = angryColor;
        }

        if (timePercentage > 0.5f && currentState != CustomerState.Happy)
        {
            ChangeState(CustomerState.Happy);
        }
        else if (timePercentage <= 0.5f && timePercentage > 0.2f && currentState != CustomerState.Impatient)
        {
            ChangeState(CustomerState.Impatient);
        }
        else if (timePercentage <= 0.2f && currentState != CustomerState.Angry)
        {
            ChangeState(CustomerState.Angry);
        }
    }

    private void ChangeState(CustomerState newState)
    {
        currentState = newState;

       if (customerImage != null)
        {
            switch (currentState)
            {
                case CustomerState.Happy:
                    if (happySprite != null) customerImage.sprite = happySprite;
                    break;
                case CustomerState.Impatient:
                    if (impatientSprite != null) customerImage.sprite = impatientSprite;
                    break;
                case CustomerState.Angry:
                    if (angrySprite != null) customerImage.sprite = angrySprite;
                    break;
            }
        }
    }

    public void ReceiveFood(Sprite servedDish)
    {
        if (servedDish == desiredDish)
        {
            Debug.Log("Makasih Bang!");

            AudioManager audioManager = FindObjectOfType<AudioManager>();
            if (audioManager != null) audioManager.PlayHappySound();

            MoneyManager bank = FindObjectOfType<MoneyManager>();
            if (bank != null) bank.customersServed++;

            if (coinPrefab != null)
            {
                // ==========================================
                // --- NEW: SPAWN AT THE EXACT TARGET ---
                // ==========================================
                // If you linked a target point, spawn there. Otherwise, spawn on the customer.
                Vector3 spawnPos = coinSpawnPoint != null ? coinSpawnPoint.position : transform.position;

                GameObject newCoin = Instantiate(coinPrefab, spawnPos, Quaternion.identity, transform.parent);

                Coin coinScript = newCoin.GetComponent<Coin>();

                if (coinScript != null)
                {
                    int finalTip = 0;

                    if (currentState == CustomerState.Happy) finalTip = happyTip;
                    else if (currentState == CustomerState.Impatient) finalTip = impatientTip;
                    else if (currentState == CustomerState.Angry) finalTip = angryTip;

                    RushHourManager rushManager = FindObjectOfType<RushHourManager>();
                    if (rushManager != null && rushManager.isRushHourActive)
                    {
                        int bonusAmount = 1500; 
                        finalTip += bonusAmount;
                        
                        if (bank != null) bank.rushHourBonusMoney += bonusAmount; 
                    }

                    coinScript.coinValue = finalTip;
                }
            }

            Destroy(gameObject); 
        }
        else
        {
            Debug.Log("Saya gak order ini!");

            AudioManager audioManager = FindObjectOfType<AudioManager>();
            if (audioManager != null) audioManager.PlayAngrySound();
            
            currentWaitTime -= 5f; 
        }
    }
}