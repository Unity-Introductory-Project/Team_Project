using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagerTitle : MonoBehaviour
{
    SettingUI settingUI;
    GameObject canvas;
    GameObject tutorial1;
    GameObject tutorial2;
    GameObject tutorial3;

    private void Awake()
    {
        settingUI = FindObjectOfType<SettingUI>(true);
        canvas = GameObject.Find("Canvas");
        tutorial1 = canvas.gameObject.transform.GetChild(9).gameObject;
        tutorial2 = canvas.gameObject.transform.GetChild(10).gameObject;
        tutorial3 = canvas.gameObject.transform.GetChild(11).gameObject;
    }
    public void Game()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void StartTutorial()
    {
        tutorial1.SetActive(true);
    }

    public void NextTutorial1()
    {
        tutorial1.SetActive(false);
        tutorial2.SetActive(true);
    }

    public void NextTutorial2()
    {
        tutorial2.SetActive(false);
        tutorial3.SetActive(true);
    }

    public void ExitTutorial()
    {
        tutorial3.SetActive(false);
    }

    public void Setting()
    {
        if(settingUI.gameObject.activeInHierarchy)
        {
            settingUI.gameObject.SetActive(false);
        }
        else
        {
            settingUI.gameObject.SetActive(true);
        }
    }

    public void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}

