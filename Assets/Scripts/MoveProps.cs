using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveProps : MonoBehaviour
{
    public GameObject rightHand;
    private float ballVerticalOffset = 0.4F;
    private float ballHorizontalOffset = -0.5F;

    public void SetBallHorizontalOffset(float offset)
    {
        ballHorizontalOffset = -1F * offset;
    }

    public void SetBallVerticalOffset(float offset)
    {
        ballVerticalOffset = offset;
    }

    public void Move()
    {
        var rightHandPosition = rightHand.transform.position;
        var numberOfBalls = this.gameObject.GetComponent<Customisation>().GetNumberOfBalls();
        var balls = this.gameObject.GetComponent<Props>().balls;

        for (var i = 0; i < balls.Length; i++)
        {
            var horizontalOffset = i % 2 != 0 ? ballHorizontalOffset : 0;
            balls[i].transform.position = rightHandPosition + new Vector3(horizontalOffset, (i + 1) * ballVerticalOffset, 0);
            balls[i].GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

            if ( i < numberOfBalls)
            {
                balls[i].SetActive(true);
            } else
            {
                balls[i].SetActive(false);
            }
        }
    }
}
