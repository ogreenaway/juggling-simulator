using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using VRTK;

namespace Tests
{
    public class TimerTest
    {
        string GetCurrentCatches() => GameObject.Find("Current catches").GetComponent<VRTK_ObjectTooltip>().displayText;
        string GetCurrentRecord() => GameObject.Find("Current record").GetComponent<VRTK_ObjectTooltip>().displayText.Substring(0, 25);
        string GetCurrentRecordWithTime() => GameObject.Find("Current record").GetComponent<VRTK_ObjectTooltip>().displayText;

        [UnityTest]
        public IEnumerator The_record_count_starts_at_zero()
        {
            yield return TestUtils.LoadScene();
            Assert.AreEqual("0", GetCurrentCatches(), "Catches count is default");
            Assert.AreEqual("3 ball record: 0 catches 0.00s", GetCurrentRecordWithTime(), "Record count is default");
        }

        [UnityTest]
        public IEnumerator A_record_is_set_on_drop()
        {
            yield return TestUtils.LoadScene();
            GameEvents.current.Launch();
            TestUtils.Juggle("3", 2);
            yield return null;
            Assert.AreEqual("11", GetCurrentCatches(), "Catches are counted");
            GameEvents.current.Drop();
            Assert.AreEqual("3 ball record: 11 catches", GetCurrentRecord(), "Record count is set");
        }

        [UnityTest]
        public IEnumerator Record_is_not_set_if_the_number_of_catches_is_lower()
        {
            yield return TestUtils.LoadScene();
            GameEvents.current.Launch();
            TestUtils.Juggle("3", 2);
            yield return null;
            Assert.AreEqual("11", GetCurrentCatches(), "Catches are counted");
            GameEvents.current.Drop();
            Assert.AreEqual("3 ball record: 11 catches", GetCurrentRecord(), "Record count is set");

            GameEvents.current.Launch();
            TestUtils.Juggle("3", 1);
            yield return null;
            Assert.AreEqual("5", GetCurrentCatches(), "Catches are counted");
            GameEvents.current.Drop();
            Assert.AreEqual("3 ball record: 11 catches", GetCurrentRecord(), "Record is not set if the number of catches is lower");
        }

        [UnityTest]
        public IEnumerator A_record_is_set_on_launch()
        {
            yield return TestUtils.LoadScene();
            GameEvents.current.Launch();
            TestUtils.Juggle("3", 2);
            yield return null;
            Assert.AreEqual("11", GetCurrentCatches(), "Catches are counted");
            GameEvents.current.Launch();
            Assert.AreEqual("3 ball record: 11 catches", GetCurrentRecord(), "Record is set if the number of catches is higher when a launch happens");
            yield return null;
        }

        [UnityTest]
        public IEnumerator A_record_is_set_on_number_of_balls_change()
        {
            yield return TestUtils.LoadScene();
            GameEvents.current.Launch();
            TestUtils.Juggle("3", 2);
            yield return null;
            Assert.AreEqual("11", GetCurrentCatches(), "Catches are counted");
            GameEvents.current.NumberOfBallsChange(15);
            Assert.AreEqual("15 ball record: 0 catches", GetCurrentRecord(), "on 15 balls");
            GameEvents.current.NumberOfBallsChange(3);
            Assert.AreEqual("3 ball record: 11 catches", GetCurrentRecord(), "on 3 balls");
            yield return null;
        }
    }
}