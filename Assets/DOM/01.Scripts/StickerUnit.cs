using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StickerUnit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private  ParfaitUI parfaitUI;
    [SerializeField] private GameObject Decription;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI decriptionText;
    [SerializeField] private GameObject lockImage;
    private RectTransform background;
    
    public ParfaitRecipeData recipeData;
    private bool isActive;


    public void SetRecipe(ParfaitRecipeData recipeData)
    {
        background = Decription.GetComponent<RectTransform>();

        parfaitUI.ShowCustomerParfaitUI(recipeData);
            
        this.recipeData = recipeData;
        nameText.text = recipeData.name;
        decriptionText.text = recipeData.menu;
        
        //배경이랑 텍스트 사이즈 조절
        Vector2 size1 = nameText.GetPreferredValues();
        Vector2 size2 = decriptionText.GetPreferredValues();

        // 가장 넓은 너비를 기준으로, 높이는 합산
        float width = Mathf.Max(size1.x, size2.x);
        float height = size1.y + size2.y;

        float originalHeight = background.sizeDelta.y;
        Vector2 padding = new Vector2(110f, 0f);
        background.sizeDelta = new Vector2(width, originalHeight) + padding;
 
        lockImage.SetActive(true);
        parfaitUI.gameObject.SetActive(false);
        Decription.SetActive(false);
    }

    void CheckActive()
    {
        
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        
        transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 10, 1)
            .OnKill(() => transform.localScale = new Vector3(0.6f, 0.6f, 0.6f))
            .OnComplete(() => transform.localScale = new Vector3(0.6f, 0.6f, 0.6f));
        AudioManager.Instance.PlaySFX("Cake");
        // Decription.SetActive(true);

        transform.SetAsLastSibling();
        if (isActive)
            Decription.SetActive(true);

        // transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Decription.SetActive(false);

        transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

        if (isActive)
            Decription.SetActive(false);

    }

    public void ActiveSticker()
    {
        isActive = true;
        lockImage.SetActive(false);
        parfaitUI.gameObject.SetActive(true);
    }
}
