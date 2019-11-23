using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMode : MonoBehaviour
{
    public GameObject furniture;
    private bool isShowing = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Toggle()
    {
        furniture.SetActive(!isShowing);
        isShowing = !isShowing;
    }

    public void Test(float n)
    {
        Debug.Log(n);
    }
}
