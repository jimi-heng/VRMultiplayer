using Unity.Netcode;
using UnityEngine;
using UnityEngine.Video;

public class PlayVideo : NetworkBehaviour
{
    public VideoPlayer videoPlayer;

    public void OnPlayButtonClicked()
    {
            PlayVideoServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    void PlayVideoServerRpc()
    {
        PlayVideoClientRpc(videoPlayer.time);
    }

    [ClientRpc]
    void PlayVideoClientRpc(double startTime)
    {
        videoPlayer.time = startTime;
        videoPlayer.Play();
    }

    public void OnPauseButtonClicked()
    {
            PauseVideoServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    void PauseVideoServerRpc()
    {
        PauseVideoClientRpc(videoPlayer.time);
    }

    [ClientRpc]
    void PauseVideoClientRpc(double pauseTime)
    {
        videoPlayer.time = pauseTime;
        videoPlayer.Pause();
    }
}
