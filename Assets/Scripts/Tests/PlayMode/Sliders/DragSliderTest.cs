using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class DragSliderTest
    {
        [UnityTest]
        public IEnumerator Has_correct_default_value()
        {
            yield return TestUtils.LoadScene();

            GameEvents.current.Launch();

            var props = GameObject.FindGameObjectsWithTag("Prop");
            Assert.Greater(props.Length, 0);
            foreach (GameObject prop in props)
            {
                Assert.That(Mathf.Approximately(4f, prop.GetComponent<Rigidbody>().drag), "All props have default drag");
            }

            yield return null;
        }

        [UnityTest]
        public IEnumerator Sets_correct_value_for_all_props()
        {
            yield return TestUtils.LoadScene();

            Object.FindObjectOfType<Props>().SetDrag(1f);
            GameEvents.current.NumberOfBallsChange(15);
            GameEvents.current.Launch();

            var props = GameObject.FindGameObjectsWithTag("Prop");
            Assert.Greater(props.Length, 0);
            foreach (GameObject prop in props)
            {
                Assert.That(Mathf.Approximately(1f, prop.GetComponent<Rigidbody>().drag), "All props have default drag");
            }
            
            yield return null;
        }
    }
}
