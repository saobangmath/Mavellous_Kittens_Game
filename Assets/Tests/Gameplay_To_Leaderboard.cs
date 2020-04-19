using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Numerics;
using Firebase;
using Firebase.Auth;
using TMPro;

namespace Tests
{
    public class GameplayToLeaderboard
    {
        [UnityTest]
        public IEnumerator Gameplay_To_Leaderboard()
        {
            // TEST INPUTS
            int testWorld = 1;
            int testLevel = 3;
            string testLevelId = "2351";

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
            turnController.LeaderboardButton();
            yield return new WaitForSeconds(2f); // Wait for initialisation to finish

            AttemptsFirebase leaderboardController = GameObject.Find("HighscoreTable").GetComponent<AttemptsFirebase>();

            Assert.AreEqual(testWorld, leaderboardController.worldId);
            Assert.AreEqual(testLevel.ToString(), leaderboardController.levelId);
            Application.Quit();
        }
    }
}
