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

            Assert.AreEqual("33333____", siteSwapCreator.GetSiteSwap());
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

            Assert.AreEqual("42342342342342342342342____", siteSwapCreator.GetSiteSwap());
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
            Assert.AreEqual("531____", siteSwapCreator.GetSiteSwap());
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
            Assert.AreEqual("4040404040404040_0__", siteSwapCreator.GetSiteSwap());
        }

        [Test]
        public void bug_fix_for_transition_from_3_to_60()
        {
            // First two are caught
            siteSwapCreator.OnCatch(right, green);
            siteSwapCreator.OnCatch(left, blue);

            siteSwapCreator.OnThrow(right, green); // 6
            siteSwapCreator.OnCatch(right, red);

            siteSwapCreator.OnThrow(left, blue); // 7 or 5 or something

            siteSwapCreator.OnThrow(right, red); // 6
            siteSwapCreator.OnCatch(right, green);

            siteSwapCreator.OnThrow(right, green); // 6
            siteSwapCreator.OnCatch(right, blue);

            siteSwapCreator.OnThrow(right, blue); // 6
            siteSwapCreator.OnCatch(right, red);

            siteSwapCreator.OnThrow(right, red); // 6
            siteSwapCreator.OnCatch(right, green);

            siteSwapCreator.OnThrow(right, green); // 6
            siteSwapCreator.OnCatch(right, blue);

            Assert.AreEqual("45606060_0_0__", siteSwapCreator.GetSiteSwap());
        }

        [Test]
        public void bug_fix_for_IndexOutOfRangeException()
        {
            siteSwapCreator.Reset();
            siteSwapCreator.Reset();
            siteSwapCreator.Reset();
            siteSwapCreator.Reset();
            siteSwapCreator.Reset();
            siteSwapCreator.OnCatch(1, -18680);
            siteSwapCreator.OnCatch(2, -16310);
            siteSwapCreator.OnThrow(1, -18680);
            siteSwapCreator.OnCatch(1, -18658);
            siteSwapCreator.OnThrow(2, -16310);
            siteSwapCreator.OnCatch(2, -18680);
            siteSwapCreator.OnThrow(1, -18658);
            siteSwapCreator.OnCatch(1, -16310);
            siteSwapCreator.OnThrow(2, -18680);
            siteSwapCreator.OnCatch(2, -18658);
            siteSwapCreator.OnThrow(1, -16310);
            siteSwapCreator.OnCatch(1, -18680);
            siteSwapCreator.OnThrow(2, -18658);
            siteSwapCreator.OnThrow(1, -18680);
            siteSwapCreator.OnCatch(1, -16310);
            siteSwapCreator.OnThrow(1, -16310);
            siteSwapCreator.OnCatch(1, -18658);
            siteSwapCreator.OnThrow(1, -18658);
            siteSwapCreator.OnCatch(1, -18680);
            siteSwapCreator.OnCatch(2, -16310);
            siteSwapCreator.OnThrow(1, -18680);
            siteSwapCreator.OnThrow(2, -16310);
            siteSwapCreator.OnCatch(1, -18658);
            siteSwapCreator.OnThrow(1, -18658);
            Assert.AreEqual("333345605040____", siteSwapCreator.GetSiteSwap());
        }

        [Test]
        public void bug_fix_for_another_IndexOutOfRangeException()
        {
            siteSwapCreator.Reset(); // OnNumberOfBallsChange 1
            siteSwapCreator.Reset(); // OnNumberOfBallsChange 3
            siteSwapCreator.Reset(); // OnNumberOfBallsChange 3
            siteSwapCreator.Reset(); // OnNumberOfBallsChange 3
            siteSwapCreator.Reset(); // OnLaunch
            siteSwapCreator.OnCatch(2, -101292);
            siteSwapCreator.OnCatch(1, -98922);
            siteSwapCreator.OnThrow(2, -101292);
            siteSwapCreator.OnCatch(2, -101270);
            siteSwapCreator.OnThrow(1, -98922);
            siteSwapCreator.OnCatch(1, -101292);
            siteSwapCreator.OnThrow(2, -101270);
            siteSwapCreator.OnCatch(2, -98922);
            siteSwapCreator.OnThrow(1, -101292);
            siteSwapCreator.OnCatch(1, -101270);
            siteSwapCreator.OnThrow(2, -98922);
            siteSwapCreator.OnCatch(2, -101292);
            siteSwapCreator.OnThrow(1, -101270);
            siteSwapCreator.OnCatch(1, -98922);
            siteSwapCreator.OnThrow(1, -98922);
            siteSwapCreator.OnCatch(1, -101270);
            siteSwapCreator.OnThrow(2, -101292);
            siteSwapCreator.OnCatch(2, -98922);
            siteSwapCreator.OnThrow(2, -98922);
            siteSwapCreator.OnCatch(2, -101292);
            siteSwapCreator.OnThrow(1, -101270);
            siteSwapCreator.OnCatch(1, -98922);
            siteSwapCreator.OnThrow(1, -98922);
            siteSwapCreator.OnCatch(1, -101270);
            siteSwapCreator.OnThrow(1, -101270);
            siteSwapCreator.OnCatch(1, -98922);
            siteSwapCreator.OnThrow(1, -98922);
            siteSwapCreator.OnCatch(1, -101270);
            siteSwapCreator.OnThrow(1, -101270);
            siteSwapCreator.OnCatch(1, -98922);
            siteSwapCreator.OnThrow(1, -98922);
            siteSwapCreator.OnCatch(1, -101270);
            siteSwapCreator.OnThrow(1, -101270);
            siteSwapCreator.OnCatch(1, -98922);
            siteSwapCreator.OnThrow(1, -98922);
            siteSwapCreator.OnCatch(1, -101270);
            siteSwapCreator.OnThrow(2, -101292);
            siteSwapCreator.OnThrow(1, -101270);
            siteSwapCreator.OnCatch(1, -98922);
            siteSwapCreator.OnThrow(1, -98922);
            Assert.AreEqual("33333423423424242424242424__0__", siteSwapCreator.GetSiteSwap());
        }
    }
}
