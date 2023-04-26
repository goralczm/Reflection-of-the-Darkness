using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton

    public static Inventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found another instance of Inventory!");
            Destroy(instance);
        }
        instance = this;
    }

    #endregion

    [SerializeField] private List<Item> _items;
    [SerializeField] private int _maxCapacity;

    [SerializeField] private InventorySlot[] _invSlots;
    [SerializeField] private Recipe[] _recipes;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        foreach (InventorySlot slot in _invSlots)
        {
            if (Input.GetKeyDown(slot.shortcutKey))
            {
                slot.SelectSlot();
                CheckRecipes();
            }
        }
    }

    public bool AddItem(Item newItem)
    {
        if (_items.Count == _maxCapacity)
        {
            DialogueManager.instance.ShowInfo("No more space in inventory...", 4f);
            return false;
        }

        _items.Add(newItem);
        UpdateUI();

        return true;
    }

    public void RemoveItem(Item oldItem)
    {
        _items.Remove(oldItem);
        UpdateUI();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < _invSlots.Length; i++)
        {
            if (i > _items.Count - 1)
            {
                _invSlots[i].Disable();
                continue;
            }

            _invSlots[i].Setup(_items[i]);
        }
    }

    private void CheckRecipes()
    {
        foreach (Recipe recipe in _recipes)
        {
            bool foundComplication = false;
            int validItems = 0;
            foreach (InventorySlot slot in _invSlots)
            {
                if (!slot.isSelected)
                    continue;

                if (foundComplication)
                    break;

                if (!recipe.inputs.Contains(slot.item))
                {
                    foundComplication = true;
                    break;
                }

                validItems++;
            }

            if (foundComplication || validItems != recipe.inputs.Count)
                continue;

            for (int i = 0; i < recipe.inputs.Count; i++)
            {
                RemoveItem(recipe.inputs[i]);
            }

            for (int i = 0; i < _invSlots.Length; i++)
            {
                _invSlots[i].DeselectSlot();
            }

            AddItem(recipe.output);
            DialogueManager.instance.ShowInfo("Created " + recipe.output.name, 4f);
        }
    }
}
