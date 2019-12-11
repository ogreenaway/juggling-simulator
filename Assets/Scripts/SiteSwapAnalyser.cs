using System.Collections.Generic;

internal class SiteSwap
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

public class SiteSwapAnalyser
{
    private Dictionary<int, SiteSwap[]> registeredSiteSwapsMap = new Dictionary<int, SiteSwap[]>()
        {
            { 1, new SiteSwap[] { new SiteSwap("1"), new SiteSwap("20") }},
            { 2, new SiteSwap[] { new SiteSwap("2"), new SiteSwap("31"), new SiteSwap("40"), new SiteSwap("501") } },
            { 3, new SiteSwap[] { new SiteSwap("3"), new SiteSwap("423"), new SiteSwap("531"),  new SiteSwap("51") } }, // new SiteSwap("441"),
            { 4, new SiteSwap[] { new SiteSwap("4"), new SiteSwap("53"), new SiteSwap("534"),  new SiteSwap("71"), new SiteSwap("7531") } },
            { 5, new SiteSwap[] { new SiteSwap("5"), new SiteSwap("645"), new SiteSwap("91"), new SiteSwap("97531") } }, // new SiteSwap("744"),
            { 6, new SiteSwap[] { new SiteSwap("6"), new SiteSwap("75"), new SiteSwap("9555") } },
            { 7, new SiteSwap[] { new SiteSwap("7"), new SiteSwap("95") } },
            { 8, new SiteSwap[] { new SiteSwap("8") } },
            { 9, new SiteSwap[] { new SiteSwap("9") } },
            { 10, new SiteSwap[] { new SiteSwap("a") } },
            { 11, new SiteSwap[] { new SiteSwap("b") } },
            { 12, new SiteSwap[] { new SiteSwap("c") } },
            { 13, new SiteSwap[] { new SiteSwap("d") } },
            { 14, new SiteSwap[] { new SiteSwap("e") } },
            { 15, new SiteSwap[] { new SiteSwap("f") } }
        };

    private int numberOfBalls = 3;

    public string[] GetDetectedSiteSwapNames(string sequenceActuallyJuggled) {
        DetectSiteSwap(sequenceActuallyJuggled);

        List<string> detectedSiteSwapNamesList = new List<string>();

        foreach (SiteSwap registeredSiteSwap in registeredSiteSwapsMap[numberOfBalls])
        {
            detectedSiteSwapNamesList.Add(registeredSiteSwap.Name);
        }

        return detectedSiteSwapNamesList.ToArray();
    }

    public string[] GetDetectedSiteSwapRecords(string sequenceActuallyJuggled)
    {
        DetectSiteSwap(sequenceActuallyJuggled);

        List<string> detectedSiteSwapRecordsList = new List<string>();

        foreach (SiteSwap registeredSiteSwap in registeredSiteSwapsMap[numberOfBalls])
        {
            detectedSiteSwapRecordsList.Add(registeredSiteSwap.Record.ToString());
        }

        return detectedSiteSwapRecordsList.ToArray();
    }

    public string[] GetDetectedSiteSwapCatches(string sequenceActuallyJuggled)
    {
        DetectSiteSwap(sequenceActuallyJuggled);

        List<string> detectedSiteSwapCatchesList = new List<string>();

        foreach (SiteSwap registeredSiteSwap in registeredSiteSwapsMap[numberOfBalls])
        {
            detectedSiteSwapCatchesList.Add(registeredSiteSwap.CurrentCatches.ToString());
        }

        return detectedSiteSwapCatchesList.ToArray();
    }


    public void DetectSiteSwap(string sequenceActuallyJuggled)
    {
        SiteSwap[] registeredSiteSwaps = registeredSiteSwapsMap[numberOfBalls];

        foreach (SiteSwap registeredSiteSwap in registeredSiteSwaps)
        {
            SiteSwapAnalyser siteSwapAnalyser = new SiteSwapAnalyser();
            registeredSiteSwap.CurrentCatches = siteSwapAnalyser.CountCatches(registeredSiteSwap.Name, sequenceActuallyJuggled);

            if (registeredSiteSwap.CurrentCatches > registeredSiteSwap.Record)
            {
                registeredSiteSwap.Record = registeredSiteSwap.CurrentCatches;
            }
        }
    }

    public void OnNumberOfBallsChange(int n)
    {
        numberOfBalls = n;
    }

    public int CountCatches(string siteSwap, string sequenceActuallyJuggled)
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
}
