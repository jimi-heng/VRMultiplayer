using UnityEngine;

public class KeepTrayVertical : MonoBehaviour
{
    public Transform targetPoint;
    void LateUpdate()
    {
        transform.position = targetPoint.position;

        transform.up = Vector3.forward;
    }
}
