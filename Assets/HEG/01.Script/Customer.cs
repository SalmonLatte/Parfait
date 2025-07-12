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

    [SerializeField] private Image faceImage;
    [SerializeField] private Sprite[] faceSprit;
    private int cur = -1;

    public void SpawnCustomer(bool isSpecial)
    {
        //�մ� �̹��� �ߺ����� �ȵǰ�
        cur = GetCount();
        customerImage.sprite = customers[cur];
        ChatEffect(isSpecial);
        //���� Ư�� �մ��̸� timeSlider������ �۵��ϰ�
        if (isSpecial)
        {
            AudioManager.Instance.PlaySFX("Special");
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
        faceImage.sprite = faceSprit[0];

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
        faceImage.sprite = faceSprit[0];
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
            float remaining = duration - elapsed;
            timeSlider.value = remaining;

            float ratio = remaining / duration;

            // 1/3 �������� ��ǥ��
            if (ratio <= 2f / 3f)
            {
                faceImage.sprite = faceSprit[1];
            }

            // 2/3 �������� ȭ��
            if (ratio <= 1f / 3f)
            {
                faceImage.sprite = faceSprit[2];
            }

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
        if (tmpData == null)
            return;
        
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
        StopCoroutine("ComeCustomer");
        StartCoroutine(OutEffect());
        chat.SetActive(false);
        ParfaitClear();
    }

    public IEnumerator Reset()
    {
        StopCoroutine("ComeCustomer");
        ParfaitClear();
        ResetTimer();
        yield return StartCoroutine(OutEffect());
    }

    public void SuccessCustomer()
    {
        StartCoroutine (SuccessEffect());
    }

    IEnumerator SuccessEffect()
    {
        AudioManager.Instance.PlaySFX("Success");
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
        AudioManager.Instance.PlaySFX("Fail");
        ResetTimer();
        yield return Shake().WaitForCompletion();
        yield return GoOutCustomer();
    }

    private Tween Shake()
    {
        Tween shake = customerObject.DOShakeAnchorPos(
            duration: 0.5f,            // ��鸮�� �ð�
            strength: new Vector2(60f, 0f),  // �¿� ��鸲 ���� (X�ุ)
            vibrato: 30,              // ���� Ƚ��
            randomness: 10f,          // ������ ����
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
