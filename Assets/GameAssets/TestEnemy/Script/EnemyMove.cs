using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField]private NavMeshAgent _agent;

    private void Start()
    {
        _agent.GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    
    private void Update()
    {
        _agent.SetDestination(player.position);
    }
}
