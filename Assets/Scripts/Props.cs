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
        GameEvents.current.OnThrow += OnThrow;
    }

    private void OnDestroy()
    {
        GameEvents.current.OnPaint -= Paint;
        GameEvents.current.OnThrow -= OnThrow;

    }

    private void OnThrow(uint controllerId, int ballId)
    {
        // Fix order as VRTK appends them to the end of the list 
        for (var i = 0; i < balls.Length; i++)
        {
            balls[i].transform.SetSiblingIndex(i);
        }
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

    private void Paint(int ballIndex, Material material)
    {
        balls[ballIndex].GetComponent<Renderer>().material = material;
        balls[ballIndex].GetComponent<TrailRenderer>().startColor = material.color;
        balls[ballIndex].GetComponent<TrailRenderer>().endColor = material.color;
    }
}
