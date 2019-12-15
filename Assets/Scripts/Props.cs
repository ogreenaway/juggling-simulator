using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Props : MonoBehaviour
{
    public GameObject[] balls = new GameObject[0];
    public GameObject exampleBallSize;
    public GameObject exampleColliderBall;

    void Start()
    {
        balls = GameObject.FindGameObjectsWithTag("Prop");

        foreach (GameObject ball in balls)
        {
            ball.SetActive(false);
        }

        GameEvents.current.OnPaint += Paint;
    }

    private void OnDestroy()
    {
        GameEvents.current.OnPaint -= Paint;

    }

    public void SetRadius(float radius)
    {
        var scale = new Vector3(radius, radius, radius);

        foreach (GameObject ball in balls)
        {
            ball.transform.localScale = scale;
        }

        exampleBallSize.transform.localScale = scale;
    }

    public void SetDrag(float drag)
    {
        foreach (GameObject ball in balls)
        {
            ball.GetComponent<Rigidbody>().drag = drag;
        }
    }

    public void SetColliderRadius(float radius)
    {
        foreach (GameObject ball in balls)
        {
            ball.GetComponent<SphereCollider>().radius = radius;
        }

        float scale = 2 * radius;
        exampleColliderBall.transform.localScale = new Vector3(scale, 0.01F, scale);
    }

    public void SetTrail(float duration)
    {
        foreach (GameObject ball in balls)
        {
            ball.GetComponent<TrailRenderer>().time = duration;
        }
    }

    private void Paint(int customId, Material material)
    {
        foreach(GameObject ball in balls)
        {
            if(ball.GetComponent<CustomId>().id == customId)
            {
                ball.GetComponent<Renderer>().material = material;
                ball.GetComponent<TrailRenderer>().startColor = material.color;

                var transparentColor = material.color;
                transparentColor.a = 0;
                ball.GetComponent<TrailRenderer>().endColor = transparentColor;
            }
        }
        
    }
}
