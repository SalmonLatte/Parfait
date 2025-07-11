using UnityEngine;
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

    private void SetVolume(float value)
    {
        audioSource.volume = value; 
    }
}
