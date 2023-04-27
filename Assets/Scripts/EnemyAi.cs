using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform _target;
    [SerializeField] private float _detectionRange;

    private void Update()
    {
        float distanceBtwTarget = Vector3.Distance(transform.position, _target.position);

        if (distanceBtwTarget <= _detectionRange)
            _agent.SetDestination(_target.position);

        //if (distanceBtwTarget <= _agent.stoppingDistance)
            //TransitionManager.instance.ResetLevel();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);
    }
}
