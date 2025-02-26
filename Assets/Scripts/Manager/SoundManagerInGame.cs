using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.ComponentModel;

public class SoundManagerInGame : MonoBehaviour
{
    [SerializeField] AudioSource bgm;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider bgmSlider;
    [SerializeField] GameObject loud;
    [SerializeField] GameObject mute;
    float bgmVolume;

    private void Awake()
    {
        bgm = GetComponent<AudioSource>();
        bgmVolume = bgmSlider.value;
    }

    private void Start()
    {
        bgmSlider.onValueChanged.AddListener(SoundSlider);
        if (PlayerPrefs.HasKey("Volume"))
        {

            bgmSlider.value = PlayerPrefs.GetFloat("Volume");
        }
        else
            bgmSlider.value = 0.5f;

        audioMixer.SetFloat("BGM", Mathf.Log10(bgmSlider.value) * 20);
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

    public void PlayBGM()//BGM 재생
    {
        bgm.Play();
    }

    public void StopBGM()//BGM 정지
    {
        bgm.Stop();
    }

    public void Soundbutton()//BGM 토글 조절
    {
        bgm.volume = bgm.volume == 0 ? 1 : 0;
    }

    public void SoundSlider(float volume)//BGM 슬라이더 조절
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);

        PlayerPrefs.SetFloat("Volume", bgmSlider.value);
    }
}
