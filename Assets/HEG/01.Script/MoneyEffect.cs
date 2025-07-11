using DG.Tweening;
using TMPro;
using UnityEngine;

public class MoneyEffect : MonoBehaviour
{
    public TextMeshProUGUI text;
    private CanvasGroup canvasGroup;
    private Vector3 originPos;
    private RectTransform rect;

    private void Awake()
    {
        originPos = transform.position;
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Init(int amount)
    {
        text.text = $"+{amount}";
        transform.localScale = Vector3.one;
        canvasGroup.alpha = 1f;

        AudioManager.Instance.PlaySFX("Coin");
        Sequence seq = DOTween.Sequence();
        seq.Append(rect.DOAnchorPosY(280f, 1.5f).SetEase(Ease.OutQuad));
        seq.Join(canvasGroup.DOFade(0f, 1.5f));
        seq.Join(rect.DOScale(1.2f, 0.5f).SetLoops(2, LoopType.Yoyo));
        seq.OnComplete(() => Reset());
    }

    public void Reset()
    {
        transform.position = originPos;
        canvasGroup.alpha = 0f;
    }
}
