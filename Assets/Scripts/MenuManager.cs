using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [SerializeField] private GameObject _menuContainer;
    [SerializeField] private GameObject _pauseContainer;
    [SerializeField] private GameObject _gameOverContainer;
    [SerializeField] private GameObject _uiPlayContainer;
    [SerializeField] private GameObject _creditContainer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void DisplayMainMenu(bool active)
    {
        _menuContainer.SetActive(active);
        _pauseContainer.SetActive(false);
        _gameOverContainer.SetActive(false);
        _uiPlayContainer.SetActive(false);
        _creditContainer.SetActive(false);
    }

    public void DisplayPause(bool active)
    {
        _pauseContainer.SetActive(active);
    }

    public void DisplayCredit()
    {
        _creditContainer.SetActive(true);
    }

    public void DisplayGameOver()
    {
        _gameOverContainer.SetActive(true);
        _uiPlayContainer.SetActive(false);
    }

    public void DisplayPlay(bool active)
    {
        _uiPlayContainer.SetActive(active);
    }

    public void PlayButtonClicked()
    {
        SoundManager.Instance.PlayButton();
        GameManager.Instance.StartGame();
    }

    public void ResumeButtonClicked()
    {
        SoundManager.Instance.PlayButton();
        GameManager.Instance.Resume();
    }

    public void QuitButtonClicked()
    {
        SoundManager.Instance.PlayButton();

        Application.Quit();
    }

    public void MenuButtonClicked()
    {
        SoundManager.Instance.PlayButton();

        SceneManager.LoadScene(0);
    }

    public void CreditButtonClicked()
    {
        SoundManager.Instance.PlayButton();

        DisplayCredit();
    }

    public void BackButtonClicked()
    {
        SoundManager.Instance.PlayButton();

        DisplayMainMenu(true);
    }
}
