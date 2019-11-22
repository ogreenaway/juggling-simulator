using UnityEngine;
using UnityEngine.UI;
using VRTK.Controllables;

public class xOffsetSlider : MonoBehaviour
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
        gameLogic.GetComponent<MoveProps>().SetBallHorizontalOffset(e.value);
        SetText(e.value);
    }

    private void SetText(float scale)
    {
        displayText.text = scale.ToString("F2");
    }
}
