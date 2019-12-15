using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using VRTK;

namespace Tests
{
    public class Integration
    {
        private readonly Vector3 defaultBallScale = new Vector3(0.2F, 0.2F, 0.2F);
        private readonly float defaultDrag = 3.3F;
        private readonly float defaultColliderRadius = 0.5F;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return TestUtils.LoadScene();
        }

        

        [UnityTest]
        public IEnumerator If_they_launch_straight_away()
        {
            GameEvents.current.Launch();

            foreach (GameObject prop in GameObject.FindGameObjectsWithTag("Prop"))
            {
                // .toString because of some unknown type error or float inaccuracy 
                Assert.AreEqual(defaultBallScale.ToString(), prop.transform.localScale.ToString(), "All props have default scale");
                Assert.AreEqual(defaultDrag.ToString(), prop.GetComponent<Rigidbody>().drag.ToString(), "All props have default drag");
                Assert.AreEqual(defaultColliderRadius.ToString(), prop.GetComponent<SphereCollider>().radius.ToString(), "All props have default collider radius");
                Assert.AreEqual("0", prop.GetComponent<TrailRenderer>().time.ToString(), "All props have default trail duration");
                Material greenPaint = GameObject.Find("Green paint").GetComponent<Renderer>().sharedMaterial;
                Assert.AreEqual(greenPaint, prop.GetComponent<Renderer>().sharedMaterial, "All props have default material");
            }
            yield return null;
        }
    }
}
