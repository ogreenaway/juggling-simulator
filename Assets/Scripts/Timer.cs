using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Record
{
    public Record(int catches = 0, float time = 0F)
    {
        Catches = catches;
        Time = time;
    }

    public int Catches { get; set; }
    public float Time { get; set; }

}

public class Timer : MonoBehaviour
{
    public VRTK.VRTK_ObjectTooltip currentCatchesText;
    public VRTK.VRTK_ObjectTooltip currentTimeText;
    public VRTK.VRTK_ObjectTooltip currentRecordText;

    private int numberOfBalls = 3;
    private float currentTime = 0F;
    private int currentCatches = 0;
    private bool isTimerRunning = false;
    private Dictionary<int, Record> records = new Dictionary<int, Record>()
    {
        { 1,  new Record()},
        { 2, new Record() },
        { 3, new Record() },
        { 4, new Record() },
        { 5, new Record() },
        { 6, new Record() },
        { 7, new Record() },
        { 8, new Record() },
        { 9, new Record() },
        { 10, new Record() },
        { 11, new Record() },
        { 12, new Record() },
        { 13, new Record() },
        { 14, new Record() },
        { 15, new Record() }
    };

    private void Start()
    {
        GameEvents.current.OnNumberOfBallsChange += OnNumberOfBallsChange;
        GameEvents.current.OnDrop += StopTimer;
        GameEvents.current.OnLaunch += StartTimer;
        GameEvents.current.OnCatch += Catch;
        SetCurrentStatsText();
        SetCurrentRecordText();
    }

    private void OnDestroy()
    {
        GameEvents.current.OnNumberOfBallsChange -= OnNumberOfBallsChange;
        GameEvents.current.OnDrop -= StopTimer;
        GameEvents.current.OnLaunch -= StartTimer;
        GameEvents.current.OnCatch -= Catch;
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            currentTime += Time.deltaTime;
            SetCurrentStatsText();
        }
    }

    private void Catch(uint controllerId, int ballId)
    {
        if (isTimerRunning)
        {
            currentCatches++;
        }
    }

    private float GetCurrentRecordTime()
    {
        return records[numberOfBalls].Time;
    }

    private int GetCurrentRecordCatches()
    {
        return records[numberOfBalls].Catches;
    }

    private void SetCurrentRecord()
    {
        // todo: if doesn't exist, create it
        records[numberOfBalls] = new Record(currentCatches, currentTime);
    }


    private void SetCurrentStatsText()
    {
        currentCatchesText.UpdateText(Math.Max(0, currentCatches - numberOfBalls).ToString());
        currentTimeText.UpdateText(currentTime.ToString("F1") + "s");

    }

    private void SetCurrentRecordText()
    {
        currentRecordText.UpdateText(numberOfBalls.ToString() + " ball record: " + Math.Max(0, GetCurrentRecordCatches() - numberOfBalls) + " catches " + GetCurrentRecordTime().ToString("F2") +"s");
    }

    private void OnNumberOfBallsChange(int newNumberOfBalls)
    {
        StopTimer();

        numberOfBalls = newNumberOfBalls;
        // Update UI with new record for new number of balls
        SetCurrentStatsText();
        SetCurrentRecordText();
    }

    private void StartTimer()
    {
        // If the timer is already running, see if they are setting a new record before restarting
        if (isTimerRunning)
        {
            if (GetCurrentRecordCatches() <= currentCatches)
            {
                SetCurrentRecord();
            }
        }
        isTimerRunning = true;
        currentCatches = 0;
        currentTime = 0F;
        SetCurrentStatsText();
        SetCurrentRecordText();
    }

    private void StopTimer()
    {
        if (isTimerRunning) // a previous ball stopped the timer
        {
            if (GetCurrentRecordCatches() <= currentCatches)
            {
                SetCurrentRecord();
                SetCurrentRecordText();
                SetCurrentStatsText();
            }

            isTimerRunning = false;
        }
    }
}
