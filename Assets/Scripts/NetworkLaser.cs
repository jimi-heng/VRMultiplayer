using UnityEngine;
using Unity.Netcode;

public class NetworkLaser : NetworkBehaviour
{
    private LineRenderer lineRenderer;

    private NetworkVariable<bool> isLaserActive = new NetworkVariable<bool>(
        false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        lineRenderer.enabled = isLaserActive.Value;

        if (isLaserActive.Value)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + transform.forward * 5f);
        }
    }

    public void SetLaserActive(bool active)
    {
        if (IsOwner)  
        {
            isLaserActive.Value = active;
        }
    }
}
