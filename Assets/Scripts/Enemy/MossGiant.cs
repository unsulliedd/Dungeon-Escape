using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MossGiant : Enemy
{
    [SerializeField] private Transform _waypointA, _waypointB;
    private bool _idleInitialState;
    private Vector3 _currentTarget;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public override void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Moss_Giant_Idle"))
        {
            _idleInitialState = true;
            return;
        }
        Flip();
        Movement();
    }

    private void Movement()
    {
        if (transform.position == _waypointA.position)
        {
            _currentTarget = _waypointB.position;
            if (!_idleInitialState)
            {            
                _animator.SetTrigger("Idle");
                _idleInitialState = false;
            }
        }
        else if (transform.position == _waypointB.position)
        {
            _currentTarget = _waypointA.position;
            _animator.SetTrigger("Idle");
        }
        transform.position = Vector3.MoveTowards(transform.position, _currentTarget, speed * Time.deltaTime);
    }

    private void Flip()
    {
        if (_currentTarget == _waypointA.position)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

}
