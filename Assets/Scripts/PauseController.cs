using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MusicManager;


public class PauseController : MonoBehaviour
{


    public static bool IsPaused => MusicManager.Instance.CurrentState == GameState.Pause;

    [Header("UI References")]
    public GameObject pauseMenuPanel;
    public GameObject optionsPanel;
    public GameObject hudPanel;

    void Update()
    {
        if (InputManager.Instance == null) return;

        if (!InputManager.Instance.PausePressed)
                return;

        if (currentMenu == MenuState.Options)
        {
            OnBackPressed();
            return;
        }

        SetPaused(Time.timeScale > 0f);
        
    }

    public void OnResumePressed()
    {
        if (Time.timeScale == 0f)
        {
            Resume();
        }
        else
        {
            Pause();
        }
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
        MusicManager.Instance.ApplyState(GameState.Pause);
        currentMenu = MenuState.Pause;

        Time.timeScale = 0f;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);

        if (optionsPanel != null)
            optionsPanel.SetActive(false);

        if (hudPanel != null)
            hudPanel.SetActive(false);

 
    }


    public void Resume()
    {
        MusicManager.Instance.ApplyState(GameState.Gameplay);
        currentMenu = MenuState.Gameplay;
        Time.timeScale = 1f;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        if(optionsPanel != null)
            optionsPanel.SetActive(false);

        if (hudPanel != null)
            hudPanel.SetActive(true);


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

    private void SetPaused(bool paused)
    {
        currentMenu = paused ? MenuState.Pause : MenuState.Gameplay;

        Time.timeScale = paused ? 0f : 1f;

        pauseMenuPanel?.SetActive(paused);
        optionsPanel?.SetActive(false);
        hudPanel?.SetActive(!paused);

        if (paused)
        {
            MusicManager.Instance.ApplyState(GameState.Pause);
        }
        else
        {
            MusicManager.Instance.StopAllSecondaryMusic();
            MusicManager.Instance.ApplyState(GameState.Gameplay);
        }
    }


}