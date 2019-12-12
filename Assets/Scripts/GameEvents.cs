using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;
    public List<string> eventLog = new List<string>();

    private void Awake()
    {
        current = this;
    }

    public void Start()
    {
        string output = "";
        foreach(string e in eventLog.ToArray()){
            output = output + e + Environment.NewLine;
        }
        Debug.Log(output);
    }

    public event Action<int> OnNumberOfBallsChange;
    public void NumberOfBallsChange(float x)
    {
        OnNumberOfBallsChange?.Invoke((int)x);
        LogEvent("siteSwapCreator.Reset(); // OnNumberOfBallsChange " + x);
    }

    public event Action OnDrop;
    public void Drop()
    {
        OnDrop?.Invoke();
        LogEvent("siteSwapCreator.Reset(); // OnDrop");
    }

    public event Action OnLaunch;
    public void Launch()
    {
        OnLaunch?.Invoke();
        LogEvent("siteSwapCreator.Reset(); // OnLaunch");
    }

    public event Action<uint, int> OnCatch;
    public void Catch(uint controllerId, int ballId)
    {
        OnCatch?.Invoke(controllerId, ballId);
        LogEvent("siteSwapCreator.OnCatch(" + controllerId + ", " + ballId + ");");
    }

    public event Action<uint, int> OnThrow;
    public void Throw(uint controllerId, int ballId)
    {
        OnThrow?.Invoke(controllerId, ballId);
        LogEvent("siteSwapCreator.OnThrow("+controllerId+", "+ballId+");");
    }

    public event Action<int, Material> OnPaint;
    public void Paint(int ballIndex, Material material)
    {
        OnPaint?.Invoke(ballIndex, material);
        // eventLog.Add("Paint ball " + ballIndex + " with " + material.name);
    }

    private void LogEvent(string message)
    {
        //eventLog.Add(message);
    }
}
