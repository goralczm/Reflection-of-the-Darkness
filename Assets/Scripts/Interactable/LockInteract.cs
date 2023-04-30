using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockInteract : Interactable
{
    [SerializeField] private UITweener _lockPanel;

    public override void Interact()
    {
        base.Interact();
        _lockPanel.Show();
    }
}
