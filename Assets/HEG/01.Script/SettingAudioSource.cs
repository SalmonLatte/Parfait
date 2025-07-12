using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingAudioSource : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;         
    [SerializeField] private AudioSource audioSource;     

    private void Start()
    {
        volumeSlider.value = audioSource.volume;

        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    void Update()
    {
        // 마우스 왼쪽 버튼을 눌렀을 때
        if (Input.GetMouseButtonDown(0))
        {
            // 1. 마우스가 UI 위에 있는지 확인
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // 2. 어떤 UI 오브젝트를 클릭했는지 확인
                GameObject clickedObj = EventSystem.current.currentSelectedGameObject;

                if (clickedObj != null)
                    Debug.Log($"[UI 클릭됨] {clickedObj.name}");
                else
                    Debug.Log("[UI 클릭됨] 선택된 UI 없음 (currentSelectedGameObject는 null)");
            }
            else
            {
                Debug.Log("UI가 아닌 게임 화면 클릭됨");
            }
        }
    }

    private void SetVolume(float value)
    {
        audioSource.volume = value; 
    }
}
