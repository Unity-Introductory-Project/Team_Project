using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    UIManager uiManager;

    private void Awake()
    {
        uiManager = GetComponent<UIManager>();
    }

    public void Start()
    {
        uiManager.InGame();
    }

    public void ExitGame()//타이틀로 돌아가기
    {
        SceneManager.LoadScene("InGame");
    }
}
