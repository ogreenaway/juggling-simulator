using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class CustomInteractGrab : VRTK.VRTK_InteractGrab
{
    public uint controlerIndex;

    public override void OnControllerGrabInteractableObject(ObjectInteractEventArgs e)
    {
        if(e.target.tag == "Prop")
        {
            GameEvents.current.Catch(controlerIndex, e.target.GetInstanceID());
        } else
        {
            // Debug.Log("You grabbed something that wasn't a prop");
        }
        
        base.OnControllerGrabInteractableObject(e);
    }

    public override void OnControllerUngrabInteractableObject(ObjectInteractEventArgs e)
    {
        if (e.target.tag == "Prop")
        {
            GameEvents.current.Throw(controlerIndex, e.target.GetInstanceID());
        }
        else
        {
            //Debug.Log("You released something that wasn't a prop");
        }
        base.OnControllerUngrabInteractableObject(e);
    }
}
