using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public MainMenuManager menuManager;

    private void Start()
    {
        Time.timeScale = 0f;

        menuManager.ShowMainMenu();

        MusicManager.Instance.ApplyState(GameState.MainMenu);
    }

    public void NewGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

    public void LoadGame()
    {
        // Placeholder for now
        Debug.Log("Load Game not implemented yet");
    }

    public void OpenOptions()
    {
        menuManager.ShowOptionsMenu();
    }

    public void OpenCredits()
    {
        Debug.Log("Credits not implemented yet");
        // later: MenuManager.Instance.ShowCredits();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}