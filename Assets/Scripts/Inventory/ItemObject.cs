using System;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public InventoryItemData referenceItem;
    public bool destroyOnPickup = true;
    private GameObject _player;
    private Transform _playerTransform;
    private bool attachedToPlayer = false;
    
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerTransform = _player.transform;
        Debug.Log("Player and transform:");
        Debug.Log(_player);
        Debug.Log(_playerTransform);
    }

    private void Update()
    {
        if (attachedToPlayer)
        {
            FollowPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Coin touched by player");
            OnHandlePickupItem();
        }
    }

    public void FollowPlayer()
    {
        var o = gameObject;
        
        // set object to be in front of player and slightly above the ground
        o.transform.position = (_playerTransform.position + _playerTransform.forward * 1.75f) + Vector3.up;
        
        // set rotation to match player rotation
        o.transform.rotation = _playerTransform.rotation;
    }

    public void OnHandlePickupItem()
    {
        Debug.Log("OnHandlePickupItem called");

        InventorySystem.instance.Add(referenceItem);
        if (destroyOnPickup)
        {
            Destroy(gameObject);
        }
        else
        {
            attachedToPlayer = true;
        }
    }
}
