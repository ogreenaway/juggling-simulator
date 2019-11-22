using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customisation : MonoBehaviour
{
    private int numberOfBalls = 3;
    private Clock clock;

    void Start()
    {
        clock = this.gameObject.GetComponent<Clock>();
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
}
