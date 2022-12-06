using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ItemSlot : MonoBehaviour
{
    [SerializeField]
    private Image icon;
    
    [SerializeField]
    private TextMeshProUGUI label;
    
    [SerializeField]
    private GameObject stackObject;
    
    [SerializeField]
    private TextMeshProUGUI stackLabel;

    public void Set(InventoryItem item)
    {
        icon.sprite = item.Data.icon;
        label.text = item.Data.displayName;
        if (item.StackSize <= 1)
        {
            stackObject.SetActive(false);
            return;
        }

        stackLabel.text = item.StackSize.ToString();
    }
    
}