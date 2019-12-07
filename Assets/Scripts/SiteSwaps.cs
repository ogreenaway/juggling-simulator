using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRTK;

public class SiteSwap
{
    public SiteSwap(string name)
    {
        Name = name;
        Record = 0;
        CurrentCatches = 0;
    }

    public string Name { get; set; }
    public int Record { get; set; }
    public int CurrentCatches { get; set; }

}

public class SiteSwaps : MonoBehaviour
{
    private int numberOfBalls = 3;
    private List<string> siteSwapList = new List<string>() { "_" };
    private int currentBeat = 0;
    private Dictionary<int, int> beatLastCaughtMap = new Dictionary<int, int>();
    private uint controllerIdOfPreviousCatch;
    private Dictionary<uint, int> ballHeldInHand = new Dictionary<uint, int>();
    public VRTK_ObjectTooltip siteSwapText;
    public VRTK_ObjectTooltip detectedSiteSwaptext;
    public bool test = false;

    private Dictionary<int, SiteSwap[]> registeredSiteSwapsMap = new Dictionary<int, SiteSwap[]>()
        {
            { 1,  new SiteSwap[] { new SiteSwap("1") }},
            { 2, new SiteSwap[] { new SiteSwap("31"),  new SiteSwap("40") } },
            { 3, new SiteSwap[] { new SiteSwap("3"),  new SiteSwap("423"),  new SiteSwap("531"),  new SiteSwap("51") } },
            { 4, new SiteSwap[] { new SiteSwap("4"),  new SiteSwap("53"),  new SiteSwap("534"),  new SiteSwap("71") } },
            { 5, new SiteSwap[] { new SiteSwap("5"),  new SiteSwap("645"),  new SiteSwap("744"),  new SiteSwap("91") } },
            { 6, new SiteSwap[] { new SiteSwap("6"),  new SiteSwap("75"),  new SiteSwap("9555") } },
            { 7, new SiteSwap[] { new SiteSwap("7") } },
            { 8, new SiteSwap[] { new SiteSwap("8") } },
            { 9, new SiteSwap[] { new SiteSwap("9") } },
            { 10, new SiteSwap[] { new SiteSwap("a") } },
            { 11, new SiteSwap[] { new SiteSwap("b") } },
            { 12, new SiteSwap[] { new SiteSwap("c") } },
            { 13, new SiteSwap[] { new SiteSwap("d") } },
            { 14, new SiteSwap[] { new SiteSwap("e") } },
            { 15, new SiteSwap[] { new SiteSwap("f") } }
        };

    private void Start()
    {
        GameEvents.current.OnLaunch += Reset;
        GameEvents.current.OnCatch += OnCatch;
        GameEvents.current.OnThrow += OnThrow;
        GameEvents.current.OnNumberOfBallsChange += OnNumberOfBallsChange;
        if (test)
        {
            Test();
        }
    }

    private void OnDestroy()
    {
        GameEvents.current.OnLaunch -= Reset;
        GameEvents.current.OnCatch -= OnCatch;
        GameEvents.current.OnThrow -= OnThrow;
        GameEvents.current.OnNumberOfBallsChange -= OnNumberOfBallsChange;
    }

    private void Render()
    {
        siteSwapText.UpdateText("..." + string.Join("", siteSwapList.ToArray().Skip(Math.Max(0, siteSwapList.Count() - 10)).Take(10)));

        DetectSiteSwap(string.Join("", siteSwapList.ToArray()));

        // TODO: DetectSiteSwap returns a string
        // TODO: DetectSiteSwap takes a list

        SiteSwap[] registeredSiteSwaps = registeredSiteSwapsMap[numberOfBalls];

        string detectedSiteSwaps = "";

        foreach (SiteSwap registeredSiteSwap in registeredSiteSwaps)
        {
            detectedSiteSwaps += registeredSiteSwap.Name + " " + registeredSiteSwap.Record + " " + registeredSiteSwap.CurrentCatches + Environment.NewLine;
        }

        detectedSiteSwaptext.UpdateText(detectedSiteSwaps);
    }

    // GameEvents
    // ===========================================

    private void Reset()
    {
        ballHeldInHand = new Dictionary<uint, int>();
        siteSwapList = new List<string>() { "_" };
        currentBeat = 0;
        beatLastCaughtMap = new Dictionary<int, int>();
        controllerIdOfPreviousCatch = 0;
        Render();
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
        Render();
    }

    private void OnThrow(uint controllerId, int ballId)
    {
        if (ballHeldInHand[controllerId] != ballId)
        {
            Debug.LogError("BUG: Somehow threw ball " + ballId + " from hand holding " + ballHeldInHand[controllerId]);
        }
        ballHeldInHand.Remove(controllerId);
    }

    private void OnNumberOfBallsChange(int n)
    {
        numberOfBalls = n;
        Reset();
    }

    // Create siteSwaps
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

    // Detect siteSwaps
    // ===========================================

    private void DetectSiteSwap(string sequenceActuallyJuggled)
    {
        SiteSwap[] registeredSiteSwaps = registeredSiteSwapsMap[numberOfBalls];

        foreach(SiteSwap registeredSiteSwap in registeredSiteSwaps)
        {
            registeredSiteSwap.CurrentCatches = CountCatches(registeredSiteSwap.Name, sequenceActuallyJuggled);

            if (registeredSiteSwap.CurrentCatches > registeredSiteSwap.Record)
            {
                registeredSiteSwap.Record = registeredSiteSwap.CurrentCatches;
            }
        }
    }

    int CountCatches(string siteSwap, string sequenceActuallyJuggled)
    {
        if (Trimmed(sequenceActuallyJuggled).Length == 0) return 0;

        string lastThrow = LastCharacter(Trimmed(sequenceActuallyJuggled));

        if (siteSwap.Contains(lastThrow))
        {
            return WasPreviousThrowValid(
                Trimmed(sequenceActuallyJuggled),
                siteSwap,
                siteSwap.IndexOf(lastThrow),
                1
            );
        }
        else
        {
            return 0;
        }
    }

    int WasPreviousThrowValid(string sequenceActuallyJuggled, string siteSwap, int indexOfCurrentThrow, int count)
    {
        // We know the last sequence is valid, now look at previous throws
        string newSequence = RemoveLastCharacter(sequenceActuallyJuggled);

        // If no previous catches then it's invalid
        if (newSequence.Length == 0)
        {
            return count;
        }

        if (LastCharacter(newSequence) == siteSwap[PreviousIndex(siteSwap, indexOfCurrentThrow)].ToString())
        {
            return WasPreviousThrowValid(
                    newSequence,
                    siteSwap,
                    PreviousIndex(siteSwap, indexOfCurrentThrow),
                    count + 1
            );
        }

        return count;
    }

    string PreviousThrowInSiteSwap(string siteSwap, int indexOfCurrentThrow)
    {
        return indexOfCurrentThrow == 0 ? LastCharacter(siteSwap) : Previous(siteSwap, indexOfCurrentThrow);
    }

    int PreviousIndex(string siteSwap, int index)
    {
        return index == 0 ? siteSwap.Length - 1 : index - 1;
    }

    // Utils
    // ===========================================

    private string IntToHex(int i)
    {
        // TODO: i > 9 => a,b,c
        // maybe printf("%x\n", i);
        return i.ToString();
    }

    string LastCharacter(string s)
    {
        return s[s.Length - 1].ToString();
    }

    string Trimmed(string s)
    {
        return s.Split('_')[0];
    }

    string Previous(string s, int i)
    {
        return s[i - 1].ToString();
    }

    string RemoveLastCharacter(string s)
    {
        return s.Remove(s.Length - 1);
    }

    // Testing
    // ===========================================

    private void Test()
    {
        //Expect("53: 1, 3: 1", DetectSiteSwap("3_"));
        //Expect("53: 1, 3: 2", DetectSiteSwap("33_"));
        //Expect("53: 1, 3: 3", DetectSiteSwap("333_"));
        //Expect("53: 1, 3: 3", DetectSiteSwap("4333_"));
        //Expect("53: 2, 3: 1", DetectSiteSwap("53_"));
        //Expect("53: 5, 3: 0", DetectSiteSwap("553535_"));
        //Expect("53: 5, 3: 1", DetectSiteSwap("5335353_"));

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

        //SS423();
        //Expect("42342342342342342342342", string.Join("", siteSwapList.ToArray()));
        //Reset();

        //SS3();
        //Expect("333333_", string.Join("", siteSwapList.ToArray()));
        //Reset();

        //SS531();
        //Expect("531", string.Join("", siteSwapList.ToArray()));
        //Reset();

        //SS40();
        //Expect("4040404040404040_0_", string.Join("", siteSwapList.ToArray()));
    }


    private void Expect(string expected, string result)
    {
        if (expected == result)
        {
            Debug.Log("TEST PASSED: '" + expected + "'");
        } else
        {
            Debug.LogError("TEST FAIL: Expected '" + expected + "' by received '" + result + "'");
        }
    }
}
