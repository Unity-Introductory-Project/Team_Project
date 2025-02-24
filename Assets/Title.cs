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

    public void UpdateHPBar(float percentage)//ü�� �ݿ�
    {
        hpBar.value = percentage;
    }

    public void UpdateScore(float score)//���� �ݿ�
    {
        scoreText.text = ((int)score).ToString();
    }
}
