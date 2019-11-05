using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateProps : MonoBehaviour
{
    public GameObject rightHand;
    public GameObject leftHand;
    public GameObject propsPrefab;
    public Vector3 propsOffset = new Vector3(0, 0.3f, 0);

    public void CreatePropsAtRightHand()
    {
        Create(rightHand);
    }

    public void CreatePropsAtLeftHand()
    {
        Create(leftHand);
    }

    private void Create(GameObject hand)
    {
        var ballInstance = Instantiate(propsPrefab, hand.transform.position + propsOffset, hand.transform.rotation);
    }
}
