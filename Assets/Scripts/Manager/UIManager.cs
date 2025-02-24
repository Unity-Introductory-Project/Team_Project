using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    SoundManager soundManager;

    GameUI gameUI;
    PauseUI pauseUI;
    GameOverUI gameOverUI;

    private void Awake()
    {
        soundManager = FindFirstObjectByType<SoundManager>();

        gameUI = FindFirstObjectByType<GameUI>();
        pauseUI = FindObjectOfType<PauseUI>(true);
        gameOverUI = FindObjectOfType<GameOverUI>(true);
    }

    public void InGame()//게임 중
    {
        Time.timeScale = 1f;
        gameUI.gameObject.SetActive(true);
        pauseUI.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(false);
    }

    public void Pause()//일시정지
    {
        Time.timeScale = 0f;
        pauseUI.gameObject.SetActive(true);
    }
    
    public void GameOver()//게임 종료
    {
        Time.timeScale = 0f;
        gameOverUI.gameObject.SetActive(true);
        pauseUI.gameObject.SetActive(false);
    }

    public void Restart()//게임 재시작
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("InGame");
    }

    public void Resume()//게임 재개
    {
        InGame();
    }
}
