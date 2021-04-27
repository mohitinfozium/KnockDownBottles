using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePlayManager: MonoBehaviour
{

    public static bool isGamePaused = false;

    [SerializeField] GameObject pauseMenu;

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isGamePaused = false;
    }

    public void PauseGame()
    {

        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        isGamePaused = true;
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
