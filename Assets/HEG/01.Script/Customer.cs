using DG.Tweening;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Customer : MonoBehaviour
{
    [SerializeField] private RectTransform customerObject;
    [SerializeField] private Image customerImage;
    [SerializeField] private GameObject chat;

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
        //�մ� �̹��� �ߺ����� �ȵǰ�
        cur = GetCount();
        customerImage.sprite = customers[cur];
        //���� Ư�� �մ��̸� timeSlider������ �۵��ϰ�
        if (isSpecial)
        {
            slider.SetActive(true);
            StartTimer(duration);
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
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            timeSlider.value = duration - elapsed;
            yield return null;
        }

        timeSlider.value = 0;

        ParfaitGameManager.instance.Fail();
        Debug.Log("�մ� �γ��� ��");
    }

    private int GetCount()
    {
        while(true)
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
    }

    public void SuccessCustomer()
    {
        StartCoroutine (SuccessEffect());
    }

    IEnumerator SuccessEffect()
    {
        yield return new WaitForSeconds(2f);
        yield return GoOutCustomer();
    }

    IEnumerator ComeEffect()
    {
        yield return ComeInCustomer().WaitForCompletion();
        chat.SetActive(true);
        ParfaitGameManager.instance.canClick = true;
    }

    IEnumerator OutEffect()
    {
        yield return GoOutCustomer();
    }

    IEnumerator FailEffect()
    {
        if (slider.activeSelf == true)
        {
            ResetTimer();
        }
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
        chat.SetActive( false );
        ParfaitClear();
        Tween goOut = customerObject.DOLocalMoveY(-500, 1.5f, true).SetEase(Ease.OutBack);
        return goOut;
    }

    public Tween ComeInCustomer()
    {
        Tween comeIn = customerObject.DOLocalMoveY(-40, 1f, true).SetEase(Ease.OutBack);
        return comeIn;
    }
}
