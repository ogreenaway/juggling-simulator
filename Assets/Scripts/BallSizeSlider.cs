using UnityEngine;
using UnityEngine.UI;
using VRTK.Controllables;

public class BallSizeSlider : MonoBehaviour
{
    public VRTK_BaseControllable controllable;
    public Text displayText;
    public GameObject exampleBall;
    public GameObject exampleColliderBall;


    protected virtual void OnEnable()
    {
        controllable = (controllable == null ? GetComponent<VRTK_BaseControllable>() : controllable);
        controllable.ValueChanged += ValueChanged;
    }

    protected virtual void ValueChanged(object sender, ControllableEventArgs e)
    {
        SetBallSize(e.value);
        SetExampleBallSize(e.value);
        SetExampleColliderBallSize(e.value);
        SetText(e.value);
        Debug.Log("BallSizeSlider");
    }

    private void SetBallSize(float scale)
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Prop");

        foreach (GameObject ball in balls)
        {
            ball.transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    private void SetExampleBallSize(float scale)
    {
        exampleBall.transform.localScale = new Vector3(scale, scale, scale);
    }

    private void SetExampleColliderBallSize(float scale)
    {
        exampleColliderBall.transform.localScale = new Vector3(scale, scale, scale);
    }

    private void SetText(float scale)
    {
        displayText.text = scale.ToString("F3");
    }
}