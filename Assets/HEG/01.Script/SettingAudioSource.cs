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
        // ���콺 ���� ��ư�� ������ ��
        if (Input.GetMouseButtonDown(0))
        {
            // 1. ���콺�� UI ���� �ִ��� Ȯ��
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // 2. � UI ������Ʈ�� Ŭ���ߴ��� Ȯ��
                GameObject clickedObj = EventSystem.current.currentSelectedGameObject;

                if (clickedObj != null)
                    Debug.Log($"[UI Ŭ����] {clickedObj.name}");
                else
                    Debug.Log("[UI Ŭ����] ���õ� UI ���� (currentSelectedGameObject�� null)");
            }
            else
            {
                Debug.Log("UI�� �ƴ� ���� ȭ�� Ŭ����");
            }
        }
    }

    private void SetVolume(float value)
    {
        audioSource.volume = value; 
    }
}
