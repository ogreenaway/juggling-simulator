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
    public List<string> siteSwapList = new List<string>() { "-" };
    public int currentBeat = 0;
    public Dictionary<int, int> beatLastCaughtMap = new Dictionary<int, int>();
    public uint controllerIdOfPreviousThrow; // not used
    public uint controllerIdOfPreviousCatch;
    public Dictionary<uint, int> ballHeldInHand = new Dictionary<uint, int>();
    public uint firstController = 999;
    public uint secondController = 999;
    public VRTK.VRTK_ObjectTooltip text;


    private void Start()
    {
        GameEvents.current.OnLaunch += Reset;
        GameEvents.current.OnCatch += OnCatch;
    }

    private void OnDestroy()
    {
        GameEvents.current.OnLaunch -= Reset;
        GameEvents.current.OnCatch -= OnCatch;
    }

    private void Reset()
    {
        ballHeldInHand = new Dictionary<uint, int>();
        siteSwapList = new List<string>() { "-" };
        currentBeat = 0;
        beatLastCaughtMap = new Dictionary<int, int>();
        controllerIdOfPreviousCatch = 0;
    }

    private void Test()
    {
        uint left = 1;
        uint right = 2;
        int green = 1;
        int blue = 2;
        int red = 3;

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
            OnCatch(2, 1); // 5
        }

        void SS423()
        {
            // First two are caught
            OnCatch(right, green);
            OnCatch(left, blue);

            OnThrow(right, green); // 4 thrown
            OnCatch(right, red);
            // Add two here
            OnThrow(right, red); // 3 thrown from same hand as last throw
            OnCatch(right, green); // 4 caught
            OnThrow(left, blue); // 4
            OnCatch(left, red); // 3 caught
            OnThrow(left, red); // 3 thrown from same hand as last throw
            OnCatch(left, blue);


            OnThrow(right, green); // 4 thrown
            OnCatch(right, red);
            // Add two here
            OnThrow(right, red); // 3 thrown from same hand as last throw
            OnCatch(right, green); // 4 caught
            OnThrow(left, blue); // 4
            OnCatch(left, red); // 3 caught
            OnThrow(left, red); // 3 thrown from same hand as last throw
            OnCatch(left, blue);

            OnThrow(right, green); // 4 thrown
            OnCatch(right, red);
            // Add two here
            OnThrow(right, red); // 3 thrown from same hand as last throw
            OnCatch(right, green); // 4 caught
            OnThrow(left, blue); // 4
            OnCatch(left, red); // 3 caught
            OnThrow(left, red); // 3 thrown from same hand as last throw
            OnCatch(left, blue);


            OnCatch(right, red);
            // Add two here            
            OnCatch(right, green); // 4 caught            
            OnCatch(left, red); // 3 caught            
            OnCatch(left, blue);
        }

        void SS40()
        {
            // First two are caught
            OnCatch(right, green);
            OnCatch(right, blue);
            OnCatch(right, green);
            OnCatch(right, blue);
            OnCatch(right, green);
            OnCatch(right, blue);
            OnCatch(right, green);
            OnCatch(right, blue);
            OnCatch(right, green);
            OnCatch(right, blue);
        }
        
        SS423();
        Debug.Log("SiteSwap 423: " + string.Join("", siteSwapList.ToArray()));
        Reset();

        SS3();
        Debug.Log("SiteSwap 3: " + string.Join("", siteSwapList.ToArray()));
        Reset();

        SS531();
        Debug.Log("SiteSwap 531: " + string.Join("", siteSwapList.ToArray()));      
        Reset();

        SS40();
        Debug.Log("SiteSwap 40: " + string.Join("", siteSwapList.ToArray()));
    }

    private string IntToHex(int i)
    {
        // TODO: i > 9 => a,b,c
        // maybe printf("%x\n", i);
        return i.ToString();
    }

    private void SaveControllerId(uint id)
    {
        if(firstController == 999)
        {
            firstController = id;
        } else
        {
            if(firstController != id)
            {
                secondController = id;
            }
        }
    }

    private uint GetOtherControllerId(uint knownId)
    {
        if(knownId == firstController)
        {
            return secondController;
        }

        if (knownId == secondController)
        {
            return firstController;
        }

        Debug.LogWarning("knownId: " + knownId.ToString() + " not equal to " + firstController.ToString() + " or " + secondController.ToString());
        return 999;
    }

    private void SetSiteSwapList(int index, string value)
    {
        // Debug.Log(index.ToString() + " " + siteSwapList.Count.ToString());
        // This is some bullshit but I don't know the correct c# way to handle a list of unknown length

        if (siteSwapList.Count < index)
        {
            for (int i = 0; i <= index - siteSwapList.Count; i++)
            {
                siteSwapList.Add("_");
            }
        }
        siteSwapList.Add("_");
        siteSwapList[index] = value;
    }

    private void OnCatch(uint controllerId, int ballId)
    {
        SaveControllerId(controllerId);
        ballHeldInHand[controllerId] = ballId;

        if (controllerIdOfPreviousCatch == controllerId)
        {
            // Debug.Log("Caught twice in same hand");
            uint otherContollerId = GetOtherControllerId(controllerId);
            // Debug.Log("otherContollerId " + otherContollerId.ToString());
            // Debug.Log("ballHeldInHand[otherContollerId] " + ballHeldInHand[otherContollerId].ToString());
            if (ballHeldInHand.TryGetValue(otherContollerId, out int heldBallId))
            {
                Catch(controllerId, heldBallId);
            } else
            {
                SetSiteSwapList(currentBeat, "EMPTY");
                currentBeat++;
            }
        }

        Catch(controllerId, ballId);
        controllerIdOfPreviousCatch = controllerId;
        text.UpdateText("SiteSwap: " + string.Join("", siteSwapList.ToArray()));
    }

    void Catch(uint controllerId, int ballId)
    {
        if (beatLastCaughtMap.TryGetValue(ballId, out int beatLastCaught))
        {
            var beatDifference = currentBeat - beatLastCaught;
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
