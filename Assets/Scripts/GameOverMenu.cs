using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [Header("Scene Names")]
    public string gameplaySceneName = "GameScene";
    public string mainMenuSceneName = "MainMenu";


    // Called by Retry button
    public void RetryGame()
    {
        Time.timeScale = 1f; // safety reset
        SceneManager.LoadScene(gameplaySceneName);
    }

    // Called by Quit button (or return to menu)
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // Optional
    public void QuitGame()
    {
        Application.Quit();
    }
}

