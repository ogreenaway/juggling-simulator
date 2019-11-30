using UnityEngine;

public class ColliderRadiusSlider : MonoBehaviour
{
    public GameObject exampleColliderBall;
    public Balls props;

    public void OnChange(float radius)
    {
        foreach (GameObject ball in props.balls)
        {
            ball.GetComponent<SphereCollider>().radius = radius;
        }

        float scale = 2 * radius;
        exampleColliderBall.transform.localScale = new Vector3(scale, 0.01F, scale);
    }
}
