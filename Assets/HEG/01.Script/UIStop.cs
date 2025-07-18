using UnityEngine;
using UnityEngine.EventSystems;

public class UIStop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject settingsUI;
    public static bool isButtonPressed = false;

    private void Start()
    {
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        isButtonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isButtonPressed = false;
    }

    public void OpenSettings()
    {
        settingsUI.SetActive(true);
        AudioManager.Instance.PlaySFX("UIOpen");
        Time.timeScale = 0f;
    }

    public void CloseSettings()
    {
        settingsUI.SetActive(false);
        Time.timeScale = 1f;
    }
}
