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
    [SerializeField] private int ingredientId;

    private void Start()
    {
        //CheckUnlock();
    }

    public void SetID(int id)
    {
        ingredientId = id;
    }

    //재료 id얻기
    public int GetID() 
    {
        return ingredientId;
    }

    //재료 잠겼는지 안 잠겼는지
    public bool IsLock() { return isLock; }

    //재료 이미지 세팅
    public void SetInfo()
    {
        
    }

    //해금
    public void CheckUnlock()
    {
        isLock = false;
        lockImg.gameObject.SetActive(false);
    }
    
    //선택
    public void Select()
    {
        selectImg.gameObject.SetActive(true);
    }

    //선택되지않음
    public void UnSelect()
    {
        selectImg.gameObject.SetActive(false);
    }
}
