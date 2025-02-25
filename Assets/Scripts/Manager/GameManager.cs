using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    UIManager uiManager;
    GameUI gameUI;
    GameOverUI gameOverUI;
    AchieveManager achieveManager;

    SoundManager soundManager;

    float maxHP = 100;
    float hp;
    float score;
    bool isDead;
    float time;

    private void Awake()
    {
        uiManager = FindFirstObjectByType<UIManager>();
        gameUI = FindFirstObjectByType<GameUI>();
        gameOverUI = FindObjectOfType<GameOverUI>(true);
        achieveManager = AchieveManager.Instance;

        soundManager = FindFirstObjectByType<SoundManager>();

        hp = maxHP;
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
        ChangeHP(Time.deltaTime);
        ChangeScore(Time.deltaTime);

        gameUI.UpdateHPBar(hp / maxHP);
        gameUI.UpdateScore(score);

        time += Time.deltaTime;

        if (hp <= 0 && isDead == false)
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

    public void ChangeHP(float currentHP)//체력 감소
    {
        hp -= currentHP;
    }

    public void ChangeScore(float currentScore)//점수 증가
    {
        score += currentScore;
    }

    // 외부 접근을 위한 Getter 매서드
    public float GetHP()
    {
        return hp;
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
