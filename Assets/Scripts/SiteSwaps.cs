using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteSwaps : MonoBehaviour
{
    // OLD
    public string siteswap = "";
    public int count = 0;
    public Dictionary<int, int> map = new Dictionary<int, int>();
    public uint previousControllerIndex;


    // NEW
    public List<string> siteSwapList = new List<string>();
    public int currentBeat = 0;
    public Dictionary<int, int> beatLastCaughtMap = new Dictionary<int, int>();

    private void Start()
    {
        Test();
    }

    private void Reset()
    {
        siteSwapList = new List<string>();
        currentBeat = 0;
        beatLastCaughtMap = new Dictionary<int, int>();
    }

    private void Test()
    {
        void SS3()
        {
            OnCatch(1, 1);
            OnCatch(2, 2);
            OnCatch(1, 3);
            OnCatch(2, 1);
            OnCatch(1, 2);
            OnCatch(2, 3);
            OnCatch(1, 1);
            OnCatch(2, 2);
            OnCatch(1, 3);
        }

        void SS531()
        {
            OnCatch(1, 1);
            OnCatch(2, 2);
            OnCatch(1, 3);
            OnCatch(2, 3); // 1
            OnCatch(1, 2); // 3
            // OnCatch(2, 1); // 5
        }

        SS3();
        SS531();

        Debug.Log("SiteSwap: " + string.Join("", siteSwapList.ToArray()));
    }

    private string IntToHex(int i)
    {
        // TODO: i > 9 => a,b,c
        // maybe printf("%x\n", i);
        return i.ToString();
    }

    private void SetSiteSwapList(int index, string value)
    {
        // This is some bullshit but I don't know the correct c# way to handle a list of unknown length

        if(siteSwapList.Count < index)
        {
            for (int i = 0; i <= index - siteSwapList.Count; i++)
            {
                siteSwapList.Add("_");
            }
        }
        siteSwapList.Add("_");
        siteSwapList[index] = value;
    }

    public void OnCatch(uint controllerId, int ballId)
    {
        // Debug.Log("OnCatch: Controller ID:" + controllerId + ". Ball ID " + ballId);

        if (beatLastCaughtMap.TryGetValue(ballId, out int beatLastCaught))
        {
            var beatDifference = currentBeat - beatLastCaught;
            // Debug.Log("beatDifference " + beatDifference.ToString() + " and " + beatLastCaught.ToString() + " " + currentBeat.ToString());
            SetSiteSwapList(beatLastCaught, IntToHex(beatDifference));
        }

        beatLastCaughtMap[ballId] = currentBeat;
        currentBeat++;
    }

    public void OnThrow(uint controllerId, int ballId)
    {
        // beatLastThrownMap[ballId] = currentBeat;
        // Debug.Log("OnThrow: Controller ID:" + controllerId + ". Ball ID " + ballId);
    }

    public void OldMethod(uint controllerIndex, int ballIndex)
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
