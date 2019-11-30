using UnityEngine;

public class NumberOfBallsSlider : MonoBehaviour
{
    public Customisation customisation;
    public PaintBrush paintBrush;

    public void OnChange(float value)
    {
        customisation.SetNumberOfBalls((int)value);
        paintBrush.DisplayBalls((int)value);
    }
}