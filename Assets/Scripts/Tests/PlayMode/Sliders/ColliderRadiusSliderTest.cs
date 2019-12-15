using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ColliderRadiusSliderTest
    {
        [UnityTest]
        public IEnumerator Initial_props_and_example_prop_have_default_value()
        {
            yield return TestUtils.LoadScene();
            GameEvents.current.Launch();

            var props = GameObject.FindGameObjectsWithTag("Prop");
            Assert.Greater(props.Length, 0);

            foreach (GameObject prop in props)
            {
                Assert.AreEqual(0.5f.ToString(), prop.GetComponent<SphereCollider>().radius.ToString(), "All props have default collider radius");
            }

            Assert.AreEqual(new Vector3(1f, 0.01f, 1f).ToString(), GameObject.Find("Example Collider Size").transform.localScale.ToString(), "Example prop has default scale");
            yield return null;
        }

        [UnityTest]
        public IEnumerator Sets_correct_value_for_all_props_and_example_prop()
        {
            yield return TestUtils.LoadScene();

            Object.FindObjectOfType<Props>().SetColliderRadius(1f);
            GameEvents.current.NumberOfBallsChange(15);
            GameEvents.current.Launch();

            var props = GameObject.FindGameObjectsWithTag("Prop");
            Assert.Greater(props.Length, 0);

            foreach (GameObject prop in props)
            {
                Assert.AreEqual(1f.ToString(), prop.GetComponent<SphereCollider>().radius.ToString(), "All props have set collider radius");
            }

            Assert.AreEqual(new Vector3(2f, 0.01f, 2f).ToString(), GameObject.Find("Example Collider Size").transform.localScale.ToString(), "Example prop has set scale");

            yield return null;
        }
    }
}
