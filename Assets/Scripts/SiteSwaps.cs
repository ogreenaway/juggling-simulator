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

    private SiteSwapCreator siteSwapCreator;
    private SiteSwapAnalyser siteSwapAnalyser;

    private void Start()
    {
        siteSwapCreator = new SiteSwapCreator();
        siteSwapAnalyser = new SiteSwapAnalyser();

        GameEvents.current.OnLaunch += OnLaunch;
        GameEvents.current.OnCatch += OnCatch;
        GameEvents.current.OnThrow += OnThrow;
        GameEvents.current.OnNumberOfBallsChange += OnNumberOfBallsChange;
    }

    private void OnDestroy()
    {
        GameEvents.current.OnLaunch -= OnLaunch;
        GameEvents.current.OnCatch -= OnCatch;
        GameEvents.current.OnThrow -= OnThrow;
        GameEvents.current.OnNumberOfBallsChange -= OnNumberOfBallsChange;
    }

    private void Render()
    {
        string sequenceJuggled = siteSwapCreator.GetSiteSwap();
        siteSwapText.UpdateText(sequenceJuggled.Substring(Math.Max(0, sequenceJuggled.Length - 15)));

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

    public void OnLaunch()
    {
        siteSwapCreator.Reset();
        Render();
    }

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
}