using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SoundTrigger : MonoBehaviour
{
    [SerializeField] private string groupName;
    [SerializeField] private string soundName;
    [SerializeField] private UnityEvent _eventOnCollision;

    private bool hasPlayed;

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed)
            return;

        if (other.name == "Player")
        {
            if (_eventOnCollision != null)
                _eventOnCollision.Invoke();
            AudioManager.instance.PlayFromGroup(groupName, soundName, 0);
            hasPlayed = true;
        }
    }
}
