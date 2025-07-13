using NUnit.Framework;

using UnityEngine;
using UnityEngine.UI;

public class ParfaitUI : MonoBehaviour
{
    [SerializeField] private Image[] parfaitIngredientLayers;
    [SerializeField] private Sprite[] ingredients;

    [SerializeField] private GameObject[] parfaitToppingLayers;

    private int[] tmpRecipe;

    public void ShowCustomerParfaitUI(ParfaitRecipeData data)
    {
        for (int i = 0; i < 8; i++)
        {
            if (i == 7 || data.ingredientIds[i + 1] == 0)
            {
                parfaitToppingLayers[i].transform.GetChild(data.ingredientIds[i] % 100).gameObject.SetActive(true);
                return;
            }
            else
            {
                parfaitIngredientLayers[i].enabled = true;
                parfaitIngredientLayers[i].sprite = ingredients[data.ingredientIds[i] % 100];
            }
        }
    }

    public void ShowCustomerParfaitUI(int[] data)
    {
        tmpRecipe = data;
        for (int i = 0; i < 8; i++)
        {
            if (i == 7 || data[i + 1] == 0)
            {
                parfaitToppingLayers[i].transform.GetChild(data[i] % 100).gameObject.SetActive(true);
                return;
            }
            else
            {
                parfaitIngredientLayers[i].enabled = true;
                parfaitIngredientLayers[i].sprite = ingredients[data[i] % 100];
            }
        }
    }

    public void ClearParfait()
    {
        if (tmpRecipe == null)
        {
            Debug.Log("Mini tmpRecipe가 없음");
            for (int i = 0; i < 8; i++)
            {
                parfaitToppingLayers[i].transform.gameObject.SetActive(false);
                parfaitIngredientLayers[i].enabled = false;
            }
            return;
        }
        for (int i = 0; i < 8; i++)
        {
            
            if (i == 7 || tmpRecipe[i + 1] == 0)
            {
                parfaitToppingLayers[i].transform.GetChild(tmpRecipe[i] % 100).gameObject.SetActive(false);
                return;
            }
            else
            {
                parfaitIngredientLayers[i].enabled = false;
            }
        }
        tmpRecipe = null;
    }
}
