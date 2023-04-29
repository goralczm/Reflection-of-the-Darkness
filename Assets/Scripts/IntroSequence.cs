using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IntroSequence : MonoBehaviour
{
    [SerializeField] private TextAsset introDialogue;
    [SerializeField] private Transform target;
    [SerializeField] private UnityEvent onEndEvents;

    private void Start()
    {
        DialogueManager.instance.EnterDialogueMode(introDialogue, target, onEndEvents);
    }
}
