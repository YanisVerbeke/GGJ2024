using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Qte Manager, handle all related to the QTE gameplay
/// </summary>
public class QteManager : MonoBehaviour
{
    [SerializeField]
    private QteKey[] _qteKeys;

    // Difficulty setters
    private const int _numberOfDifficulties = 3;

    [SerializeField]
    private int _baseQteCounter;
    private int[] _basesQteCounterDifficulty = new int[_numberOfDifficulties] { 5, 10, 5 };
    private int _totalGlobalCount;
    private int _currentGlobaCount;

    [SerializeField]
    private int _numberOfQte;
    private int[] _numberOfQteAtSameTimeDifficulty = new int[_numberOfDifficulties] { 1, 1, 2 };

    [SerializeField]
    private int _secondsBeforeFailure;
    private int[] _secondsBeforeFailDifficulty = new int[_numberOfDifficulties] { 3, 3, 3 };

    // Victory and Defeat
    private bool _won;
    private bool _lose;

    private Animator _animator;

    [SerializeField] private Transform _timerFillBar;
    private float _timeLeft;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // Reset
        _won = false;
        _lose = false;

        // Default
        int diff = GameManager.Instance.TargetDifficulty switch
        {
            TargetDifficulties.EASY => 0,
            TargetDifficulties.MEDIUM => 1,
            _ => 2
        };

        _baseQteCounter = _basesQteCounterDifficulty[diff];
        _numberOfQte = _numberOfQteAtSameTimeDifficulty[diff];
        _secondsBeforeFailure = _secondsBeforeFailDifficulty[diff];

        _totalGlobalCount = 0;
        for (int i = 0; i < _qteKeys.Length; i++)
        {
            bool active = false;
            if (i < _numberOfQte)
            {
                _qteKeys[i].BaseQteCounter = _baseQteCounter;
                _totalGlobalCount += _baseQteCounter;
                active = true;
            }
            _qteKeys[i].gameObject.SetActive(active);
        }

        _animator.SetTrigger("Level1");
        //StartCoroutine(TimeCountdown(_secondsBeforeFailure));
        _timeLeft = _secondsBeforeFailure;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.IsQte())
            return;

        // Count
        bool isOver = true;
        _currentGlobaCount = 0;
        for (int i = 0; i < _numberOfQte; i++)
        {
            _currentGlobaCount += _qteKeys[i].CurrentQteCounter;
            if (_qteKeys[i].CurrentQteCounter <= 0)
            {
                _qteKeys[i].gameObject.SetActive(false);

            }
            else
            {
                isOver = false;
            }


        }
        Debug.Log(_currentGlobaCount);

        // Laugh animation
        if (_currentGlobaCount <= _totalGlobalCount * 40 /100) //40%
        {
            _animator.SetTrigger("Level3");
        }
        else if (_currentGlobaCount <= _totalGlobalCount * 80 / 100) // 80%
        {
            _animator.SetTrigger("Level2");
        }
        else
        {
            _animator.SetTrigger("Level1");
        }

        // Win or Lose
        if (isOver && !_won)
        {
            _won = true;
            GameManager.Instance.SwitchStateToRoaming();
        }

        if (_lose)
        {
            GameManager.Instance.LoseLife();
            GameManager.Instance.SwitchStateToRoaming();
        }

        if (_timeLeft > 0f)
        {
            _timeLeft -= Time.deltaTime;
        }
        if (_timeLeft <= 0f)
        {
            _lose = true;
        }

        float timerNormalized = _timeLeft / _secondsBeforeFailure;
        _timerFillBar.localScale = new Vector3(timerNormalized, 1, 1);
    }

    IEnumerator TimeCountdown(int timeLeft)
    {
        _timeLeft = timeLeft;
        yield return new WaitForSeconds(1);
        if (timeLeft == 0) _lose = true;
        else StartCoroutine(TimeCountdown(timeLeft - 1));
    }
}
