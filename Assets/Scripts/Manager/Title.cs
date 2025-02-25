using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  

public class Title : MonoBehaviour
{
   [SerializeField] Button tutorialButton;
   
   [SerializeField] Button startButton;
   
   [SerializeField] Button settingButton;
   
   [SerializeField] Button exitButton;

   public void StartButton()
   {
      SceneManager.LoadScene("MainScene");
   }
}
