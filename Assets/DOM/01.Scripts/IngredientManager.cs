using System.Collections.Generic;
using UnityEngine;

public class IngredientManager : MonoBehaviour
{
    public static IngredientManager Instance;

    [SerializeField] private UI_Ingredient[] allIngredients;
    [SerializeField] private List<UI_Ingredient> unlockIngredients;

    private int index = 0;
    private UI_Ingredient curIngredient;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CSVManager cSVManager = CSVManager.instance;

        for (int i = 0; i < allIngredients.Length; i++)
        {
            if (!allIngredients[i].IsLock()) unlockIngredients.Add(allIngredients[i]);
            allIngredients[i].SetID(cSVManager.ingredientsDic[i + 100].id);
            ParfaitGenerateManager.instance.unlockedIngredients = unlockIngredients;
        }

        if (unlockIngredients.Count > 0)
        {
            unlockIngredients[0].Select();
            curIngredient = unlockIngredients[0];
        }
    }

    void Update()
    {
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
}
