using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StickerUnit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject Decription;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI decriptionText;
    private RectTransform background;
    
    public ParfaitRecipeData recipeData;
    private bool isActive;

    private void Start()
    {
        background = Decription.GetComponent<RectTransform>();
    }

    public void SetRecipe(ParfaitRecipeData recipeData)
    {
        this.recipeData = recipeData;
        nameText.text = recipeData.name;
        decriptionText.text = recipeData.menu;
        
        //배경이랑 텍스트 사이즈 조절
        Vector2 size1 = nameText.GetPreferredValues();
        Vector2 size2 = decriptionText.GetPreferredValues();

        // 가장 넓은 너비를 기준으로, 높이는 합산
        float width = Mathf.Max(size1.x, size2.x);
        float height = size1.y + size2.y;

        Vector2 padding = new Vector2(40f, 40f);
        background.sizeDelta = new Vector2(width, height) + padding;
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
