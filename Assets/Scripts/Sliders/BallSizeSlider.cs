using UnityEngine;

public class BallSizeSlider : MonoBehaviour
{
    public GameObject exampleColliderBall;
    public Balls props;

    public void OnChange(float radius)
    {
        var scale = new Vector3(radius, radius, radius);

        foreach (GameObject ball in props.balls)
        {
            ball.transform.localScale = scale;
        }

        exampleColliderBall.transform.localScale = scale;
    }
}