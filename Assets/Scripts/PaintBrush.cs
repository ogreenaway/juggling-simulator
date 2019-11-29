using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBrush : MonoBehaviour
{
    public GameObject[] fakeJugglingBalls = new GameObject[0];
    public GameObject gameLogic;
    public Renderer bristles;

    void Start()
    {
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("BOOM 2!");

        if (collider.gameObject.tag == "Paint")
        {
            bristles.material = collider.gameObject.GetComponent<Renderer>().material;
        }

        if (collider.gameObject.tag == "FakeProp")
        {
            int index = System.Array.IndexOf(fakeJugglingBalls, collider.gameObject);
            Debug.Log("index: " + index);
            Material material = bristles.material;

            collider.GetComponent<Renderer>().material = material;
            gameLogic.GetComponent<Balls>().balls[index].GetComponent<Renderer>().material = material;
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
