using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QteKey : MonoBehaviour
{
    // Actions and Inputs
    private QTEInputActions _qteActions;
    private InputAction[] _qteInputs;
    private InputAction _selectedAction;

    // Sprites and animamtions
    private bool _isTurnedRight;
    [SerializeField]
    private Image _qteImage;
    [SerializeField]
    private Sprite[] _sprites;
    [SerializeField]
    private Image _keySprite;
    [SerializeField]
    private TMP_Text _text;

    // Counter
    private int _baseQteCounter;
    public int BaseQteCounter { get { return _baseQteCounter; } set {  _baseQteCounter = value; } }
    private int _currentQteCounter;
    public int CurrentQteCounter { get { return _currentQteCounter; } }

    private void OnEnable()
    {
        if (_qteActions == null)
        {
            _qteActions = new QTEInputActions();
        }
        _qteActions.QtePossibilities.Enable();

        _qteInputs = new InputAction[8];
        _qteInputs[0] = _qteActions.QtePossibilities.qte_3;
        _qteInputs[1] = _qteActions.QtePossibilities.qte_4;
        _qteInputs[2] = _qteActions.QtePossibilities.qte_6;
        _qteInputs[3] = _qteActions.QtePossibilities.qte_7;
        _qteInputs[4] = _qteActions.QtePossibilities.qte_8;
        _qteInputs[5] = _qteActions.QtePossibilities.qte_10;
        _qteInputs[6] = _qteActions.QtePossibilities.qte_11;
        _qteInputs[7] = _qteActions.QtePossibilities.qte_12;

        SelectNewInput();

    }

    private void OnDisable()
    {
        _qteActions.QtePossibilities.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        _text.text = _currentQteCounter.ToString();

        if (_selectedAction.triggered)
        {
            PressedAnim();
            _currentQteCounter--;
        }
    }

    private void SelectNewInput()
    {
        _currentQteCounter = _baseQteCounter;
        int rand = Random.Range(0, _qteInputs.Length);
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
