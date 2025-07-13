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

    // ========== �Ϲ� �ĸ��� ���� ==========
    //public ParfaitRecipeData GenerateRandomNormalParfait()
    //{
    //    if (recipeManager.makeableNormalParfaitDic.Count == 0)
    //    {
    //        Debug.LogWarning("���� �� �ִ� �Ϲ� �ĸ��䰡 �����ϴ�!");
    //        return null;
    //    }

    //    // ��ųʸ��� ������ ����Ʈ�� ��ȯ
    //    List<ParfaitRecipeData> candidates = new List<ParfaitRecipeData>(recipeManager.makeableNormalParfaitDic.Values);

    //    // ������ ����
    //    int randIndex = Random.Range(0, candidates.Count);
    //    return candidates[randIndex];
    //}

    // ========== �Ϲ� �ĸ��� ���� ==========
    public ParfaitRecipeData GenerateRandomNormalParfait()
    {
        int[] layerWeights = { 0, 5, 15, 25, 25, 15, 15 }; // 1~7�� Ȯ�� (���� 100)
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
            int cost = uiIngredient.GetPirce(); // ���� ������ �ʿ�

            ingredients[i] = id;
            price += cost;
        }

        for (int i = layerCount; i < 8; i++)
        {
            ingredients[i] = 0; // ������ ĭ�� ��ĭ
        }

        ParfaitRecipeData recipe = new ParfaitRecipeData
        {
            id = -1, // ���� ID �ƴ�
            name = $"���� �ĸ��� ({layerCount}��)",
            ingredientIds = ingredients,
            price = price
        };

        return recipe;
    }

    // ========== Ư�� �ĸ��� ���� ==========
    public ParfaitRecipeData GenerateSpecialParfait()
    {
        if (ParfaitGameManager.instance.isEndEvent)
            return recipeManager.knownSpecialParfaits[200];
        
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
        SaveLoadManager.Instance.AddRecipe(tmpData.id);
        print(("특별 레시피 성공!!: " +tmpData.id));
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
