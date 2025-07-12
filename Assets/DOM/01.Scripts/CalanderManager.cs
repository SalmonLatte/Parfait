using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CalanderManager : MonoBehaviour
{
    [SerializeField] private int curDay = 1;
    [SerializeField] private RectTransform dayMarker;
    [SerializeField] private StickerUnit[] stickerUnits;
    
    void Start()
    {
        SetDayMarker();
        SaveLoadManager.Instance.onGetRecipe = GetRecipe;


        Init();
    }

    void Init()
    {
        ParfaitRecipeManager parfaitRecipeManager = ParfaitGameManager.instance.recipeManager;
        List<ParfaitRecipeData> allRecipeDatas = parfaitRecipeManager.unknownSpecialParfaits.Values.ToList();
        
        for (int i = 0; i < stickerUnits.Length; i++)
        {
            stickerUnits[i].SetRecipe(allRecipeDatas[i]);
        }
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
