using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PauseController : MonoBehaviour
{
    public static bool IsPaused { get; private set; }

    [Header("UI References")]
    public GameObject pauseMenuPanel;
    public GameObject optionsPanel;
    public GameObject hudPanel;

    void Update()
    {
        if (InputManager.Instance != null && InputManager.Instance.PausePressed)
        {
            if (IsPaused) Resume();
            else Pause();
        }
    }

    public void OnResumePressed()
    { 
        Resume();
    }

    public void OnOptionsPressed()
    {
        previousMenu = currentMenu;
        currentMenu = MenuState.Options;

        pauseMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void OnBackPressed()
    {
        ReturnToPreviousMenu();
    }

    public void OnQuitPressed()
    {
        Time.timeScale = 1f; // important reset

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

    public void Pause()
    {
        IsPaused = true;

        Time.timeScale = 0f;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);

        if (hudPanel != null)
            hudPanel.SetActive(false);

        AudioListener.pause = true;
    }

    public void Resume()
    {
        IsPaused = false;

        Time.timeScale = 1f;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        if (hudPanel != null)
            hudPanel.SetActive(true);

        AudioListener.pause = false;
    }

    private enum MenuState
    {
        None,
        Pause, 
        Options
    }

    private MenuState currentMenu = MenuState.None;
    private MenuState previousMenu = MenuState.None;

    private void ReturnToPreviousMenu()
    {
        if (currentMenu == MenuState.Options)
        {
            optionsPanel.SetActive(false);
        }

        currentMenu = previousMenu;

        switch (previousMenu)
        {
            case MenuState.Pause:
                pauseMenuPanel.SetActive(true);
                break;
        }
    }

    private enum InputMode
    {
        Gameplay,
        UI
    }
}