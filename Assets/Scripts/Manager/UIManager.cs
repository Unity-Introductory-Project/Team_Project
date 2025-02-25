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
        Time.timeScale = 0f;
        pauseUI.gameObject.SetActive(true);

        pauseRect.anchoredPosition = new Vector2(pauseRect.anchoredPosition.x, Screen.height);
        isPauseMoving = true;
        blackImage.SetActive(true);
        image.color = new Color(74 / 255f, 107 / 255f, 114 / 255f);

        soundEffect.Play();
    }
    
    public void GameOver()//게임 종료
    {
        Time.timeScale = 0f;
        gameOverUI.gameObject.SetActive(true);
        pauseUI.gameObject.SetActive(false);
        blackImage.SetActive(true);
        image.color = new Color(74 / 255f, 107 / 255f, 114 / 255f);


        soundEffect.Play();
        Debug.Log("asd");
    }

    public void Restart()//게임 재시작
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene");
        image.color = new Color(114 / 255f, 161 / 255f, 172 / 255f);
    }

    public void Resume()//게임 재개
    {
        InGame();
        blackImage.SetActive(false);
        image.color = new Color(114 / 255f, 161 / 255f, 172 / 255f);
    }
}
