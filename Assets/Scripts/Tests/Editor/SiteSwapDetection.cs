using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class SiteSwapDetection
    {
        // A Test behaves as an ordinary method
        [Test]
        public void SiteSwapDetectionSimplePasses()
        {
            SiteSwapAnalyser siteSwapAnalyser = new SiteSwapAnalyser();
            Assert.AreEqual(3, siteSwapAnalyser.CountCatches("3", "333"));
        }
    }
}
