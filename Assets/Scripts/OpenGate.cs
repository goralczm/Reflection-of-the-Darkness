using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour
{
    [SerializeField] private Vector3 _newPos;
    [SerializeField] private Vector3 _newRot;

    public void Open()
    {
        transform.rotation = Quaternion.Euler(_newRot);
        transform.position = _newPos;
    }
}
