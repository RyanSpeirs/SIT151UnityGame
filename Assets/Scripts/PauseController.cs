using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MusicManager;


public class PauseController : MonoBehaviour
{


    public static bool IsPaused => MusicManager.Instance.CurrentState == MusicManager.GameState.Pause;

    [Header("UI References")]
    public GameObject pauseMenuPanel;
    public GameObject optionsPanel;
    public GameObject hudPanel;

    void Update()
    {
        if (InputManager.Instance != null && InputManager.Instance.PausePressed)
        {
            
            if (currentMenu == MenuState.Options)
            {
                OnBackPressed();
                return;
            }

           
            if (IsPaused)
                Resume();
            else
                Pause();
        }
    }

    public void OnResumePressed()
    { 
        Resume();
    }

    public void OnOptionsPressed()
    {
        currentMenu = MenuState.Options;
        if (pauseMenuPanel != null) 
            pauseMenuPanel.SetActive(false);
        if (optionsPanel != null)
            optionsPanel.SetActive(true);
    }

    public void OnBackPressed()
    {
        switch (currentMenu)
        {
            case MenuState.Options:
                currentMenu = MenuState.Pause;

                optionsPanel.SetActive(false);
                pauseMenuPanel.SetActive(true);
                break;
        }
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
        MusicManager.Instance.SetGameState(MusicManager.GameState.Pause);
        currentMenu = MenuState.Pause;

        Time.timeScale = 0f;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);

        if (optionsPanel != null)
            optionsPanel.SetActive(false);

        if (hudPanel != null)
            hudPanel.SetActive(false);

        AudioListener.pause = true;
    }

    public void Resume()
    {
        MusicManager.Instance.SetGameState(MusicManager.GameState.Gameplay);
        currentMenu = MenuState.Gameplay;
        Time.timeScale = 1f;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        if(optionsPanel != null)
            optionsPanel.SetActive(false);

        if (hudPanel != null)
            hudPanel.SetActive(true);

        AudioListener.pause = false;
    }

    private enum MenuState
    {
        Gameplay,
        Pause, 
        Options
    }

    private MenuState currentMenu = MenuState.Gameplay;


    private enum InputMode
    {
        Gameplay,
        UI
    }
}