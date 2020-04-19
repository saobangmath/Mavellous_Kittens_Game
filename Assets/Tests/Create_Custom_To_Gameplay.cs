using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

namespace Tests
{
    public class Create_Custom_To_Gameplay
    {
        [UnityTest]
        public IEnumerator Create_Custom__Level_To_Gameplay()
        {
            // TEST INPUTS 
            int[] customQuestions = new int[] { 1, 2, 4, 5 };
            int bossId = 1; // 0 = Chicken, , 1 = Rhino, 2 = Turtle

            // SETUP
            string[] boss_constants = new string[] { "chicken", "rhino", "turtle" };

            SceneManager.LoadScene("Persistent"); // Initialise DataController and enter World Selection
            yield return new WaitForSeconds(4f);

            SceneManager.LoadScene("CreateCustom"); // Load the Custom Level Creation screen
            yield return new WaitForSeconds(3f);

            GameObject customControllerGameObject = GameObject.Find("CustomLevelController");
            ToggleContainer toggleContainer = customControllerGameObject.GetComponent<ToggleContainer>();
            List<GameObject> toggles = toggleContainer.getToggles();

            for (int i = 0; i < customQuestions.Length; i++) // Set the Test Questions
                toggles[customQuestions[i]].GetComponentInChildren<Toggle>().isOn = true;

            CustomLevelController customController = customControllerGameObject.GetComponent<CustomLevelController>();
            customController.setBoss(bossId); // Set the Test Boss
            customController.OnFinishLevel(); // Finish button
            customController.OnConfirmButton(); // Confirm Button

            string customLvlId = GameObject.Find("LevelIDTxt").GetComponent<TextMeshProUGUI>().text;
            customLvlId = customLvlId.Split(new string[] { "Level ID: " }, StringSplitOptions.RemoveEmptyEntries)[0];

            SceneManager.LoadScene("CustomLevels"); // Load the custom level scene
            yield return new WaitForSeconds(1f);

            // SETTING THE TEST INPUT
            GameObject idInput = GameObject.Find("IDInput"); // Find the IDInput
            idInput.GetComponent<TMP_InputField>().text = customLvlId; // Set the test input
            yield return new WaitForSeconds(1f);
            GameObject customLoadController = GameObject.Find("CustomLoadController"); // Find the Controller component
            CustomLevelLoadController script = customLoadController.GetComponent<CustomLevelLoadController>(); // Get the controller script
            script.LoadLevel();  // Load the level
            yield return new WaitForSeconds(1f);

            // CHECK TURN BASED SCENE GOT THE CORRECT LEVEL TO USE
            GameObject gameController = GameObject.Find("GameController");
            QuestionController questionScript = gameController.GetComponent<QuestionController>();

            // Get the level ID that the Gameplay subsytem is using to initialise the level
            string returnedLevelId = questionScript.getRoundData().lvlId;
            Assert.AreEqual(customLvlId, returnedLevelId); // Same level?
            Assert.AreEqual(boss_constants[bossId], questionScript.getRoundData().boss); // Same boss?

            // Get the list of questions the Gameplay Subsystem is going to use
            List<QnA> returnedQn = questionScript.getRoundData().questions;
            // Fetch all Questions from Firebase to query the questions
            List<QnA> allQuestions = GameObject.FindObjectOfType<FirebaseScript>().GetQuestionData(); // Get all questions
            // Assert that the number of questions is the same
            Assert.AreEqual(returnedQn.Count, customQuestions.Length);
            for (int i = 0; i < customQuestions.Length; i++)
            {
                Assert.AreEqual(allQuestions[customQuestions[i]], returnedQn[i]); // Assert that the questions are the same
            }
            Application.Quit();
        }
    }
}
