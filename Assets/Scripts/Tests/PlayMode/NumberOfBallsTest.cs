using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using VRTK;

namespace Tests
{
    public class NumberOfBallsTest
    {
        [UnityTest]
        public IEnumerator Has_correct_default_value_and_can_be_updated()
        {
            yield return TestUtils.LoadScene();
            Assert.AreEqual(0, GameObject.FindGameObjectsWithTag("Prop").Length, "At the start there are no props visible");
            Assert.AreEqual("3", GameObject.Find("Number of balls text").GetComponent<VRTK_ObjectTooltip>().displayText, "Number of balls text is the correct default value");

            GameEvents.current.Launch();
            Assert.AreEqual(3, GameObject.FindGameObjectsWithTag("Prop").Length, "Number of props is the correct default value");

            GameEvents.current.NumberOfBallsChange(15);
            GameEvents.current.Launch();
            Assert.AreEqual(15, GameObject.FindGameObjectsWithTag("Prop").Length, "Number of balls should update");

            GameEvents.current.NumberOfBallsChange(1);
            GameEvents.current.Launch();
            Assert.AreEqual(1, GameObject.FindGameObjectsWithTag("Prop").Length, "Number of balls should update");
            yield return null;
        }
    }
}
