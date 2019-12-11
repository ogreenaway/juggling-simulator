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

        TestInitialisation();
        TestInitialProps();
        TestChangingNumberOfBalls();
        TestPainting();
        yield return StartCoroutine(TestTimer());

        // Sliders
        TestGameModeSlider();
        TestGravitySlider();

        yield return null;
    }

    private IEnumerator TestTimer()
    {
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

    private void TestInitialisation()
    {
        Info("At the start");
        Test("No props are visible", 0, GameObject.FindGameObjectsWithTag("Prop").Length);
        Test("Gravity is default", defaultGravity, Physics.gravity);
        Test("Timer catch count is default", "0", TimerCatchText.displayText);
        Test("Timer time count is default", "0.0s", TimerTimeText.displayText);
        Test("Timer record count is default", defaultRecord, TimerRecordText.displayText);
        Test("Number of balls display text is default", "3", numberOfBallsText.displayText);
        Test("Game mode display text is default", "Party mode", gameModeText.displayText);
        Test("The number of fake props is default", 3, GameObject.FindGameObjectsWithTag("FakeProp").Length);
        Test("The full game section is hidden", -2, fullGameSection.transform.position.y);
        Test("The paint brush bristles' material is default", defaultMaterial, paintBrushBristles.sharedMaterial);
    }

    private void TestInitialProps()
    {
        Info("Initial props");
        GameEvents.current.Launch();
        Test("Three props are launched", 3, GameObject.FindGameObjectsWithTag("Prop").Length);

        foreach(GameObject prop in GameObject.FindGameObjectsWithTag("Prop"))
        {
            // .toString because of some unknown type error or float inaccuracy 
            Test("All props have default scale", defaultBallScale.ToString(), prop.transform.localScale.ToString());
            Test("All props have default drag", defaultDrag.ToString(), prop.GetComponent<Rigidbody>().drag.ToString());
            Test("All props have default collider radius", defaultColliderRadius.ToString(), prop.GetComponent<SphereCollider>().radius.ToString());
            Test("All props have default trail duration", 0.ToString(), prop.GetComponent<TrailRenderer>().time.ToString());
            Test("All props have default material", defaultMaterial, prop.GetComponent<Renderer>().sharedMaterial);
        }
    }

    private void TestChangingNumberOfBalls()
    {
        Info("Changing number of balls");
        GameEvents.current.NumberOfBallsChange(3);
        GameEvents.current.Launch();
        Test("Three props are launched", 3, GameObject.FindGameObjectsWithTag("Prop").Length);
        GameEvents.current.NumberOfBallsChange(4);
        GameEvents.current.Launch();
        Test("Three props are launched", 4, GameObject.FindGameObjectsWithTag("Prop").Length);
    }

    private void TestPainting()
    {
        Info("Painting");
        GameEvents.current.NumberOfBallsChange(1);
        GameEvents.current.Launch();
        Test("The number of fake props matches the number of balls", 1, GameObject.FindGameObjectsWithTag("FakeProp").Length);
        GameEvents.current.NumberOfBallsChange(3);
        Test("Changing the number of balls changes the number of fake props instantly", 3, GameObject.FindGameObjectsWithTag("FakeProp").Length);
        GameEvents.current.Launch();
        GameEvents.current.Paint(1, redMaterial);
        Test("The correct fake prop is painted", redMaterial, GameObject.FindGameObjectsWithTag("FakeProp")[1].GetComponent<Renderer>().sharedMaterial);
        Test("The correct real prop is painted", redMaterial, GameObject.FindGameObjectsWithTag("Prop")[1].GetComponent<Renderer>().sharedMaterial);
        Test("Other fake props are uneffected", defaultMaterial, GameObject.FindGameObjectsWithTag("FakeProp")[0].GetComponent<Renderer>().sharedMaterial);
        Test("Other real props are uneffected", defaultMaterial, GameObject.FindGameObjectsWithTag("Prop")[0].GetComponent<Renderer>().sharedMaterial);
        Test("The correct real prop's trail is painted", redMaterial.color, GameObject.FindGameObjectsWithTag("Prop")[1].GetComponent<TrailRenderer>().startColor);
        Test("The correct real prop's trail is painted", redMaterial.color, GameObject.FindGameObjectsWithTag("Prop")[1].GetComponent<TrailRenderer>().endColor);
    }

    private void TestGameModeSlider()
    {
        Info("Game mode slider");
        gameModeSlider.OnChange(1);
        Test("If value is 1, the full game section is visible", 1, fullGameSection.transform.position.y);
        Test("If value is 1, the text is 'Full Game'", "Full game", gameModeText.displayText);
        gameModeSlider.OnChange(0);
        Test("If value is 0, the full game section is hidden", -2, fullGameSection.transform.position.y);
        Test("If value is 0, the text is 'Party mode'", "Party mode", gameModeText.displayText);
    }

    private void TestGravitySlider()
    {
        Info("Gravity slider");
        var newGravity = 9.8f;
        gravitySlider.OnChange(newGravity);
        Test("GravitySlider should set the gravity", new Vector3(0, -newGravity, 0), Physics.gravity);
        gravitySlider.OnChange(defaultGravityFloat);
        Test("After the gravity test it is the default", defaultGravity, Physics.gravity);
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
