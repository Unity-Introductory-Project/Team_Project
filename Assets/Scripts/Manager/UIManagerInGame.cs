using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] AudioSource soundEffect;

    SoundManager soundManager;

    GameUI gameUI;
    PauseUI pauseUI;
    GameOverUI gameOverUI;
    Canvas canvas;
    GameObject blackImage;
    GameObject hpBg;
    Image image;

    private float pauseSpeed = 3000f;
    private bool isPauseMoving = false;
    private RectTransform pauseRect;
    private float targetY = 0f;

    private void Awake()
    {
        soundManager = FindFirstObjectByType<SoundManager>();

        gameUI = FindFirstObjectByType<GameUI>();
        pauseUI = FindObjectOfType<PauseUI>(true);
        gameOverUI = FindObjectOfType<GameOverUI>(true);
        canvas = FindFirstObjectByType<Canvas>();
        blackImage = canvas.transform.GetChild(1).gameObject;
        hpBg = GameObject.FindGameObjectWithTag("Fill");
        image = hpBg.GetComponent<Image>();

        pauseRect = pauseUI.GetComponent<RectTransform>();

        soundEffect = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // 사운드 버튼 초기 설정
        SetupSoundButton();
    }

    private void SetupSoundButton()
    {
        if (pauseUI != null && soundManager != null)
        {
            // 사운드 버튼 찾기
            Button soundButton = pauseUI.transform.Find("SoundButton")?.GetComponent<Button>();
            if (soundButton != null)
            {
                soundButton.onClick.RemoveAllListeners();
                soundButton.onClick.AddListener(() => {
                    soundManager.ToggleBGM();
                });
            }
            else
            {
                // 정확한 버튼을 찾지 못했을 경우 대안 방법 시도
                
                // 버튼의 정확한 경로를 모르는 경우를 위한 대안
                Button[] allButtons = pauseUI.GetComponentsInChildren<Button>(true);
                foreach (Button btn in allButtons)
                {
                    if (btn.name.Contains("Sound") || btn.name.Contains("소리") || btn.name.Contains("볼륨"))
                    {
                        btn.onClick.RemoveAllListeners();
                        btn.onClick.AddListener(() => {
                            soundManager.ToggleBGM();
                        });
                    }
                }
            }
        }
    }

    private void Update()
    {
        if(isPauseMoving)
        {
            if(pauseRect.anchoredPosition.y > targetY)
            {
                float newY = pauseRect.anchoredPosition.y - pauseSpeed * Time.unscaledDeltaTime;

                if (newY <= targetY)
                {
                    newY = targetY;
                    isPauseMoving = true;
                }

                pauseRect.anchoredPosition = new Vector2(pauseRect.anchoredPosition.x, newY);
            }
            else
            {
                isPauseMoving = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!pauseUI.gameObject.activeInHierarchy && !gameOverUI.gameObject.activeInHierarchy)
            {
                Pause();
            }
            else if(pauseUI.gameObject.activeInHierarchy)
            {
                InGame();
                blackImage.gameObject.SetActive(false);
            }
        }
    }

    public void InGame()//게임 중
    {
        Time.timeScale = 1f;
        gameUI.gameObject.SetActive(true);
        pauseUI.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(false);
        image.color = new Color(114 / 255f, 161 / 255f, 172 / 255f);
    }

    public void Pause()//일시정지
    {
        if(pauseUI.gameObject.activeInHierarchy)
        {
            Time.timeScale = 1f;
            gameUI.gameObject.SetActive(true);
            pauseUI.gameObject.SetActive(false);
            gameOverUI.gameObject.SetActive(false);
            image.color = new Color(114 / 255f, 161 / 255f, 172 / 255f);
            blackImage.SetActive(false);
        }
        else if(!gameOverUI.gameObject.activeInHierarchy)
        {
            Time.timeScale = 0f;
            pauseUI.gameObject.SetActive(true);

            pauseRect.anchoredPosition = new Vector2(pauseRect.anchoredPosition.x, Screen.height);
            isPauseMoving = true;
            blackImage.SetActive(true);
            image.color = new Color(74 / 255f, 107 / 255f, 114 / 255f);

            soundEffect.Play();
            
            // 일시정지 UI가 활성화될 때 버튼 이벤트 다시 설정
            SetupSoundButton();
        }
    }
    
    public void GameOver()//게임 종료
    {
        gameUI.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(true);
        pauseUI.gameObject.SetActive(false);
        blackImage.SetActive(true);
        image.color = new Color(74 / 255f, 107 / 255f, 114 / 255f);
        soundEffect.Play();
        StartCoroutine(DelayedTimeStop());
    }
    private IEnumerator DelayedTimeStop()
    {
        // 업적 애니메이션 등이 실행될 시간 제공
        yield return new WaitForSeconds(1.0f); // 필요한 딜레이 시간으로 조정
        
        // 딜레이 후 타임스케일을 0으로 설정
        Time.timeScale = 0f;
    }

    public void Restart()//게임 재시작
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene");
    }

    public void Resume()//게임 재개
    {
        InGame();
        blackImage.SetActive(false);
        image.color = new Color(114 / 255f, 161 / 255f, 172 / 255f);
    }
}