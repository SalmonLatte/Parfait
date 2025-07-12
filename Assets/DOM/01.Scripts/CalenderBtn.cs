using UnityEngine;

public class CalenderBtn : MonoBehaviour
{
    [SerializeField] private GameObject calenderUI;
    
    public void OpenCalender()
    {
        calenderUI.SetActive(true);
    }

    public void CloseCalender()
    {
        calenderUI.SetActive(false);
    }
    
}
