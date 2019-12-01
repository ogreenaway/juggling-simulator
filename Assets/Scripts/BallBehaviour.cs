using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Prop")
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }

        if (collision.gameObject.tag == "Floor")
        {
            GameEvents.current.Drop();
        }
    }
}
