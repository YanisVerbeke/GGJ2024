using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesHandler : MonoBehaviour
{
    [SerializeField]
    private Sprite[] _livesSprites;

    [SerializeField]
    private Image _uILife;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(GameManager.Instance.GetCurrentLives())
        {
            case 3:
                _uILife.sprite = _livesSprites[2];
                break;
            case 2:
                _uILife.sprite = _livesSprites[1];
                break;
            case 1:
                _uILife.sprite = _livesSprites[0];
                break;
            default:
                GameManager.Instance.GameOver();
                break;

        }
    }
}
