using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuUI : MonoBehaviour
{
    public static bool easyMode = false;

    public GameObject playertype;
    public GameObject Difftype; 

   
    public void PlayMultiplayer()
    {
        SceneManager.LoadScene("LevelOne_Multiplayer");
    } 

   public void PlaySinglePlayer()
   {
        SceneManager.LoadScene("LevelOne_SinglePlayer");
    }

    public void difficultyCheck()
    {
       easyMode = true;
    }

    public void DiffAnswer()
    {
        Difftype.SetActive(false);
        playertype.SetActive(true);
       
    }
    

    public void Quit()
    {
        Application.Quit();

    }
}
