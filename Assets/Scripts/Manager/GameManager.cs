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

    SoundManager soundManager;

    float maxHP = 5;
    float hp;
    float score;
    bool isDead;
    float time;

    private void Awake()
    {
        uiManager = FindFirstObjectByType<UIManager>();
        gameUI = FindFirstObjectByType<GameUI>();
        gameOverUI = FindObjectOfType<GameOverUI>(true);

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

        if (hp <= 0)
        {
            isDead = true;
            uiManager.GameOver();
            gameOverUI.PlayTime(time);
            gameOverUI.Score(score);
            soundManager.StopBGM();
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
}
