using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using VRTK;

namespace Tests
{
    public class GameModeSliderTest
    {
        [UnityTest]
        public IEnumerator Has_correct_default_value_and_can_be_updated()
        {
            yield return TestUtils.LoadScene();
            yield return null; // Sometimes slow to load
            VRTK_ObjectTooltip gameModeSliderText = GameObject.Find("Game mode text").GetComponent<VRTK_ObjectTooltip>();
            GameModeSlider gameModeSlider = Object.FindObjectOfType<GameModeSlider>();
            GameObject fullGameModeSection = GameObject.Find("Full game mode");

            Assert.AreEqual("Party mode", gameModeSliderText.displayText, "Game mode text is the correct default value");
            Assert.Less(fullGameModeSection.transform.position.y, 0, "At the start of the game the full game section is hidden");

            gameModeSlider.OnChange(1);
            Assert.AreEqual(1, fullGameModeSection.transform.position.y, "If value is 1, the full game section is visible");
            Assert.AreEqual("Full game", gameModeSliderText.displayText, "If value is 1, the text is 'Full Game'");

            gameModeSlider.OnChange(0);
            Assert.Less(fullGameModeSection.transform.position.y, 0, "If value is 0, the full game section is hidden");
            Assert.AreEqual("Party mode", gameModeSliderText.displayText, "If value is 0, the text is 'Party mode'");
        }
    }
}
