using UnityEngine;

public class DragSlider : MonoBehaviour
{
    public Balls Props;

    public void OnChange(float drag)
    {
        foreach (GameObject ball in Props.balls)
        {
            ball.GetComponent<Rigidbody>().drag = drag;
        }
    }
}
