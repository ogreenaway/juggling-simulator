using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PaintingTest
    {
        [UnityTest]
        public IEnumerator The_brush_has_the_correct_default_material()
        {
            yield return TestUtils.LoadScene();
            Material greenPaint = GameObject.Find("Green paint").GetComponent<Renderer>().sharedMaterial;
            Assert.AreEqual(greenPaint, GameObject.Find("Bristles").GetComponent<Renderer>().sharedMaterial, "The paint brush bristles' material is the correct default value");
        }

        [UnityTest]
        public IEnumerator Props_have_correct_default_materials_and_trails()
        {
            yield return TestUtils.LoadScene();
            Material greenPaint = GameObject.Find("Green paint").GetComponent<Renderer>().sharedMaterial;
            Material transparentGreenPaint = GameObject.Find("Green paint").GetComponent<Renderer>().sharedMaterial;
            var transparentGreenColor = transparentGreenPaint.color;
            transparentGreenColor.a = 0;

            GameEvents.current.NumberOfBallsChange(15);
            GameEvents.current.Launch();

            var props = GameObject.FindGameObjectsWithTag("Prop");
            Assert.Greater(props.Length, 0);

            foreach (GameObject prop in props)
            {
                Assert.AreEqual(greenPaint, prop.GetComponent<Renderer>().sharedMaterial, "All props have default material");
                Assert.AreEqual(greenPaint.color.ToString(), prop.GetComponent<TrailRenderer>().startColor.ToString(), "All props have default trail start colour");
                Assert.AreEqual(transparentGreenColor.ToString(), prop.GetComponent<TrailRenderer>().endColor.ToString(), "All props have default trail start endColor");
            }
        }

        [UnityTest]
        public IEnumerator Has_correct_number_of_fake_props()
        {
            yield return TestUtils.LoadScene();
            Assert.AreEqual(3, GameObject.FindGameObjectsWithTag("FakeProp").Length, "Number of fake props is the correct default value");

            GameEvents.current.NumberOfBallsChange(1);
            //GameEvents.current.Launch();
            Assert.AreEqual(1, GameObject.FindGameObjectsWithTag("FakeProp").Length, "Changing the number of balls changes the number of fake props instantly");
            GameEvents.current.NumberOfBallsChange(15);
            Assert.AreEqual(15, GameObject.FindGameObjectsWithTag("FakeProp").Length, "Changing the number of balls changes the number of fake props instantly");
         }

        [UnityTest]
        public IEnumerator Painting_changes_the_material_and_trail_colour()
        {
            yield return TestUtils.LoadScene();
            Material greenPaint = GameObject.Find("Green paint").GetComponent<Renderer>().sharedMaterial;
            var transparentGreenColor = greenPaint.color;
            transparentGreenColor.a = 0;
            Material redPaint = GameObject.Find("Red paint").GetComponent<Renderer>().sharedMaterial;
            var transparentRedColor = redPaint.color;
            transparentRedColor.a = 0;

            GameEvents.current.NumberOfBallsChange(15);
            GameEvents.current.Launch();

            GameEvents.current.Paint(1, redPaint);
            var props = GameObject.FindGameObjectsWithTag("Prop");
            Assert.Greater(props.Length, 0);

            foreach (GameObject prop in props)
            {
                if(prop.GetComponent<CustomId>().id == 1)
                {
                    Assert.AreEqual(redPaint, prop.GetComponent<Renderer>().sharedMaterial, "The correct real prop is painted");
                    Assert.AreEqual(redPaint.color, prop.GetComponent<TrailRenderer>().startColor, "The correct real prop's trail's start color is painted");
                    Assert.AreEqual(transparentRedColor, prop.GetComponent<TrailRenderer>().endColor, "The correct real prop's trail's end color is painted");
                }
                else
                {
                    Assert.AreEqual(greenPaint, prop.GetComponent<Renderer>().sharedMaterial, "Other real props's materials are uneffected");
                    Assert.AreEqual(greenPaint.color.ToString(), prop.GetComponent<TrailRenderer>().startColor.ToString(), "Other real prop's trail's start color are uneffected");
                    Assert.AreEqual(transparentGreenColor.ToString(), prop.GetComponent<TrailRenderer>().endColor.ToString(), "Other real prop's trail's end color are uneffected");
                }
            }

            var fakeProps = GameObject.FindGameObjectsWithTag("FakeProp");
            Assert.Greater(fakeProps.Length, 0);

            foreach (GameObject fakeProp in fakeProps)
            {
                if (fakeProp.GetComponent<CustomId>().id == 1)
                {
                    Assert.AreEqual(redPaint, fakeProp.GetComponent<Renderer>().sharedMaterial, "The correct fake prop is painted");
                }
                else
                {
                    Assert.AreEqual(greenPaint, fakeProp.GetComponent<Renderer>().sharedMaterial, "Other fake props are uneffected");
                }
            }
            
            yield return null;
        }
    }
}
