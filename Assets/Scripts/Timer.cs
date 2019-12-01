using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public VRTK.VRTK_ObjectTooltip currentTimeText;
    public VRTK.VRTK_ObjectTooltip currentRecordText;

    private int numberOfBalls = 3;
    private float currentTime = 0F;
    private bool isTimerRunning = false;
    private Dictionary<int, float> records = new Dictionary<int, float>()
    {
        { 1, 0F },
        { 2, 0F },
        { 3, 0F },
        { 4, 0F },
        { 5, 0F },
        { 6, 0F },
        { 7, 0F },
        { 8, 0F },
        { 9, 0F },
        { 10, 0F },
        { 11, 0F },
        { 12, 0F },
        { 13, 0F },
        { 14, 0F },
        { 15, 0F }
    };

    private void Start()
    {
        GameEvents.current.OnNumberOfBallsChange += OnNumberOfBallsChange;
        GameEvents.current.OnDrop += StopTimer;
        GameEvents.current.OnLaunch += StartTimer;
        SetCurrentTimeText();
        SetCurrentRecordText();
    }

    private void OnDestroy()
    {
        GameEvents.current.OnNumberOfBallsChange -= OnNumberOfBallsChange;
        GameEvents.current.OnDrop -= StopTimer;
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            currentTime += Time.deltaTime;
            SetCurrentTimeText();
        }
    }

    private float GetCurrentRecord()
    {
        // todo: throw error if not underfined
        // if undefined then return 0
        return records[numberOfBalls];
    }

    private void SetCurrentRecord(float newRecord)
    {
        // todo: if doesn't exist, create it
        records[numberOfBalls] = newRecord;
    }

    private void SetCurrentTimeText()
    {
        currentTimeText.UpdateText(currentTime.ToString("F1"));
    }

    private void SetCurrentRecordText()
    {
        currentRecordText.UpdateText(numberOfBalls.ToString() + " ball record: " + GetCurrentRecord().ToString("F2"));
    }

    private void OnNumberOfBallsChange(int newNumberOfBalls)
    {
        StopTimer();

        numberOfBalls = newNumberOfBalls;
        // Update UI with new record for new number of balls
        SetCurrentTimeText();
        SetCurrentRecordText();
    }

    private void StartTimer()
    {
        // If the timer is already running, see if they are setting a new record before restarting
        if (isTimerRunning)
        {
            if (GetCurrentRecord() <= currentTime)
            {
                SetCurrentRecord(currentTime);
            }
        }
        isTimerRunning = true;
        currentTime = 0F;
        SetCurrentTimeText();
        SetCurrentRecordText();
    }

    private void StopTimer()
    {
        if (isTimerRunning) // a previous ball stopped the timer
        {
            if (GetCurrentRecord() <= currentTime)
            {
                SetCurrentRecord(currentTime);
                SetCurrentRecordText();
                SetCurrentTimeText();
            }

            isTimerRunning = false;
        }
    }
}
