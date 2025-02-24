using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    [SerializeField] Button pauseBotton;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Slider hpBar;

    private void Start()
    {
        UpdateHPBar(1);
        UpdateScore(0);
    }

    public void UpdateHPBar(float percentage)//체력 반영
    {
        hpBar.value = percentage;
    }

    public void UpdateScore(float score)//점수 반영
    {
        scoreText.text = ((int)score).ToString();
    }
}
