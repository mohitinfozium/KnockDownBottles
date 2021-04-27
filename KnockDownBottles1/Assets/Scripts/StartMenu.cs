using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public GameObject StartPanel, WorldsPanel;
    public GameObject SandLevelsPanel,SnowLevelsPanel, JungleLevelsPanel;
    public GameObject QuitPanel, SettingsPanel;
    public static bool isGamePaused = false;

    // Start is called before the first frame update
    public void ResumeGame()
    {
        QuitPanel.SetActive(false);
        Time.timeScale = 1;
        isGamePaused = false;
    }

    public void PauseGame()
    {
     
        QuitPanel.SetActive(true);
        Time.timeScale = 0;
        isGamePaused = true;
    }

    public void settings()
    {
        SettingsPanel.SetActive(true);
        Time.timeScale = 0;
        isGamePaused = true;
    }

    public void SettingsBackButton()
    {
        SettingsPanel.SetActive(false);
        Time.timeScale = 1;
        isGamePaused = false;
    }
    public void LevelsBackButton()
    {
        StartPanel.SetActive(false);
        WorldsPanel.SetActive(true);
        SandLevelsPanel.SetActive(false);
    }
    public void WorldsBackButton()
    {
        StartPanel.SetActive(true);
        WorldsPanel.SetActive(false);
        SandLevelsPanel.SetActive(false);
    }
    void Start()
    {
        StartPanel.SetActive(true);
    }

    public void LostWonBackButton()
    {
        StartPanel.SetActive(false);
        WorldsPanel.SetActive(false);
        SandLevelsPanel.SetActive(true);
    }
    public void LoadWorlds()
    {
        StartPanel.SetActive(false);
        WorldsPanel.SetActive(true);
    }


    //Load your Levels Panel According to theme Selection
    public void SandLoadLevels()
    {
        StartPanel.SetActive(false);
        WorldsPanel.SetActive(false);
        SandLevelsPanel.SetActive(true);
    }
    public void SnowLoadLevels()
    {
        StartPanel.SetActive(false);
        WorldsPanel.SetActive(false);
        SnowLevelsPanel.SetActive(true);
    }
    public void JungleLoadLevels()
    {
        StartPanel.SetActive(false);
        WorldsPanel.SetActive(false);
        SandLevelsPanel.SetActive(true);
    }


    //Levels Loading According theme
    public void LoadLevel1()
    {
        PlayerPrefs.SetInt("Levels", 0);
        SceneManager.LoadScene(1);
    }
    public void LoadLevel2()
    {
        PlayerPrefs.SetInt("Levels", 1);
        SceneManager.LoadScene(1);
    }
    public void LoadLevel3()
    {
        PlayerPrefs.SetInt("Levels", 2);
        SceneManager.LoadScene(1);
    }
    public void LoadLevel4()
    {
        PlayerPrefs.SetInt("Levels", 3);
        SceneManager.LoadScene(1);
    }
    public void LoadLevel5()
    {
        PlayerPrefs.SetInt("Levels", 4);
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
