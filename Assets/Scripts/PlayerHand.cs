using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private Player _player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.Instance.IsPlaying())
            return;

        if (collision.GetComponent<Target>() != null)
        {
            _player.TeleportToTarget(collision.transform);
        }
    }
}
