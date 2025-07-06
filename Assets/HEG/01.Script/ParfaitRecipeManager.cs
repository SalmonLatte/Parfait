using System.Collections.Generic;
using UnityEngine;

public class ParfaitRecipeManager : MonoBehaviour
{
    public Dictionary<int, ParfaitRecipeData> knownSpecialParfaits;
    public Dictionary<int, ParfaitRecipeData> unknownSpecialParfaits;
    public Dictionary<int, ParfaitRecipeData> cantMakeSpecialParfaits;
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
                cantMakeSpecialParfaits[kv.Key] = recipe;
            else if (knownIds.Contains(kv.Key))
                knownSpecialParfaits[kv.Key] = recipe;
            else
                unknownSpecialParfaits[kv.Key] = recipe;
        }
    }
}
