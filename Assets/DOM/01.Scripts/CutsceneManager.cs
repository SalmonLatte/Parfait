using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public GameObject cutScenePanel;
    public static CutsceneManager Instance;
    //인트로
    public RectTransform intro01Panel;
    public RectTransform intro02Panel;

    public float moveDistance = 100f;  // 위로 올릴 거리
    public float duration = 0.5f;      // 애니메이션 시간
    public Image[] intro01;
    public Image[] intro02;

    //해피엔딩
    public GameObject final01Panel;
    public GameObject final02Panel;
    public Image[] final02;
    public GameObject happyEnd01Panel;
    public GameObject happyEnd02Panel;

    
    //노말엔딩
    public GameObject normalEnd;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }
        
        
        //intro01 안보이게
        for (int i = 0; i < intro01.Length; i++)
        {
            Image img = intro01[i];
            TextMeshProUGUI txt = img.GetComponentInChildren<TextMeshProUGUI>();

            Color imgColor = img.color;
            Color txtColor = txt.color;

            imgColor.a = 0f;
            txtColor.a = 0f;

            img.color = imgColor;
            txt.color = txtColor;
        }
        
        for (int i = 0; i < intro02.Length; i++)
        {
            Image img = intro02[i];
            TextMeshProUGUI txt = img.GetComponentInChildren<TextMeshProUGUI>();

            Color imgColor = img.color;
            Color txtColor = txt.color;

            imgColor.a = 0f;
            txtColor.a = 0f;

            img.color = imgColor;
            txt.color = txtColor;
        }
        
        for (int i = 0; i < final02.Length; i++)
        {
            Image img = final02[i];
            TextMeshProUGUI txt = img.GetComponentInChildren<TextMeshProUGUI>();

            Color imgColor = img.color;
            Color txtColor = txt.color;

            imgColor.a = 0f;
            txtColor.a = 0f;

            img.color = imgColor;
            txt.color = txtColor;
        }
    }

    private void Start()
    {
        // Intro();
        // HappyEnding01();
    }

    public void Intro()
    {
        
        Time.timeScale = 0f;
        cutScenePanel.SetActive(true);
        intro01Panel.gameObject.SetActive(true);
        
        StartCoroutine(Intro01FadeIn());
        
        // Vector2 endPos = intro01Panel.anchoredPosition + new Vector2(0, moveDistance);
        // intro01Panel.DOAnchorPos(endPos, duration)
        //     .SetEase(Ease.OutQuad);
    }

    IEnumerator Intro01FadeIn()
    {
        for (int i = 0; i < intro01.Length; i++)
        {
            Image img = intro01[i];
            TextMeshProUGUI txt = img.GetComponentInChildren<TextMeshProUGUI>();
            
            // 시작 전에 알파 0으로 설정
            Color imgColor = img.color;
            Color txtColor = txt.color;

            imgColor.a = 0f;
            txtColor.a = 0f;

            img.color = imgColor;
            txt.color = txtColor;

            img.gameObject.SetActive(true);
            txt.gameObject.SetActive(true);

            float duration = 1f;
            float time = 0f;

            // Fade In
            while (time < duration)
            {
                float alpha = Mathf.Lerp(0f, 1f, time / duration);

                imgColor.a = alpha;
                txtColor.a = alpha;

                img.color = imgColor;
                txt.color = txtColor;

                time += Time.unscaledDeltaTime;
                yield return null;
            }


            // 보정: 완전히 보이게
            imgColor.a = 1f;
            txtColor.a = 1f;

            img.color = imgColor;
            txt.color = txtColor;

            // 다음 이미지 나오기 전 딜레이
            yield return new WaitForSecondsRealtime(0.3f);
            
        }
        yield return new WaitForSecondsRealtime(1f);
        intro01Panel.gameObject.SetActive(false);
        intro02Panel.gameObject.SetActive(true);

        for (int i = 0; i < intro02.Length; i++)
        {
            Image img = intro02[i];
            TextMeshProUGUI txt = img.GetComponentInChildren<TextMeshProUGUI>();
            
            // 시작 전에 알파 0으로 설정
            Color imgColor = img.color;
            Color txtColor = txt.color;

            imgColor.a = 0f;
            txtColor.a = 0f;

            img.color = imgColor;
            txt.color = txtColor;

            img.gameObject.SetActive(true);
            txt.gameObject.SetActive(true);

            float duration = 1f;
            float time = 0f;

            // Fade In
            while (time < duration)
            {
                float alpha = Mathf.Lerp(0f, 1f, time / duration);

                imgColor.a = alpha;
                txtColor.a = alpha;

                img.color = imgColor;
                txt.color = txtColor;

                time += Time.unscaledDeltaTime;
                yield return null;
            }


            // 보정: 완전히 보이게
            imgColor.a = 1f;
            txtColor.a = 1f;

            img.color = imgColor;
            txt.color = txtColor;

            // 다음 이미지 나오기 전 딜레이
            yield return new WaitForSecondsRealtime(0.3f);
            
        }
        yield return new WaitForSecondsRealtime(1f);

        // foreach (Image img in intro02)
        // {
        //     RectTransform rt = img.GetComponent<RectTransform>();
        //
        //     // 시작 위치를 아래로 살짝 내림
        //     Vector2 startPos = rt.anchoredPosition;
        //     rt.anchoredPosition = startPos - new Vector2(0, 400); // 위로 이동할 거리
        //
        //     // 이미지 활성화 (비활성 상태일 경우)
        //     img.gameObject.SetActive(true);
        //
        //     // 위로 올라오게 애니메이션
        //     rt.DOAnchorPos(startPos, 0.4f).SetEase(Ease.OutQuad);// 이동 시간
        //
        //     yield return new WaitForSeconds(0.4f);// 이미지 간 딜레이
        // }
        intro02Panel.gameObject.SetActive(false);
        
        
        cutScenePanel.SetActive(false);
        Time.timeScale = 1f;
        ParfaitGameManager.instance.StartDay();
    }
    
    public void NormalEnding()
    {
        AudioManager.Instance.PlaySFX("RecipeBook");
        Time.timeScale = 0f;
        cutScenePanel.SetActive(true);
        normalEnd.SetActive(true);
    }

    //게임 클리어 조건 채우면 그 날 결과창 띄우지 말고 HappyEnding01호출, 엔딩 컷씬 1 실행 후 다시 게임으로
    //게임 클리어 하면 HappyEnding02으로 컷씬 2호출
    public void HappyEnding01()
    {
        AudioManager.Instance.PlaySFX("RecipeBook");

        print("엔딩!!!!!!!!!!!");
        cutScenePanel.SetActive(true);
        Time.timeScale = 0f;
        final01Panel.SetActive(true);
        StartCoroutine(CloseAndResume());
    }
    
    IEnumerator CloseAndResume()
    {
        // 실제 시간 기준으로 1초 기다림
        yield return new WaitForSecondsRealtime(3f);

        // 오브젝트 비활성화
        final01Panel.SetActive(false);
        final02Panel.SetActive(true);

        for (int i = 0; i < final02.Length; i++)
        {
            Image img = final02[i];
            TextMeshProUGUI txt = img.GetComponentInChildren<TextMeshProUGUI>();
            
            // 시작 전에 알파 0으로 설정
            Color imgColor = img.color;
            Color txtColor = txt.color;

            imgColor.a = 0f;
            txtColor.a = 0f;

            img.color = imgColor;
            txt.color = txtColor;

            img.gameObject.SetActive(true);
            txt.gameObject.SetActive(true);

            float duration = 1f;
            float time = 0f;

            // Fade In
            while (time < duration)
            {
                float alpha = Mathf.Lerp(0f, 1f, time / duration);

                imgColor.a = alpha;
                txtColor.a = alpha;

                img.color = imgColor;
                txt.color = txtColor;

                time += Time.unscaledDeltaTime;
                yield return null;
            }


            // 보정: 완전히 보이게
            imgColor.a = 1f;
            txtColor.a = 1f;

            img.color = imgColor;
            txt.color = txtColor;

            // 다음 이미지 나오기 전 딜레이
            yield return new WaitForSecondsRealtime(0.3f);
            
        }
        yield return new WaitForSecondsRealtime(1f);
        final02Panel.SetActive(false);

        cutScenePanel.SetActive(false);

        // 타임스케일 다시 1로
        Time.timeScale = 1f;

        ParfaitGameManager.instance.StartHappyEndDay();
    }
    
    public void HappyEnding02()
    {
        // CanvasGroup 세팅
        CanvasGroup cg1 = happyEnd01Panel.AddComponent<CanvasGroup>();
        CanvasGroup cg2 = happyEnd02Panel.AddComponent<CanvasGroup>();
        
        AudioManager.Instance.PlaySFX("RecipeBook");

        Time.timeScale = 0f;
        cutScenePanel.SetActive(true);

        

        cg1.alpha = 0f;
        cg2.alpha = 0f;
        happyEnd01Panel.SetActive(true);
        happyEnd02Panel.SetActive(true); // 미리 켜두되 투명 상태

        Sequence seq = DOTween.Sequence();

        seq.SetUpdate(true); // 타임스케일 무시
        
        seq.Append(cg1.DOFade(1f, 1f));
        seq.AppendInterval(1f); // 2초 대기
        seq.Append(cg1.DOFade(0f, 1f)); // 1초간 사라짐
        seq.AppendCallback(() => happyEnd01Panel.SetActive(false)); // 완전히 끔
        seq.Append(cg2.DOFade(1f, 1f)); // 1초간 등장
    }
}
