using UnityEngine;

public class RoomTransport : MonoBehaviour
{
    public Transform head;
    public Transform origin;
    public Transform room1;
    public Transform room2;
    public Transform room3;
    public Transform room4;

    public void Transform1()
    {
        Vector3 offset = head.position - origin.position;
        origin.position = room1.position - offset;
    }

    public void Transform2()
    {
        Vector3 offset = head.position - origin.position;
        origin.position = room2.position - offset;
    }

    public void Transform3()
    {
        Vector3 offset = head.position - origin.position;
        origin.position = room3.position - offset;
    }

    public void Transform4()
    {
        Vector3 offset = head.position - origin.position;
        origin.position = room4.position - offset;
    }
}
