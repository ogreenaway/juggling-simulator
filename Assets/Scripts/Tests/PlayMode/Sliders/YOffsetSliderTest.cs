using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class YOffsetSliderTest
    {
        [UnityTest]
        public IEnumerator Has_the_correct_default_value()
        {
            yield return TestUtils.LoadScene();
            GameEvents.current.Launch();
            yield return null;
            var props = GameObject.FindGameObjectsWithTag("Prop");
            Assert.Greater(props.Length, 0);

            CheckYOffset(0.4f, props);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Sets_correct_value()
        {
            yield return TestUtils.LoadScene();
            Object.FindObjectOfType<Launcher>().SetBallVerticalOffset(0.9f);
            GameEvents.current.NumberOfBallsChange(15);
            GameEvents.current.Launch();

            var props = GameObject.FindGameObjectsWithTag("Prop");
            Assert.Greater(props.Length, 0);
            CheckYOffset(0.9f, props);

            yield return null;
        }

        void CheckYOffset(float expectedOffset, GameObject[] props)
        {
            for (int i = 0; i < props.Length; i++)
            {
                var y = props[i].transform.position.y;
                Assert.That(Mathf.Approximately((i + 1) * expectedOffset, y), "Got " + y.ToString() + " but expected " + expectedOffset.ToString());
            }
        }
    }
}
