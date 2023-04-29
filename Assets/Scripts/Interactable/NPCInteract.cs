using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCInteract : Interactable
{
    [SerializeField] private NPC _npc;
    [SerializeField] private UnityEvent _onEndDialogueEvents;

    private DialogueManager _dialogueManager;

    private void Start()
    {
        _dialogueManager = DialogueManager.instance;
    }

    public override void Interact()
    {
        base.Interact();
        if (!_dialogueManager.dialogueIsPlaying)
        {
            Debug.Log("Talking with " + _npc.name);
            _dialogueManager.EnterDialogueMode(_npc.dialogue, transform, _onEndDialogueEvents);
        }
    }
}
