using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class ShowCurtain : NetworkBehaviour
{
    public Transform target;
    public float moveDistance = 1f;
    public float moveDuration = 1f;

    private NetworkVariable<bool> hasMoved = new NetworkVariable<bool>(false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);
    public void moveCurtain()
    {
        moveServerRpc();
    }

    [ServerRpc(RequireOwnership =false)]
    void moveServerRpc()
    {
        moveClientRpc();
    }

    [ClientRpc]
    void moveClientRpc()
    {
        StartCoroutine(MoveUpCurtain());
    }

    private IEnumerator MoveUpCurtain()
    {
        if (hasMoved.Value == false)
        {
            Vector3 startPos = target.position;
            Vector3 endPos = startPos + Vector3.up * moveDistance;
            float elapsedTime = 0f;

            while (elapsedTime < moveDuration)
            {
                target.position = Vector3.Lerp(startPos, endPos, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            target.position = endPos;

            hasMoved.Value = true;
        }
    }
}
