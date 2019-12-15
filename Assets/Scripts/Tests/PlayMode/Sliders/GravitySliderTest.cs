using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GravitySliderTest
    {
        [UnityTest]
        public IEnumerator Has_correct_default_value_and_can_be_updated()
        {
            yield return TestUtils.LoadScene();
            Assert.AreEqual(new Vector3(0, -2.4f, 0), Physics.gravity, "Gravity is the correct default value");

            GravitySlider gravitySlider = GameObject.FindObjectOfType<GravitySlider>();
            float newGravity = 9.8f;

            gravitySlider.OnChange(newGravity);
            Assert.AreEqual(new Vector3(0, -newGravity, 0), Physics.gravity, "GravitySlider should set the gravity");
            yield return null;
        }
    }
}
