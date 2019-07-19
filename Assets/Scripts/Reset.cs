using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    public GameObject ball1;
    public GameObject ball2;
    public GameObject ball3;
    public Vector3 position1;
    public Vector3 position2;
    public Vector3 position3;

    private void Start()
    {
        position1 = ball1.transform.position;
        position2 = ball2.transform.position;
        position3 = ball3.transform.position;
    }


    public void Go()
    {
        ball1.transform.position = position1;
        ball2.transform.position = position2;
        ball3.transform.position = position3;

        ball1.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball2.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball3.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
