using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Numerics;
// using Firebase;
// using Firebase.Auth;
//using TMPro;

namespace Tests
{
    public class Gameplay_Functional_Test
    {

        [UnityTest]
        public IEnumerator CheckScoreUpdate()
        {
            Debug.Log("CheckScoreUpdate");

            // TEST INPUTS
            int testWorld = 1;
            int testLevel = 1;
            string testLevelId = "2349";
            int totalScore = 0;

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

            QuestionController questionController = gameController.GetComponent<QuestionController>();
            int prevHP = turnController.GetPlayerHP(); // get initial healthpoints

            questionController.CheckAns(2); // Correct answer for the first question is 2
            yield return new WaitForSeconds(4f); // wait for attack
            int prevScore = questionController.getScore();
            Assert.IsTrue(prevScore >= 25);
            Debug.Log(prevScore >= 25); // checks that score is added correctly based on time
            int newHP = turnController.GetPlayerHP(); // get player healthpoints
            Assert.IsTrue(prevHP == newHP);
            Debug.Log(prevHP == newHP); // check healthpoints remained unchanged
            prevHP = newHP;

            questionController.StartNewRound(); // advance to next question

            questionController.CheckAns(2); // Correct answer for the second question is 1
            yield return new WaitForSeconds(4f); // wait for attack
            Assert.IsTrue((questionController.getScore() - prevScore) == 0);
            Debug.Log((questionController.getScore() - prevScore) == 0); // checks that no score is added
            newHP = turnController.GetPlayerHP(); // get player healthpoints
            Assert.IsTrue(prevHP > newHP);
            Debug.Log(prevHP > newHP); // check healthpoints decreased

            Debug.Log("Done");

            Application.Quit();

            //Object.Destroy(testDC);
        }

        [UnityTest]
        public IEnumerator CheckQnsAllWrong()
        {
            Debug.Log("CheckQnsAllWrong");

            // TEST INPUTS
            int testWorld = 1;
            int testLevel = 1;
            string testLevelId = "2349";
            int totalScore = 0;

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

            QuestionController questionController = gameController.GetComponent<QuestionController>();
            int prevHP = turnController.GetPlayerHP(); // get initial healthpoints

            questionController.CheckAns(1); // Correct answer for the first question is 2
            yield return new WaitForSeconds(4f); // wait for attack

            questionController.StartNewRound(); // advance to next question

            questionController.CheckAns(2); // Correct answer for the second question is 1
            yield return new WaitForSeconds(4f); // wait for attack

            questionController.StartNewRound(); // advance to next question

            questionController.CheckAns(1); // Correct answer for the third question is 2
            yield return new WaitForSeconds(4f); // wait for attack
            int newHP = turnController.GetPlayerHP(); // get player healthpoints

            GameObject levelFinished = GameObject.Find("LevelFinished");
            Assert.IsTrue(levelFinished.activeSelf);
            Debug.Log(levelFinished.activeSelf); // checks if level has ended

            Assert.IsTrue(newHP == 0);
            Debug.Log(newHP == 0); // checks if healthpoints remains at 0

            Assert.IsTrue(questionController.getScore() == 0);
            Debug.Log(questionController.getScore() == 0); // check if score has increased

            Debug.Log("Done");

            Application.Quit();

            //Object.Destroy(testDC);
        }

        [UnityTest]
        public IEnumerator CheckQnsAllCorrect()
        {
            Debug.Log("CheckQnsAllCorrect");

            // TEST INPUTS
            int testWorld = 1;
            int testLevel = 1;
            string testLevelId = "2349";
            int totalScore = 0;

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

            QuestionController questionController = gameController.GetComponent<QuestionController>();
            int prevHP = turnController.GetPlayerHP(); // get initial healthpoints

            questionController.CheckAns(2); // Correct answer for the first question is 2
            yield return new WaitForSeconds(4f); // wait for attack

            questionController.StartNewRound(); // advance to next question

            questionController.CheckAns(1); // Correct answer for the second question is 1
            yield return new WaitForSeconds(4f); // wait for attack

            questionController.StartNewRound(); // advance to next question

            questionController.CheckAns(2); // Correct answer for the third question is 2
            yield return new WaitForSeconds(4f); // wait for attack

            questionController.StartNewRound(); // advance to next question

            questionController.CheckAns(1); // Correct answer for the fourth question is 1
            yield return new WaitForSeconds(4f); // wait for attack

            questionController.StartNewRound(); // advance to next question

            questionController.CheckAns(4); // Correct answer for the fifth question is 4
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

            //Object.Destroy(testDC);
        }
    }
}
