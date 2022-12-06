using System;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public InventoryItemData referenceItem;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Coin touched by player");
            OnHandlePickupItem();
        }
    }
    

    public void OnHandlePickupItem()
    {
        Debug.Log("OnHandlePickupItem called");
        InventorySystem.instance.Add(referenceItem);
        Destroy(gameObject);
    }
}
