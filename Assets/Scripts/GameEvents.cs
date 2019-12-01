using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action<int> OnNumberOfBallsChange;
    public void NumberOfBallsChange(float x)
    {
        OnNumberOfBallsChange?.Invoke((int)x);
    }

    public event Action OnDrop;
    public void Drop()
    {
        OnDrop?.Invoke();
    }

    public event Action OnLaunch;
    public void Launch()
    {
        OnLaunch?.Invoke();
    }

    public event Action<uint, int> OnCatch;
    public void Catch(uint controllerId, int ballId)
    {
        OnCatch?.Invoke(controllerId, ballId);
    }
}
