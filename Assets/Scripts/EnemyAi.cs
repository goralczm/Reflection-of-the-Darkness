using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform _target;
    [SerializeField] private float _detectionRange;

    private bool _isChasing;

    private void Update()
    {
        float distanceBtwTarget = Vector3.Distance(transform.position, _target.position);

        if (distanceBtwTarget <= _detectionRange && !_isChasing)
            StartChase();

        if (distanceBtwTarget <= _agent.stoppingDistance + .1f)
        {
            EndScreen.instance.endScreenPanel.Show();
            Destroy(this);
        }

        if (_isChasing)
            _agent.SetDestination(_target.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);
    }

    public void StartChase()
    {
        _isChasing = true;
        _agent.isStopped = false;
        AudioManager.instance.StopFromGroup("Music", "theme");
        AudioManager.instance.PlayOnceFromGroup("Music", "chase");
    }

    public void StopChase()
    {
        _isChasing = false;
        _agent.isStopped = true;
        AudioManager.instance.StopFromGroup("Music", "chase");
        AudioManager.instance.PlayOnceFromGroup("Music", "theme");
    }
}
