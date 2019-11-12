using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public float currentRecord = 0f;
    public float currentTime = 0f;
    public bool shouldTime = false;
    public GameObject currentTimeText;
    public GameObject currentRecordText;


    public void Start()
    {
        shouldTime = true;
        currentTimeText.GetComponent<VRTK.VRTK_ObjectTooltip>().UpdateText(currentTime.ToString());
        currentRecordText.GetComponent<VRTK.VRTK_ObjectTooltip>().UpdateText(currentRecord.ToString());
    }

    void Update()
    {
        if (shouldTime)
        {
            currentTime += Time.deltaTime;
            currentTimeText.GetComponent<VRTK.VRTK_ObjectTooltip>().UpdateText(currentTime.ToString());
        }
    }



    public void Stop()
    {
        if (currentRecord <= currentTime)
        {
            currentRecord = currentTime;
            currentRecordText.GetComponent<VRTK.VRTK_ObjectTooltip>().UpdateText(currentRecord.ToString());
        }

        shouldTime = false;
        currentTime = 0f;
    }
}
