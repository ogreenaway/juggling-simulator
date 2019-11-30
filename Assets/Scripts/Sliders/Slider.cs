using UnityEngine;
using UnityEngine.Events;
using VRTK.Controllables;

[System.Serializable]
public class UnityEventWithFloat : UnityEvent<float> { }

public class Slider : MonoBehaviour
{
    public VRTK_BaseControllable controllable;
    public VRTK.VRTK_ObjectTooltip valueText;
    public UnityEventWithFloat onChange;
    public string textFormat = "F2";

    protected virtual void OnEnable()
    {
        controllable.ValueChanged += ValueChanged;
    }

    protected virtual void ValueChanged(object sender, ControllableEventArgs e)
    {
        onChange.Invoke(e.value);
        valueText.UpdateText(e.value.ToString(textFormat));
    }
}