using UnityEngine;
using UnityEngine.EventSystems;

// IPointerClickHandler lets us detect when the player clicks this UI object
public class Coin : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("How much money is this coin worth?")]
    public int coinValue = 10;

    public void OnPointerClick(PointerEventData eventData)
    {
        // BOUNCER: Don't allow clicking if the game is paused or over
        if (Time.timeScale == 0f) return;

        // 1. Find the Money Manager and add the money
        MoneyManager bank = FindObjectOfType<MoneyManager>();
        if (bank != null)
        {
            bank.AddMoney(coinValue);
        }

        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager !=null) audioManager.PlayCoinSound();

        // 2. Destroy the coin so it disappears from the counter
        Destroy(gameObject);
    }
}