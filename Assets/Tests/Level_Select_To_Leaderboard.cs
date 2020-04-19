using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class LevelSelectToLeaderboard
    {
        [UnityTest]
        public IEnumerator Level_Select_To_Leaderboard()
        {
            // TEST INPUTS
            int levelToTest = 3;
            int worldToTest = 2;

            SceneManager.LoadScene("Persistent"); // Initialise DataController and enter World Selection
            yield return new WaitForSeconds(5);
            // Get the Menu Controller in the World Selection screen
            MenuScreenController menuScreenController = GameObject.Find("MenuScreenController").GetComponent<MenuScreenController>();
            // Trigger the next button until desired World
            for (int i = 0; i < worldToTest - 1; i++)
            {
                menuScreenController.nextLevelButton(); // Click next buttons 
                yield return new WaitForSeconds(0.5f);
            }
            // Trigger the Select World button
            menuScreenController.SelectWorld();
            yield return new WaitForSeconds(0.5f);
            // Find the script of the Level Selection
            LevelScript levelScript = GameObject.FindObjectOfType<LevelScript>();
            levelScript.ButtonPasser(levelToTest); // Select the level to test
            yield return new WaitForSeconds(3f);
            menuScreenController.LeaderboardButton(); // Enter the leaderboard for the Level
            yield return new WaitForSeconds(2f);
            // Compare the world and level used in Leaderboard Subsystem to Test input.
            AttemptsFirebase leaderboardController = GameObject.Find("HighscoreTable").GetComponent<AttemptsFirebase>();
            Assert.AreEqual(worldToTest, leaderboardController.worldId);
            Assert.AreEqual(levelToTest.ToString(), leaderboardController.levelId);
        }
    }
}
