using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParfaitGameManager : MonoBehaviour
{
    public int currentDay = 1;

    public ParfaitRecipeManager recipeManager;
    public ParfaitGenerateManager generateManager;

    Dictionary<int, IngredientData> ingredientsDic;

    private int curParfaitPrice = 0;

    private int specialGeustCount = 0;
    private int specialMaxGeustCount = 0;
    private int specialFixedGeustCount = 0;

    float specialGuestTimer;

    [SerializeField] private Customer customer;
    [SerializeField] private ParfaitBuilder parfaitBuilder;

    private void Start()
    {
        ingredientsDic = CSVManager.instance.ingredientsDic;
        //�� �� Ư���մ� ���� �ο� ����, �ִ� �ο� ���
        (specialFixedGeustCount, specialMaxGeustCount) = GetSpecialGuestLimit(currentDay);
        //
        StartCoroutine(WaitForSecond());
        //�Ϲ� �մ����� Ư�� �մ����� �����ϰ� �޴� �ֹ�
        //�մ� Ÿ�̸�
        StartCoroutine(SpecialGuestTimerChecker());
    }

    IEnumerator WaitForSecond()
    {
        yield return new WaitForSeconds(3f);

        OrderCustomer();
    }

    private void OrderCustomer()
    {
        ParfaitRecipeData recipe;
        bool isSpecial = false;

        if (ShouldBeSpecialCustomer(currentDay))
        {
            recipe = generateManager.GenerateSpecialParfait();
            if (recipe == null)
            {
                recipe = generateManager.GenerateRandomNormalParfait();
            }
            else
            {
                isSpecial = true;
            }
        }
        else
        {
            recipe = generateManager.GenerateRandomNormalParfait();
            curParfaitPrice = recipe.price;
        }

        if (recipe != null)
        {
            //���� �մ� ������ �ĸ��� UI�� �߰� �ؾ���
            parfaitBuilder.StartNewRecipe(recipe.ingredientIds);
            customer.SpawnCustomer(isSpecial);
            customer.ShowCustomerParfaitUI(recipe);
        }
        else
        {
            Debug.Log("�����ǰ� �����ϴ�. ���� ���� �Ǵ� ��� ó�� �ʿ�");
        }
    }

    private IEnumerator SpecialGuestTimerChecker()
    {
        while (specialGeustCount < specialFixedGeustCount)
        {
            specialGuestTimer += 1f;
            Debug.Log($"Ư�� �մ� ��� �ð�: {specialGuestTimer}��");
            yield return new WaitForSeconds(1f);
        }
    }

    private int calculateMoney(int[] ingredient)
    {
        int price = 0;
        for (int i = 0; i < ingredient.Length; i++)
        {
            price += ingredientsDic[ingredient[i]].price;
        }

        return price;
    }

    private bool ShouldBeSpecialCustomer(int day)
    {
        if (specialGeustCount >= specialMaxGeustCount)
            return false;
        
        //���� 30�� ���� Ư�� �մ� �ȿ��� Ư���մ� �߻�
        if (specialGeustCount < specialFixedGeustCount)
        {
            if (specialGuestTimer >= 30)
            {
                specialGuestTimer = 0;
                return true;
            }
        }

        float chance = GetExtraChance(day);
        return Random.value < chance;
    }


    int GetWeek(int day) => Mathf.Clamp((day - 1) / 7 + 1, 1, 4);

    (int fixedCount, int maxCount) GetSpecialGuestLimit(int day)
    {
        int week = GetWeek(day);
        return week switch
        {
            1 => (0, 1),
            2 => (1, 2),
            3 => (2, 3),
            _ => (3, 4),
        };
    }

    float GetExtraChance(int day)
    {
        return Mathf.Lerp(0.1f, 0.7f, (day - 1) / 29f);
    }
}
