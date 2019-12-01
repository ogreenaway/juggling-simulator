using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class SiteSwapTest
    {
        readonly uint left = 1;
        readonly uint right = 2;
        readonly int green = 1;
        readonly int blue = 2;
        readonly int red = 3;

        GameObject gameEventsGameObject;
        GameEvents gameEvents;
        GameObject siteSwapsGameObject;
        SiteSwaps siteSwaps;

        private string FormatOutput()
        {
            return string.Join("", siteSwaps.siteSwapList.ToArray());
        }

        [SetUp]
        public void RunsBeforeEveryTest()
        {
            gameEventsGameObject = new GameObject();
            gameEventsGameObject.AddComponent<GameEvents>();
            gameEvents = gameEventsGameObject.GetComponent<GameEvents>();

            siteSwapsGameObject = new GameObject();
            siteSwapsGameObject.AddComponent<SiteSwaps>();
            siteSwaps = siteSwapsGameObject.GetComponent<SiteSwaps>();
        }

        [Test]
        public void Output_3()
        {
            siteSwaps.OnCatch(1, 1);
            siteSwaps.OnCatch(2, 2);
            siteSwaps.OnCatch(1, 3);
            siteSwaps.OnCatch(2, 1);
            siteSwaps.OnCatch(1, 2);
            siteSwaps.OnCatch(2, 3);
            siteSwaps.OnCatch(1, 1);
            siteSwaps.OnCatch(2, 2);
            siteSwaps.OnCatch(1, 3);

            Assert.AreEqual("333333_", FormatOutput());
        }

        [Test]
        public void Output_531()
        {
            siteSwaps.OnCatch(1, 1);
            siteSwaps.OnCatch(2, 2);
            siteSwaps.OnCatch(1, 3);
            siteSwaps.OnCatch(2, 3);
            siteSwaps.OnCatch(1, 2);
            siteSwaps.OnCatch(2, 1);

            Assert.AreEqual("531__", FormatOutput());
        }

        [Test]
        public void Output_423()
        {
            siteSwaps.OnCatch(right, green);
            siteSwaps.OnCatch(left, blue);

            siteSwaps.OnCatch(right, red);
            siteSwaps.OnCatch(right, green);
            siteSwaps.OnCatch(left, red);
            siteSwaps.OnCatch(left, blue);

            siteSwaps.OnCatch(right, red);
            siteSwaps.OnCatch(right, green);
            siteSwaps.OnCatch(left, red);
            siteSwaps.OnCatch(left, blue);

            siteSwaps.OnCatch(right, red);
            siteSwaps.OnCatch(right, green);
            siteSwaps.OnCatch(left, red);
            siteSwaps.OnCatch(left, blue);

            siteSwaps.OnCatch(right, red);
            siteSwaps.OnCatch(right, green);
            siteSwaps.OnCatch(left, red);
            siteSwaps.OnCatch(left, blue);

            Assert.AreEqual("42342342342342342342342_", FormatOutput());
        }

        [Test]
        public void Output_40()
        {
            siteSwaps.OnCatch(right, green);
            siteSwaps.OnCatch(right, blue);
            siteSwaps.OnCatch(right, green);
            siteSwaps.OnCatch(right, blue);
            siteSwaps.OnCatch(right, green);
            siteSwaps.OnCatch(right, blue);
            siteSwaps.OnCatch(right, green);
            siteSwaps.OnCatch(right, blue);
            siteSwaps.OnCatch(right, green);
            siteSwaps.OnCatch(right, blue);

            Assert.AreEqual("4EMPTY4EMPTY4EMPTY4EMPTY4EMPTY4EMPTY4EMPTY4EMPTY_EMPTY_", FormatOutput());
        }
    }
}
