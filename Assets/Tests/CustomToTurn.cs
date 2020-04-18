using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using TMPro;

namespace Tests
{
    public class CustomToTurn
    {
        [UnityTest]
        public IEnumerator Test_Custom_to_Turnbased()
        {

            // Use the Assert class to test conditions.
            SceneManager.LoadScene("Persistent");
            // Use yield to skip a frame.
            yield return null;
            SceneManager.LoadScene("CustomLevels");
            yield return null;
            GameObject levelIdGameObject = GameObject.Find("Text"); // Find the text component
            TextMeshProUGUI levelId = levelIdGameObject.GetComponent<TextMeshProUGUI>();
            levelId.text = "TEACH1"; // Set the test input
            GameObject customLoadController = GameObject.Find("CustomLoadController"); // Find the Controller component
            CustomLevelLoadController script = customLoadController.GetComponent<CustomLevelLoadController>();
            script.LoadLevel();
            yield return null;



        }
    }
}
