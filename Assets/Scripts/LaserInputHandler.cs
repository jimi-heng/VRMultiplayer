using UnityEngine;
using UnityEngine.InputSystem;

public class LaserInputHandler : MonoBehaviour
{
    public InputActionProperty activateAction; 
    private NetworkLaser playerLaser;

    void Update()
    {
        if (playerLaser == null && PlayerReference.LocalInstance != null)
        {
            playerLaser = PlayerReference.LocalInstance.GetComponentInChildren<NetworkLaser>();
        }

        if (playerLaser != null)
        {
            float value = activateAction.action.ReadValue<float>();
            bool pressed = value > 0.1f;
            playerLaser.SetLaserActive(pressed);
        }
    }
}
