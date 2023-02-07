using UnityEngine;

public class Mars : MonoBehaviour
{

    public string itemName;
    public MarsData referenceItem;
    private GameObject _player;
    private Transform _playerTransform;


    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerTransform = _player.transform;
    }

    private void Update()
    {

    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(itemName + " touched by player");
        }
    }

}
