using UnityEngine;
using UnityEngine.UI;
using VRTK.Controllables;

public class NumberOfBallsSlider : MonoBehaviour
{
    private VRTK_BaseControllable controllable;
    public VRTK.VRTK_ObjectTooltip valueText;
    public Customisation customisation;
    public PaintBrush paintBrush;

    protected virtual void OnEnable()
    {
        controllable = (controllable == null ? this.gameObject.GetComponent<VRTK_BaseControllable>() : controllable);
        controllable.ValueChanged += ValueChanged;
    }

    protected virtual void ValueChanged(object sender, ControllableEventArgs e)
    {
        customisation.SetNumberOfBalls((int)e.value);
        valueText.UpdateText(e.value.ToString("F0"));
        paintBrush.DisplayBalls((int)e.value);
    }
}