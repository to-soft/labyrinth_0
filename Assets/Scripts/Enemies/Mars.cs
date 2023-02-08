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
        if (other.gameObject.CompareTag("PlayerBody"))
        {
            Debug.Log(itemName + " touched by player");
            GameObject playerBody = other.gameObject;
            GameObject parent = playerBody.transform.parent.gameObject;
            Player playerComponent = _player.gameObject.GetComponent<Player>();
            playerComponent.Damage(5);

        }
    }

}
