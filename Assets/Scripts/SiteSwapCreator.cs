using System.Collections.Generic;
using UnityEngine;

public class SiteSwapCreator
{
    public List<string> siteSwapList = new List<string>() {"_"};

    private int currentBeat = 0;
    private uint controllerIdOfPreviousCatch;
    private Dictionary<int, int> beatLastCaughtMap = new Dictionary<int, int>();
    private Dictionary<uint, int> ballHeldInHand = new Dictionary<uint, int>();

    public void Reset()
    {
        ballHeldInHand = new Dictionary<uint, int>();
        siteSwapList = new List<string>() { "_" };
        currentBeat = 0;
        beatLastCaughtMap = new Dictionary<int, int>();
        controllerIdOfPreviousCatch = 0;
    }

    public string GetSiteSwap() {
        return string.Join("", siteSwapList.ToArray());
    }

    // GameEvents
    // ===========================================

    public void OnCatch(uint controllerId, int ballId)
    {
        ballHeldInHand[controllerId] = ballId;

        // If you catch twice from the same hand then either a 2 or 0 just happened
        if (controllerIdOfPreviousCatch == controllerId)
        {
            // The controllerIds are hardcoded to 1 and 2
            uint otherContollerId = controllerId == 1 ? 2 : (uint)1;
            // Find out if the other hand is holding a ball
            if (ballHeldInHand.TryGetValue(otherContollerId, out int heldBallId))
            {
                // A 2 is effectively a held catch
                Catch(controllerId, heldBallId);
            }
            else
            {
                // Not much to do for a 0
                SetSiteSwapList(currentBeat, "0");
                currentBeat++;
                siteSwapList.Add("_");
            }
        }

        // We've already dealt with the 0 or 2 before this catch so now deal with this catch
        Catch(controllerId, ballId);
        // Tidy up
        controllerIdOfPreviousCatch = controllerId;
    }

    public void OnThrow(uint controllerId, int ballId)
    {
        if (ballHeldInHand.TryGetValue(controllerId, out int heldBallId))
        {
            if (heldBallId == ballId)
            {
                ballHeldInHand.Remove(controllerId);
            }
            else
            {
                Debug.LogError("BUG: Somehow threw ball " + ballId + " from hand holding " + ballHeldInHand[controllerId]);
            }
        }
    }

    // Private
    // ===========================================

    private void SetSiteSwapList(int index, string value)
    {
        try
        {
            siteSwapList[index] = value;
        } catch
        {
            Debug.LogError("GetSiteSwap: " + GetSiteSwap());
            Debug.LogError("index: " + index);
            Debug.LogError("value: "+ value);
            Debug.LogError("siteSwapList.Count: " + siteSwapList.Count);
            throw new System.IndexOutOfRangeException();
        }
    }

    void Catch(uint controllerId, int ballId)
    {
        if (beatLastCaughtMap.TryGetValue(ballId, out int beatLastCaught))
        {
            // Ball has been caught before so calculate it's siteswap
            var beatDifference = currentBeat - beatLastCaught;
            SetSiteSwapList(beatLastCaught, IntToHex(beatDifference));
        }

        beatLastCaughtMap[ballId] = currentBeat;
        currentBeat++;
        siteSwapList.Add("_");
    }

    private string IntToHex(int i)
    {
        // TODO: i > 9 => a,b,c
        // maybe printf("%x\n", i);
        return i.ToString();
    }
}
