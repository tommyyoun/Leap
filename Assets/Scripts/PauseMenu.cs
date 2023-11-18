using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public static bool  VolumeCheck = false;
    public GameObject pauseMenuUI;
    public GameObject volMenuUI; 

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {   if (VolumeCheck)
                {
                }
                else
                {
                    Resume();
                } 
            }

            else
            {
                Paused();
            }
        }
        
    }


     public void Resume ()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Paused() 
    { 
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }


    public void loadVolumePanel ()
    {
        VolumeCheck = true; 
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(false);
        volMenuUI.SetActive(true); 
        
    }

    public void returnPaused()
    {
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        volMenuUI.SetActive(false);
        VolumeCheck = false;
    }
}
