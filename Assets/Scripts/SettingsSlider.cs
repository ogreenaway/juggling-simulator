using UnityEngine;
using UnityEngine.UI;
using VRTK.Controllables;

public class SettingsSlider : MonoBehaviour
{
    private VRTK_BaseControllable controllable;
    public VRTK.VRTK_ObjectTooltip valueText;
    public GameObject settings;

    protected virtual void OnEnable()
    {
        controllable = (controllable == null ? this.gameObject.GetComponent<VRTK_BaseControllable>() : controllable);
        controllable.ValueChanged += ValueChanged;
    }

    protected virtual void ValueChanged(object sender, ControllableEventArgs e)
    {
        if(e.value > 0.5F)
        {
            settings.SetActive(true);
            valueText.UpdateText("Shown");
        } else
        {
            settings.SetActive(false);
            valueText.UpdateText("Hidden");
        }
    }
}
