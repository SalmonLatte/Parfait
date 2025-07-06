using System.Collections.Generic;
using UnityEngine;

public class ParfaitRecipeManager : MonoBehaviour
{
    public Dictionary<int, ParfaitRecipeData> knownSpecialParfaits = new Dictionary<int, ParfaitRecipeData>();
    public Dictionary<int, ParfaitRecipeData> unknownSpecialParfaits = new Dictionary<int, ParfaitRecipeData>();
    public Dictionary<int, ParfaitRecipeData> cantMakeSpecialParfaits = new Dictionary<int, ParfaitRecipeData>();

    public Dictionary<int, ParfaitRecipeData> normalParfaitRecipeDic = new();
    public Dictionary<int, ParfaitRecipeData> makeableNormalParfaitDic = new();

    HashSet<int> knownIds;

    private void Start()
    {
        SetknownHashcode();
        CategorizeRecipes(CSVManager.instance.parfaitRecipeDic);
    }

    private void SetknownHashcode()
    {
        if (knownSpecialParfaits != null)
        {
            for (int i = 0; i < knownSpecialParfaits.Count; i++)
            {
                knownIds.Add(knownSpecialParfaits[i].id);
            }
        }
    }

    public void CategorizeRecipes(Dictionary<int, ParfaitRecipeData> allRecipes)
    {
        knownSpecialParfaits.Clear();
        unknownSpecialParfaits.Clear();
        cantMakeSpecialParfaits.Clear();

        List<int> unlocked = IngredientManager.Instance.GetUnlockedIngredientIDs();

        foreach (var kv in allRecipes)
        {
            var recipe = kv.Value;
            bool canMake = true;
            foreach (int id in recipe.ingredientIds)
            {
                if (!unlocked.Contains(id))
                {
                    canMake = false;
                    break;
                }
            }

            if (!canMake)
            {
                cantMakeSpecialParfaits[kv.Key] = recipe;
            }
            else if (knownIds.Contains(kv.Key))
                knownSpecialParfaits[kv.Key] = recipe;
            else
                unknownSpecialParfaits[kv.Key] = recipe;
        }
    }

    public void UpdateMakeableNormalParfaits(List<int> unlockedIds)
    {
        normalParfaitRecipeDic = CSVManager.instance.normalParfaitRecipeDic;
        makeableNormalParfaitDic.Clear();

        foreach (var kv in normalParfaitRecipeDic)
        {
            ParfaitRecipeData recipe = kv.Value;
            bool canMake = true;
            string missing = "";

            foreach (int id in recipe.ingredientIds)
            {
                if (id == 0) continue;
                if (!unlockedIds.Contains(id))
                {
                    canMake = false;
                    missing += id + ", ";
                }
            }

            if (canMake)
            {
                makeableNormalParfaitDic[kv.Key] = recipe;
            }
            else
            {
                //Debug.Log($"[����] {recipe.name} - ������ ���: {missing}");
            }
        }

        Debug.Log($"���� ���� �� �ִ� �Ϲ� �ĸ��� ��: {makeableNormalParfaitDic.Count}");
    }
}
