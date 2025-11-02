using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRRigReferences : MonoBehaviour
{
    public static VRRigReferences Singleton;

    public Transform root;
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;
    // Start is called before the first frame update

    private void Awake()
    {
        Singleton = this;
    }

}
