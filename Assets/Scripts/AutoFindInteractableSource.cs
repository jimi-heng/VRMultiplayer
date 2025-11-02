using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.State;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class AutoFindInteractableSource : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<XRInteractableAffordanceStateProvider>().interactableSource = GetComponentInParent<XRBaseInteractable>();
    }
}
