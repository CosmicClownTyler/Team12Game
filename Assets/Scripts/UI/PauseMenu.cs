﻿using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public MenuPageGroup pauseMenuPageGroup;
    public MenuPage mainPage;
    public MenuPage settingsPage;
    public GameObject menuPopup;

    public void Resume()
    {
        PauseManager.Instance.Resume();
    }

    public void ShowPauseMenu()
    {
        gameObject.SetActive(true);
    }
    public void HidePauseMenu()
    {
        gameObject.SetActive(false);

        pauseMenuPageGroup.SelectPage(mainPage);
        HideMenuModal();
    }

    public void ShowMenuModal()
    {
        menuPopup.SetActive(true);
    }
    public void HideMenuModal()
    {
        menuPopup.SetActive(false);
    }

    public void ReturnToMenu()
    {
        GameManager.Instance.LoadScene("Menu");
        HidePauseMenu();
        PauseManager.Instance.IsPaused = false;
        Time.timeScale = 1;
    }
}