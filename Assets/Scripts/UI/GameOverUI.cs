using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] Button restartButton;
    [SerializeField] Button exitButton;
    [SerializeField] TextMeshProUGUI playMinuteText;
    [SerializeField] TextMeshProUGUI playSecondText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highScoreText;

    public void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public void PlayTime(float time)
    {
        playMinuteText.text = ((int)(time/60)).ToString();
        playSecondText.text = ((int)(time%60)).ToString();
    }

    public void Score(float score)
    {
        scoreText.text = ((int)score).ToString();

        if(score > PlayerPrefs.GetFloat("HighScore"))
        {
            PlayerPrefs.SetFloat("HighScore", score);
        }

        highScoreText.text = ((int)PlayerPrefs.GetFloat("HighScore")).ToString();
    }
}

