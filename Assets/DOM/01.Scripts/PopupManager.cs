using UnityEngine;

enum Popup
{
    Setting = 0,
    Result,
    Calendar
}
public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;
    
    [SerializeField] private GameObject[] popups;

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
    }

    public void popupOn(int i)
    {
        popups[i].SetActive(true);
    }

    public void popupOff(int i)
    {
        popups[i].SetActive(false);
    }
}
