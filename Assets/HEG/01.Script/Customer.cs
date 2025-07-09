using DG.Tweening;
using NUnit.Framework;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Customer : MonoBehaviour
{
    [SerializeField] private RectTransform customerObject;
    [SerializeField] private Image customerImage;
    
    [SerializeField] private GameObject chat;
    [SerializeField] private Image chatImage;
    [SerializeField] private GameObject particle;


    [SerializeField] private Slider timeSlider;
    [SerializeField] private GameObject slider;
    private Coroutine timerCoroutine;
    public float duration = 10;

    [SerializeField] private Sprite[] customers;

    [SerializeField] private GameObject customerParfait;
    [SerializeField] private Image[] parfaitIngredientLayers;
    [SerializeField] private Sprite[] ingredients;
    [SerializeField] private GameObject[] parfaitToppingLayers;

    private ParfaitRecipeData tmpData;

    private int cur = -1;

    public void SpawnCustomer(bool isSpecial)
    {
        //손님 이미지 중복으로 안되게
        cur = GetCount();
        customerImage.sprite = customers[cur];
        ChatEffect(isSpecial);
        //만약 특별 손님이면 timeSlider나오고 작동하게
        if (isSpecial)
        {
            StartTimer(duration);
        }
    }

    private void ChatEffect(bool isSpecial)
    {
        if(isSpecial)
        {
            if (ColorUtility.TryParseHtmlString("#E4FFAA", out Color color))
            {
                chatImage.color = color;
                particle.SetActive(true);
            }
        }
        else
        {
            chatImage.color = Color.white;
            particle.SetActive(false);
        }
    }

    public void StartTimer(float customDuration)
    {
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        duration = customDuration;
        timeSlider.maxValue = duration;
        timeSlider.value = duration;

        timerCoroutine = StartCoroutine(RunTimer());
    }

    public void ResetTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        timeSlider.value = duration;
        slider.SetActive(false);
    }

    private IEnumerator RunTimer()
    {
        float elapsed = 0f;
        yield return new WaitForSeconds(1);
        slider.SetActive(true);
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            timeSlider.value = duration - elapsed;
            yield return null;
        }

        timeSlider.value = 0;

        ParfaitGameManager.instance.Fail();
    }

    private int GetCount()
    {
        if (customers.Length <= 1) return 0;

        while (true)
        {
            int count = Random.Range(0, customers.Length);
            if (count != cur)
            {
                return count;
            }
        }
    }

    public void ShowCustomerParfaitUI(ParfaitRecipeData data)
    {
        tmpData = data;
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

    private void ParfaitClear()
    {
        for (int i = 0; i < 8; i++)
        {
            if (i == 7 || tmpData.ingredientIds[i + 1] == 0)
            {
                parfaitToppingLayers[i].transform.GetChild(tmpData.ingredientIds[i] % 100).gameObject.SetActive(false);
                return;
            }
            else
            {
                parfaitIngredientLayers[i].enabled = false;
            }
        }
        tmpData = null;
    }

    public void FailCustomer()
    {
        StartCoroutine(FailEffect());
    }

    public void ComeCustomer()
    {
        StartCoroutine(ComeEffect());
    }

    public void OutCustomer()
    {
        StartCoroutine(OutEffect());
        chat.SetActive(false);
        ParfaitClear();
    }

    public void SuccessCustomer()
    {
        StartCoroutine (SuccessEffect());
    }

    IEnumerator SuccessEffect()
    {
        ResetTimer();
        chat.SetActive(false);
        yield return new WaitForSeconds(1f);
        yield return GoOutCustomer();
    }

    IEnumerator ComeEffect()
    {
        chat.SetActive(true);
        ParfaitGameManager.instance.canClick = true;
        yield return ComeInCustomer().WaitForCompletion();
    }

    IEnumerator OutEffect()
    {
        yield return GoOutCustomer();
    }

    IEnumerator FailEffect()
    {
        ResetTimer();
        yield return Shake().WaitForCompletion();
        yield return GoOutCustomer();
    }

    private Tween Shake()
    {
        Tween shake = customerObject.DOShakeAnchorPos(
            duration: 0.5f,            // 흔들리는 시간
            strength: new Vector2(60f, 0f),  // 좌우 흔들림 강도 (X축만)
            vibrato: 30,              // 진동 횟수
            randomness: 10f,          // 무작위 정도
            snapping: false,
            fadeOut: true
        );
        return shake;
    }

    private Tween GoOutCustomer()
    {
        chat.SetActive(false);
        ParfaitClear();
        Tween goOut = customerObject.DOLocalMoveY(-500, 1f, true).SetEase(Ease.OutBack);
        return goOut;
    }

    public Tween ComeInCustomer()
    {
        Tween comeIn = customerObject.DOLocalMoveY(-50, 1f, true).SetEase(Ease.OutBack);
        return comeIn;
    }
}
