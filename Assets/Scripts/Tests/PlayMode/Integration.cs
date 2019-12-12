using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using VRTK;

namespace Tests
{
    public class Integration
    {
        private readonly float defaultGravityFloat = 2.5F;
        private readonly Vector3 defaultGravity = new Vector3(0F, -2.5F, 0F);
        private readonly Vector3 defaultBallScale = new Vector3(0.2F, 0.2F, 0.2F);
        private readonly float defaultDrag = 3.3F;
        private readonly float defaultColliderRadius = 0.5F;
        private readonly string defaultRecord = "3 ball record: 0 catches 0.00s";

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return TestUtils.LoadScene();
        }

        [UnityTest]
        public IEnumerator At_the_start_of_the_game()
        {
            Assert.AreEqual(0, GameObject.FindGameObjectsWithTag("Prop").Length, "Number of props");
            Assert.AreEqual(defaultGravity, Physics.gravity, "gravity");
            Assert.AreEqual("0", GameObject.Find("Current catches").GetComponent<VRTK_ObjectTooltip>().displayText, "Timer catch text");
            Assert.AreEqual("0.0s", GameObject.Find("Current time").GetComponent<VRTK_ObjectTooltip>().displayText, "Timer time text");
            Assert.AreEqual(defaultRecord, GameObject.Find("Current record").GetComponent<VRTK_ObjectTooltip>().displayText, "Timer record text");
            Assert.AreEqual("3", GameObject.Find("Number of balls text").GetComponent<VRTK_ObjectTooltip>().displayText, "Number of balls text");
            Assert.AreEqual("Party mode", GameObject.Find("Game mode text").GetComponent<VRTK_ObjectTooltip>().displayText, "Game mode text");
            Assert.AreEqual(3, GameObject.FindGameObjectsWithTag("FakeProp").Length, "Number of fake props");
            Assert.Less(GameObject.Find("Full game mode").transform.position.y, 0, "The full game section is hidden");
            Material greenPaint = GameObject.Find("Green paint").GetComponent<Renderer>().sharedMaterial;
            Assert.AreEqual(greenPaint, GameObject.Find("Bristles").GetComponent<Renderer>().sharedMaterial, "The paint brush bristles' material is default");

            yield return null;
        }

        [UnityTest]
        public IEnumerator If_they_launch_straight_away()
        {
            GameEvents.current.Launch();
            Assert.AreEqual(3, GameObject.FindGameObjectsWithTag("Prop").Length, "Three props are launched");

            foreach (GameObject prop in GameObject.FindGameObjectsWithTag("Prop"))
            {
                // .toString because of some unknown type error or float inaccuracy 
                Assert.AreEqual(defaultBallScale.ToString(), prop.transform.localScale.ToString(), "All props have default scale");
                Assert.AreEqual(defaultDrag.ToString(), prop.GetComponent<Rigidbody>().drag.ToString(), "All props have default drag");
                Assert.AreEqual(defaultColliderRadius.ToString(), prop.GetComponent<SphereCollider>().radius.ToString(), "All props have default collider radius");
                Assert.AreEqual("0", prop.GetComponent<TrailRenderer>().time.ToString(), "All props have default trail duration");
                Material greenPaint = GameObject.Find("Green paint").GetComponent<Renderer>().sharedMaterial;
                Assert.AreEqual(greenPaint, prop.GetComponent<Renderer>().sharedMaterial, "All props have default material");
            }
            yield return null;
        }

        [UnityTest]
        public IEnumerator Change_number_of_balls()
        {
            GameEvents.current.NumberOfBallsChange(1);
            GameEvents.current.Launch();
            Assert.AreEqual(1, GameObject.FindGameObjectsWithTag("Prop").Length, "Three props are launched");
            GameEvents.current.NumberOfBallsChange(15);
            GameEvents.current.Launch();
            Assert.AreEqual(15, GameObject.FindGameObjectsWithTag("Prop").Length, "Three props are launched");
            yield return null;
        }

        [UnityTest]
        public IEnumerator Painting()
        {
            // Number of fake props
            GameEvents.current.NumberOfBallsChange(1);
            GameEvents.current.Launch();
            Assert.AreEqual(1, GameObject.FindGameObjectsWithTag("FakeProp").Length, "The number of fake props matches the number of balls");
            GameEvents.current.NumberOfBallsChange(3);
            Assert.AreEqual(3, GameObject.FindGameObjectsWithTag("FakeProp").Length, "Changing the number of balls changes the number of fake props instantly");
            GameEvents.current.Launch();

            // Painting
            Material redPaint = GameObject.Find("Red paint").GetComponent<Renderer>().sharedMaterial;
            GameEvents.current.Paint(1, redPaint);
            Assert.AreEqual(redPaint, GameObject.FindGameObjectsWithTag("FakeProp")[1].GetComponent<Renderer>().sharedMaterial, "The correct fake prop is painted");
            Assert.AreEqual(redPaint, GameObject.FindGameObjectsWithTag("Prop")[1].GetComponent<Renderer>().sharedMaterial, "The correct real prop is painted");
            Material greenPaint = GameObject.Find("Green paint").GetComponent<Renderer>().sharedMaterial;
            Assert.AreEqual(greenPaint, GameObject.FindGameObjectsWithTag("FakeProp")[0].GetComponent<Renderer>().sharedMaterial, "Other fake props are uneffected");
            Assert.AreEqual(greenPaint, GameObject.FindGameObjectsWithTag("Prop")[0].GetComponent<Renderer>().sharedMaterial, "Other real props are uneffected");

            // Trails
            Assert.AreEqual(redPaint.color, GameObject.FindGameObjectsWithTag("Prop")[1].GetComponent<TrailRenderer>().startColor, "The correct real prop's trail's start color is painted");
            Assert.AreEqual(redPaint.color, GameObject.FindGameObjectsWithTag("Prop")[1].GetComponent<TrailRenderer>().endColor, "The correct real prop's trail's end color is painted");

            yield return null;
        }
    }
}
