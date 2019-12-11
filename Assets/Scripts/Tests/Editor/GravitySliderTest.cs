using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GravitySliderTest
    {
        private readonly float defaultGravity = 2.5F;

        [UnityTest]
        public IEnumerator Changes_the_strength_of_gravity()
        {
            GravitySlider gravitySlider = GameObject.Find("Gravity Slider").GetComponent<GravitySlider>();
            float newGravity = 9.8f;

            gravitySlider.OnChange(newGravity);
            Assert.AreEqual(new Vector3(0, -newGravity, 0), Physics.gravity, "GravitySlider should set the gravity");

            gravitySlider.OnChange(defaultGravity);
            Assert.AreEqual(new Vector3(0, -defaultGravity, 0), Physics.gravity, "After the gravity test it is the default");

            yield return null;
        }
    }
}