using UnityEngine;

public class CalanderManager : MonoBehaviour
{
    [SerializeField] private int curDay = 1;
    [SerializeField] private RectTransform dayMarker;

    void Start()
    {
        SetDayMarker();
    }

    void SetDayMarker()
    {
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
}
