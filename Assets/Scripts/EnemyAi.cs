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
        {
            _agent.isStopped = false;
            _agent.SetDestination(_target.position);
            AudioManager.instance.StopFromGroup("Music", "theme");
            AudioManager.instance.PlayOnceFromGroup("Music", "chase");
        }
        else
        {
            _agent.isStopped = true;
            AudioManager.instance.StopFromGroup("Music", "chase");
            AudioManager.instance.PlayOnceFromGroup("Music", "theme");
        }

        if (distanceBtwTarget <= _agent.stoppingDistance)
        {
            EndScreen.instance.endScreenPanel.Show();
            Destroy(this);
        }
        //TransitionManager.instance.ResetLevel();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);
    }
}
