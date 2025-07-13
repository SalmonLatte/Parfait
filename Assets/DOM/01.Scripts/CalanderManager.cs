using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CalanderManager : MonoBehaviour
{
    public static CalanderManager Instance;

    [SerializeField] private GameObject calenderPanel;
    [SerializeField] private int curDay = 1;
    [SerializeField] private RectTransform dayMarker;
    [SerializeField] private StickerUnit[] stickerUnits;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 이동에도 유지
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 있다면 중복 방지
        }
    }

    void Start()
    {
        SetDayMarker();
        SaveLoadManager.Instance.onGetRecipe = GetRecipe;
        Init();
    }

    public void Init()
    {
        
        List<ParfaitRecipeData> allRecipeDatas = CSVManager.instance.parfaitRecipeDic.Values.ToList();
        
        calenderPanel.SetActive(true);
        // Debug.Log(parfaitRecipeManager.unknownSpecialParfaits.Count);
        for (int i = 0; i < stickerUnits.Length; i++)
        {
            stickerUnits[i].SetRecipe(allRecipeDatas[i]);
        }
    
        for (int i = 0; i < SaveLoadManager.Instance.OpenRecipe.Count; i++)
        {
            GetRecipe(SaveLoadManager.Instance.OpenRecipe[i]);
        }
        calenderPanel.SetActive(false);

    }

    void SetDayMarker()
    {
        curDay = SaveLoadManager.Instance.Day;
        int startDayOfWeek = 2; //월요일부터 시작 = 0, 화요일부터 시작 = 1 ...
        int x = (curDay + startDayOfWeek) % 7;
        int y = (curDay + startDayOfWeek) / 7;
        Vector2 pos = new Vector2(x * 200 - 600, y * -170 + 170);
        dayMarker.anchoredPosition = pos;
    }

    public void TestNextDay()
    {
        curDay++;
        SetDayMarker();
    }

    private void GetRecipe(int id)
    {
        for (int i = 0; i < stickerUnits.Length; i++)
        {
            if (stickerUnits[i].recipeData != null)
            {
                if (stickerUnits[i].recipeData.id == id)
                {
                    stickerUnits[i].ActiveSticker();
                }
            }
        }
    }
}
