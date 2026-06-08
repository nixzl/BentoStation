using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Trash : MonoBehaviour
{
    [Tooltip("Drag the Plate you want to clear into here")]
    public ItemSlot plate; 

    private void Start()
    {
        // Automatically hook up the mouse click to the ThrowAway method
        GetComponent<Button>().onClick.AddListener(ThrowAway);
    }

    private void ThrowAway()
    {
        if (plate != null)
        {
            // Tell the plate to run its new clear function
            plate.ClearPlate();
        }
        else
        {
            Debug.LogError("The Trash Can doesn't know which plate to clear!");
        }
    }
}