using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{   
    public static GameManager Instance { get; private set; }
    CharacterBase Player;
    UIManager uiManager;
    GameUI gameUI;
    GameOverUI gameOverUI;
    AchieveManager achieveManager;
    SoundManager soundManager;

    float score;
    bool isDead;
    float time;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("GameManager가 이미 존재합니다!");
            Destroy(gameObject);
        }
        
        uiManager = FindFirstObjectByType<UIManager>();
        gameUI = FindFirstObjectByType<GameUI>();
        gameOverUI = FindObjectOfType<GameOverUI>(true);
        achieveManager = AchieveManager.Instance;
        soundManager = FindFirstObjectByType<SoundManager>();
        Player = FindObjectOfType<CharacterBase>(true);
        score = 0;
        isDead = false;
        time = 0;
    }

    public void Start()
    {
        uiManager.InGame();
        soundManager.PlayBGM();
    }

    public void Update()
    {
        ChangeScore(Time.deltaTime);

        gameUI.UpdateHPBar(Player.life / Player.maxlife);
        gameUI.UpdateScore(score);

        time += Time.deltaTime;

        if (Player.life <= 0 && isDead == false)
        {
            isDead = true;
            uiManager.GameOver();
            gameOverUI.PlayTime(time);
            gameOverUI.Score(score);
            soundManager.StopBGM();

            // 업적 매니저에 사망 알림
            if (achieveManager != null)
            {
                achieveManager.AddDeath();
            }
        }
    }

    public void ExitGame()//타이틀로 돌아가기
    {
        Debug.Log("ExitGame");
        SceneManager.LoadScene("TitleScene");
    }
    public void ChangeScore(float currentScore)//점수 증가
    {
        score += currentScore;
    }
    public float GetScore()
    {
        return score;
    }
    public float GetTime()
    {
        return time;
    }
    
    public bool IsDead()
    {
        return isDead;
    }
}
