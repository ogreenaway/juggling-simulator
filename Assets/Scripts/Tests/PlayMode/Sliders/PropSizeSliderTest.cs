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
            yield return TestUtils.LoadScene();
            GameEvents.current.Launch();

            var props = GameObject.FindGameObjectsWithTag("Prop");
            Assert.Greater(props.Length, 0);

            foreach (GameObject prop in props)
            {
                Assert.AreEqual(new Vector3(0.2F, 0.2F, 0.2F).ToString(), prop.transform.localScale.ToString(), "All props have default scale");
            }

            Assert.AreEqual(new Vector3(0.2F, 0.2F, 0.2F).ToString(), GameObject.Find("Example Ball Size").transform.localScale.ToString(), "Example prop has default scale");
            yield return null;
        }

        [UnityTest]
        public IEnumerator Sets_correct_value_for_all_props_and_example_prop()
        {
            yield return TestUtils.LoadScene();

            Object.FindObjectOfType<Props>().SetRadius(3f);
            GameEvents.current.NumberOfBallsChange(15);
            GameEvents.current.Launch();

            var props = GameObject.FindGameObjectsWithTag("Prop");
            Assert.Greater(props.Length, 0);

            foreach (GameObject prop in props)
            {
                Assert.AreEqual(new Vector3(3f, 3f, 3f), prop.transform.localScale, "All props have set scale");
            }

            Assert.AreEqual(new Vector3(3f, 3f, 3f).ToString(), GameObject.Find("Example Ball Size").transform.localScale.ToString(), "Example prop has set scale");

            yield return null;
        }
    }
}
