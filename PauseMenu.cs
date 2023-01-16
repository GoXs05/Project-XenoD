using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [SerializeField] public GameObject pauseMenuUI;

    private bool canPauseGame = true;
    private bool canResumeGame = false;

    // Update is called once per frame
    void Update()
    {
        //Write Code for determining if you can pause/resume the game

        Pauser();
    }



    void Pauser()
    {
        if (((canPauseGame && !GameIsPaused) || (canResumeGame && GameIsPaused)) && Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }

            else 
            {
                Pause();
            }       
        }
    }



    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        canResumeGame = false;
        canPauseGame = true;
    }



    public void Pause() 
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        canResumeGame = true;
        canPauseGame = false;
    }



    public void ExitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
