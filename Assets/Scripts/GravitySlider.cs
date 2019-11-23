using UnityEngine;
using UnityEngine.UI;
using VRTK.Controllables;

public class GravitySlider : MonoBehaviour
{
    private VRTK_BaseControllable controllable;
    public Text displayText;

    protected virtual void OnEnable()
    {
        controllable = (controllable == null ? this.gameObject.GetComponent<VRTK_BaseControllable>() : controllable);
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