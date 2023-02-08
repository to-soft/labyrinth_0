using UnityEngine;

[CreateAssetMenu(menuName = "MarsData")]
public class MarsData : ScriptableObject
{
    public string id;
    public string displayName;
    public GameObject prefab;
}