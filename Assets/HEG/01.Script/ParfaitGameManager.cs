using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ParfaitGameManager : MonoBehaviour
{
    public GameObject titlePanel;
    public RectTransform titleImage;
    
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

    [SerializeField] private TextMeshProUGUI dayUIText;
    [SerializeField] private TextMeshProUGUI moneyUIText;
    [SerializeField] private MoneyEffect moneyEffect;


    [SerializeField] private GameObject resultUI;
    [SerializeField] private ResultManager resultManager;

    public bool canClick = false;
    public bool isFinish = false;
    public bool isSuccess = false;

    public bool getEndTrigger = false; //해피엔딩 조건 달성
    public bool isEndEvent = false; //해피엔딩 파르페 제작 이벤트 중
    private int currentMoney;

    [SerializeField] private GameObject tmpSuccess;
    [SerializeField] private GameObject tmpFail;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Time.timeScale = 0f;

        //타이틀 애니메이션
        titleImage.DOAnchorPosY(titleImage.anchoredPosition.y + 10f, 0.4f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
            // .SetUpdate(true);

        Init();
        // �Ϸ� Ÿ�̸� ����
        // StartDay();
    }

    void Init()
    {
        //오늘 날짜
        currentDay = SaveLoadManager.Instance.Day;

        IngredientManager.Instance.Init();
        recipeManager.Init();

        //�� �� Ư���մ� ���� �ο� ����, �ִ� �ο� ���
        (specialFixedGeustCount, specialMaxGeustCount) = GetSpecialGuestLimit(currentDay);
        // �Ϸ� Ÿ�̸� ����
        // StartDay();
    }
    public void StartDay()
    {
        dayUIText.text = "Day " + SaveLoadManager.Instance.Day.ToString();
        moneyUIText.text = SaveLoadManager.Instance.Money.ToString();
        currentMoney = SaveLoadManager.Instance.Money;
        remainingTime = totalTime;
        timerSlider.maxValue = totalTime;
        timerSlider.value = totalTime;
        StartCoroutine(DayTimerRoutine());
        //�Ϲ� �մ����� Ư�� �մ����� �����ϰ� �޴� �ֹ�
        StartCoroutine(WaitForSecond(1));
        //�մ� Ÿ�̸�
        StartCoroutine(SpecialGuestTimerChecker());
    }

    IEnumerator DayTimerRoutine()
    {
        if (!isEndEvent)
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
            
            // ���� ó��

            timerSlider.value = 0f;
            timerText.text = "0:00";
            TryEndDay();
        }
        else
        {
            timerText.text = "FRIEND";
        }
        yield return null;
        
    }

    public IEnumerator WaitForSecond(float duration)
    {
        yield return new WaitForSeconds(duration);
        AudioManager.Instance.PlaySFX("CustomerIn");
        OrderCustomer();
        customer.ComeCustomer();
    }

    private void OrderCustomer()
    {
        ParfaitRecipeData recipe = generateManager.GenerateSpecialParfait();

        bool isSpecial = false;

        if (isEndEvent)
        {
            recipe = ParfaitGenerateManager.instance.GenerateSpecialParfait();
        }
        else
        {
            if (ShouldBeSpecialCustomer(currentDay))
            {
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
                curParfaitPrice = recipe.price;
            }
            else
            {
                recipe = generateManager.GenerateRandomNormalParfait();
                curParfaitPrice = recipe.price;
            }
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
            //Debug.Log($"Ư�� �մ� ��� �ð�: {specialGuestTimer}��");
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
        return Mathf.Lerp(0.1f, 0.6f, (day - 1) / 29f);
    }

    public void Fail()
    {
        if (isEndEvent)
        {
            canClick = false;
            parfaitBuilder.ShowFail();
            // parfaitBuilder.Remove();
            ParfaitRecipeData recipe = ParfaitGenerateManager.instance.GenerateSpecialParfait();
            parfaitBuilder.StartNewRecipe(recipe.ingredientIds);
            // print("실패: " + parfaitBuilder.targetRecipe);
        }
        else
        {
            canClick = false;
            parfaitBuilder.ShowFail();
            customer.FailCustomer();
            StartCoroutine(WaitForSecond(2));
        }
        
    }

    public void Success()
    {
        if (isEndEvent)
        {
            CutsceneManager.Instance.HappyEnding02();
            print("해피 엔딩 마지막 컷씬 출력");
        }
        isSuccess = true;
        canClick = false;
        generateManager.SuccessUnknowRecipe();
        parfaitBuilder.ShowSucceess();
        customer.SuccessCustomer();
        todayTotal += curParfaitPrice;
        moneyEffect.Init(curParfaitPrice);
        int moeny = todayTotal + currentMoney;
        moneyUIText.text = moeny.ToString();
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

        parfaitBuilder.Remove();
        moneyEffect.Reset();
        generateManager.ResetRecipe();
        IngredientManager.Instance.Reset();

        isFinish = true;
        canClick = false;
        SaveData();

        StopCoroutine("WaitForSecond");
        yield return customer.Reset();
        yield return new WaitForSeconds(1f); 
        
        EndDay();
    }

    private void EndDay()
    {
        StopAllCoroutines();
        SaveLoadManager.Instance.Save();

        print(" ++++++++++++" + SaveLoadManager.Instance.OpenRecipe.Count );
        if (getEndTrigger)
        {
            CutsceneManager.Instance.HappyEnding01();
        }
        
        if (SaveLoadManager.Instance.Day >= 30 && SaveLoadManager.Instance.OpenRecipe.Count < 12)
        {
            print("쳑ㄱㄷ" + currentDay);
            CutsceneManager.Instance.NormalEnding();

            // tmpFail.SetActive(true);
            return;
        }
        else if (SaveLoadManager.Instance.Day < 31 && SaveLoadManager.Instance.OpenRecipe.Count >= 12)
        {
            CutsceneManager.Instance.HappyEnding01();

            // tmpSuccess.SetActive(true);
            return;
        }
        resultManager.SetInfo(SaveLoadManager.Instance.Day, SaveLoadManager.Instance.Money, todayTotal);
        
        resultUI.transform.localScale = Vector3.zero;
        resultUI.SetActive(true);
        resultUI.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
    }
    
    public void SaveData() //HEG
    {
        SaveLoadManager.Instance.Day += 1;
        SaveLoadManager.Instance.Money += todayTotal;
        // SaveLoadManager.Instance.OpenRecipe = recipeManager.GetKnownRecipeIds();
    }

    public void ResetGame()
    {
        // ���� �ʱ�ȭ
        currentDay = SaveLoadManager.Instance.Day;
        todayTotal = 0;
        specialGeustCount = 0;
        specialGuestTimer = 0;
        isFinish = false;
        isSuccess = false;
        canClick = false;

        // ���, ������ �ʱ�ȭ
        IngredientManager.Instance.Init();
        recipeManager.Init();

        //�մ� �ʱ�ȭ
        customer.OutCustomer();

        //�ĸ��� �ʱ�ȭ
        parfaitBuilder.RemoveReset();

        // Ư�� �մ� ���� �ʱ�ȭ
        (specialFixedGeustCount, specialMaxGeustCount) = GetSpecialGuestLimit(currentDay);

        // UI �ʱ�ȭ
        resultUI.transform.DOScale(0f, 0.5f)
            .SetEase(Ease.InBack)
            .OnComplete(() => resultUI.SetActive(false));
        

        // Ÿ�̸� �� �մ� ȣ�� �����
        StartDay();
    }

    public void Shake(RectTransform rt)
    {
        Sequence wave = DOTween.Sequence();

        wave.Append(rt.DOAnchorPosY(rt.anchoredPosition.y, 0.3f).SetEase(Ease.InOutSine))
            .Join(rt.DORotate(new Vector3(0, 0, 8f), 0.3f).SetEase(Ease.InOutSine))
            .Append(rt.DOAnchorPosY(rt.anchoredPosition.y, 0.3f).SetEase(Ease.InOutSine))
            .Join(rt.DORotate(new Vector3(0, 0, -8f), 0.3f).SetEase(Ease.InOutSine))
            .Append(rt.DORotate(Vector3.zero, 0.2f).SetEase(Ease.OutSine));
        wave.Play();
    }
    
    //타이틀
    public void OnClickLoad()
    {
        AudioManager.Instance.PlaySFX("Biscket");
        titlePanel.SetActive(false);
        SaveLoadManager.Instance.Load();
        Init();
        CalanderManager.Instance.Init();

    }
    
    public void OnClickStart()
    {
        AudioManager.Instance.PlaySFX("Biscket");
        titlePanel.SetActive(false);
        print("스타트");
        SaveLoadManager.Instance.Reset();
        ManagerInit();
    }

    public void ManagerInit()
    {
        Init();
        CalanderManager.Instance.Init();
        CutsceneManager.Instance.Intro();
    }
    
    
    ////// 엔딩
    public void StartHappyEndDay()
    {
        isEndEvent = true;
        totalTime = 10000;
        ResetGame();
    }
    
    //타이틀로
    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
