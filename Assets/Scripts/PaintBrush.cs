using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBrush : MonoBehaviour
{
    public GameObject[] fakeJugglingBalls = new GameObject[0];
    public GameObject gameLogic;
    public Renderer bristles;

    private void ColourRealAndFakeProps(int index, Material material)
    {
        fakeJugglingBalls[index].GetComponent<Renderer>().material = material;
        gameLogic.GetComponent<Props>().balls[index].GetComponent<Renderer>().material = material;
    }

    private void OnTriggerEnter(Collider collider)
    {
        string tag = collider.gameObject.tag;
        Renderer colliderRenderer = collider.gameObject.GetComponent<Renderer>();

        switch (tag)
        {
            case "Paint":
                bristles.material = colliderRenderer.material;
                break;
            case "FakeProp":
                int fakePropIndex = System.Array.IndexOf(fakeJugglingBalls, collider.gameObject);
                ColourRealAndFakeProps(fakePropIndex, bristles.material);
                break;
            case "Prop":
                int realPropIndex = System.Array.IndexOf(gameLogic.GetComponent<Props>().balls, collider.gameObject);
                ColourRealAndFakeProps(realPropIndex, bristles.material);
                break;
            default:                
                if (colliderRenderer)
                {
                    colliderRenderer.material = bristles.material;
                }                    
                break;
        }
    }

    public void DisplayBalls(int numberOfBalls)
    {
        for (int i = 0; i < fakeJugglingBalls.Length; i++)
        {
            fakeJugglingBalls[i].SetActive(i < numberOfBalls);
        }
    }
}