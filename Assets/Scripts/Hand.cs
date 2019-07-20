using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void CreateBall()
    {
        this.GetComponent<VRTK.VRTK_ObjectAutoGrab>().enabled = true;
    }

    public void DropBall()
    {
        this.GetComponent<VRTK.VRTK_ObjectAutoGrab>().enabled = false;
    }
}
