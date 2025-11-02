using UnityEngine;
using Unity.Netcode;

public class NetworkTagColorChanger : NetworkBehaviour
{
    public Renderer renderer1;
    public Renderer renderer2;
    public Renderer renderer3;

    public Color hoverColor = Color.red;
    public Color defaultColor = Color.white;

    public void changeColor1()
    {
        changeColor1ServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void changeColor1ServerRpc()
    {
        changeColor1ClientRpc();
    }

    [ClientRpc]
    public void changeColor1ClientRpc()
    {
        renderer1.material.color = hoverColor;
        renderer2.material.color = defaultColor;
        renderer3.material.color = defaultColor;
    }

    public void changeColor2()
    {
        changeColor2ServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void changeColor2ServerRpc()
    {
        changeColor2ClientRpc();
    }

    [ClientRpc]
    public void changeColor2ClientRpc()
    {
        renderer1.material.color = defaultColor;
        renderer2.material.color = hoverColor;
        renderer3.material.color = defaultColor;
    }

    public void changeColor3()
    {
        changeColor3ServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void changeColor3ServerRpc()
    {
        changeColor3ClientRpc();
    }

    [ClientRpc]
    public void changeColor3ClientRpc()
    {
        renderer1.material.color = defaultColor;
        renderer2.material.color = defaultColor;
        renderer3.material.color = hoverColor;
    }
}
