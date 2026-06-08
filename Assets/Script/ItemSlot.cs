using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public struct RecipeStep
{
    public string expectedFood; 
    public Sprite resultingPlateSprite; 
}

// NOTICE: We added IPointerClickHandler to the list below!
public class ItemSlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [Tooltip("The picture of the plate when it has absolutely nothing on it")]
    public Sprite emptyPlateSprite; // <--- ADDED: So the plate can reset

    [Tooltip("Set up your recipe sequence here!")]
    public List<RecipeStep> recipeSequence = new List<RecipeStep>();

    private int currentStep = 0; 
    private Image plateImage;

    private void Awake()
    {
        plateImage = GetComponent<Image>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (Time.timeScale == 0f) return;
        if (eventData.pointerDrag != null)
        {
            Drag droppedFood = eventData.pointerDrag.GetComponent<Drag>();
            
            if (droppedFood != null)
            {
                if (currentStep >= recipeSequence.Count)
                {
                    Debug.Log("The plate is already finished! Click it to serve it.");
                    return; 
                }

                if (droppedFood.foodName == recipeSequence[currentStep].expectedFood)
                {
                    if (recipeSequence[currentStep].resultingPlateSprite != null)
                    {
                        plateImage.sprite = recipeSequence[currentStep].resultingPlateSprite;
                    }
                    currentStep++;
                }
                else
                {
                    Debug.LogWarning("Wrong order! We need " + recipeSequence[currentStep].expectedFood + " next.");
                }
            }
        }
    }
    // --- NEW CODE: DETECTS MOUSE CLICKS ---
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Time.timeScale == 0f) return;
        // 1. Check if the plate has finished all its recipe steps
        if (currentStep >= recipeSequence.Count)
        {
            ServePlate();
        }
        else
        {
            Debug.Log("The dish isn't finished yet! You can't serve it.");
        }
    }

    // --- NEW CODE: CLEARS THE PLATE ---
private void ServePlate()
    {
        // 1. Find EVERY customer currently in the restaurant
        Customer[] allCustomers = FindObjectsOfType<Customer>();

        if (allCustomers.Length == 0)
        {
            Debug.Log("There are no customers to serve!");
            return; 
        }

        Customer oldestCustomer = allCustomers[0];
        Customer matchingCustomer = null;

        // 2. Look at every single customer...
        foreach (Customer c in allCustomers)
        {
            // Keep track of whoever has the lowest timer (meaning they have been waiting longest)
            if (c.currentWaitTime < oldestCustomer.currentWaitTime)
            {
                oldestCustomer = c;
            }

            // Did this customer actually order the dish we are holding?
            if (c.desiredDish == plateImage.sprite)
            {
                // If multiple people ordered the same thing, give it to the oldest one!
                if (matchingCustomer == null || c.currentWaitTime < matchingCustomer.currentWaitTime)
                {
                    matchingCustomer = c;
                }
            }
        }

        // 3. DECIDE WHO GETS THE FOOD:
        // If someone actually wanted this dish, give it to them. 
        // If nobody wanted it, throw it at the oldest customer so they take the penalty!
        Customer customerToServe = (matchingCustomer != null) ? matchingCustomer : oldestCustomer;

        // 4. Serve them!
        customerToServe.ReceiveFood(plateImage.sprite);

        // 5. Reset the plate back to empty
        currentStep = 0;
        if (emptyPlateSprite != null)
        {
            plateImage.sprite = emptyPlateSprite;
        }
    }
    // --- NEW CODE: CLEARS THE PLATE ---
    public void ClearPlate()
    {
        // 1. Reset the recipe tracker back to zero
        currentStep = 0;

        // 2. Change the picture back to a completely empty plate
        if (emptyPlateSprite != null)
        {
            plateImage.sprite = emptyPlateSprite;
        }

        Debug.Log("🗑️ Plate thrown in the trash! Ready to start over.");
    }
}