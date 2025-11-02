using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class WaveButtonTrigger : NetworkBehaviour
{
    public Renderer earthquakePlane;
    public Renderer earthquakePlane2;

    public float power1 = 0;
    public float power2 = 0;
    public float power3 = 0;

    public void StartWave1()
    {
        Start1ServerRpc();
    }

    [ServerRpc(RequireOwnership=false)]
    void Start1ServerRpc()
    {
        Start1ClientRpc();
    }

    [ClientRpc]
    void Start1ClientRpc()
    {
        float startTime = Time.time;
        earthquakePlane.material.SetFloat("_Trigger", startTime);
        earthquakePlane2.material.SetFloat("_Trigger", startTime);

        earthquakePlane.material.SetFloat("_power", power1);
        earthquakePlane2.material.SetFloat("_power", power1);
    }

    public void StartWave2()
    {
        Start2ServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    void Start2ServerRpc()
    {
        Start2ClientRpc();
    }

    [ClientRpc]
    void Start2ClientRpc()
    {
        float startTime = Time.time;
        earthquakePlane.material.SetFloat("_Trigger", startTime);
        earthquakePlane2.material.SetFloat("_Trigger", startTime);

        earthquakePlane.material.SetFloat("_power", power2);
        earthquakePlane2.material.SetFloat("_power", power2);
    }

    public void StartWave3()
    {
        Start3ServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    void Start3ServerRpc()
    {
        Start3ClientRpc();
    }

    [ClientRpc]
    void Start3ClientRpc()
    {
        float startTime = Time.time;
        earthquakePlane.material.SetFloat("_Trigger", startTime);
        earthquakePlane2.material.SetFloat("_Trigger", startTime);

        earthquakePlane.material.SetFloat("_power", power3);
        earthquakePlane2.material.SetFloat("_power", power3);
    }

    public void StopWave()
    {
        StopServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    void StopServerRpc()
    {
        StopClientRpc();
    }

    [ClientRpc]
    void StopClientRpc()
    {
        earthquakePlane.material.SetFloat("_power", 0);
        earthquakePlane2.material.SetFloat("_power", 0);
        earthquakePlane.material.SetFloat("_Trigger", 0);
        earthquakePlane2.material.SetFloat("_Trigger", 0);
    }
}
