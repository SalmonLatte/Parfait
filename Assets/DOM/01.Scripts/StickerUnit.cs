using UnityEngine;
using UnityEngine.EventSystems;

public class StickerUnit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("마우스 들어옴");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("마우스 나감");
    }
}
