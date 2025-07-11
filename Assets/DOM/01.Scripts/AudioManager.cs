using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private AudioClip[] sfxClips;
    
    private bool bgmOn = true;
    private bool sfxOn = true;

    private int index = 0;
    private bool isParfaitSFXPlaying = false;
    [SerializeField] private AudioClip[] parfaitSFX;
    
    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        if (PlayerPrefs.HasKey("BGM"))
        {
            bgmOn = PlayerPrefs.GetInt("BGM") == 1;
            BgmOnOff(bgmOn);
        }

        if (PlayerPrefs.HasKey("SFX"))
        {
            sfxOn = PlayerPrefs.GetInt("SFX") == 1;
            SfxOnOff(sfxOn);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isParfaitSFXPlaying == false)
            {
                PlaySFX("Click");
            }
        }
    }

    public void PlaySFX(string clipName)
    {
        foreach (var clip in sfxClips)
        {
            if (clip != null && clip.name == clipName)
            {
                sfxSource.PlayOneShot(clip);
                return;
            }
        }

        Debug.LogWarning($"[AudioManager] '{clipName}' 이름의 효과음을 찾을 수 없습니다.");
    }

    public void PlayParfaitSFX()
    {
        if (index == parfaitSFX.Length)
            index = 0;

        isParfaitSFXPlaying = true;

        sfxSource.clip = parfaitSFX[index];
        sfxSource.Play();

        float clipLength = parfaitSFX[index].length;
        index++;

        StartCoroutine(ResetParfaitSFXFlag(clipLength));
    }

    private IEnumerator ResetParfaitSFXFlag(float delay)
    {
        yield return new WaitForSeconds(delay);
        isParfaitSFXPlaying = false;
    }

    public void BgmOnOff(bool b)
    {
        bgmOn = b;
        bgmSource.volume = b ? 1 : 0;
        PlayerPrefs.SetInt("BGM", b ? 1 : 0);
    }
    
    public void SfxOnOff(bool b)
    {
        sfxOn = b;
        sfxSource.volume = b ? 1 : 0;
        PlayerPrefs.SetInt("SFX", b ? 1 : 0);
    }
    
    public bool BgmOn() { return bgmOn; }
    public bool SfxOn() { return sfxOn; }
}
