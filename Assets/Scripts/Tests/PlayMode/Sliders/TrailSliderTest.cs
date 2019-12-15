using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TrailSliderTest
    {
        [UnityTest]
        public IEnumerator Initial_props_have_default_value()
        {
            yield return TestUtils.LoadScene();
            GameEvents.current.Launch();

            var props = GameObject.FindGameObjectsWithTag("Prop");
            Assert.Greater(props.Length, 0);

            foreach (GameObject prop in props)
            {
                Assert.AreEqual("0", prop.GetComponent<TrailRenderer>().time.ToString(), "All props have default trail duration");
            }

            yield return null;
        }

        [UnityTest]
        public IEnumerator Sets_correct_value_for_all_props()
        {
            yield return TestUtils.LoadScene();

            Object.FindObjectOfType<Props>().SetTrail(10f);
            GameEvents.current.NumberOfBallsChange(15);
            GameEvents.current.Launch();

            var props = GameObject.FindGameObjectsWithTag("Prop");
            Assert.Greater(props.Length, 0);

            foreach (GameObject prop in props)
            {
                Assert.AreEqual("10", prop.GetComponent<TrailRenderer>().time.ToString(), "All props have set trail duration");

            }

            yield return null;
        }
    }
}
