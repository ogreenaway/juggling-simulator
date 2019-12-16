using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/*
 * launch, juggle, (counts catches)
 * launch, juggle, launch, (sets record)
 * launch, juggle less than current record, launch, (!sets record)
 * launch, juggle, drop, (sets record)
 * launch, juggle less than current record, drop, (!sets record)
 * #balls, (new records shown)
 * launch, juggle, change #balls, (sets record)
 * launch, juggle less than current record, change #balls, (!sets record)
 */

namespace Tests
{
    public class SiteSwapAnalyserTest
    {
        SiteSwapAnalyser siteSwapAnalyser = new SiteSwapAnalyser();

        //[SetUp]
        //public void RunsBeforeTests()
        //{
        //}

        [Test]
        public void detects_one_catch()
        {
            Assert.AreEqual(1, siteSwapAnalyser.CountCatches("3", "3_"));
        }

        [Test]
        public void detects_two_catches()
        {
            Assert.AreEqual(2, siteSwapAnalyser.CountCatches("3", "33_"));
        }

        [Test]
        public void detects_three_catches()
        {
            Assert.AreEqual(3, siteSwapAnalyser.CountCatches("3", "333_"));
        }

        [Test]
        public void knows_when_to_start_detecting()
        {
            Assert.AreEqual(3, siteSwapAnalyser.CountCatches("3", "4333_"));
        }

        [Test]
        public void detects_multiple_siteswaps()
        {
            Assert.AreEqual(1, siteSwapAnalyser.CountCatches("3", "53_"));
            Assert.AreEqual(2, siteSwapAnalyser.CountCatches("53", "53_"));
        }

        [Test]
        public void detects_partial_siteswaps()
        {
            Assert.AreEqual(5, siteSwapAnalyser.CountCatches("53", "553535_"));
        }

        [Test]
        public void ignores_other_siteswaps_before()
        {
            Assert.AreEqual(1, siteSwapAnalyser.CountCatches("3", "5335353_"));
        }

        [Test]
        public void ignores_other_siteswaps_before_2()
        {
            Assert.AreEqual(0, siteSwapAnalyser.CountCatches("3", "533535_"));
        }
    }
}