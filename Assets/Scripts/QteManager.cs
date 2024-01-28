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

    [SerializeField]
    private int _numberOfQte;
    private int[] _numberOfQteAtSameTimeDifficulty = new int[_numberOfDifficulties] { 1, 1, 2 };

    [SerializeField]
    private int _secondsBeforeFailure;
    private int[] _secondsBeforeFailDifficulty = new int[_numberOfDifficulties] { 3, 3, 3 };

    // Victory and Defeat
    private bool _won;
    private bool _lose;

    private void OnEnable()
    {
        // Reset
        _won = false;
        _lose = false;

        // Default
        int diff = GameManager.Instance.TargetDifficulty switch
        {
            TargetDifficulties.EASY => 2,
            TargetDifficulties.MEDIUM => 2,
            _ => 2
        };

        Debug.Log("diff : " + diff);

        _baseQteCounter = _basesQteCounterDifficulty[diff];
        _numberOfQte = _numberOfQteAtSameTimeDifficulty[diff];
        _secondsBeforeFailure = _secondsBeforeFailDifficulty[diff];

        for (int i = 0; i < _qteKeys.Length; i++)
        {
            bool active = false;
            if(i < _numberOfQte)
            {
                _qteKeys[i].BaseQteCounter = _baseQteCounter;
                active = true;
            }
            Debug.Log("qte keys " + i + " active : " + active);
            _qteKeys[i].gameObject.SetActive(active);
        }

        StartCoroutine(TimeCountdown(_secondsBeforeFailure));
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.IsQte())
            return;

        bool isOver = true;
        for (int i = 0; i < _numberOfQte; i++)
        {
            if(_qteKeys[i].CurrentQteCounter <= 0)
            {
                _qteKeys[i].gameObject.SetActive(false);

            } else
            {
                isOver = false;
            }
        }

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
        
    }

    IEnumerator TimeCountdown(int timeLeft)
    {
       
        yield return new WaitForSeconds(1);
        if (timeLeft == 0) _lose = true;
        else StartCoroutine(TimeCountdown(timeLeft - 1));
    }
}
