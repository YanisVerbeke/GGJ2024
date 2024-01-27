using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    private Vector2 _target;

    private NavMeshAgent _agent;

    private float _newTargetTimer;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        _target = new Vector2(Random.Range(-23f, 23f), Random.Range(-13f, 13f));
    }

    private void Update()
    {
        if (!GameManager.Instance.IsRoaming())
            return;

        _agent.SetDestination(_target);

        Vector2 movement = _agent.velocity.normalized;
        float angleDirection = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angleDirection);

        _newTargetTimer -= Time.deltaTime;

        if (_newTargetTimer <= 0)
        {
            SetNewTarget();
        }
    }


    private void SetNewTarget()
    {
        _target = new Vector2(Random.Range(-20f, 20f), Random.Range(-10f, 10f));
        _newTargetTimer = Random.Range(2f, 20f);
    }
}
