using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

public class NetworkedBalance : NetworkBehaviour
{
    public TrayDetector leftTray;
    public TrayDetector rightTray;
    public Transform beam;

    public float maxAngle = 30f;
    public float balanceSensitivity = 5f;
    public float lerpSpeed = 2f;

    private NetworkVariable<float> syncedAngle = new NetworkVariable<float>(
        0f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    private float currentAngle = 0f;

    // 服务端管理的托盘物体
    private readonly List<Rigidbody> leftObjects = new List<Rigidbody>();
    private readonly List<Rigidbody> rightObjects = new List<Rigidbody>();

    void Update()
    {
        if (IsServer)
        {
            float leftMass = GetTotalMass(leftObjects);
            float rightMass = GetTotalMass(rightObjects);

            float targetAngle = Mathf.Clamp((rightMass - leftMass) * balanceSensitivity, -maxAngle, maxAngle);
            syncedAngle.Value = targetAngle;
        }

        currentAngle = Mathf.Lerp(currentAngle, syncedAngle.Value, Time.deltaTime * lerpSpeed);
        beam.localRotation = Quaternion.Euler(0, -currentAngle, 0);
    }

    private float GetTotalMass(List<Rigidbody> rbs)
    {
        float total = 0f;
        foreach (var rb in rbs)
        {
            if (rb != null)
                total += rb.mass;
        }
        return total;
    }

    // 玩家松手时调用
    public void OnReleased(SelectExitEventArgs args)
    {
        var netObj = args.interactableObject.transform.GetComponent<NetworkObject>();
        if (netObj != null)
        {
            AddToTrayServerRpc(netObj.NetworkObjectId);
        }
    }

    // 玩家抓起时调用
    public void OnGrabbed(SelectEnterEventArgs args)
    {
        var netObj = args.interactableObject.transform.GetComponent<NetworkObject>();
        if (netObj != null)
        {
            RemoveFromTrayServerRpc(netObj.NetworkObjectId);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddToTrayServerRpc(ulong netId)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(netId, out var netObj))
        {
            Rigidbody rb = netObj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                if (leftTray.IsInsideTray(rb.transform.position))
                {
                    if (!leftObjects.Contains(rb)) leftObjects.Add(rb);
                    rightObjects.Remove(rb);
                }
                else if (rightTray.IsInsideTray(rb.transform.position))
                {
                    if (!rightObjects.Contains(rb)) rightObjects.Add(rb);
                    leftObjects.Remove(rb);
                }
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void RemoveFromTrayServerRpc(ulong netId)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(netId, out var netObj))
        {
            Rigidbody rb = netObj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                leftObjects.Remove(rb);
                rightObjects.Remove(rb);
            }
        }
    }
}
