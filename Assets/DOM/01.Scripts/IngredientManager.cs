using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class IngredientManager : MonoBehaviour
{
    public static IngredientManager Instance;

    [SerializeField] private UI_Ingredient[] allIngredients;
    public List<UI_Ingredient> unlockIngredients { get; private set; }
    [SerializeField] private ParfaitRecipeManager parfaitRecipeManager;
    [SerializeField] private ParfaitBuilder parfaitBuilder;
   
    private int index = 0;
    private UI_Ingredient curIngredient;

    private void Awake()
    {
        Instance = this;
        unlockIngredients = new List<UI_Ingredient>();
    }


    public void Init()
    {
        CSVManager cSVManager = CSVManager.instance;
        List<int> ids = new();

        for (int i = 0; i < allIngredients.Length; i++)
        {
            allIngredients[i].SetInfo(cSVManager.ingredientsDic[i + 100].id, cSVManager.ingredientsDic[i+100].price);
        }

        // unlockIngredients.Clear();
        // if (SaveLoadManager.Instance.OpenIngredient != null)
        // {
        //     foreach (var ingredient in allIngredients)
        //     {
        //         if (SaveLoadManager.Instance.OpenIngredient.Contains(ingredient.GetID()))
        //         {
        //             ingredient.CheckUnlock();
        //             unlockIngredients.Add(ingredient);
        //         }
        //         else
        //         {
        //             ingredient.transform.SetAsLastSibling();
        //         }
        //     }
        // }
        
        unlockIngredients.Clear();

        // 1. 먼저 Dictionary로 ID → Ingredient 매핑
        Dictionary<int, UI_Ingredient> ingredientDict = allIngredients.ToDictionary(i => i.GetID());

        // 2. open 순서대로 정렬해서 넣기
        foreach (int id in SaveLoadManager.Instance.OpenIngredient)
        {
            if (ingredientDict.TryGetValue(id, out var ingredient))
            {
                ingredient.CheckUnlock();
                unlockIngredients.Add(ingredient);
                ingredient.transform.SetAsLastSibling();
            }
        }

        // 3. 나머지 잠긴 애들만 맨 아래로 보내기
        foreach (var ingredient in allIngredients)
        {
            if (!SaveLoadManager.Instance.OpenIngredient.Contains(ingredient.GetID()))
            {
                ingredient.transform.SetAsLastSibling();
            }
        }
        
        for (int i = 0; i < unlockIngredients.Count; i++)
            ids.Add(unlockIngredients[i].GetID());

        //parfaitRecipeManager.UpdateMakeableNormalParfaits(ids);

        if (unlockIngredients.Count > 0)
        {
            unlockIngredients[0].Select();
            curIngredient = unlockIngredients[0];
        }
    }

    void Update()
    {
        if (ParfaitGameManager.instance.isFinish == true)
        {
            return;
        }

        if (UIStop.isButtonPressed || ClickControl.isUIOpen) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll < 0f)
        {
            index++;
            if (index > unlockIngredients.Count - 1)
                index = 0;

            SwitchIngredient();
        }
        else if (scroll > 0f)
        {
            index--;
            if (index < 0)
                index = unlockIngredients.Count - 1;

            SwitchIngredient();
        }

        if (ParfaitGameManager.instance.canClick == false) { return; }
        
        if (Input.GetMouseButtonDown(0)) // ���� Ŭ��
        {
            if (curIngredient != null)
            {
                Sequence seq = DOTween.Sequence();

                seq.Append(curIngredient.transform.DOScale(1.1f, 0.1f).SetEase(Ease.OutQuad))   // 커짐
                    .Append(curIngredient.transform.DOScale(1f, 0.15f).SetEase(Ease.InQuad)); 
                SelectCurrentIngredient();
            }
        }
    }

    private void SwitchIngredient()
    {
        if (curIngredient != null)
            curIngredient.UnSelect();

        curIngredient = unlockIngredients[index];
        curIngredient.Select();
    }

    public List<int> GetUnlockedIngredientIDs()
    {
        List<int> ids = new List<int>();
        foreach (var ingredient in unlockIngredients)
        {
            ids.Add(ingredient.GetID());
        }
        return ids;
    }

    public bool IsUnlocked(int id)
    {
        foreach (var ingredient in unlockIngredients)
        {
            if (ingredient.GetID() == id)
                return true;
        }
        return false;
    }

    private void SelectCurrentIngredient()
    {
        int selectedId = curIngredient.GetID();

        parfaitBuilder.OnIngredientClicked(selectedId); 
    }

    public void Reset()
    {
        foreach(var ingredient in unlockIngredients)
        {
            ingredient.UnSelect();
        }
    }
}
