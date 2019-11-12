using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class CustomInteractGrab : VRTK.VRTK_InteractGrab
{
    public GameObject siteSwap;
    public override void OnControllerGrabInteractableObject(ObjectInteractEventArgs e)
    {
        // Debug.Log("Grabed:" + e.controllerReference.index + " and " + e.target.GetInstanceID());
        // siteSwap.GetComponent<SiteSwaps>().RecordGrab(e.controllerReference.index, e.target.GetInstanceID());
        base.OnControllerGrabInteractableObject(e);
    }
}
