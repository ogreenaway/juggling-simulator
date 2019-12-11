using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class SiteSwapCreatorTest
    {
        uint left = 1;
        uint right = 2;
        int green = 1;
        int blue = 2;
        int red = 3;

        SiteSwapCreator siteSwapCreator = new SiteSwapCreator();

        [SetUp]
        public void RunsBeforeTests()
        {
            siteSwapCreator.Reset();
        }

        [Test]
        public void creates_siteswap_for_3()
        {
            siteSwapCreator.OnCatch(left, green);
            siteSwapCreator.OnCatch(right, blue);
            siteSwapCreator.OnCatch(left, red);
            siteSwapCreator.OnCatch(right, green);
            siteSwapCreator.OnCatch(left, blue);
            siteSwapCreator.OnCatch(right, red);
            siteSwapCreator.OnCatch(left, green);
            siteSwapCreator.OnCatch(right, blue);

            Assert.AreEqual("33333", siteSwapCreator.GetSiteSwap());
        }

        [Test]
        public void creates_siteswap_for_423()
        {
            // First two are caught
            siteSwapCreator.OnCatch(right, green);
            siteSwapCreator.OnCatch(left, blue);

            siteSwapCreator.OnCatch(right, red);
            siteSwapCreator.OnCatch(right, green);
            siteSwapCreator.OnCatch(left, red);
            siteSwapCreator.OnCatch(left, blue);

            siteSwapCreator.OnCatch(right, red);
            siteSwapCreator.OnCatch(right, green);
            siteSwapCreator.OnCatch(left, red);
            siteSwapCreator.OnCatch(left, blue);

            siteSwapCreator.OnCatch(right, red);
            siteSwapCreator.OnCatch(right, green);
            siteSwapCreator.OnCatch(left, red);
            siteSwapCreator.OnCatch(left, blue);

            siteSwapCreator.OnCatch(right, red);
            siteSwapCreator.OnCatch(right, green);
            siteSwapCreator.OnCatch(left, red);
            siteSwapCreator.OnCatch(left, blue);

            Assert.AreEqual("42342342342342342342342", siteSwapCreator.GetSiteSwap());
        }

        [Test]
        public void creates_siteswap_for_531()
        {
            siteSwapCreator.OnCatch(left, green);
            siteSwapCreator.OnCatch(right, blue);
            siteSwapCreator.OnCatch(left, red);
            siteSwapCreator.OnCatch(right, red);
            siteSwapCreator.OnCatch(left, blue);
            siteSwapCreator.OnCatch(right, green);
            Assert.AreEqual("531", siteSwapCreator.GetSiteSwap());
        }

        [Test]
        public void creates_siteswap_for_40()
        {
            siteSwapCreator.OnCatch(right, green);
            siteSwapCreator.OnCatch(right, blue);
            siteSwapCreator.OnCatch(right, green);
            siteSwapCreator.OnCatch(right, blue);
            siteSwapCreator.OnCatch(right, green);
            siteSwapCreator.OnCatch(right, blue);
            siteSwapCreator.OnCatch(right, green);
            siteSwapCreator.OnCatch(right, blue);
            siteSwapCreator.OnCatch(right, green);
            siteSwapCreator.OnCatch(right, blue);
            Assert.AreEqual("4040404040404040_0_", siteSwapCreator.GetSiteSwap());
        }
    }
}
