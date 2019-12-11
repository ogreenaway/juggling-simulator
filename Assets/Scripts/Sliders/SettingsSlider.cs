using UnityEngine;

public class SettingsSlider : MonoBehaviour
{
    public VRTK.VRTK_ObjectTooltip valueText;
    public GameObject fullGameMode;

    public void OnChange(float value)
    {
        if (value > 0.5F)
        {
            fullGameMode.transform.position = new Vector3(0, 1, 0);
            valueText.UpdateText("Full game");
        }
        else
        {
            fullGameMode.transform.position = new Vector3(0, -2, 0);
            valueText.UpdateText("Party mode");
        }
    }
}
