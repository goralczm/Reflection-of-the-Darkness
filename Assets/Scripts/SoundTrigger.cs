using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [SerializeField] private string groupName;
    [SerializeField] private string soundName;

    private bool hasPlayed;

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed)
            return;

        if (other.name == "Player")
        {
            AudioManager.instance.PlayFromGroup(groupName, soundName, 0);
            hasPlayed = true;
        }
    }
}
