using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PropSizeSliderTest
    {
        [UnityTest]
        public IEnumerator Initial_props_and_example_prop_have_default_value()
        {
            float defaultScale = 0.2f;
            yield return TestUtils.LoadScene();
            GameEvents.current.Launch();

            var props = GameObject.FindGameObjectsWithTag("Prop");
            Assert.Greater(props.Length, 0);

            foreach (GameObject prop in props)
            {
                var scale = prop.transform.localScale;
                Assert.That(Mathf.Approximately(defaultScale, scale.x), "All props have "+defaultScale+" scale in x but received " + scale.x);
                Assert.That(Mathf.Approximately(defaultScale, scale.y), "All props have " + defaultScale + " scale in y but received " + scale.y);
                Assert.That(Mathf.Approximately(defaultScale, scale.z), "All props have " + defaultScale + " scale in z but received " + scale.z);
                Assert.That(Mathf.Approximately(defaultScale, prop.GetComponent<TrailRenderer>().startWidth), "Trail width");
            }

            var examplePropScale = GameObject.Find("Example Ball Size").transform.localScale;
            Assert.That(Mathf.Approximately(defaultScale, examplePropScale.x), "All props have " + defaultScale + " scale in x but received " + examplePropScale.x);
            Assert.That(Mathf.Approximately(defaultScale, examplePropScale.y), "All props have " + defaultScale + " scale in y but received " + examplePropScale.y);
            Assert.That(Mathf.Approximately(defaultScale, examplePropScale.z), "All props have " + defaultScale + " scale in z but received " + examplePropScale.z);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Sets_correct_value_for_all_props_and_example_prop()
        {
            float newScale = 3f;
            yield return TestUtils.LoadScene();

            Object.FindObjectOfType<Props>().SetRadius(newScale);
            GameEvents.current.NumberOfBallsChange(15);
            GameEvents.current.Launch();

            var props = GameObject.FindGameObjectsWithTag("Prop");
            Assert.Greater(props.Length, 0);

            foreach (GameObject prop in props)
            {
                var scale = prop.transform.localScale;
                Assert.That(Mathf.Approximately(newScale, scale.x), "All props have " + newScale + " scale in x but received " + scale.x);
                Assert.That(Mathf.Approximately(newScale, scale.y), "All props have " + newScale + " scale in y but received " + scale.y);
                Assert.That(Mathf.Approximately(newScale, scale.z), "All props have " + newScale + " scale in z but received " + scale.z);
                Assert.That(Mathf.Approximately(newScale, prop.GetComponent<TrailRenderer>().startWidth), "Trail width");
            }

            var examplePropScale = GameObject.Find("Example Ball Size").transform.localScale;
            Assert.That(Mathf.Approximately(newScale, examplePropScale.x), "All props have " + newScale + " scale in x but received " + examplePropScale.x);
            Assert.That(Mathf.Approximately(newScale, examplePropScale.y), "All props have " + newScale + " scale in y but received " + examplePropScale.y);
            Assert.That(Mathf.Approximately(newScale, examplePropScale.z), "All props have " + newScale + " scale in z but received " + examplePropScale.z);

            yield return null;
        }
    }
}
