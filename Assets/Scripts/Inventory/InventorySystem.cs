using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;
    private Dictionary<InventoryItemData, InventoryItem> _inventoryDictionary;
    public List<InventoryItem> Inventory { get; private set; }

    public event Action OnInventoryChangedEvent;

    public void InventoryChangedEvent()
    {
        OnInventoryChangedEvent?.Invoke();
    }

    private void Awake()
    {
        instance = this;
        Inventory = new List<InventoryItem>();
        _inventoryDictionary = new Dictionary<InventoryItemData, InventoryItem>();
    }

    public InventoryItem Get(InventoryItemData referenceData)
    {
        if (_inventoryDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            return value;
        }

        return null;
    }

    public void Add(InventoryItemData referenceData)
    {
        if (_inventoryDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            value.AddToStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(referenceData);
            Inventory.Add(newItem);
            _inventoryDictionary.Add(referenceData, newItem);
        }
        InventoryChangedEvent();
    }

    public void Remove(InventoryItemData referenceData)
    {
        if (_inventoryDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            value.RemoveFromStack();

            if (value.StackSize == 0)
            {
                Inventory.Remove(value);
                _inventoryDictionary.Remove(referenceData);
            }
            InventoryChangedEvent();
        }
    }
}
