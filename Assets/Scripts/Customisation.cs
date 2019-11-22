using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customisation : MonoBehaviour
{
    private int numberOfBalls = 3;
    private Clock clock;
    private float ballSize;
    private GameObject[] balls;

    // Start is called before the first frame update
    void Start()
    {
        clock = this.gameObject.GetComponent<Clock>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNumberOfBalls(int newNumberOfBalls)
    {
        clock.StopTimer();
        numberOfBalls = newNumberOfBalls;
        clock.UpdateScoreBoard();
    }

    public int GetNumberOfBalls()
    {
        return numberOfBalls;
    }

    public void SetGravity(float newGravity)
    {
        Physics.gravity = new Vector3(0, -1 * newGravity, 0);
    }
}
