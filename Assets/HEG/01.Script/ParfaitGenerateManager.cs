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

    // ========== 일반 파르페 생성 ==========
    public ParfaitRecipeData GenerateRandomNormalParfait()
    {
        if (recipeManager.makeableNormalParfaitDic.Count == 0)
        {
            Debug.LogWarning("만들 수 있는 일반 파르페가 없습니다!");
            return null;
        }

        // 딕셔너리의 값들을 리스트로 변환
        List<ParfaitRecipeData> candidates = new List<ParfaitRecipeData>(recipeManager.makeableNormalParfaitDic.Values);

        // 무작위 선택
        int randIndex = Random.Range(0, candidates.Count);
        return candidates[randIndex];
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
