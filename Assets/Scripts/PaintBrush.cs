using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBrush : MonoBehaviour
{
    public GameObject[] fakeJugglingBalls = new GameObject[0];
    public Renderer bristles;

    private void Start()
    {
        fakeJugglingBalls = GameObject.FindGameObjectsWithTag("FakeProp");
        GameEvents.current.OnNumberOfBallsChange += DisplayBalls;
        GameEvents.current.OnPaint += Paint;
    }

    private void OnDestroy()
    {
        GameEvents.current.OnNumberOfBallsChange -= DisplayBalls;
        GameEvents.current.OnPaint -= Paint;
    }

    private void Paint(int ballIndex, Material material)
    {
        fakeJugglingBalls[ballIndex].GetComponent<Renderer>().material = material;
    }

    private void OnTriggerEnter(Collider collider)
    {
        Renderer colliderRenderer = collider.gameObject.GetComponent<Renderer>();

        switch (collider.gameObject.tag)
        {
            case "Paint":
                bristles.material = colliderRenderer.material;
                break;
            case "FakeProp":
                GameEvents.current.Paint(collider.gameObject.transform.GetSiblingIndex(), bristles.material);
                break;
            case "Prop":
                GameEvents.current.Paint(collider.gameObject.transform.GetSiblingIndex(), bristles.material);
                break;
            default:                
                if (colliderRenderer)
                {
                    colliderRenderer.material = bristles.material;
                }                    
                break;
        }
    }

    private void DisplayBalls(int numberOfBalls)
    {
        for (int i = 0; i < fakeJugglingBalls.Length; i++)
        {
            fakeJugglingBalls[i].SetActive(i < numberOfBalls);
        }
    }
}