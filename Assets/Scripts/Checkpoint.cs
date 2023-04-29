using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    public UnityEvent onLoadEvents;
    public Item[] savedItems;
    public Vector3 targetRot;

    [SerializeField] private CheckpointManager checkpointManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            checkpointManager.SaveCheckpoint(this);
        }
    }
}
