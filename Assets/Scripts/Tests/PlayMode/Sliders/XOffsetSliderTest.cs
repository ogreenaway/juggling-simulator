using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class XOffsetSliderTest
    {
        [UnityTest]
        public IEnumerator Has_the_correct_default_value()
        {
            yield return TestUtils.LoadScene();
            GameEvents.current.Launch();

            var props = GameObject.FindGameObjectsWithTag("Prop");
            Assert.Greater(props.Length, 0);

            CheckXOffset(-0.4f, props);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Sets_correct_value()
        {
            yield return TestUtils.LoadScene();
            Object.FindObjectOfType<Launcher>().SetBallHorizontalOffset(0.9f);
            GameEvents.current.NumberOfBallsChange(15);
            GameEvents.current.Launch();

            var props = GameObject.FindGameObjectsWithTag("Prop");
            Assert.Greater(props.Length, 0);
            CheckXOffset(-0.9f, props);

            yield return null;
        }

        void CheckXOffset(float expectedOffset, GameObject[] props)
        {
            for(int i = 0; i < props.Length; i++)
            {
                if(i != 0)
                {
                    float offset = props[i].transform.position.x - props[i-1].transform.position.x;

                    if (i % 2 == 0)
                    {
                        offset = offset * -1;
                    }

                    Assert.That(Mathf.Approximately(expectedOffset, offset), "Got " + offset.ToString() + " but expected " + expectedOffset.ToString());
                }
            }
        }
    }
}
