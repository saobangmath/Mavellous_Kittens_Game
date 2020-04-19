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
            string previousRoundQn; // To track previous round questions
            float startTime; // To track start times in order to measure code execution time

            SceneManager.LoadScene("Persistent"); // Load Persistent Screen to initialise Data Controller
            yield return new WaitForSeconds(2f); // Wait for initialisation to finish

            DataController _dataController = GameObject.FindObjectOfType<DataController>();
            _dataController.LoadGameData();
            // Set a level to start the Gameplay
            _dataController.setCurrWorld(testWorld);
            _dataController.setCurrLevel(testLevel);
            _dataController.setLvlID(testLevelId);
            _dataController.setCustom(false);

            SceneManager.LoadScene("Turn-Based");
            yield return new WaitForSeconds(2f); // Wait for initialisation to finish

            GameObject gameController = GameObject.Find("GameController");
            TurnController turnController = gameController.GetComponent<TurnController>();

            TextMeshProUGUI qnTMP = GameObject.Find("Question").GetComponent<TextMeshProUGUI>();

            QuestionController questionController = gameController.GetComponent<QuestionController>();
           // int prevHP = turnController.GetPlayerHP(); // get initial healthpoints

            for (int i = 0; i < testAnswers.Length - 1; i++)
            {
                questionController.CheckAns(testAnswers[i]); 
                yield return new WaitForSeconds(4f); // wait for attack
                previousRoundQn = qnTMP.text;
                startTime = Time.realtimeSinceStartup;
                questionController.StartNewRound(); // advance to next question
                Assert.AreNotEqual(previousRoundQn, qnTMP.text); // Check if the qn displayed is changed.
                Assert.GreaterOrEqual(1f,Time.realtimeSinceStartup - startTime);
            }

            questionController.CheckAns(testAnswers[testAnswers.Length -1]); 
            yield return new WaitForSeconds(4f); // wait for attack

            Application.Quit();
        }

        [UnityTest]
        public IEnumerator System_Receives_Answers_Within_One_Second()
        {
            // TEST INPUTS
            int testWorld = 1;
            int testLevel = 1;
            string testLevelId = "2349";
            int totalScore = 0;
            int[] testAnswers = new int[] { 2, 1, 2, 1, 4 };

            // Set up
            float startTime; // To track start times in order to measure code execution time


            SceneManager.LoadScene("Persistent"); // Load Persistent Screen to initialise Data Controller
            yield return new WaitForSeconds(2f); // Wait for initialisation to finish

            DataController _dataController = GameObject.FindObjectOfType<DataController>();
            _dataController.LoadGameData();
            // Set a level to start the Gameplay
            _dataController.setCurrWorld(testWorld);
            _dataController.setCurrLevel(testLevel);
            _dataController.setLvlID(testLevelId);
            _dataController.setCustom(false);

            SceneManager.LoadScene("Turn-Based");
            yield return new WaitForSeconds(2f); // Wait for initialisation to finish

            GameObject gameController = GameObject.Find("GameController");
            TurnController turnController = gameController.GetComponent<TurnController>();

            QuestionController questionController = gameController.GetComponent<QuestionController>();
            // int prevHP = turnController.GetPlayerHP(); // get initial healthpoints

            for (int i = 0; i < testAnswers.Length - 1; i++)
            {
                startTime = Time.realtimeSinceStartup; // Check time elapsed to receive and process answer
                questionController.CheckAns(testAnswers[i]);
                Assert.GreaterOrEqual(1f, Time.realtimeSinceStartup - startTime); // Check time elapsed to receive and process answer
                yield return new WaitForSeconds(4f); // wait for attack
                questionController.StartNewRound(); // advance to next question
            }

            startTime = Time.realtimeSinceStartup; // Check time elapsed to receive and process answer
            questionController.CheckAns(testAnswers[testAnswers.Length - 1]);
            Assert.GreaterOrEqual(1f, Time.realtimeSinceStartup - startTime); // Check time elapsed to receive and process answer
            yield return new WaitForSeconds(4f); // wait for attack

            Application.Quit();
        }
    }
}
