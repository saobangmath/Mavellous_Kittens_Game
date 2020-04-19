using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using TMPro;

namespace Tests
{
    public class CustomLevelsToGameplay
    {
        [UnityTest]
        public IEnumerator Custom_Transfers_LevelID_Correctly_To_TurnBased()
        {

            string testLevelId = "TEACH1";

            // Use the Assert class to test conditions.
            SceneManager.LoadScene("Persistent"); // Load Persistent Screen to initialise Data Controller
            yield return new WaitForSeconds(2f);
            // Use yield to skip a frame.
            SceneManager.LoadScene("MenuScreen"); // Load Menu Screen to load level data
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("CustomLevels"); // Load the custom level scene
            yield return new WaitForSeconds(1f);

            // SETTING THE TEST INPUT
            GameObject idInput = GameObject.Find("IDInput"); // Find the IDInput
            idInput.GetComponent<TMP_InputField>().text = testLevelId; // Set the test input
            yield return new WaitForSeconds(1f);
            GameObject customLoadController = GameObject.Find("CustomLoadController"); // Find the Controller component
            CustomLevelLoadController script = customLoadController.GetComponent<CustomLevelLoadController>(); // Get the controller script
            script.LoadLevel();  // Load the level
            yield return new WaitForSeconds(1f);

            // CHECK TURN BASED SCENE GOT THE CORRECT LEVEL TO USE
            GameObject gameController = GameObject.Find("GameController");
            QuestionController questionScript = gameController.GetComponent<QuestionController>();

            string returnedLevelId = questionScript.getRoundData().lvlId;

            Assert.AreEqual(testLevelId, returnedLevelId);

            Application.Quit();
        }
    }
}
