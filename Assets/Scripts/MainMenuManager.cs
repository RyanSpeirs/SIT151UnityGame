using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject creditsMenu;

    public void ShowMainMenu()
    {
        mainMenu?.SetActive(true);
        optionsMenu?.SetActive(false);
        creditsMenu?.SetActive(false);
    }

    public void ShowOptionsMenu()
    {
        mainMenu?.SetActive(false);
        optionsMenu?.SetActive(true);
    }

    public void ShowCredits()
    {
        mainMenu?.SetActive(false);
        creditsMenu?.SetActive(true);
    }
}
