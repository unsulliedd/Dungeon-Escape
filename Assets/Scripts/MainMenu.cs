using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _optionsMenuPanel;
    [SerializeField] private GameObject _levelsPanel;
    [SerializeField] private GameObject _statsPanel;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _creditsPanel;

    private GameObject currentPanel;

    private void Start()
    {
        ShowPanel(_mainMenuPanel);
    }

    private void ShowPanel(GameObject panelToShow)
    {
        if (currentPanel != null)
            currentPanel.SetActive(false);

        panelToShow.SetActive(true);
        currentPanel = panelToShow;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenMenu()
    {
        _mainMenuPanel.SetActive(false);
        _optionsMenuPanel.SetActive(true);
    }

    public void OpenLevels()
    {
        _optionsMenuPanel.SetActive(false);
        _levelsPanel.SetActive(true);
    }

    public void OpenSettings()
    {
        _optionsMenuPanel.SetActive(false);
        _settingsPanel.SetActive(true);
    }

    public void OpenStats()
    {
        _optionsMenuPanel.SetActive(false);
        _statsPanel.SetActive(true);
    }

    public void OpenCredits()
    {
        _optionsMenuPanel.SetActive(false);
        _creditsPanel.SetActive(true);
    }

    public void NavigateBack()
    {
        if (_optionsMenuPanel.activeSelf)
        {
            currentPanel = _optionsMenuPanel;
            ShowPanel(_mainMenuPanel);
        }
        else if (_levelsPanel.activeSelf)
        {
            currentPanel = _levelsPanel;
            ShowPanel(_optionsMenuPanel);
        }
        else if (_statsPanel.activeSelf)
        {
            currentPanel = _statsPanel;
            ShowPanel(_optionsMenuPanel);
        }
        else if (_settingsPanel.activeSelf)
        {
            currentPanel = _settingsPanel;
            ShowPanel(_optionsMenuPanel);
        }
        else if (_creditsPanel.activeSelf)
        {
            currentPanel = _creditsPanel;
            ShowPanel(_optionsMenuPanel);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
