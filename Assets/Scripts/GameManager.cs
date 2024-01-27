using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { MENU, PAUSE, PLAY, GAMEOVER }
    private GameState _currentState;

    public event EventHandler OnGameStart;
    public event EventHandler OnGamePause;
    public event EventHandler OnGameResume;
    public event EventHandler OnGameOver;

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

    private void Start()
    {
        Menu();
        //StartGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void StartGame()
    {
        // Init de tout le jeu je sais pas on verra
        OnGameStart?.Invoke(this, EventArgs.Empty);
        MenuManager.Instance.DisplayMainMenu(false);
        Resume();
    }

    public GameState GetCurrentState()
    {
        return _currentState;
    }

    public bool IsPlaying()
    {
        return _currentState == GameState.PLAY;
    }

    public void Menu()
    {
        Time.timeScale = 1;
        _currentState = GameState.MENU;
        MenuManager.Instance.DisplayMainMenu(true);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        _currentState = GameState.PLAY;
        OnGameResume?.Invoke(this, EventArgs.Empty);
        MenuManager.Instance.DisplayPause(false);
    }

    public void Pause()
    {
        _currentState = GameState.PAUSE;
        OnGamePause?.Invoke(this, EventArgs.Empty);
        MenuManager.Instance.DisplayPause(true);
        Time.timeScale = 0;
    }

    public void GameOver()
    {
        _currentState = GameState.GAMEOVER;
        OnGameOver?.Invoke(this, EventArgs.Empty);
    }

    public void TogglePause()
    {
        if (_currentState == GameState.PAUSE)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }
}
