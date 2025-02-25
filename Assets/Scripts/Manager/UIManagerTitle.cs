using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagerTitle : MonoBehaviour
{   
    public void Game()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Tutorial()
    {

    }

    public void Setting()
    {
        
    }

    public void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
