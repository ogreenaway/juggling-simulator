using UnityEngine;
using UnityEngine.UI;
using VRTK.Controllables;

public class SettingsSlider : MonoBehaviour
{
    private VRTK_BaseControllable controllable;
    public VRTK.VRTK_ObjectTooltip valueText;
    public GameObject ballSettings;
    public GameObject launcherSettings;

    private void Start()
    {
        // ballSettings.SetActive(false);
        // launcherSettings.SetActive(false);
    }

    protected virtual void OnEnable()
    {
        controllable = (controllable == null ? this.gameObject.GetComponent<VRTK_BaseControllable>() : controllable);
        controllable.ValueChanged += ValueChanged;
    }

    protected virtual void ValueChanged(object sender, ControllableEventArgs e)
    {
        if(e.value > 0.5F)
        {
            ballSettings.SetActive(true);
            launcherSettings.SetActive(true);
            valueText.UpdateText("shown");
        } else
        {
            ballSettings.SetActive(false);
            launcherSettings.SetActive(false);
            valueText.UpdateText("hidden");
        }
    }
}
