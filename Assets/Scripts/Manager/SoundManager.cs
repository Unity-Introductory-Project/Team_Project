using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] AudioSource bgmSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider bgmSlider;
    [SerializeField] GameObject loud;
    [SerializeField] GameObject mute;
    
    public AudioClip titleBGM;
    public AudioClip inGameBGM;
    public AudioClip jumpSFX;
    public AudioClip slideSFX;
    public AudioClip hitSFX;
    public AudioClip itemSFX;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("Volume"))
        {
            bgmSlider.value = PlayerPrefs.GetFloat("Volume");
        }
        else
        {
            bgmSlider.value = 0.5f;
        }
        
        audioMixer.SetFloat("BGM", Mathf.Log10(bgmSlider.value) * 20);
        bgmSlider.onValueChanged.AddListener(SoundSlider);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void Update()
    {
        // UI 오브젝트가 사라졌다면 다시 찾기
        if (bgmSlider == null)
        {
            GameObject sliderObj = GameObject.Find("SoundBar"); // 슬라이더의 정확한 이름 확인 필요
            if (sliderObj != null)
                bgmSlider = sliderObj.GetComponent<Slider>();
                

            if (bgmSlider != null)
            {
                bgmSlider.onValueChanged.RemoveListener(SoundSlider); // 중복 방지
                bgmSlider.onValueChanged.AddListener(SoundSlider);
            }
        }

        if (loud == null)
            loud = GameObject.Find("Loud");  // UI 오브젝트 이름 확인 필요

        if (mute == null)
        {
            mute = FindInactiveObjectByTag("Mute");  // 비활성화된 오브젝트 찾기
        }

        // UI가 null이 아니어야 SetActive() 호출 가능
        if (bgmSource.volume == 0 || (bgmSlider != null && bgmSlider.value <= 0.001))
        {
            if (loud != null) loud.SetActive(false);
            if (mute != null) mute.SetActive(true);
        }
        else
        {
            if (loud != null) loud.SetActive(true);
            if (mute != null) mute.SetActive(false);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 로드될 때마다 bgmSlider를 다시 찾고 설정
        GameObject sliderObj = GameObject.Find("SoundBar"); // 슬라이더의 정확한 이름 확인 필요
        if (sliderObj != null)
        {
            bgmSlider = sliderObj.GetComponent<Slider>();
            bgmSlider.onValueChanged.RemoveListener(SoundSlider); // 중복 방지
            bgmSlider.onValueChanged.AddListener(SoundSlider);
            bgmSlider.value = PlayerPrefs.HasKey("Volume") ? PlayerPrefs.GetFloat("Volume") : 0.5f;
        }

        // Mute 오브젝트를 다시 찾기
        if (mute == null)
        {
            mute = FindInactiveObjectByTag("Mute"); // 비활성화된 오브젝트 찾기
        }

        // 씬에 따라 BGM 설정
        if (scene.name == "TitleScene")
        {
            PlayBGM(titleBGM);
        }
        else if (scene.name == "MainScene")
        {
            // 약간의 지연을 두고 사운드 설정을 시도
            StartCoroutine(PlayBGMWithDelay(inGameBGM));
        }
    }

    private IEnumerator PlayBGMWithDelay(AudioClip clip)
    {
        yield return new WaitForSeconds(0.1f); // 아주 짧은 지연 시간
        PlayBGM(clip);
    }

    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip == clip) return;
        
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void ToggleBGM()
    {
        bgmSource.volume = bgmSource.volume == 0 ? 1 : 0;
    }

    public void SoundSlider(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Volume", bgmSlider.value);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    private GameObject FindInactiveObjectByTag(string tag)
    {
        GameObject[] objs = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in objs)
        {
            if (obj.CompareTag(tag) && !obj.activeInHierarchy)
            {
                return obj;
            }
        }
        return null;
    }
}