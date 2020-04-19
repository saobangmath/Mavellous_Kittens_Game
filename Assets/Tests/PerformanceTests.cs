using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using TMPro;

namespace Tests
{
    public class PerformanceTests
    {
        [UnityTest]
        public IEnumerator Boot_within_five_seconds_after_login()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            int iterations = 3;

            for (int i = 0; i < iterations; i++)
            {
                SceneManager.LoadSceneAsync("Persistent"); // Load the scene which plays after the user logged in
                yield return new WaitForSecondsRealtime(4.5f);
                Assert.AreEqual(SceneManager.GetActiveScene().name, "MenuScreen");
            }
        }

        [UnityTest]
        public IEnumerator Display_Question_Within_One_Second()
        {
            // TEST INPUTS
            int testWorld = 1;
            int testLevel = 1;
            string testLevelId = "2349";
            int totalScore = 0;
            int[] testAnswers = new int[] { 2, 1, 2, 1, 4 };

            // Setup
            string previousRoundQn;

            SceneManager.LoadScene("Persistent"); // Load Persistent Screen to initialise Data Controller
            yield return new WaitForSeconds(4f); // Wait for initialisation to finish

            DataController _dataController = GameObject.FindObjectOfType<DataController>();
            _dataController.LoadGameData();
            // Set a level to start the Gameplay
            _dataController.setCurrWorld(testWorld);
            _dataController.setCurrLevel(testLevel);
            _dataController.setLvlID(testLevelId);
            _dataController.setCustom(false);

            SceneManager.LoadScene("Turn-Based");
            yield return new WaitForSeconds(4f); // Wait for initialisation to finish

            GameObject gameController = GameObject.Find("GameController");
            TurnController turnController = gameController.GetComponent<TurnController>();

            //TextMeshProUGUI qnTMP = GameObject.Find("Question").GetComponent<TextMeshProUGUI>();

            QuestionController questionController = gameController.GetComponent<QuestionController>();
            int prevHP = turnController.GetPlayerHP(); // get initial healthpoints

            for (int i = 0; i < testAnswers.Length - 1; i++)
            {
                questionController.CheckAns(testAnswers[i]); // Correct answer for the first question is 2
                yield return new WaitForSeconds(4f); // wait for attack
                questionController.StartNewRound(); // advance to next question
            }

            questionController.CheckAns(testAnswers[testAnswers.Length -1]); // Correct answer for the first question is 2
            yield return new WaitForSeconds(4f); // wait for attack

            int newHP = turnController.GetPlayerHP(); // get player healthpoints

            GameObject levelFinished = GameObject.Find("LevelFinished");
            Assert.IsTrue(levelFinished.activeSelf);
            Debug.Log(levelFinished.activeSelf); // checks if level has ended

            Assert.IsTrue(newHP == 3);
            Debug.Log(newHP == 3); // checks if remains as 3

            Assert.IsTrue(questionController.getScore() >= 125);
            Debug.Log(questionController.getScore() >= 125); // check if score has increased

            Debug.Log("Done");

            Application.Quit();
        }
    }
}
