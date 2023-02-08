using System;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public string ItemName;
    public InventoryItemData referenceItem;
    public bool destroyOnPickup = true;
    public bool autoPickup = true;
    
    private GameObject _player;
    private Transform _playerTransform;
    private bool _attachedToPlayer = false;
    public float rotationSpeed = 0.5f;
    
    public KeyCode dropKey = KeyCode.X;

    private void Awake()
    {
        Debug.Log("ItemObject Awake():");
        _player = GameObject.FindGameObjectWithTag("PlayerBody");
        _playerTransform = _player.transform;
        Debug.Log("Player and transform:");
        Debug.Log(_player);
        Debug.Log(_playerTransform);
    }

    private void Update()
    {
        if (_attachedToPlayer)
        {
            FollowPlayer();
        }
        else
        {
            IdleSpin();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("PlayerBody"))
        {
            
            Debug.Log(ItemName + " touched by player");
            OnHandlePickupItem();
        }
    }

    public void FollowPlayer()
    {
        // this sets the position and rotation of the object to match those of the player (also moves it up a bit)
        // this might be better achieved by setting the object's Parent to the Player instance
        var o = gameObject;
        
        // set object to be in front of player and slightly above the ground
        o.transform.position = (_playerTransform.position + _playerTransform.forward * 1.75f) + Vector3.up;
        
        // set rotation to match player rotation
        o.transform.rotation = _playerTransform.rotation;
        
        // detect drop
        if (Input.GetKey(dropKey))
        {
            OnDropItem();
        }
    }

    public void OnDropItem()
    {
        Debug.Log(ItemName + " detached from player...");
        var o = gameObject;
        _attachedToPlayer = false;
        o.transform.position = (_playerTransform.position + _playerTransform.forward * 1.75f);
        o.transform.rotation = _playerTransform.rotation;
    }

    public void IdleSpin()
    {
        gameObject.transform.Rotate(new Vector3(0, rotationSpeed, 0));
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
            Debug.Log(ItemName + " attached to player...");
            _attachedToPlayer = true;
        }
    }
}
