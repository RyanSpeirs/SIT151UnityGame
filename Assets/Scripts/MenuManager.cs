using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject hud;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowMainMenu()
    {
        SetAllHidden();

        mainMenu?.SetActive(true);
    }

    public void ShowPauseMenu()
    {
        SetAllHidden();

        pauseMenu?.SetActive(true);
    }

    public void ShowOptionsMenu(bool fromPause = true)
    {
        SetAllHidden();

        optionsMenu?.SetActive(true);
    }

    public void ShowHUD()
    {
        SetAllHidden();

        hud?.SetActive(true);
    }

    public void SetAllHidden()
    {
        mainMenu?.SetActive(false);
        pauseMenu?.SetActive(false);
        optionsMenu?.SetActive(false);
        hud?.SetActive(false);
    }
}