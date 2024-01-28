using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private float _secondsBeforeFailure;
    private float[] _secondsBeforeFailDifficulty = new float[_numberOfDifficulties] { 1.5f, 2.1f, 1.8f };

    // Victory and Defeat
    private bool _won;
    private bool _lose;

    private Animator _animator;

    [SerializeField] private Transform _timerFillBar;
    private float _timeLeft;

    private bool _isChecked = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // Reset
        _won = false;
        _lose = false;
        _isChecked = false;

        // Default
        int diff = GameManager.Instance.TargetDifficulty switch
        {
            TargetDifficulties.EASY => 0,
            TargetDifficulties.MEDIUM => 1,
            _ => 2
        };
        Debug.Log("Diificulty : " + diff);


        _baseQteCounter = _basesQteCounterDifficulty[diff];
        _numberOfQte = _numberOfQteAtSameTimeDifficulty[diff];
        _secondsBeforeFailure = _secondsBeforeFailDifficulty[diff];

        _totalGlobalCount = 0;
        for (int i = 0; i < _qteKeys.Length; i++)
        {
            if (i < _numberOfQte)
            {
                _qteKeys[i].BaseQteCounter = _baseQteCounter;
                _totalGlobalCount += _baseQteCounter;
                _qteKeys[i].gameObject.SetActive(true);
            }
            else
            {
                _qteKeys[i].gameObject.SetActive(false);
            }
            
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
        if (!_isChecked) CheckIfSimilarKeys();

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
            SoundManager.Instance.PlayQTEWin();
            _won = true;
            GameManager.Instance.SwitchStateToRoaming();
        }

        if (_lose)
        {
            SoundManager.Instance.PlayQTELose();
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

    private void CheckIfSimilarKeys()
    {

        for (int i = 0; i < _qteKeys.Length; i++)
        {
            if (i > 0)
            {
                if (_qteKeys[i].SelectedRandom == _qteKeys[i - 1].SelectedRandom)
                {
                    _qteKeys[i].SetNewInputAction();
                }
            }
        }
        _isChecked = true;
    }


    IEnumerator TimeCountdown(int timeLeft)
    {
        _timeLeft = timeLeft;
        yield return new WaitForSeconds(1);
        if (timeLeft == 0) _lose = true;
        else StartCoroutine(TimeCountdown(timeLeft - 1));
    }
}
