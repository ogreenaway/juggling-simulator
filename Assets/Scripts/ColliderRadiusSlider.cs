using UnityEngine;
using UnityEngine.UI;
using VRTK.Controllables;

public class ColliderRadiusSlider : MonoBehaviour
{
    public VRTK_BaseControllable controllable;
    public Text displayText;
    public GameObject exampleColliderBall;
    public GameObject gameLogic;

    protected virtual void OnEnable()
    {
        controllable = (controllable == null ? GetComponent<VRTK_BaseControllable>() : controllable);
        controllable.ValueChanged += ValueChanged;
    }

    protected virtual void ValueChanged(object sender, ControllableEventArgs e)
    {
        SetColliderRadius(e.value);
        SetExampleColliderRadius(e.value);
        SetText(e.value);
        Debug.Log("ColliderRadiusSlider");
    }

    private void SetColliderRadius(float radius)
    {
        var balls = gameLogic.GetComponent<Balls>().balls;

        foreach (GameObject ball in balls)
        {
            ball.GetComponent<SphereCollider>().radius = radius;
        }
    }

    private void SetExampleColliderRadius(float radius)
    {
        float scale = 2 * radius;
        exampleColliderBall.transform.localScale = new Vector3(scale, 0.01F, scale);
    }

    private void SetText(float scale)
    {
        displayText.text = scale.ToString("F3");
    }
}
