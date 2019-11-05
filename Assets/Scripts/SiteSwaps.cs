using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteSwaps : MonoBehaviour
{
    public string siteswap = "";
    public int count = 0;
    public Dictionary<int, int> map = new Dictionary<int, int>();
    public uint previousControllerIndex;

    private void Start()
    {

    }

    public void RecordGrab(uint controllerIndex, int ballIndex)
    {
        Debug.Log(controllerIndex + ", " + ballIndex);
        count++;

        

        if (map.ContainsKey(ballIndex))
        {
            if (controllerIndex == previousControllerIndex)
            {
                Debug.Log("Same hand, Old count: " + map[ballIndex] + ", current count " + count);
                int siteswapForGrabbedBall = count - map[ballIndex];
                siteswap = siteswap + siteswapForGrabbedBall.ToString();
                map[ballIndex] = count;
            }
            else
            {
                previousControllerIndex = controllerIndex;
                Debug.Log("Other hand, Old count: " + map[ballIndex] + ", current count " + count);
                int siteswapForGrabbedBall = count - map[ballIndex];
                siteswap = siteswap + siteswapForGrabbedBall.ToString();
                map[ballIndex] = count;
            }
            

        } else
        {
            map.Add(ballIndex, count);
        }     

        Debug.Log("SiteSwap: " + siteswap);
    }
}
