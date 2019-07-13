using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
public class CustomTeleporter : VRTK_BasicTeleport
{

    public GameObject cameraRig;
    public GameObject cameraEye;
    public float height = 0.1f;
    public Vector3 currentUp = Vector3.up;
    private Vector3 perpendicular;

    override protected void OnTeleported(object sender, DestinationMarkerEventArgs e)
    {
        if (currentUp != e.raycastHit.normal)
        {
            Vector3 targetUp = e.raycastHit.normal;

            if (Vector3.Angle(currentUp, targetUp) == 180)
            {
                Debug.Log("OWEN: Special case for 180");
            }

            Quaternion playerRotationDelta = Quaternion.FromToRotation(currentUp, targetUp);
            cameraRig.transform.rotation = playerRotationDelta * cameraRig.transform.rotation;
            currentUp = targetUp;
        }
        else
        {
            Debug.Log("OWEN: On Same Surface, No Rotate");
        }

        cameraRig.transform.position = e.raycastHit.point + (e.raycastHit.normal * height);
        base.OnTeleported(sender, e);
    }
}
