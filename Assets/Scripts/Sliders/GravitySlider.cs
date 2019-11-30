using UnityEngine;

public class GravitySlider : MonoBehaviour
{
    public void OnChange(float gravity)
    {
        Physics.gravity = new Vector3(0, -1 * gravity, 0);
    }
}