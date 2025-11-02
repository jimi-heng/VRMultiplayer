using UnityEngine;

public class TrayDetector : MonoBehaviour
{
    public bool IsInsideTray(Vector3 pos)
    {
        return GetComponent<Collider>().bounds.Contains(pos);
    }
}
