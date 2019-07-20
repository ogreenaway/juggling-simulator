using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Props : MonoBehaviour
{
    public GameObject rightHand;
    public GameObject leftHand;
    public Rigidbody ballPrefab;
    public Vector3 ballOffset = new Vector3(0,0.3f,0);
    public float ballSpeed = 2;

    public void CreateBallAtRightHand()
    {
        CreateBall(rightHand);
    }

    public void CreateBallAtLeftHand()
    {
        CreateBall(leftHand);
    }

    private void CreateBall(GameObject hand)
    {
        var ballInstance = Instantiate(ballPrefab, hand.transform.position + ballOffset, hand.transform.rotation);
        ballInstance.velocity = hand.transform.up * ballSpeed;
    }
}
