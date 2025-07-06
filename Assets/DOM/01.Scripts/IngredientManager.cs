using System.Collections.Generic;
using UnityEngine;

public class IngredientManager : MonoBehaviour
{
    public static IngredientManager Instance;

    [SerializeField] private UI_Ingredient[] allIngredients;
    [SerializeField] private List<UI_Ingredient> unlockIngredients;
    [SerializeField] private ParfaitRecipeManager parfaitRecipeManager;

    [SerializeField] private ParfaitBuilder parfaitBuilder;

    private int index = 0;
    private UI_Ingredient curIngredient;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CSVManager cSVManager = CSVManager.instance;
        List<int> ids = new List<int>();
        for (int i = 0; i < allIngredients.Length; i++)
        {
            if (!allIngredients[i].IsLock()) unlockIngredients.Add(allIngredients[i]);
            allIngredients[i].SetID(cSVManager.ingredientsDic[i + 100].id);
        }

        for (int i = 0; i < unlockIngredients.Count; i++)
        {
            print(unlockIngredients[i].GetID());
            ids.Add(unlockIngredients[i].GetID());
        }
        parfaitRecipeManager.UpdateMakeableNormalParfaits(ids);

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
        
        if (Input.GetMouseButtonDown(0)) // øﬁ¬  ≈¨∏Ø
        {
            if (curIngredient != null)
            {
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
        Debug.Log($"¿Á∑· º±≈√µ : ID = {selectedId}");

        parfaitBuilder.OnIngredientClicked(selectedId); 
    }
}
