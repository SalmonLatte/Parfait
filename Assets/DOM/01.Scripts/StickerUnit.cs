using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class StickerUnit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("마우스 들어옴");
        transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 10, 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("마우스 나감");
    }
}
