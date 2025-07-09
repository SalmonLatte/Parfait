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

    private int curParfaitPrice = 0;
    [SerializeField]private int todayTotal = 0;

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
    [SerializeField] private ResultManager resultManager;

    public bool canClick = false;
    public bool isFinish = false;
    public bool isSuccess = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentDay = SaveLoadManager.Instance.Day;

        IngredientManager.Instance.Init();
        recipeManager.Init();

        //그 주 특별손님 고정 인원 수랑, 최대 인원 계산
        (specialFixedGeustCount, specialMaxGeustCount) = GetSpecialGuestLimit(currentDay);
        // 하루 타이머 시작
        StartDay();
    }

    void StartDay()
    {
        remainingTime = totalTime;
        timerSlider.maxValue = totalTime;
        timerSlider.value = totalTime;
        StartCoroutine(DayTimerRoutine());
        //일반 손님인지 특별 손님인지 구분하고 메뉴 주문
        StartCoroutine(WaitForSecond(1));
        //손님 타이머
        StartCoroutine(SpecialGuestTimerChecker());
    }

    IEnumerator DayTimerRoutine()
    {
        while (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;

            timerSlider.value = remainingTime;

            int minutes = Mathf.FloorToInt(remainingTime / 60f);
            int seconds = Mathf.FloorToInt(remainingTime % 60f);
            timerText.text = $"{minutes:0}:{seconds:00}";

            yield return null;
        }

        // 종료 처리
        timerSlider.value = 0f;
        timerText.text = "0:00";
        TryEndDay();

        Debug.Log("하루 종료! 다음 손님 또는 다음 날로 전환");
    }

    public IEnumerator WaitForSecond(float duration)
    {
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
                specialGeustCount++;
                specialGuestTimer = 0;
            }
        }
        else
        {
            recipe = generateManager.GenerateRandomNormalParfait();
            curParfaitPrice = recipe.price;
        }

        if (recipe != null)
        {
            //이제 손님 나오고 파르페 UI에 뜨게 해야함
            parfaitBuilder.StartNewRecipe(recipe.ingredientIds);
            customer.SpawnCustomer(isSpecial);
            customer.ShowCustomerParfaitUI(recipe);
        }
        else
        {
            Debug.Log("레시피가 없습니다. 게임 종료 또는 대기 처리 필요");
        }
    }

    private IEnumerator SpecialGuestTimerChecker()
    {
        while (specialGeustCount < specialFixedGeustCount)
        {
            specialGuestTimer += 1f;
            //Debug.Log($"특별 손님 대기 시간: {specialGuestTimer}초");
            yield return new WaitForSeconds(1f);
        }
    }

    private bool ShouldBeSpecialCustomer(int day)
    {
        if (specialGeustCount >= specialMaxGeustCount)
            return false;
        
        //만약 30초 동안 특별 손님 안오면 특별손님 발생
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
        return Mathf.Lerp(0.1f, 0.6f, (day - 1) / 29f);
    }

    public void Fail()
    {
        canClick = false;
        parfaitBuilder.ShowFail();
        customer.FailCustomer();
        StartCoroutine(WaitForSecond(2));
    }

    public void Success()
    {
        isSuccess = true;
        canClick = false;
        parfaitBuilder.ShowSucceess();
        customer.SuccessCustomer();
        todayTotal += curParfaitPrice;
        isSuccess = false;
        StartCoroutine(WaitForSecond(2.1f));
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

        StopAllCoroutines();
        parfaitBuilder.Remove();
        customer.OutCustomer();
        
        isFinish = true;
        canClick = false;
        yield return new WaitForSeconds(1);

        EndDay();
    }

    private void EndDay()
    {
        resultManager.SetInfo(currentDay, SaveLoadManager.Instance.Money, todayTotal);
        
        SaveData();
        resultUI.SetActive(true);
    }
    
    public void SaveData()
    {
        SaveLoadManager.Instance.Day += 1;
        SaveLoadManager.Instance.Money += todayTotal;
        SaveLoadManager.Instance.OpenRecipe = recipeManager.GetKnownRecipeIds();
    }

    public void ResetGame()
    {
        // 상태 초기화
        currentDay = SaveLoadManager.Instance.Day;
        todayTotal = 0;
        specialGeustCount = 0;
        specialGuestTimer = 0;
        isFinish = false;
        isSuccess = false;
        canClick = false;

        // 재료, 레시피 초기화
        IngredientManager.Instance.Init();
        recipeManager.Init();

        //손님 초기화
        customer.OutCustomer();

        //파르페 초기화
        parfaitBuilder.RemoveReset();

        //위치 초기화

        // 특별 손님 조건 초기화
        (specialFixedGeustCount, specialMaxGeustCount) = GetSpecialGuestLimit(currentDay);

        // UI 초기화
        resultUI.SetActive(false);

        // 타이머 및 손님 호출 재시작
        StartDay();
    }

}
