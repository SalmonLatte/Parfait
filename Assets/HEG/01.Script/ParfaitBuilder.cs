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

    [SerializeField] private Test miniParfait;
    [SerializeField] private RectTransform miniParfaitUI;
    [SerializeField] private Vector3 miniOriginPos;
    [SerializeField] private Vector3 parfaitPos;

    public int click = 0;
    
    void Start()
    {
        miniOriginPos = miniParfaitUI.anchoredPosition;
        parfaitPos = new Vector3(655, -840, 0);
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
        // 너무 많이 쌓음
        if (currentParfait.Count >= targetRecipe.Length)
        {
            Debug.Log("틀렸습니다!");
            ParfaitGameManager.instance.Fail();
            return;
        }

        // 틀렸는지 실시간 확인
        if (targetRecipe[currentParfait.Count] != id)
        {
            Debug.Log("틀렸습니다!");
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

        if (click == 8 || targetRecipe[click + 1] == 0)
        {
            parfaitToppingLayers[click].transform.GetChild(id % 100).gameObject.SetActive(true);
            RectTransform rectTransform = parfaitToppingLayers[click].transform.GetChild(id % 100).GetComponent<RectTransform>();

            Vector3 original = rectTransform.anchoredPosition;
            Vector3 start = original + new Vector3(0, 300); // 위에서 시작

            rectTransform.anchoredPosition = start;

            seq.Append(rectTransform.DOAnchorPos(original, 0.5f).SetEase(Ease.OutBack));

            scaleSeq.Append(rectTransform.DOScale(1.3f, 0.1f).SetEase(Ease.OutQuad));
            scaleSeq.Append(rectTransform.DOScale(0.7f, 0.1f).SetEase(Ease.InOutQuad));
            scaleSeq.Append(rectTransform.DOScale(1.2f, 0.1f).SetEase(Ease.OutQuad));
            scaleSeq.Append(rectTransform.DOScale(0.8f, 0.1f).SetEase(Ease.InOutQuad));
            scaleSeq.Append(rectTransform.DOScale(1.1f, 0.1f).SetEase(Ease.OutQuad));
            scaleSeq.Append(rectTransform.DOScale(0.9f, 0.1f).SetEase(Ease.InOutQuad));
            scaleSeq.Append(rectTransform.DOScale(1f, 0.1f).SetEase(Ease.OutElastic));

            seq.Insert(0f, scaleSeq);
            return;
        }

        Image layer = parfaitIngredientLayers[click];
        layer.enabled = true;
        layer.sprite = ingredients[id % 100];

        RectTransform rect = layer.GetComponent<RectTransform>();
        Vector3 originalPos = rect.anchoredPosition;
        Vector3 startPos = originalPos + new Vector3(0, 200); // 위에서 시작

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
    //성공 보여주기
    public void ShowSucceess()
    {
        StartCoroutine(Success());
    }

    //실패 보여주기
    public void ShowFail()
    {
        StartCoroutine(Fail());
    }

    IEnumerator Fail()
    {
        yield return RemoveParfait().WaitForCompletion();
        ClearVisualStack();
        yield return new WaitForSeconds(1);
        ResetParfait();
    }

    IEnumerator Success()
    {
        SetMiniParfat();
        yield return GiveParfait().WaitForCompletion();
        yield return GiveMiniParfait().WaitForCompletion();
        ClearVisualStack();
        yield return new WaitForSeconds(0.5f);
        yield return miniParfaitUI.DOAnchorPosY(-140, 0.5f, true).SetEase(Ease.OutBack).WaitForCompletion();
        miniParfaitUI.anchoredPosition = miniOriginPos;
        parfait.anchoredPosition = parfaitPos;
        yield return new WaitForSeconds(1);
        ResetParfait();
    }

    private Tween RemoveParfait()
    {
        Tween remove = parfait.DOLocalMoveY(-840, 1f, true).SetEase(Ease.InCubic);
        return remove;
    }

    private Tween GiveParfait()
    {
        Tween give = parfait.DOLocalMoveX(1120, 0.5f, true).SetEase(Ease.Linear);
        return give;
    }

    private Tween ResetParfait()
    {
        Tween reset = parfait.DOLocalMoveY(-62, 1f, true).SetEase(Ease.InCubic);
        return reset;
    }

    private Tween GiveMiniParfait()
    {
        Tween giveMini = miniParfaitUI.DOAnchorPosX(-296, 1f, true).SetEase(Ease.Linear);
        return giveMini;
    }
}
