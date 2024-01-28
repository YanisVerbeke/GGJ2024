using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


/// <summary>
/// Qte manager without GameManager
/// </summary>
public class QteManagerDev : MonoBehaviour
{
    private QTEInputActions _qteActions;
    private InputAction[] _qteInputs;
    private InputAction _selectedAction;

    [SerializeField]
    private int _baseQteCounter;

    private int _currentQteCounter;

    private bool _won;
    private bool _lose;

    private bool _isTurnedRight;

    [SerializeField]
    private Image _qteImage;

    [SerializeField]
    private Sprite[] _sprites = new Sprite[12];

    [SerializeField]
    private Image _keySprite;

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
        Debug.Log("Start");
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
        _text.text = _currentQteCounter.ToString();
        if (!_won)
        {
            if (_selectedAction.triggered)
            {
                Debug.Log("oui : " + _currentQteCounter);
                PressedAnim();
                _currentQteCounter--;
            }

            if (_currentQteCounter <= 0 && !_won)
            {
                Debug.Log("assjhfghiuasdhgiasdgfiasfhgasdgpaisdgvpijebpibspbsljhbvbljdshbvsdbksdbvkjsbvkbskjkbsbfksdbfksdbfis");
                _won = true;
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
        _qteImage.sprite = _sprites[rand];
        PressedAnim();
    }

    private void PressedAnim()
    {
        //Rotation
        int angle = _isTurnedRight ? 30 : -30;
        _keySprite.transform.rotation = Quaternion.Euler(0, 0, angle);
        _isTurnedRight = !_isTurnedRight;

        StartCoroutine(SmallBig());

    }

    IEnumerator SmallBig()
    {
        _keySprite.transform.localScale = new Vector3(.8f, .8f, .8f);
        yield return new WaitForSeconds(.1f);
        _keySprite.transform.localScale = Vector3.one;
    }
}
