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
        gameLogic.GetComponent<Customisation>().SetGravity(e.value);

        if (displayText != null)
        {
            displayText.text = e.value.ToString();
        }
    }
}