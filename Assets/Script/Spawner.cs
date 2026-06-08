using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawning Rules")]
    public GameObject customerPrefab;
    public float timeBetweenCustomers = 2f; 
    
    private float spawnTimer;

    [Header("Positioning")]
    [Tooltip("Drag all your spawn points (e.g., Register 1, Register 2) here")]
    public Transform[] spawnPoints; // <--- Changed to an Array [] so it can hold multiple!

    // This array remembers exactly who is standing at each spawn point
    private GameObject[] currentCustomers; 

    private void Start()
    {
        spawnTimer = timeBetweenCustomers; 
        
        // Tell the script to make exactly enough memory slots for our spawn points
        currentCustomers = new GameObject[spawnPoints.Length];
    }

    private void Update()
    {

        if (Time.timeScale ==0f) return;

        float currentSpawnSpeed = 5f;

        RushHourManager rushManager = FindObjectOfType<RushHourManager>();
        if(rushManager !=null && rushManager.isRushHourActive)
        {
            currentSpawnSpeed =2f;
        }
        
        spawnTimer -=Time.deltaTime;

        if (spawnTimer <=0)
        {
            SpawnCustomer();
            spawnTimer = currentSpawnSpeed;
        }
        // 1. Check if there is ANY empty spot at the counters
        bool hasEmptySpot = false;
        
        for (int i = 0; i < currentCustomers.Length; i++)
        {
            if (currentCustomers[i] == null)
            {
                hasEmptySpot = true;
                break; // We found an empty spot, no need to keep searching
            }
        }

        // 2. If there is an empty spot, start counting down to spawn someone
        if (hasEmptySpot)
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0)
            {
                SpawnCustomer();
                
                // Reset the timer for the NEXT person
                spawnTimer = timeBetweenCustomers; 
            }
        }
    }

    private void SpawnCustomer()
    {
        // Look through our spawn points to find the first one that is empty
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (currentCustomers[i] == null)
            {
                // Spawn the customer and lock them into this specific memory slot
                currentCustomers[i] = Instantiate(customerPrefab, spawnPoints[i]);
                
                // Force UI Fix: Keep them perfectly centered and scaled
                currentCustomers[i].transform.localPosition = Vector3.zero;
                currentCustomers[i].transform.localScale = Vector3.one;

                Debug.Log("A new customer arrived at Spawn Point " + i);
                
                return; // Stop the loop so we only spawn ONE customer at a time!
            }
        }
    }
}