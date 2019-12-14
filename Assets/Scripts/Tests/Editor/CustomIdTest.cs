using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class CustomIdTest
    {
        private readonly float defaultGravity = 2.5F;

        [UnityTest]
        public IEnumerator All_the_props_have_a_custom_id()
        {
            GameObject[] props = GameObject.FindGameObjectsWithTag("Prop");

            for(int i = 0; i<15;i++)
            {
                Assert.AreEqual(i, props[i].GetComponent<CustomId>().id);
            }

            yield return null;
        }

        [UnityTest]
        public IEnumerator All_the_fake_props_have_a_custom_id()
        {
            GameObject[] fakeProps = GameObject.FindGameObjectsWithTag("FakeProp");

            for (int i = 0; i < 15; i++)
            {
                Assert.AreEqual(i, fakeProps[i].GetComponent<CustomId>().id);
            }

            yield return null;
        }
    }
}