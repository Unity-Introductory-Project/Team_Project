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

    public void InGame()//���� ��
    {
        Time.timeScale = 1f;
        gameUI.gameObject.SetActive(true);
        pauseUI.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(false);
    }

    public void Pause()//�Ͻ�����
    {
        Time.timeScale = 0f;
        pauseUI.gameObject.SetActive(true);
    }
    
    public void GameOver()//���� ����
    {
        Time.timeScale = 0f;
        gameOverUI.gameObject.SetActive(true);
        pauseUI.gameObject.SetActive(false);
    }
}
