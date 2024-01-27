using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QteManager : MonoBehaviour
{
    private QTEInputActions _qteActions;
    private InputAction[] _qteInputs;
    private InputAction _selectedAction;

    [SerializeField]
    private int _baseQteCounter;

    private int _currentQteCounter;

    private bool _won;
    private bool _lose;

    [SerializeField]
    private SpriteRenderer _qteSpriteRenderer;

    [SerializeField]
    private Sprite[] _sprites = new Sprite[12];

    [SerializeField]
    private TMP_Text _text;

    private void OnEnable()
    {
        if (_qteActions == null)
        {
            _qteActions = new QTEInputActions();
        }
        _qteActions.QtePossibilities.Enable();

        _qteInputs = new InputAction[12];
        _qteInputs[0] = _qteActions.QtePossibilities.qte_1;
        _qteInputs[1] = _qteActions.QtePossibilities.qte_2;
        _qteInputs[2] = _qteActions.QtePossibilities.qte_3;
        _qteInputs[3] = _qteActions.QtePossibilities.qte_4;
        _qteInputs[4] = _qteActions.QtePossibilities.qte_5;
        _qteInputs[5] = _qteActions.QtePossibilities.qte_6;
        _qteInputs[6] = _qteActions.QtePossibilities.qte_7;
        _qteInputs[7] = _qteActions.QtePossibilities.qte_8;
        _qteInputs[8] = _qteActions.QtePossibilities.qte_9;
        _qteInputs[9] = _qteActions.QtePossibilities.qte_10;
        _qteInputs[10] = _qteActions.QtePossibilities.qte_11;
        _qteInputs[11] = _qteActions.QtePossibilities.qte_12;

        SelectNewInput();
    }

    private void OnDisable()
    {
        _qteActions.QtePossibilities.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.IsQte())
            return;

        _text.text = _currentQteCounter.ToString();
        if (!_won)
        {
            if (_selectedAction.triggered)
            {
                Debug.Log("action pressed");
                _currentQteCounter--;
            }

            if (_currentQteCounter <= 0 && !_won)
            {
                _won = true;
                GameManager.Instance.SwitchStateToRoaming();
                Debug.Log("Yay you won");
            }
        }

        if(_qteActions.QtePossibilities.Randmize.triggered)
        {
            SelectNewInput();
        }
    }

    private void SelectNewInput()
    {
        _won = false;
        _currentQteCounter = _baseQteCounter;
        int rand = Random.Range(0, 11);
        _selectedAction = _qteInputs[rand];
        _qteSpriteRenderer.sprite = _sprites[rand];
    }
}
