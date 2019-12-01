using UnityEngine;

public class SettingsSlider : MonoBehaviour
{
    public VRTK.VRTK_ObjectTooltip valueText;
    public GameObject ballSettings;
    public GameObject launcherSettings;

    public void OnChange(float value)
    {
        if(value > 0.5F)
        {
            ballSettings.transform.position = new Vector3(0, 1, 0);
            launcherSettings.transform.position = new Vector3(0, 1, 0);
            valueText.UpdateText("on");
        } else
        {
            ballSettings.transform.position = new Vector3(0, -2, 0);
            launcherSettings.transform.position = new Vector3(0, -2, 0);
            valueText.UpdateText("off");
        }
    }
}
