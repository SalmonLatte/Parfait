using System.Collections.Generic;
using UnityEngine;

public class IngredientManager : MonoBehaviour
{
    [SerializeField] private UI_Ingredient[] allIngredients;
    [SerializeField] private List<UI_Ingredient> unlockIngredients;

    private int index = 0;
    private UI_Ingredient curIngredient;
    void Start()
    {
        for (int i = 0; i < allIngredients.Length; i++)
            if (!allIngredients[i].IsLock()) unlockIngredients.Add(allIngredients[i]);
        unlockIngredients[0].Select();
        curIngredient = unlockIngredients[0];
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll < 0f)
        {
            index++;
            if (index > unlockIngredients.Count - 1)
                index = 0;
            
            if (curIngredient)
            {
                curIngredient.UnSelect();
            }
            curIngredient = unlockIngredients[index];
            curIngredient.Select();
        }
        else if (scroll > 0f)
        {
            index--;
            if (index < 0)
                index = unlockIngredients.Count - 1;
           
            
            if (curIngredient)
            {
                curIngredient.UnSelect();
            }
            curIngredient = unlockIngredients[index];
            curIngredient.Select();
        }
        
        
    }
}
