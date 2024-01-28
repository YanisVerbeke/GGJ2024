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
    }

    public void DisplayPause(bool active)
    {
        _pauseContainer.SetActive(active);
    }

    public void DisplayGameOver()
    {
        _gameOverContainer.SetActive(true);
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
        SceneManager.LoadScene(0);
    }
}
