using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.ComponentModel;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource bgm;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider bgmSlider;
    [SerializeField] GameObject loud;
    [SerializeField] GameObject mute;
    [SerializeField] string mixerParameter = "BGM"; // 오디오 믹서에서 사용할 파라미터 이름 (BGM 또는 Title)
    [SerializeField] string volumePrefsKey = "Volume"; // PlayerPrefs에 저장할 키 이름
    
    float bgmVolume;

    private void Awake()
    {
        bgm = GetComponent<AudioSource>();
        bgmVolume = bgmSlider.value;
    }

    private void Start()
    {
        bgmSlider.onValueChanged.AddListener(SoundSlider);
        if (PlayerPrefs.HasKey(volumePrefsKey))
        {
            bgmSlider.value = PlayerPrefs.GetFloat(volumePrefsKey);
        }
        else
            bgmSlider.value = 0.5f;

        audioMixer.SetFloat(mixerParameter, Mathf.Log10(bgmSlider.value) * 20);
    }

    public void Update()
    {
        if (bgm.volume == 0 || bgmSlider.value <= 0.001)
        {
            loud.SetActive(false);
            mute.SetActive(true);
        }
        else
        {
            loud.SetActive(true);
            mute.SetActive(false);
        }
    }

    public void PlayBGM() // BGM 재생
    {
        bgm.Play();
    }

    public void StopBGM() // BGM 정지
    {
        bgm.Stop();
    }

    public void Soundbutton() // BGM 토글 조절
    {
        bgm.volume = bgm.volume == 0 ? 1 : 0;
    }

    public void SoundSlider(float volume) // BGM 슬라이더 조절
    {
        audioMixer.SetFloat(mixerParameter, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(volumePrefsKey, bgmSlider.value);
    }
}