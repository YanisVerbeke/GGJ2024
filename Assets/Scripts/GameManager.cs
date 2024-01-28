using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { MENU, PAUSE, ROAMING, QTE, GAMEOVER }
    private GameState _currentState;
    private GameState _lastPlayedState;

    public event EventHandler OnGameStart;
    public event EventHandler OnGamePause;
    public event EventHandler OnGameResume;
    public event EventHandler OnGameOver;


    [SerializeField] private GameObject _roamingScene;
    [SerializeField] private GameObject _qteScene;
    private TargetDifficulties _targetDifficulty = TargetDifficulties.EASY;
    public TargetDifficulties TargetDifficulty { get { return _targetDifficulty; } set { _targetDifficulty = value; } }

    private int _currentLives;

    [SerializeField] private Transform _transitionPrefab;
    private float _transitionTime = 1.3f;

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
        SoundManager.Instance.StartGameMusic();
        _currentLives = 3;
        OnGameStart?.Invoke(this, EventArgs.Empty);
        MenuManager.Instance.DisplayMainMenu(false);
        SwitchStateToRoaming();
        Resume();
    }

    public GameState GetCurrentState()
    {
        return _currentState;
    }

    public bool IsPlaying()
    {
        return _currentState == GameState.QTE || _currentState == GameState.ROAMING;
    }

    public bool IsQte()
    {
        return _currentState == GameState.QTE;
    }

    public void SwitchStateToQte()
    {
        _currentState = GameState.QTE;
        _lastPlayedState = _currentState;
        SpawnTransition();
        StartCoroutine(SwitchQteState(_transitionTime));
        MenuManager.Instance.DisplayPlay(false);
    }

    public bool IsRoaming()
    {
        return _currentState == GameState.ROAMING;
    }

    public void SwitchStateToRoaming()
    {
        _currentState = GameState.ROAMING;
        _lastPlayedState = _currentState;
        StartCoroutine(SwitchRoamingState(0));
        MenuManager.Instance.DisplayPlay(true);
    }

    public int GetCurrentLives()
    {
        return _currentLives;
    }

    public void LoseLife()
    {
        _currentLives--;
        if (_currentLives <= 0)
        {
            GameOver();
        }
    }

    public void Menu()
    {
        SoundManager.Instance.StartMenuMusic();
        Time.timeScale = 1;
        _currentState = GameState.MENU;
        MenuManager.Instance.DisplayMainMenu(true);
        _roamingScene.SetActive(false);
        _qteScene.SetActive(false);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        _currentState = _lastPlayedState;
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
        MenuManager.Instance.DisplayGameOver();
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

    public void SpawnTransition()
    {
        SoundManager.Instance.PlayTransition();
        GameObject transition = Instantiate(_transitionPrefab, Camera.main.transform).gameObject;
        transition.transform.localPosition = new Vector3(0, 0, 10);
        Destroy(transition, _transitionTime);
    }

    IEnumerator SwitchRoamingState(float transitionTime)
    {
        yield return new WaitForSeconds(transitionTime);
        _roamingScene.SetActive(true);
        _qteScene.SetActive(false);
    }

    IEnumerator SwitchQteState(float transitionTime)
    {
        yield return new WaitForSeconds(transitionTime);
        _roamingScene.SetActive(false);
        _qteScene.SetActive(true);
    }
}
