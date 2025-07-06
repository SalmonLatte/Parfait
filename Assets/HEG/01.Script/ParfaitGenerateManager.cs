using System.Collections.Generic;
using UnityEngine;

public class ParfaitGenerateManager : MonoBehaviour
{
    public static ParfaitGenerateManager instance;

    [SerializeField] private ParfaitRecipeManager recipeManager;
    public List<UI_Ingredient> unlockedIngredients;

    private void Awake()
    {
        instance = this;
    }

    // ========== �Ϲ� �ĸ��� ���� ==========
    public ParfaitRecipeData GenerateRandomNormalParfait()
    {
        if (recipeManager.makeableNormalParfaitDic.Count == 0)
        {
            Debug.LogWarning("���� �� �ִ� �Ϲ� �ĸ��䰡 �����ϴ�!");
            return null;
        }

        // ��ųʸ��� ������ ����Ʈ�� ��ȯ
        List<ParfaitRecipeData> candidates = new List<ParfaitRecipeData>(recipeManager.makeableNormalParfaitDic.Values);

        // ������ ����
        int randIndex = Random.Range(0, candidates.Count);
        return candidates[randIndex];
    }

    // ========== Ư�� �ĸ��� ���� ==========
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
