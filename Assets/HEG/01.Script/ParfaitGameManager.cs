using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ParfaitGameManager : MonoBehaviour
{
    public static ParfaitGameManager instance;
    public int currentDay = 1;

    public ParfaitRecipeManager recipeManager;
    public ParfaitGenerateManager generateManager;

    Dictionary<int, IngredientData> ingredientsDic;

    private int curParfaitPrice = 0;
    private int todayTotal = 0;

    private int specialGeustCount = 0;
    private int specialMaxGeustCount = 0;
    private int specialFixedGeustCount = 0;

    float specialGuestTimer;

    [SerializeField] private float totalTime = 120f;
    private float remainingTime;
    public Slider timerSlider;
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private Customer customer;
    [SerializeField] private ParfaitBuilder parfaitBuilder;

    [SerializeField] private GameObject resultUI;

    public bool canClick = false;
    public bool isFinish = false;
    public bool isSuccess = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ingredientsDic = CSVManager.instance.ingredientsDic;
        //�� �� Ư���մ� ���� �ο� ����, �ִ� �ο� ���
        (specialFixedGeustCount, specialMaxGeustCount) = GetSpecialGuestLimit(currentDay);
        // �Ϸ� Ÿ�̸� ����
        StartDay();
        //�Ϲ� �մ����� Ư�� �մ����� �����ϰ� �޴� �ֹ�
        StartCoroutine(WaitForSecond(1));
        //�մ� Ÿ�̸�
        StartCoroutine(SpecialGuestTimerChecker());
    }

    void StartDay()
    {
        remainingTime = totalTime;
        timerSlider.maxValue = totalTime;
        timerSlider.value = totalTime;
        StartCoroutine(DayTimerRoutine());
    }

    IEnumerator DayTimerRoutine()
    {
        while (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;

            timerSlider.value = remainingTime;

            int displayTime = Mathf.CeilToInt(remainingTime);
            timerText.text = $"{displayTime}��";

            yield return null;
        }

        // ���� ó��
        timerSlider.value = 0f;
        timerText.text = "0��";
        TryEndDay();

        Debug.Log("�Ϸ� ����! ���� �մ� �Ǵ� ���� ���� ��ȯ");
    }

    public IEnumerator WaitForSecond(float duration)
    {
        Debug.Log("���ο� �մ� ȣ��");
        yield return new WaitForSeconds(duration);
        OrderCustomer();
        customer.ComeCustomer();
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

    public void Fail()
    {
        canClick = false;
        parfaitBuilder.ShowFail();
        customer.FailCustomer();
        StartCoroutine(WaitForSecond(3));
    }

    public void Success()
    {
        isSuccess = true;
        canClick = false;
        parfaitBuilder.ShowSucceess();
        customer.SuccessCustomer();
        todayTotal += curParfaitPrice;
        isSuccess = false;
        StartCoroutine(WaitForSecond(4));
    }

    public void TryEndDay()
    {
        StartCoroutine(WaitUntilSuccessEndsThenEndDay());
    }

    IEnumerator WaitUntilSuccessEndsThenEndDay()
    {
        while (isSuccess)
        {
            yield return null;
        }
        
        customer.OutCustomer();
        isFinish = true;
        canClick = false;

        yield return new WaitForSeconds(1);

        EndDay();
    }

    private void EndDay()
    {
        StopAllCoroutines();
        resultUI.SetActive(true);
    }
}
