using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using VRTK;

namespace Tests
{
    public class NewTestScript
    {
        private readonly float defaultGravityFloat = 2.5F;
        private readonly Vector3 defaultGravity = new Vector3(0F, -2.5F, 0F);
        private readonly Vector3 defaultBallScale = new Vector3(0.2F, 0.2F, 0.2F);
        private readonly float defaultDrag = 3.3F;
        private readonly float defaultColliderRadius = 0.5F;
        private readonly string defaultRecord = "3 ball record: 0 catches 0.00s";

        //IEnumerator LoadLevel()
        //{
        //    AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("Virtual Juggling", LoadSceneMode.Single);
        //    while (!asyncLoadLevel.isDone)
        //    {
        //        // yield return null;
        //        yield return new WaitForSeconds(0.1f);
        //    }
        //}

        public IEnumerator LoadScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Virtual Juggling");
            yield return new WaitForSeconds(0.1f);
        }

        [UnityTest]
        public IEnumerator at_the_start()
        {
            yield return LoadScene();
            Assert.AreEqual(0, GameObject.FindGameObjectsWithTag("Prop").Length, "Number of props");
            Assert.AreEqual(defaultGravity, Physics.gravity, "gravity");
            Assert.AreEqual("0", GameObject.Find("Current catches").GetComponent<VRTK.VRTK_ObjectTooltip>().displayText, "Timer catch text");
            Assert.AreEqual("0.0s", GameObject.Find("Current time").GetComponent<VRTK.VRTK_ObjectTooltip>().displayText, "Timer time text");
            Assert.AreEqual(defaultRecord, GameObject.Find("Current record").GetComponent<VRTK.VRTK_ObjectTooltip>().displayText, "Timer record text");
            Assert.AreEqual("3", GameObject.Find("Number of balls text").GetComponent<VRTK.VRTK_ObjectTooltip>().displayText, "Number of balls text");
            Assert.AreEqual("Party mode", GameObject.Find("Game mode text").GetComponent<VRTK.VRTK_ObjectTooltip>().displayText, "Game mode text");
            Assert.AreEqual(3, GameObject.FindGameObjectsWithTag("FakeProp").Length, "Number of fake props");
            Assert.Less(GameObject.Find("Full game mode").transform.position.y, 0, "The full game section is hidden");
            Material greenPaint = GameObject.Find("Green paint").GetComponent<Renderer>().sharedMaterial;
            Assert.AreEqual(greenPaint, GameObject.Find("Bristles").GetComponent<Renderer>().sharedMaterial, "The paint brush bristles' material is default");
            
            yield return null;
        }

        [UnityTest]
        public IEnumerator initial_props()
        {
            yield return LoadScene();
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
        public IEnumerator change_number_of_balls()
        {
            yield return LoadScene();
            GameEvents.current.NumberOfBallsChange(3);
            GameEvents.current.Launch();
            Assert.AreEqual(3, GameObject.FindGameObjectsWithTag("Prop").Length, "Three props are launched");
            GameEvents.current.NumberOfBallsChange(4);
            GameEvents.current.Launch();
            Assert.AreEqual(4, GameObject.FindGameObjectsWithTag("Prop").Length, "Three props are launched");
            yield return null;
        }
    }
}
