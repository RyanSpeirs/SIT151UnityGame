using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        // Ensure correct game state + music
        MusicManager.Instance.ApplyState(GameState.MainMenu);

        // Make sure time is paused in menu
        Time.timeScale = 0f;

        // Open the main menu UI
        MenuManager.Instance.ShowMainMenu();
    }

    public void StartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("GameScene");

        // Set gameplay music AFTER scene loads (important)
        MusicManager.Instance.ApplyState(GameState.Gameplay);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenOptions()
    {
        MenuManager.Instance.ShowOptions();
    }
}
