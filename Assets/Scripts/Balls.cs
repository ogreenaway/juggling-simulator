using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balls : MonoBehaviour
{
    public GameObject[] balls = new GameObject[0];

    void Start()
    {
        balls = GameObject.FindGameObjectsWithTag("Prop");


        foreach (GameObject ball in balls)
        {
            ball.SetActive(false);
        }
    }
}
