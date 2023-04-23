using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private int _waypointIndex = 0;
    
    private INPC _npc;

    private void Awake() {
        _npc = GetComponent<INPC>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, _waypoints[_waypointIndex].transform.position, _moveSpeed * Time.deltaTime);


        if (Vector2.Distance(transform.position, _waypoints[_waypointIndex].transform.position) < 0.1f)
        {
            _waypointIndex += 1;
        }

        if (_waypointIndex == _waypoints.Length) {
            _waypointIndex = 0;
            (_npc as INPC).Collect();
        }
    }
}