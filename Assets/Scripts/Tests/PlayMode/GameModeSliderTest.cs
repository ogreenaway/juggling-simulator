using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GameModeSliderTest
    {
        [UnityTest]
        public IEnumerator Displays_and_hides_the_full_game_UI_panels()
        {
            yield return TestUtils.LoadScene();
            GameObject.Find("Game mode slider").GetComponent<SettingsSlider>().OnChange(1);
            Assert.AreEqual(1, GameObject.Find("Full game mode").transform.position.y, "If value is 1, the full game section is visible");
            Assert.AreEqual("Full game", GameObject.Find("Game mode text").GetComponent<VRTK.VRTK_ObjectTooltip>().displayText, "If value is 1, the text is 'Full Game'");

            GameObject.Find("Game mode slider").GetComponent<SettingsSlider>().OnChange(0);
            Assert.Less(GameObject.Find("Full game mode").transform.position.y, 0, "If value is 0, the full game section is hidden");
            Assert.AreEqual("Party mode", GameObject.Find("Game mode text").GetComponent<VRTK.VRTK_ObjectTooltip>().displayText, "If value is 0, the text is 'Party mode'");

            yield return null;
        }
    }
}
