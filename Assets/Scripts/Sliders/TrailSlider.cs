using UnityEngine;

public class TrailSlider : MonoBehaviour
{
    public Balls props;

    public void OnChange(float duration)
    {

        foreach (GameObject ball in props.balls)
        {
            ball.GetComponent<TrailRenderer>().time = duration;
        }
    }
}
