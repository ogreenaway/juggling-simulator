using System.Collections.Generic;
using UnityEngine;

public class SiteSwapCreator
{
    public List<string> siteSwapList = new List<string>() { "_" };

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
        // We are calculating using the catches so 531 would be calculated
        // __1, _31, 531
        if (siteSwapList.Count <= index)
        {
            // In the example of 531 we need to add the "__" before the 1
            // There might be a nicer way to do this in C#
            for (int i = 0; i <= index - siteSwapList.Count; i++)
            {
                siteSwapList.Add("_");
            }
            // I'm not quite sure why there is an off-by-one error here
            siteSwapList.Add("_");
        }
        // Now it is safe to set the value
        siteSwapList[index] = value;
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
    }

    private string IntToHex(int i)
    {
        // TODO: i > 9 => a,b,c
        // maybe printf("%x\n", i);
        return i.ToString();
    }
}
