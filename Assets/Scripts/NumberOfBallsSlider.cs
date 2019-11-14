using UnityEngine;
using UnityEngine.UI;
using VRTK.Controllables;

public class NumberOfBallsSlider : MonoBehaviour
{
    public VRTK_BaseControllable controllable;
    public Text displayText;
    public string outputOnMax = "Maximum Reached";
    public string outputOnMin = "Minimum Reached";
    public GameObject gameLogic;

    protected virtual void OnEnable()
    {
        controllable = (controllable == null ? GetComponent<VRTK_BaseControllable>() : controllable);
        controllable.ValueChanged += ValueChanged;
        controllable.MaxLimitReached += MaxLimitReached;
        controllable.MinLimitReached += MinLimitReached;
    }

    protected virtual void ValueChanged(object sender, ControllableEventArgs e)
    {
        gameLogic.GetComponent<MoveProps>().SetNumberOfBalls((int)e.value);
        if (displayText != null)
        {
            displayText.text = e.value.ToString("F1");
        }
    }

    protected virtual void MaxLimitReached(object sender, ControllableEventArgs e)
    {
        if (outputOnMax != "")
        {
            Debug.Log(outputOnMax);
        }
    }

    protected virtual void MinLimitReached(object sender, ControllableEventArgs e)
    {
        if (outputOnMin != "")
        {
            Debug.Log(outputOnMin);
        }
    }
}