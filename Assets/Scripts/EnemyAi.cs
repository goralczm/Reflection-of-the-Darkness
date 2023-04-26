using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform _target;

    private void Update()
    {
        _agent.SetDestination(_target.position);
        float distanceBtwTarget = Vector3.Distance(transform.position, _target.position);

        //if (distanceBtwTarget <= _agent.stoppingDistance)
            //TransitionManager.instance.ResetLevel();
    }
}
