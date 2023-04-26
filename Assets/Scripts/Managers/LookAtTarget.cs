using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private Vector3 _startRot;

    private void Start()
    {
        _startRot = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    private void Update()
    {
        transform.LookAt(_target);
        transform.rotation = Quaternion.Euler(_startRot.x, transform.eulerAngles.y, _startRot.z);
    }
}
