using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
<<<<<<< HEAD:Assets/Scripts/Manager/UIManager.cs
    SoundManager soundManager;

    GameUI gameUI;
    PauseUI pauseUI;
    GameOverUI gameOverUI;

    private void Awake()
=======
    public TextMeshProUGUI OptionText;
    void Start()
>>>>>>> JH_title:Assets/UIManager.cs
    {
        soundManager = FindFirstObjectByType<SoundManager>();

        gameUI = FindFirstObjectByType<GameUI>();
        pauseUI = FindObjectOfType<PauseUI>(true);
        gameOverUI = FindObjectOfType<GameOverUI>(true);
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

    public void Restart()//���� �����
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("InGame");
    }

    public void Resume()//���� �簳
    {
        InGame();
    }
}
