using UnityEngine;

public class WaterSurface : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        Floating floatingScript = other.GetComponentInParent<Floating>();
        if (floatingScript != null)
        {
            floatingScript.SetFloating();
        }
    }
}