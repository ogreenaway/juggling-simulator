using UnityEngine;
using UnityEngine.UI;
using VRTK.Controllables;

public class GravitySlider : MonoBehaviour
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
        Physics.gravity = new Vector3(0, -1 * e.value, 0);

        if (displayText != null)
        {
            displayText.text = e.value.ToString("F2");
        }
    }
}