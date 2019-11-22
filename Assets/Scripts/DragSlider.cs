using UnityEngine;
using UnityEngine.UI;
using VRTK.Controllables;

public class DragSlider : MonoBehaviour
{
    public VRTK_BaseControllable controllable;
    public Text displayText;
    public GameObject gameLogic;

    protected virtual void OnEnable()
    {
        controllable = (controllable == null ? GetComponent<VRTK_BaseControllable>() : controllable);
        controllable.ValueChanged += ValueChanged;
    }

    protected virtual void ValueChanged(object sender, ControllableEventArgs e)
    {
        SetDrag(e.value);
        SetText(e.value);
    }

    private void SetDrag(float drag)
    {
        var balls = gameLogic.GetComponent<Balls>().balls;

        foreach (GameObject ball in balls)
        {
            ball.GetComponent<Rigidbody>().drag = drag;
        }
    }

    private void SetText(float scale)
    {
        displayText.text = scale.ToString("F3");
    }
}
