using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public Toggle bgmToggle;
    public Toggle sfxToggle;

    void Start()
    {
        bgmToggle.isOn = AudioManager.Instance.BgmOn();
        sfxToggle.isOn = AudioManager.Instance.SfxOn();
        
        bgmToggle.onValueChanged.AddListener((value) => {
            AudioManager.Instance.BgmOnOff(value);
        });
        
        sfxToggle.onValueChanged.AddListener((value) => {
            AudioManager.Instance.SfxOnOff(value);
        });
    }

}
