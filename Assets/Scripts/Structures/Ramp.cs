using UnityEngine;
// ReSharper disable Unity.InefficientPropertyAccess

public class Ramp : MonoBehaviour
{
    public GameObject floor;

    public void InitializeRamp(Orientation o)
    {
        Debug.Log($"RAMP: Orientation: {o}");
        Debug.Log($"RAMP: Initial position: {transform.position}");

        Vector3 xExtension = new Vector3(0.9f, 0, 0);
        Vector3 zExtension = new Vector3(0, 0, 0.9f);
        
        switch (o)
        {
            case Orientation.Front:
                transform.localScale += zExtension;
                transform.rotation = Quaternion.Euler(315, 0, 0);
                break;
            case Orientation.Back:
                transform.localScale += zExtension;
                transform.rotation = Quaternion.Euler(45, 0, 0);
                break;
            case Orientation.Right:
                transform.localScale += xExtension;
                transform.rotation = Quaternion.Euler(0, 0, 45);
                break;
            case Orientation.Left:
                transform.localScale += xExtension;
                transform.rotation = Quaternion.Euler(0, 0, 315);
                // adjust for slight asymmetry
                transform.position += new Vector3(-0.05f, 0.075f, 0);
                break;
        }
        Debug.Log($"WALL: Final position: {transform.position}");
    }
}