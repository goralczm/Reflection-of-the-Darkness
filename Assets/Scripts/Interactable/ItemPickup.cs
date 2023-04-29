using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemPickup : Interactable
{
    [SerializeField] private Item _item;
    [SerializeField] private UnityEvent _onPickupEvents;

    private Inventory _inventory;
    private SpriteRenderer _rend;

    private void Start()
    {
        _inventory = Inventory.instance;
        _rend = GetComponent<SpriteRenderer>();
        _rend.sprite = _item.icon;
    }

    public override void Interact()
    {
        base.Interact();
        if (_inventory.AddItem(_item))
        {
            if (_onPickupEvents != null)
                _onPickupEvents.Invoke();
            Destroy(gameObject);
        }
    }
}
