using System;
using UnityEngine;
using UnityEngine.UI;

//재료 정보 담아서 화면에 표시
public class UI_Ingredient : MonoBehaviour
{
    [SerializeField] private Image lockImg;
    [SerializeField] private Image ingredientImg;
    [SerializeField] private Image selectImg;
    [SerializeField] private bool isLock = true;

    private void Start()
    {
        CheckUnlock();
    }

    //재료 이미지 세팅
    public void SetInfo()
    {
        
    }

    //해금
    public void CheckUnlock()
    {
        if (!isLock)
            lockImg.gameObject.SetActive(false);
    }
    
    //선택
    public void Select()
    {
        selectImg.gameObject.SetActive(true);
    }

    public void UnSelect()
    {
        selectImg.gameObject.SetActive(false);

    }
    public bool IsLock() { return isLock; }
}
