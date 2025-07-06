using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    [SerializeField] private Image customerImage;

    [SerializeField] private Slider timeSlider;
    [SerializeField] private GameObject slider;
    private Coroutine timerCoroutine;
    public float duration = 10;

    [SerializeField] private Sprite[] customers;

    [SerializeField] private GameObject customerParfait;
    [SerializeField] private Image[] parfaitIngredientLayers;
    [SerializeField] private Sprite[] ingredients;
    [SerializeField] private GameObject[] parfaitToppingLayers;

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
}
