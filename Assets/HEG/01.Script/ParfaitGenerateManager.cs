using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParfaitGenerateManager : MonoBehaviour
{
    public static ParfaitGenerateManager instance;

    [SerializeField] private ParfaitRecipeManager recipeManager;
    public List<UI_Ingredient> unlockedIngredients;
    private ParfaitRecipeData tmpData;
    public bool comeOutSpecial = false;

    [SerializeField] private NewRecipeEffect textEffect;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            
            SuccessUnknowRecipe();
        }
    }

    // ========== 일반 파르페 생성 ==========
    //public ParfaitRecipeData GenerateRandomNormalParfait()
    //{
    //    if (recipeManager.makeableNormalParfaitDic.Count == 0)
    //    {
    //        Debug.LogWarning("만들 수 있는 일반 파르페가 없습니다!");
    //        return null;
    //    }

    //    // 딕셔너리의 값들을 리스트로 변환
    //    List<ParfaitRecipeData> candidates = new List<ParfaitRecipeData>(recipeManager.makeableNormalParfaitDic.Values);

    //    // 무작위 선택
    //    int randIndex = Random.Range(0, candidates.Count);
    //    return candidates[randIndex];
    //}

    // ========== 일반 파르페 생성 ==========
    public ParfaitRecipeData GenerateRandomNormalParfait()
    {
        int[] layerWeights = { 5, 10, 15, 20, 20, 15, 15 }; // 1~7단 확률 (총합 100)
        int totalWeight = 100;
        int rand = Random.Range(0, totalWeight);

        int layerCount = 1;
        int acc = 0;
        for (int i = 0; i < layerWeights.Length; i++)
        {
            acc += layerWeights[i];
            if (rand < acc)
            {
                layerCount = i + 1;
                break;
            }
        }

        int[] ingredients = new int[8];
        int price = 0;

        for (int i = 0; i < layerCount; i++)
        {
            int randIndex = Random.Range(0, IngredientManager.Instance.unlockIngredients.Count);
            var uiIngredient = IngredientManager.Instance.unlockIngredients[randIndex];
            int id = uiIngredient.GetID();
            int cost = uiIngredient.GetPirce(); // 가격 정보가 필요

            ingredients[i] = id;
            price += cost;
        }

        for (int i = layerCount; i < 8; i++)
        {
            ingredients[i] = 0; // 나머지 칸은 빈칸
        }

        ParfaitRecipeData recipe = new ParfaitRecipeData
        {
            id = -1, // 고정 ID 아님
            name = $"랜덤 파르페 ({layerCount}단)",
            ingredientIds = ingredients,
            price = price
        };

        return recipe;
    }

    // ========== 특별 파르페 선택 ==========
    public ParfaitRecipeData GenerateSpecialParfait()
    {
        var known = recipeManager.knownSpecialParfaits;
        var unknown = recipeManager.unknownSpecialParfaits;

        float roll = Random.value;

        if (roll < 0.6f && known.Count > 0)
        {
            return GetRandom(known);
        }
        else if (unknown.Count > 0)
        {
            tmpData = GetRandom(unknown);
            comeOutSpecial = true;
            return tmpData;
        }
        else if (known.Count > 0)
        {
            return GetRandom(known);
        }
        else
        {
            return null;
        }
    }

    public void SuccessUnknowRecipe()
    {
        if (comeOutSpecial == false) { return; }

        recipeManager.unknownSpecialParfaits.Remove(tmpData.id);
        recipeManager.knownSpecialParfaits[tmpData.id] = tmpData;

        recipeManager.AddKnownId(tmpData.id);

        textEffect.StartWaveEffect();
        comeOutSpecial = false;
        tmpData = null;
    }

    public void ResetRecipe()
    {
        comeOutSpecial = false;
        tmpData = null;
    }

    private ParfaitRecipeData GetRandom(Dictionary<int, ParfaitRecipeData> dic)
    {
        var values = new List<ParfaitRecipeData>(dic.Values);
        return values[Random.Range(0, values.Count)];
    }
}
