using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [HideInInspector] public Item item;
    [HideInInspector] public bool isSelected;

    public KeyCode shortcutKey;

    [SerializeField] private Vector2 _activeOffset;

    [Header("Instances")]
    [SerializeField] private Image _background;
    [SerializeField] private Image _icon;
    [SerializeField] private Sprite _slotIdle;
    [SerializeField] private Sprite _slotActive;

    private RectTransform _itemIconRect;
    private Vector2 _startOffset;

    private void Start()
    {
        _itemIconRect = _icon.GetComponent<RectTransform>();
        _startOffset = _itemIconRect.anchoredPosition;
    }

    public void Setup(Item newItem)
    {
        item = newItem;
        _icon.enabled = true;
        _icon.sprite = item.icon;
    }

    public void Disable()
    {
        item = null;
        _icon.enabled = false;
    }

    public void SelectSlot()
    {
        if (item == null || isSelected)
        {
            DeselectSlot();
            return;
        }

        isSelected = true;
        _background.sprite = _slotActive;
        _itemIconRect.anchoredPosition = _startOffset + _activeOffset;
        if (item.description != "")
        {
            GameManager.instance.DisplayNote(item.description, item.mirror);
            DeselectSlot();
        }
    }

    public void DeselectSlot()
    {
        isSelected = false;
        _background.sprite = _slotIdle;
        _itemIconRect.anchoredPosition = _startOffset;
    }
}
