using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParfaitBuilder : MonoBehaviour
{
    private List<int> currentParfait = new List<int>();
    private int[] targetRecipe;

    [SerializeField] private Image[] parfaitIngredientLayers;
    [SerializeField] private Sprite[] ingredients;
    
    [SerializeField] private GameObject[] parfaitToppingLayers;

    [SerializeField] private RectTransform parfait;

    [SerializeField] private ParfaitUI miniParfait;
    [SerializeField] private RectTransform miniParfaitUI;
    [SerializeField] private Vector3 miniOriginPos;
    [SerializeField] private Vector3 parfaitPos;
    [SerializeField] private Vector3 parfaitOriginPos;

    public int click = 0;
    
    void Start()
    {
        miniOriginPos = miniParfaitUI.anchoredPosition;
        parfaitPos = new Vector3(662, -950, 0);
        parfaitOriginPos = parfait.anchoredPosition;
    }

    private void Update()
    {
        if (ParfaitGameManager.instance.isFinish == true)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GiveTheParfait();
        }
    }

    public void StartNewRecipe(int[] recipe)
    {
        currentParfait.Clear();
        targetRecipe = recipe;
    }

    public void OnIngredientClicked(int id)
    {
        // �ʹ� ���� ����
        if (currentParfait.Count >= targetRecipe.Length)
        {
            Debug.Log("Ʋ�Ƚ��ϴ�!");
            ParfaitGameManager.instance.Fail();
            return;
        }

        // Ʋ�ȴ��� �ǽð� Ȯ��
        if (targetRecipe[currentParfait.Count] != id)
        {
            Debug.Log("Ʋ�Ƚ��ϴ�!");
            ParfaitGameManager.instance.Fail();
            return;
        }

        currentParfait.Add(id);
        AddVisualLayer(id);
        click++;
    }

    void AddVisualLayer(int id)
    {
        Sequence seq = DOTween.Sequence();
        Sequence scaleSeq = DOTween.Sequence();

        AudioManager.Instance.PlayParfaitSFX();
        if (click >= targetRecipe.Length - 1 || (click < targetRecipe.Length - 1 && targetRecipe[click + 1] == 0))
        {
            parfaitToppingLayers[click].transform.GetChild(id % 100).gameObject.SetActive(true);
            RectTransform rectTransform = parfaitToppingLayers[click].transform.GetChild(id % 100).GetComponent<RectTransform>();

            Vector3 original = rectTransform.anchoredPosition;
            Vector3 start = original + new Vector3(0, 400); // ������ ����

            rectTransform.anchoredPosition = start;
            Vector3 originalScale = rectTransform.localScale;

            seq.Append(rectTransform.DOAnchorPos(original, 0.5f).SetEase(Ease.OutBack));

            scaleSeq.Append(rectTransform.DOScale(originalScale * 1.3f, 0.1f).SetEase(Ease.OutQuad));
            scaleSeq.Append(rectTransform.DOScale(originalScale * 0.7f, 0.1f).SetEase(Ease.InOutQuad));
            scaleSeq.Append(rectTransform.DOScale(originalScale * 1.2f, 0.1f).SetEase(Ease.OutQuad));
            scaleSeq.Append(rectTransform.DOScale(originalScale * 0.8f, 0.1f).SetEase(Ease.InOutQuad));
            scaleSeq.Append(rectTransform.DOScale(originalScale * 1.1f, 0.1f).SetEase(Ease.OutQuad));
            scaleSeq.Append(rectTransform.DOScale(originalScale * 0.9f, 0.1f).SetEase(Ease.InOutQuad));
            scaleSeq.Append(rectTransform.DOScale(originalScale * 1f, 0.1f).SetEase(Ease.OutElastic));

            seq.Insert(0f, scaleSeq);
            return;
        }

        Image layer = parfaitIngredientLayers[click];
        layer.enabled = true;
        layer.sprite = ingredients[id % 100];

        RectTransform rect = layer.GetComponent<RectTransform>();
        Vector3 originalPos = rect.anchoredPosition;
        Vector3 startPos = originalPos + new Vector3(0, 200); // ������ ����

        rect.anchoredPosition = startPos;

        seq.Append(rect.DOAnchorPos(originalPos, 0.5f).SetEase(Ease.OutBack));

        scaleSeq.Append(rect.DOScale(1.3f, 0.1f).SetEase(Ease.OutQuad));
        scaleSeq.Append(rect.DOScale(0.7f, 0.1f).SetEase(Ease.InOutQuad));
        scaleSeq.Append(rect.DOScale(1.2f, 0.1f).SetEase(Ease.OutQuad));
        scaleSeq.Append(rect.DOScale(0.8f, 0.1f).SetEase(Ease.InOutQuad));
        scaleSeq.Append(rect.DOScale(1.1f, 0.1f).SetEase(Ease.OutQuad));
        scaleSeq.Append(rect.DOScale(0.9f, 0.1f).SetEase(Ease.InOutQuad));
        scaleSeq.Append(rect.DOScale(1f, 0.1f).SetEase(Ease.OutElastic));

        seq.Insert(0f, scaleSeq);
    }

    public void GiveTheParfait()
    {
        int cnt = 0;
        for (int i = 0; i < targetRecipe.Length; i++)
        {
            if (targetRecipe[i] != 0)
            {
                cnt++;
            }
        }
        if (currentParfait.Count == cnt)
        {
            AudioManager.Instance.PlaySFX("Give");
            ParfaitGameManager.instance.Success();
        }
    }

    private void SetMiniParfat()
    {
        miniParfait.ShowCustomerParfaitUI(targetRecipe);
    }

    private void ClearVisualStack()
    {
        if (targetRecipe == null) return;
        for (int i = 0; i < targetRecipe.Length; i++)
        {
            if (i == targetRecipe.Length - 1 || targetRecipe[i + 1] == 0)
            {
                parfaitToppingLayers[i].transform.GetChild(targetRecipe[i] % 100).gameObject.SetActive(false);
            }
            else
            {
                parfaitIngredientLayers[i].enabled = false;
            }
        }
        click = 0;
    }
    //���� �����ֱ�
    public void ShowSucceess()
    {
        StartCoroutine(Success());
    }

    //���� �����ֱ�
    public void ShowFail()
    {
        StartCoroutine(Fail());
    }

    public void Remove()
    {
        RemoveParfait2();
    }

    public void RemoveReset()
    {
        ClearVisualStack();
        miniParfait.ClearParfait();
        miniParfaitUI.anchoredPosition = miniOriginPos;
        parfait.anchoredPosition = parfaitOriginPos;
    }

    IEnumerator Fail()
    {
        AudioManager.Instance.PlaySFX("Remove");
        yield return RemoveParfait().WaitForCompletion();
        ClearVisualStack();
        yield return new WaitForSeconds(0.5f);
        ResetParfait();
        if (ParfaitGameManager.instance.isEndEvent)
            ParfaitGameManager.instance.canClick = true;
    }

    IEnumerator Success()
    {
        SetMiniParfat();
        yield return GiveParfait().WaitForCompletion();
        yield return GiveMiniParfait().WaitForCompletion();
        ClearVisualStack();
        yield return new WaitForSeconds(0.3f);
        yield return miniParfaitUI.DOAnchorPosY(-170, 0.5f, true).SetEase(Ease.OutBack).WaitForCompletion();
        miniParfaitUI.anchoredPosition = miniOriginPos;
        parfait.anchoredPosition = parfaitPos;
        ResetParfait();
        miniParfait.ClearParfait();
    }

    private Tween RemoveParfait()
    {
        Tween remove = parfait.DOLocalMoveY(-950, 1f, true).SetEase(Ease.InCubic);
        return remove;
    }

    private Tween RemoveParfait2()
    {
        Tween remove = parfait.DOLocalMoveY(-950, 0.5f, true).SetEase(Ease.InCubic);
        return remove;
    }

    private Tween GiveParfait()
    {
        Tween give = parfait.DOLocalMoveX(1160, 0.3f, true).SetEase(Ease.Linear);
        return give;
    }

    private Tween ResetParfait()
    {
        Tween reset = parfait.DOLocalMoveY(-50, 0.5f, true).SetEase(Ease.InCubic);
        return reset;
    }

    private Tween GiveMiniParfait()
    {
        Tween giveMini = miniParfaitUI.DOAnchorPosX(-470, 0.5f, true).SetEase(Ease.Linear);
        return giveMini;
    }
}
