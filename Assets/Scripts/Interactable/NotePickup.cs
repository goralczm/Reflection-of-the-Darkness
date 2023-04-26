using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotePickup : Interactable
{
    [TextArea(10, 10)] [SerializeField] private string noteDescription;

    public override void Interact()
    {
        base.Interact();
        GameManager.instance.DisplayNote(noteDescription);
        Destroy(gameObject);
    }
}
