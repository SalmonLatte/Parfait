using UnityEngine;
using UnityEngine.EventSystems;

public class ClickControl : MonoBehaviour
{
    [SerializeField] private GameObject settingUI;
    [SerializeField] private GameObject recipeUI;
    [SerializeField] private GameObject resultUI;
    public static bool isUIOpen = false;

    private void Update()
    {
        if (settingUI.activeSelf == true || recipeUI.activeSelf == true || resultUI.activeSelf == true)
        {
            isUIOpen = true;
        }

        if (settingUI.activeSelf == false && recipeUI.activeSelf == false && resultUI.activeSelf == false)
        {
            isUIOpen = false;
        }
    }

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    isButtonPressed = true;
    //}

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    isButtonPressed = false;
    //}
}
