using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private Player _player;

    private bool _isHandThrown;
    public bool IsHandThrown { get { return _isHandThrown; } set { _isHandThrown = value; } }

    private float _cooldown;

    private void Update()
    {
        if (_cooldown >= 0f)
        {
            _cooldown -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.Instance.IsRoaming())
            return;

        if (collision.GetComponent<Target>() != null && _cooldown <= 0 && _isHandThrown)
        {
            _cooldown = 1f;
            _player.TeleportToTarget(collision.transform);
            //collision.GetComponent<Target>().
        }
    }
}
