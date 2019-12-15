using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PaintingTest
    {
        [UnityTest]
        public IEnumerator Has_correct_default_value_and_can_be_updated()
        {
            yield return TestUtils.LoadScene();
            Material greenPaint = GameObject.Find("Green paint").GetComponent<Renderer>().sharedMaterial;
            Material redPaint = GameObject.Find("Red paint").GetComponent<Renderer>().sharedMaterial;

            // At the start of the game
            Assert.AreEqual(3, GameObject.FindGameObjectsWithTag("FakeProp").Length, "Number of fake props is the correct default value");
            Assert.AreEqual(greenPaint, GameObject.Find("Bristles").GetComponent<Renderer>().sharedMaterial, "The paint brush bristles' material is the correct default value");

            // Number of fake props
            GameEvents.current.NumberOfBallsChange(1);
            GameEvents.current.Launch();
            Assert.AreEqual(1, GameObject.FindGameObjectsWithTag("FakeProp").Length, "Changing the number of balls changes the number of fake props instantly");
            GameEvents.current.NumberOfBallsChange(15);
            Assert.AreEqual(15, GameObject.FindGameObjectsWithTag("FakeProp").Length, "Changing the number of balls changes the number of fake props instantly");
            GameEvents.current.Launch();

            // Painting
            GameEvents.current.Paint(1, redPaint);
            Assert.AreEqual(redPaint, GameObject.FindGameObjectsWithTag("FakeProp")[1].GetComponent<Renderer>().sharedMaterial, "The correct fake prop is painted");
            Assert.AreEqual(redPaint, GameObject.FindGameObjectsWithTag("Prop")[1].GetComponent<Renderer>().sharedMaterial, "The correct real prop is painted");
            Assert.AreEqual(greenPaint, GameObject.FindGameObjectsWithTag("FakeProp")[0].GetComponent<Renderer>().sharedMaterial, "Other fake props are uneffected");
            Assert.AreEqual(greenPaint, GameObject.FindGameObjectsWithTag("Prop")[0].GetComponent<Renderer>().sharedMaterial, "Other real props are uneffected");

            // Trails
            Assert.AreEqual(redPaint.color, GameObject.FindGameObjectsWithTag("Prop")[1].GetComponent<TrailRenderer>().startColor, "The correct real prop's trail's start color is painted");
            Assert.AreEqual(redPaint.color, GameObject.FindGameObjectsWithTag("Prop")[1].GetComponent<TrailRenderer>().endColor, "The correct real prop's trail's end color is painted");

            yield return null;
        }
    }
}
