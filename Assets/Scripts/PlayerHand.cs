using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private Player _player;

    private bool _isHandThrown;
    public bool IsHandThrown { get { return _isHandThrown; } set { _isHandThrown = value; } }

    private int _cooldown = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.Instance.IsRoaming())
            return;

        //_cooldown = _cooldown <= 0 ? 0 : _cooldown - 1;

        if (collision.GetComponent<Target>() != null && _cooldown <= 0 && _isHandThrown)
        {
            //_cooldown = 10;
            GameManager.Instance.SetCurrentDifficulty(collision.GetComponent<Target>().TargetDifficulty);
            _player.TeleportToTarget(collision.transform);
        }
    }
}
