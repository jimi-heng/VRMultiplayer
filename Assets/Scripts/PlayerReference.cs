using UnityEngine;
using Unity.Netcode;

public class PlayerReference : NetworkBehaviour
{
    public static PlayerReference LocalInstance;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            LocalInstance = this;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsOwner && LocalInstance == this)
        {
            LocalInstance = null;
        }
    }
}
