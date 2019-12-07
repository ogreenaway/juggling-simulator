using System.Collections;
using UnityEngine;

public class IntegrationTests : MonoBehaviour
{
    public bool runTests = true;

    [Header("Sliders")]
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

    private readonly float defaultGravityFloat = 2.5F;
    private readonly Vector3 defaultGravity = new Vector3(0F, -2.5F, 0F);
    private readonly Vector3 defaultBallScale = new Vector3(0.2F, 0.2F, 0.2F);
    private readonly float defaultDrag = 3.3F;
    private readonly float defaultColliderRadius = 0.5F;


    void Start()
    {
        if (runTests)
        {
            StartCoroutine(TestRunner());
        }
    }

    IEnumerator TestRunner()
    {
        // Wait for scene to load
        yield return null;
        yield return null;
        // yield return new WaitForSeconds(1f);
        TestInitialisation();
        TestInitialProps();
        TestGravitySlider();

        yield return null;
    }

    private void TestInitialisation()
    {
        Info("At the start");
        Test("No props are visible", 0, GameObject.FindGameObjectsWithTag("Prop").Length);
        Test("Gravity is default", defaultGravity, Physics.gravity);
        Test("Timer catch count is default", "0", TimerCatchText.displayText);
        Test("Timer time count is default", "0.0s", TimerTimeText.displayText);
        Test("Timer record count is default", "3 ball record: 0 catches 0.00s", TimerRecordText.displayText);
        Test("Number of balls display text is default", "3", numberOfBallsText.displayText);
        Test("Game mode display text is default", "party mode", gameModeText.displayText);
        Test("The number of fake props is default", 3, GameObject.FindGameObjectsWithTag("FakeProp").Length);
    }

    private void TestInitialProps()
    {
        Info("Initial props");
        GameEvents.current.Launch();
        Test("Three props are launched", 3, GameObject.FindGameObjectsWithTag("Prop").Length);

        foreach(GameObject prop in GameObject.FindGameObjectsWithTag("Prop"))
        {
            // .toString because of some type error or float inaccuracy 
            Test("All props have default scale", defaultBallScale.ToString(), prop.transform.localScale.ToString());
            Test("All props have default drag", defaultDrag.ToString(), prop.GetComponent<Rigidbody>().drag.ToString());
            Test("All props have default collider radius", defaultColliderRadius.ToString(), prop.GetComponent<SphereCollider>().radius.ToString());
            Test("All props have default trail duration", 0.ToString(), prop.GetComponent<TrailRenderer>().time.ToString());
            Test("All props have default material", defaultMaterial, prop.GetComponent<Renderer>().sharedMaterial);
        }
    }

    private void TestGravitySlider()
    {
        Info("Gravity slider");
        var newGravity = 9.8f;
        Test("Before the gravity test it is the default", defaultGravity, Physics.gravity);
        gravitySlider.OnChange(newGravity);
        Test("GravitySlider should set the gravity", new Vector3(0, -newGravity, 0), Physics.gravity);
        gravitySlider.OnChange(defaultGravityFloat);
        Test("After the gravity test it is the default", defaultGravity, Physics.gravity);
    }

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
}
