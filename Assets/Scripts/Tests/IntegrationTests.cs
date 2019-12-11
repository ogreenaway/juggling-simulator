using System.Collections;
using UnityEngine;

public class IntegrationTests : MonoBehaviour
{
    public bool runTests = true;

    [Header("Sliders")]
    public SettingsSlider gameModeSlider;
    public GravitySlider gravitySlider;

    [Header("Start UI")]
    public VRTK.VRTK_ObjectTooltip numberOfBallsText;
    public VRTK.VRTK_ObjectTooltip gameModeText;

    [Header("Timer")]
    public VRTK.VRTK_ObjectTooltip TimerCatchText;
    public VRTK.VRTK_ObjectTooltip TimerTimeText;
    public VRTK.VRTK_ObjectTooltip TimerRecordText;

    [Header("Materials")]
    public Material defaultMaterial;
    public Material redMaterial;

    [Header("Other")]
    public GameObject fullGameSection;
    public Renderer paintBrushBristles;
    public GameObject rightHandControllerAlias;

    private readonly float defaultGravityFloat = 2.5F;
    private readonly Vector3 defaultGravity = new Vector3(0F, -2.5F, 0F);
    private readonly Vector3 defaultBallScale = new Vector3(0.2F, 0.2F, 0.2F);
    private readonly float defaultDrag = 3.3F;
    private readonly float defaultColliderRadius = 0.5F;
    private readonly string defaultRecord = "3 ball record: 0 catches 0.00s";

    void Start()
    {
        if (runTests)
        {
            StartCoroutine(TestRunner());
        }
    }

    IEnumerator TestRunner()
    {
        // Don't launch into the floor
        rightHandControllerAlias.transform.position = new Vector3(0,3,0);

        // Wait for scene to load
        yield return null;
        yield return null;
        // yield return new WaitForSeconds(1f);

        yield return StartCoroutine(TestTimer());
        
        yield return null;
    }

    private IEnumerator TestTimer()
    {
        // TODO: Move to a edit mode unit test
        Info("Timer");
        GameEvents.current.NumberOfBallsChange(3);
        GameEvents.current.Launch();
        Test("Record count is default", defaultRecord, TimerRecordText.displayText);

        Juggle("3", 2);
        yield return null;
        Test("Catches are counted", "11", TimerCatchText.displayText);
        GameEvents.current.Drop();
        Test("Record count is set", "3 ball record: 11 catches", TimerRecordText.displayText.Substring(0, 25));

        GameEvents.current.Launch();
        Juggle("3", 1);
        yield return null;
        Test("Catches are counted", "5", TimerCatchText.displayText);
        GameEvents.current.Drop();
        Test("Record is not set if the number of catches is lower", "3 ball record: 11 catches", TimerRecordText.displayText.Substring(0, 25));

        GameEvents.current.Launch();
        Juggle("3", 3);
        yield return null;
        Test("Catches are counted", "17", TimerCatchText.displayText);
        GameEvents.current.Launch();
        Test("Record is not set if the number of catches is higher when a launch happens", "3 ball record: 17 catches", TimerRecordText.displayText.Substring(0, 25));

        yield return null;
    }

    //private void TestTimer()
    //{
        
    //}

    private void Info(string info)
    {
        Debug.Log("<color=blue>" + info + "</color>");
    }

    private void Test<T>(string name, T expected, T result)
    {
         if (expected.Equals(result))
        {
            Debug.Log("<color=green>TEST PASSED: " + name + "</color>");
        }
        else
        {
            Debug.LogError("<color=red>TEST FAIL: " + name + ". Expected '" + expected + "' by received '" + result + "'</color>");
        }
    }

    private void Juggle(string siteswap, int rounds)
    {
        uint left = 1;
        uint right = 2;
        int green = 1;
        int blue = 2;
        int red = 3;

        switch (siteswap)
        {
            case "3":
                GameEvents.current.Catch(left, green);
                GameEvents.current.Catch(right, blue);

                for (int i = 0; i < rounds; i++)
                {
                    GameEvents.current.Catch(left, red);
                    GameEvents.current.Catch(right, green);
                    GameEvents.current.Catch(left, blue);
                    GameEvents.current.Catch(right, red);
                    GameEvents.current.Catch(left, green);
                    GameEvents.current.Catch(right, blue);
                }
                break;
            case "531":
                GameEvents.current.Catch(left, green);
                GameEvents.current.Catch(right, blue);
                GameEvents.current.Catch(left, red);
                GameEvents.current.Catch(right, red); // 1
                GameEvents.current.Catch(left, blue); // 3
                GameEvents.current.Catch(right, green); // 5
                break;
            case "423":
                // First two are caught
                GameEvents.current.Catch(right, green);
                GameEvents.current.Catch(left, blue);

                GameEvents.current.Catch(right, red);
                // Add two here
                GameEvents.current.Catch(right, green); // 4 caught
                GameEvents.current.Catch(left, red); // 3 caught
                GameEvents.current.Catch(left, blue);

                GameEvents.current.Catch(right, red);
                // Add two here
                GameEvents.current.Catch(right, green); // 4 caught
                GameEvents.current.Catch(left, red); // 3 caught
                GameEvents.current.Catch(left, blue);

                GameEvents.current.Catch(right, red);
                // Add two here
                GameEvents.current.Catch(right, green); // 4 caught
                GameEvents.current.Catch(left, red); // 3 caught
                GameEvents.current.Catch(left, blue);


                GameEvents.current.Catch(right, red);
                // Add two here            
                GameEvents.current.Catch(right, green); // 4 caught            
                GameEvents.current.Catch(left, red); // 3 caught            
                GameEvents.current.Catch(left, blue);
                break;
            case "40":
                GameEvents.current.Catch(right, green);
                GameEvents.current.Catch(right, blue);
                GameEvents.current.Catch(right, green);
                GameEvents.current.Catch(right, blue);
                GameEvents.current.Catch(right, green);
                GameEvents.current.Catch(right, blue);
                GameEvents.current.Catch(right, green);
                GameEvents.current.Catch(right, blue);
                GameEvents.current.Catch(right, green);
                GameEvents.current.Catch(right, blue);
                break;
        }
    }
}
