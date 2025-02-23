using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    GameUI gameUI;
    PauseUI pauseUI;
    GameOverUI gameOverUI;

    private void Awake()
    {
        gameUI = FindFirstObjectByType<GameUI>();
        pauseUI = FindFirstObjectByType<PauseUI>();
        gameOverUI = FindFirstObjectByType<GameOverUI>();
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
}
