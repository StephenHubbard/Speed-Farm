using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private int _waypointIndex = 0;

    private INPC _npc;
    private Animator _animator;
    private Rigidbody2D _rb;

    private Vector2 _moveDir;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _npc = GetComponent<INPC>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _moveDir = _waypoints[_waypointIndex].transform.position;
        AnimationMovement();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, _moveDir, _moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, _waypoints[_waypointIndex].transform.position) < 0.01f)
        {
            _waypointIndex += 1;

            if (_waypointIndex == _waypoints.Length)
            {
                _waypointIndex = 0;
                (_npc as INPC).Collect();
            }

            _moveDir = _waypoints[_waypointIndex].transform.position;
            
            AnimationMovement();
        }

       

    }

    private void AnimationMovement()
    {
        Vector2 direction = _moveDir - (Vector2)transform.position;

        if (direction.x > 0 && direction.x > 1)
        {
            _animator.SetTrigger("walking_right");
        }
        else if (direction.x < 0 && direction.x < -1)
        {
            _animator.SetTrigger("walking_left");
        }

        if (direction.y > 0 && direction.y > 1)
        {
            _animator.SetTrigger("walking_up");
        }
        else if (direction.y < 0 && direction.y < -1)
        {
            _animator.SetTrigger("walking_down");
        }
 
    }
}