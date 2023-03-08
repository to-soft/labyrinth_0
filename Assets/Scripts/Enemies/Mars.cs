using UnityEngine;

public class Mars : MonoBehaviour
{

    public string itemName;
    public MarsData referenceItem;
    private GameObject _player;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Player"))
        {
            // TODO this prolly doesn't work anymore
            Debug.Log(itemName + " touched by player");
            Player playerComponent = other.gameObject.GetComponent<Player>();
            playerComponent.Damage(5);
        }
    }

}
