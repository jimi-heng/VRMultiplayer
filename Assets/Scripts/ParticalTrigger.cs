using Unity.Netcode;
using UnityEngine;

public class ParticalTrigger : NetworkBehaviour
{
    public ParticleSystem Particle1;
    public ParticleSystem Particle2;
    public ParticleSystem Particle3;
    public ParticleSystem Particle4;

    public void playParticle()
    {
        playParticleServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void playParticleServerRpc()
    {
        playParticleClientRpc();
    }

    [ClientRpc]
    public void playParticleClientRpc()
    {
        Particle1.Play();
        Particle2.Play();
        Particle3.Play();
        Particle4.Play();
    }
}
