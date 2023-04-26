using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    [SerializeField] private Item _item;

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
            Destroy(gameObject);
    }
}
