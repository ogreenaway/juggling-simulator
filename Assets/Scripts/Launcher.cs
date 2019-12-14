using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    private float ballVerticalOffset = 0.4F;
    private float ballHorizontalOffset = -0.5F;
    private  int numberOfBalls = 3;

    public GameObject rightHand;

    private void Start()
    {
        GameEvents.current.OnNumberOfBallsChange += SetNumberOfBalls;
        GameEvents.current.OnLaunch += Launch;
    }

    private void OnDestroy()
    {
        GameEvents.current.OnNumberOfBallsChange -= SetNumberOfBalls;
        GameEvents.current.OnLaunch -= Launch;
    }

    public void SetBallHorizontalOffset(float offset)
    {
        ballHorizontalOffset = -1F * offset;
    }

    public void SetBallVerticalOffset(float offset)
    {
        ballVerticalOffset = offset;
    }

    private void SetNumberOfBalls(int newNumberOfBalls)
    {
        numberOfBalls = newNumberOfBalls;
    }

    private void Launch()
    {
        var rightHandPosition = rightHand.transform.position;
        var balls = gameObject.GetComponent<Props>().balls;

        foreach(GameObject ball in balls)
        {
            int i = ball.GetComponent<CustomId>().id;

            var horizontalOffset = i % 2 != 0 ? ballHorizontalOffset : 0;
            var position= rightHandPosition + new Vector3(horizontalOffset, (i + 1) * ballVerticalOffset, 0);
            ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            ball.GetComponent<TrailRenderer>().Clear();
            ball.SetActive(i < numberOfBalls);
        }
    }
}
