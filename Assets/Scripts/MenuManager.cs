using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [SerializeField] private GameObject _menuContainer;
    [SerializeField] private GameObject _pauseContainer;

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
        DontDestroyOnLoad(this);
    }


    public void DisplayMainMenu(bool active)
    {
        _menuContainer.SetActive(active);
        _pauseContainer.SetActive(false);
    }

    public void DisplayPause(bool active)
    {
        _pauseContainer.SetActive(active);
    }



    public void PlayButtonClicked()
    {
        GameManager.Instance.StartGame();
    }

    public void ResumeButtonClicked()
    {
        GameManager.Instance.Resume();
    }

    public void QuitButtonClicked()
    {
        Application.Quit();
    }
}
