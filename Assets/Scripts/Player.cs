using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Vector2 _inputValue;

    private float _speed = 8f;

    [SerializeField] private bool _isHandThrown = false;
    [SerializeField] private bool _isHandComing = false;
    private float _handThrowTimer;
    private const float _handThrowTimerValue = 0.3f;
    private Vector2 _handDirection;

    private Rigidbody2D _rigidbody;

    [SerializeField] private GameObject _hand;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        if (!GameManager.Instance.IsPlaying())
            return;


        if (!_isHandThrown && !_isHandComing)
        {
            Vector2 handDirection = Vector2.ClampMagnitude(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position, 1f);
            _hand.transform.position = (Vector2)transform.position + handDirection;
            float handAngle = Mathf.Atan2(handDirection.y, handDirection.x) * Mathf.Rad2Deg;
            _hand.transform.rotation = Quaternion.Euler(0, 0, handAngle);
        }


        if (_handThrowTimer >= 0f)
        {
            _isHandThrown = true;
            _hand.transform.position += (Vector3)(_handDirection * _speed * 4f * Time.deltaTime);
            _handThrowTimer -= Time.deltaTime;
        }
        else
        {
            if (_isHandThrown)
            {
                _isHandComing = true;
                _isHandThrown = false;
            }
        }

        if (_isHandComing)
        {
            _hand.transform.position += (transform.position - _hand.transform.position) * _speed * 2f * Time.deltaTime;
            if (Vector2.Distance(_hand.transform.position, transform.position) <= 2f)
            {
                _isHandComing = false;
            }
        }

        _hand.GetComponent<PlayerHand>().IsHandThrown = _isHandThrown || _isHandComing;
    }


    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsRoaming())
            return;

        _rigidbody.velocity = _inputValue * _speed;
        if (_rigidbody.velocity != Vector2.zero)
        {
            Vector2 movement = _rigidbody.velocity.normalized;
            float angleDirection = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angleDirection);
        }
    }


    private void ThrowHand()
    {
        if (!_isHandThrown && !_isHandComing)
        {
            _handDirection = (_hand.transform.position - transform.position).normalized;
            _handThrowTimer = _handThrowTimerValue;
        }
    }

    public void TeleportToTarget(Transform target)
    {
        if (_isHandThrown)
        {
            transform.position = (Vector2)target.position + Vector2.ClampMagnitude(transform.position - target.position, 2f);
            ResetHandThrow();
        }
    }

    private void ResetHandThrow()
    {
        _handThrowTimer = 0f;
        _isHandThrown = false;
        _isHandComing = false;
    }

    public void OnMove(InputValue value)
    {
        _inputValue = value.Get<Vector2>();
    }

    public void OnFire(InputValue value)
    {
        if (!GameManager.Instance.IsPlaying())
            return;

        ThrowHand();
    }
}
