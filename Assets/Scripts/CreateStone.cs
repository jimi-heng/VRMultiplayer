using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using XRMultiplayer;
using Unity.Netcode;
using UnityEngine.UI;

public class CreateStone : NetworkBehaviour
{
    public List<GameObject> StoneList;
    public Transform Position1;

    public Scrollbar Scrollbar;

    public void CreateStone1()
    {
        if (Scrollbar.value > 0.5)
        {
            CreateStone1ServerRpc();
        }
    }

    [ServerRpc(RequireOwnership =false)]
    void CreateStone1ServerRpc()
    {
        CreateStone1ClientRpc();
    }

    [ClientRpc]
    void CreateStone1ClientRpc()
    {
        Instantiate(StoneList[0], Position1.position, Quaternion.identity);
    }

}
