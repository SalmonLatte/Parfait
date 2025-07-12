using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StickerUnit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject Decription;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI decriptionText;
    public ParfaitRecipeData recipeData;
    private bool isActive;
    public void SetRecipe(ParfaitRecipeData recipeData)
    {
        this.recipeData = recipeData;
        nameText.text = recipeData.name;
        decriptionText.text = recipeData.menu;
    }

    void CheckActive()
    {
        
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("마우스 들어옴");
        transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 10, 1);
        if (isActive)
            Decription.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("마우스 나감");
        if (isActive)
            Decription.SetActive(false);

    }

    public void ActiveSticker()
    {
        isActive = true;
    }
}
