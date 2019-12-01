using System;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SiteSwaps : MonoBehaviour
{
    private List<string> siteSwapList = new List<string>() { "_" };
    private int currentBeat = 0;
    private Dictionary<int, int> beatLastCaughtMap = new Dictionary<int, int>();
    private uint controllerIdOfPreviousCatch;
    private Dictionary<uint, int> ballHeldInHand = new Dictionary<uint, int>();
    public VRTK_ObjectTooltip text;

    private void Start()
    {
        GameEvents.current.OnLaunch += Reset;
        GameEvents.current.OnCatch += OnCatch;
        GameEvents.current.OnThrow += OnThrow;
        Test();
    }

    private void OnDestroy()
    {
        GameEvents.current.OnLaunch -= Reset;
        GameEvents.current.OnCatch -= OnCatch;
        GameEvents.current.OnThrow -= OnThrow;
    }

    private string IntToHex(int i)
    {
        // TODO: i > 9 => a,b,c
        // maybe printf("%x\n", i);
        return i.ToString();
    }

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
            } else
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
        // Render to screen
        text.UpdateText("SiteSwap: " + string.Join("", siteSwapList.ToArray()));
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

    private void OnThrow(uint controllerId, int ballId)
    {
        if (ballHeldInHand[controllerId] != ballId)
        {
            Debug.LogError("BUG: Somehow threw ball " + ballId + " from hand holding " + ballHeldInHand[controllerId]);
        }
        ballHeldInHand.Remove(controllerId);
    }

    private string DetectSiteSwap(string completeSiteSwap)
    {
        string withoutUnderscores = completeSiteSwap.Split('_')[0];

        string[] registeredSiteSwaps = new string[] { "53", "3" };

        bool HasDetectedRegisteredSiteSwap = false;


        string detectedSiteSwap = "";
        int rounds = 0;

        foreach(string registeredSiteSwap in registeredSiteSwaps)
        {
            if (!HasDetectedRegisteredSiteSwap)
            {
                rounds = TestSiteSwap(withoutUnderscores, registeredSiteSwap, 0);
                if (rounds > 0)
                {
                    HasDetectedRegisteredSiteSwap = true;
                    detectedSiteSwap = registeredSiteSwap;
                }
            }
        }

        return HasDetectedRegisteredSiteSwap ? rounds + " round of " + detectedSiteSwap : "No pattern detected";
    }

    private int TestSiteSwap(string WholeSiteSwap, string validSiteSwap, int count)
    {
        if (WholeSiteSwap.EndsWith(validSiteSwap))
        {
            var remainingSiteSwap = WholeSiteSwap.Substring(0, WholeSiteSwap.Length - validSiteSwap.Length);
            return TestSiteSwap(remainingSiteSwap, validSiteSwap, count + 1);
        }
        return count;
    }

    private void Test()
    {
        Expect("No pattern detected", DetectSiteSwap("1_"));
        Expect("1 round of 3", DetectSiteSwap("3_"));
        Expect("2 round of 3", DetectSiteSwap("33_"));
        Expect("1 round of 53", DetectSiteSwap("53_"));
        Expect("2 round of 53", DetectSiteSwap("5353_3"));

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

            OnCatch(right, red);
            // Add two here
            OnCatch(right, green); // 4 caught
            OnCatch(left, red); // 3 caught
            OnCatch(left, blue);

            OnCatch(right, red);
            // Add two here
            OnCatch(right, green); // 4 caught
            OnCatch(left, red); // 3 caught
            OnCatch(left, blue);

            OnCatch(right, red);
            // Add two here
            OnCatch(right, green); // 4 caught
            OnCatch(left, red); // 3 caught
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
        Expect("42342342342342342342342", string.Join("", siteSwapList.ToArray()));
        Reset();

        SS3();
        Expect("333333_", string.Join("", siteSwapList.ToArray()));
        Reset();

        SS531();
        Expect("531", string.Join("", siteSwapList.ToArray()));
        Reset();

        SS40();
        Expect("4040404040404040_0_", string.Join("", siteSwapList.ToArray()));

        
    }

    // To reset unit tests
    private void Reset()
    {
        ballHeldInHand = new Dictionary<uint, int>();
        siteSwapList = new List<string>() { "-" };
        currentBeat = 0;
        beatLastCaughtMap = new Dictionary<int, int>();
        controllerIdOfPreviousCatch = 0;
    }

    private void Expect(string expected, string result)
    {
        if (expected == result)
        {
            Debug.Log("TEST PASSED: '" + expected + "' equalled '" + result + "'");
        } else
        {
            Debug.LogError("TEST FAIL: Expected '" + expected + "' by received '" + result + "'");
        }
    }
}
