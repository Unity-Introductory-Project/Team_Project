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
    
    // UI 참조
    private Slider bgmSlider;
    private GameObject loud;
    private GameObject mute;
    private GameObject soundButton;
    
    public AudioClip titleBGM;
    public AudioClip inGameBGM;
    public AudioClip jumpSFX;
    public AudioClip slideSFX;
    public AudioClip hitSFX;
    public AudioClip itemSFX;

    private float currentVolume = 0.5f;
    private bool initialized = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            // AudioSource가 없으면 추가
            if (bgmSource == null)
                bgmSource = gameObject.AddComponent<AudioSource>();
                
            if (sfxSource == null)
                sfxSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // 저장된 볼륨 불러오기
        if (PlayerPrefs.HasKey("Volume"))
        {
            currentVolume = PlayerPrefs.GetFloat("Volume");
        }
        
        // 초기 믹서 볼륨 설정
        if (audioMixer != null)
        {
            audioMixer.SetFloat("BGM", Mathf.Log10(currentVolume) * 20);
        }

        // 씬 로딩 콜백 추가
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        // 현재 씬에서 UI 요소 찾기
        FindAndSetupUIElements();
        initialized = true;
    }

    private void OnDestroy()
    {
        // 소멸 시 콜백 제거
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // UI 요소 찾기 및 설정
    private void FindAndSetupUIElements()
    {
        // 슬라이더 찾기
        GameObject sliderObj = GameObject.Find("SoundBar");
        if (sliderObj != null)
        {
            bgmSlider = sliderObj.GetComponent<Slider>();
            if (bgmSlider != null)
            {
                bgmSlider.onValueChanged.RemoveAllListeners(); // 리스너 중복 방지
                bgmSlider.onValueChanged.AddListener(SoundSlider);
                bgmSlider.value = currentVolume; // 현재 볼륨 값 설정
            }
        }

        // 사운드 버튼 찾기
        GameObject soundButtonObj = GameObject.Find("SoundButton");
        if (soundButtonObj != null)
        {
            if (soundButtonObj.transform.childCount > 0)
            {
                soundButton = soundButtonObj.transform.GetChild(0).gameObject;
                Button btn = soundButton.GetComponent<Button>();
                if (btn != null)
                {
                    btn.onClick.RemoveAllListeners(); // 리스너 중복 방지
                    btn.onClick.AddListener(ToggleBGM);
                }
            }
        }

        // Loud와 Mute 찾기
        loud = GameObject.Find("Loud");
        
        // 비활성화된 Mute 찾기
        if (mute == null)
        {
            mute = FindInactiveObjectByTag("Mute");
        }

        // UI 상태 업데이트
        UpdateVolumeUI();
    }

    // 볼륨 UI 상태 업데이트
    private void UpdateVolumeUI()
    {
        bool isMuted = bgmSource.volume <= 0.001f || (bgmSlider != null && bgmSlider.value <= 0.001f);
        
        if (loud != null) loud.SetActive(!isMuted);
        if (mute != null) mute.SetActive(isMuted);
    }

    public void Update()
    {
        if (!initialized) return;
        
        // UI 요소가 없으면 다시 찾기
        if (bgmSlider == null || loud == null || mute == null)
        {
            FindAndSetupUIElements();
        }

        // 볼륨 상태에 따라 UI 업데이트
        UpdateVolumeUI();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 완전히 로드될 시간을 주기
        StartCoroutine(SetupAfterSceneLoad(scene));
    }

    private IEnumerator SetupAfterSceneLoad(Scene scene)
    {
        // 모든 오브젝트가 초기화될 수 있도록 한 프레임 대기
        yield return null;
        
        // UI 요소 찾기 및 설정
        FindAndSetupUIElements();
        
        // 씬별 BGM 설정
        if (scene.name == "TitleScene")
        {
            PlayBGM(titleBGM);
        }
        else if (scene.name == "MainScene")
        {
            PlayBGM(inGameBGM);
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null || bgmSource == null) return;
        
        if (bgmSource.clip == clip && bgmSource.isPlaying) return;
        
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        if (bgmSource != null)
            bgmSource.Stop();
    }

    public void ToggleBGM()
    {
        if (bgmSource == null) return;
        
        if (bgmSource.volume > 0)
        {
            bgmSource.volume = 0;
        }
        else
        {
            bgmSource.volume = 1;
        }
        
        UpdateVolumeUI();
    }

    public void SoundSlider(float volume)
    {
        currentVolume = volume;
        
        if (audioMixer != null)
        {
            audioMixer.SetFloat("BGM", Mathf.Log10(Mathf.Max(0.001f, volume)) * 20);
        }
        
        PlayerPrefs.SetFloat("Volume", volume);
        UpdateVolumeUI();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    private GameObject FindInactiveObjectByTag(string tag)
    {
        GameObject[] objs = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in objs)
        {
            if (obj.CompareTag(tag))
            {
                return obj;
            }
        }
        return null;
    }
}