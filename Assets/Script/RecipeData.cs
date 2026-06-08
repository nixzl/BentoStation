using UnityEngine;

// This magic line adds a brand new button to Unity's Right-Click menu!
[CreateAssetMenu(fileName = "New Recipe", menuName = "Restaurant/Recipe Data")]
public class RecipeData : ScriptableObject // Notice we changed MonoBehaviour to ScriptableObject!
{
    public string itemName;
    public Sprite itemSprite;
    
    [TextArea(3, 5)] 
    public string itemDescription;
}