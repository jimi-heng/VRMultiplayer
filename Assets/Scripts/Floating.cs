using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(Rigidbody))]
public class Floating : NetworkBehaviour
{
    public float underWaterDrag = 3f;
    public float underWaterAngularDrag = 1f;
    public float airDrag = 0f;
    public float airAngularDrag = 0.05f;
    public float floatingPower = 9.8f;
    public float maxDepth = 0.5f;

    private float lastRpcTime;
    private float rpcCooldown = 0.1f;

    private bool underwater = false;
    private Rigidbody stone;
    public GameObject water;

    // Network variables for position and rotation synchronization
    private NetworkVariable<Vector3> netPosition = new NetworkVariable<Vector3>(
        default,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    private NetworkVariable<Quaternion> netRotation = new NetworkVariable<Quaternion>(
        default,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    private NetworkVariable<bool> isFloatingActive = new NetworkVariable<bool>(
        default,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    private float syncInterval = 0.1f;
    private float lastSyncTime;

    void Start()
    {
        stone = GetComponent<Rigidbody>();
        stone.interpolation = RigidbodyInterpolation.Interpolate;

        // Initialize network variables
        if (IsServer)
        {
            netPosition.Value = transform.position;
            netRotation.Value = transform.rotation;
            isFloatingActive.Value = false;
        }
    }

    void Update()
    {
        if (!IsOwner && IsClient)
        {
            // Smoothly interpolate towards the network position/rotation
            transform.position = Vector3.Lerp(transform.position, netPosition.Value, Time.deltaTime * 10f);
            transform.rotation = Quaternion.Slerp(transform.rotation, netRotation.Value, Time.deltaTime * 10f);
        }
    }

    void FixedUpdate()
    {
        if (IsServer && Time.time - lastSyncTime > syncInterval)
        {
            lastSyncTime = Time.time;
            netPosition.Value = transform.position;
            netRotation.Value = transform.rotation;
        }
    }

    public void SetFloating()
    {
        if (!IsServer && IsClient)
        {
            // Clients request the server to check floating status
            if (Time.time - lastRpcTime < rpcCooldown) return;
            lastRpcTime = Time.time;
            SetFloatingServerRpc();
        }
        else if (IsServer)
        {
            // Server can directly check
            CheckFloatingStatus();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SetFloatingServerRpc()
    {
        CheckFloatingStatus();
    }

    private void CheckFloatingStatus()
    {
        float difference = transform.position.y - (water.transform.position.y + water.transform.localScale.y / 2f);

        if (difference < 0)
        {
            isFloatingActive.Value = true;
            SetFloatingClientRpc(true, difference);
        }
        else
        {
            isFloatingActive.Value = false;
            SetFloatingClientRpc(false, 0);
        }
    }

    [ClientRpc]
    void SetFloatingClientRpc(bool isUnderwater, float difference)
    {
        if (isUnderwater)
        {
            float buoyancForce = floatingPower * Mathf.Clamp01(Mathf.Abs(difference) / maxDepth);

            stone.AddForceAtPosition(Vector3.up *buoyancForce, transform.position, ForceMode.Force);

            float targetDrag = underWaterDrag * Mathf.Clamp01(buoyancForce / floatingPower);
            stone.linearDamping = Mathf.Lerp(stone.linearDamping, targetDrag, Time.deltaTime * 5f);
            if (!underwater)
            {
                underwater = true;
                SwitchState(true);
                stone.linearVelocity *= 0.5f;
            }

            if (stone.linearVelocity.y > 1f)
            {
                stone.linearVelocity = new Vector3(stone.linearVelocity.x, 1f, stone.linearVelocity.z);
            }
        }
        else if (underwater)
        {
            underwater = false;
            SwitchState(false);
        }
    }

    void SwitchState(bool isUnderwater)
    {
        if (isUnderwater)
        {
            stone.linearDamping = underWaterDrag;
            stone.angularDamping = underWaterAngularDrag;
        }
        else
        {
            stone.linearDamping = airDrag;
            stone.angularDamping = airAngularDrag;
        }
    }
}