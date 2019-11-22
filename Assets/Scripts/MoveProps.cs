using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveProps : MonoBehaviour
{
    public GameObject rightHand;
    public Rigidbody[] balls;
    public float ballVerticalOffset = 0.4F;
    public float ballHorizontalOffset = -0.5F;
    private Customisation customisation;

    // Start is called before the first frame update
    void Start()
    {
        customisation = this.gameObject.GetComponent<Customisation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
        var numberOfBalls = customisation.GetNumberOfBalls();

        for (var i = 0; i < balls.Length; i++)
        {
            var horizontalOffset = i % 2 != 0 ? ballHorizontalOffset : 0;
            balls[i].transform.position = rightHandPosition + new Vector3(horizontalOffset, (i + 1) * ballVerticalOffset, 0);
            balls[i].velocity = new Vector3(0, 0, 0);

            if ( i < numberOfBalls)
            {
                balls[i].gameObject.SetActive(true);
            } else
            {
                balls[i].gameObject.SetActive(false);
            }
        }

        // ball1.transform.position = rightHandPosition + new Vector3(0, ballVerticalOffset, 0);
        // ball2.transform.position = rightHandPosition + new Vector3(ballHorizontalOffset, 2 * ballVerticalOffset, 0);
        // ball3.transform.position = rightHandPosition + new Vector3(0, 3 * ballVerticalOffset, 0);

        // var stationary = new Vector3(0, 0, 0);
        // ball1.velocity = stationary;
        // ball2.velocity = stationary;
        // ball3.velocity = stationary;
    }
}
