using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; 

public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string foodName;
    public Sprite draggingSprite;
    
    [Tooltip("The picture to spawn on the plate when dropped")]
    public Sprite platedSprite; // <--- ADDED THIS 

    public float dragScale = 1.2f; 

    private Image itemImage;
    private GameObject ghostIcon; 

    private void Awake()
    {
        itemImage = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Time.timeScale == 0f) return;
        
        ghostIcon = new GameObject("DragGhost");
        ghostIcon.transform.SetParent(GetComponentInParent<Canvas>().transform);
        ghostIcon.transform.SetAsLastSibling(); 

        Image ghostImage = ghostIcon.AddComponent<Image>();
        ghostImage.sprite = (draggingSprite != null) ? draggingSprite : itemImage.sprite;
        ghostImage.rectTransform.sizeDelta = itemImage.rectTransform.sizeDelta;
        ghostImage.raycastTarget = false;

        ghostIcon.transform.localScale = new Vector3(dragScale, dragScale, dragScale);
        ghostIcon.transform.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Time.timeScale == 0f) return;
        if (ghostIcon != null)
        {
            ghostIcon.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Time.timeScale == 0f) return;
        if (ghostIcon != null)
        {
            Destroy(ghostIcon);
        }
    }
}