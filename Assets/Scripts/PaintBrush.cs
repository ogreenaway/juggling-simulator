using UnityEngine;

public class PaintBrush : MonoBehaviour
{
    public Renderer bristles;
    private GameObject[] fakeProps;

    private void Start()
    {
        fakeProps = GameObject.FindGameObjectsWithTag("FakeProp");
        GameEvents.current.OnNumberOfBallsChange += DisplayBalls;
        GameEvents.current.OnPaint += Paint;
    }

    private void OnDestroy()
    {
        GameEvents.current.OnNumberOfBallsChange -= DisplayBalls;
        GameEvents.current.OnPaint -= Paint;
    }

    private void Paint(int customId, Material material)
    {
        foreach(GameObject fakeProp in fakeProps)
        {
            if (fakeProp.GetComponent<CustomId>().id == customId)
            {
                fakeProp.GetComponent<Renderer>().material = material;
            }
        }        
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
            case "Prop":
                GameEvents.current.Paint(collider.gameObject.GetComponent<CustomId>().id, bristles.material);
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
        foreach(GameObject fakeProp in fakeProps)
        {
            fakeProp.SetActive(fakeProp.GetComponent<CustomId>().id < numberOfBalls);
        }
    }
}