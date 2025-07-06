using System.Collections.Generic;
using UnityEngine;

public class ParfaitGenerateManager : MonoBehaviour
{
    public static ParfaitGenerateManager instance;

    [SerializeField] private ParfaitRecipeManager recipeManager;
    private Dictionary<int, ParfaitRecipeData> parfaitRecipeDic;
    public List<UI_Ingredient> unlockedIngredients;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        parfaitRecipeDic = CSVManager.instance.parfaitRecipeDic;
    }

    // ========== 일반 파르페 생성 ==========
    public int[] GenerateNormalParfait()
    {
        int[] layerWeights = { 10, 15, 20, 20, 20, 15 }; // 1~6단
        int totalWeight = 100;
        int rand = Random.Range(0, totalWeight);
        int count = 1;
        int acc = 0;
        for (int i = 0; i < layerWeights.Length; i++)
        {
            acc += layerWeights[i];
            if (rand < acc)
            {
                count = i + 1;
                break;
            }
        }

        int[] result = new int[count];
        for (int i = 0; i < count; i++)
        {
            int id = Random.Range(0, unlockedIngredients.Count);
            result[i] = unlockedIngredients[id].GetID();
        }
        return result;
    }

    // ========== 특별 파르페 선택 ==========
    public ParfaitRecipeData GenerateSpecialParfait()
    {
        var known = recipeManager.knownSpecialParfaits;
        var unknown = recipeManager.unknownSpecialParfaits;

        float roll = Random.value;

        if (roll < 0.6f && known.Count > 0)
            return GetRandom(known);
        else if (unknown.Count > 0)
            return GetRandom(unknown);
        else if (known.Count > 0)
            return GetRandom(known);
        else
            return null;
    }

    private ParfaitRecipeData GetRandom(Dictionary<int, ParfaitRecipeData> dic)
    {
        var values = new List<ParfaitRecipeData>(dic.Values);
        return values[Random.Range(0, values.Count)];
    }
}
