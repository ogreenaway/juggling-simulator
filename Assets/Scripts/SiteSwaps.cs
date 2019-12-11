using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRTK;

public class SiteSwaps : MonoBehaviour
{
    public VRTK_ObjectTooltip siteSwapText;
    public VRTK_ObjectTooltip detectedSiteSwapNameText;
    public VRTK_ObjectTooltip detectedSiteSwapRecordText;
    public VRTK_ObjectTooltip detectedSiteSwapCatchesText;
    public bool test = false;

    private SiteSwapCreator siteSwapCreator;
    private SiteSwapAnalyser siteSwapAnalyser;


    private void Start()
    {
        siteSwapCreator = new SiteSwapCreator();
        siteSwapAnalyser = new SiteSwapAnalyser();

        GameEvents.current.OnLaunch += siteSwapCreator.Reset;
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
        GameEvents.current.OnLaunch -= siteSwapCreator.Reset;
        GameEvents.current.OnCatch -= OnCatch;
        GameEvents.current.OnThrow -= OnThrow;
        GameEvents.current.OnNumberOfBallsChange -= OnNumberOfBallsChange;
    }

    private void Render()
    {
        string sequenceJuggled = siteSwapCreator.GetSiteSwap();
        siteSwapText.UpdateText(sequenceJuggled);

        string detectedSiteSwapsNames = "Siteswap" + Environment.NewLine + Environment.NewLine;
        string detectedSiteSwapsRecords = "Record" + Environment.NewLine + Environment.NewLine;
        string detectedSiteSwapsCatches = "Catches" + Environment.NewLine + Environment.NewLine;

        foreach (string name in siteSwapAnalyser.GetDetectedSiteSwapNames(sequenceJuggled))
        {
            detectedSiteSwapsNames += name + Environment.NewLine;
        }

        foreach (string record in siteSwapAnalyser.GetDetectedSiteSwapRecords(sequenceJuggled))
        {
            detectedSiteSwapsRecords += record + Environment.NewLine;
        }

        foreach (string catches in siteSwapAnalyser.GetDetectedSiteSwapCatches(sequenceJuggled))
        {
            detectedSiteSwapsCatches += catches + Environment.NewLine;
        }

        detectedSiteSwapNameText.UpdateText(detectedSiteSwapsNames);
        detectedSiteSwapRecordText.UpdateText(detectedSiteSwapsRecords);
        detectedSiteSwapCatchesText.UpdateText(detectedSiteSwapsCatches);
    }

    // GameEvents
    // ===========================================    

    public void OnCatch(uint controllerId, int ballId)
    {
        siteSwapCreator.OnCatch(controllerId, ballId);
        Render();
    }

    private void OnThrow(uint controllerId, int ballId)
    {
        siteSwapCreator.OnThrow(controllerId, ballId);
        Render();
    }

    private void OnNumberOfBallsChange(int n)
    {
        siteSwapAnalyser.OnNumberOfBallsChange(n);
        siteSwapCreator.Reset();
        Render();
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