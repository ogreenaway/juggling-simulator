using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class SiteSwapTest
    {
        /*
         * default values on start
         * launch, juggle, (counts catches)
         * launch, juggle, drop, (sets record)
         * #balls, (new records), launch, juggle, change #balls, (sets record)
         */

        [UnityTest]
        public IEnumerator SiteSwapTestWithEnumeratorPasses()
        {
            Assert.AreEqual(true, false);
            yield return null;
        }
    }
}
