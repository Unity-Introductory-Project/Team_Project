using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagerTitle : MonoBehaviour
{
    SettingUI settingUI;

    private void Awake()
    {
        settingUI = FindObjectOfType<SettingUI>(true);
    }
    public void Game()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Tutorial()
    {

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
