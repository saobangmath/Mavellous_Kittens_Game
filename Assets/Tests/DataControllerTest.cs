using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using  UnityEngine.SceneManagement;

namespace Tests
{
    public class TestSuite
    {
        private DataController testDC;

        [UnityTest]
        public IEnumerator DataControllerExists()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            SceneManager.LoadScene("Persistent");
            yield return null;
            testDC = GameObject.FindObjectOfType<DataController>();

            Assert.IsTrue(testDC != null);
            Debug.Log("Done");

            //Object.Destroy(testDC);
        }

        [Test]
        public void DataControllerLargerScore()
        {
            testDC.SubmitNewPlayerScore(6);
            Assert.IsTrue(testDC.GetHighestPlayerScore() == 6);
            Debug.Log("Done");
        }

        [Test]
        public void DataControllerSameScore()
        {
            testDC.SubmitNewPlayerScore(6);
            Assert.IsTrue(testDC.GetHighestPlayerScore() == 6);
            Debug.Log("Done");
        }

        [Test]
        public void DataControllerSmallerScore()
        {
            testDC.SubmitNewPlayerScore(3);
            Assert.IsTrue(testDC.GetHighestPlayerScore() != 3);
            Debug.Log("Done");

            Object.Destroy(testDC);
        }

        [UnityTest]
        public IEnumerator DataControllerCheckLevelName()
        {
            SceneManager.LoadScene("Persistent");
            yield return null;
            testDC = GameObject.FindObjectOfType<DataController>();

            Assert.IsTrue(testDC.GetLevelName() == "Level 1");
            Debug.Log("Done");

            //Object.Destroy(testDC);
        }

        [UnityTest]
        public IEnumerator DataControllerCheckLevelDescription()
        {
            SceneManager.LoadScene("Persistent");
            yield return null;
            testDC = GameObject.FindObjectOfType<DataController>();

            Assert.IsTrue(testDC.GetLevelDescription() == "A game about animals and their diets");
            Debug.Log("Done");

            //Object.Destroy(testDC);
        }

        [UnityTest]
        public IEnumerator DataControllerCheckLevelDifficulty()
        {
            SceneManager.LoadScene("Persistent");
            yield return null;
            testDC = GameObject.FindObjectOfType<DataController>();

            Assert.IsTrue(testDC.GetLevelDifficulty() == 1);
            Debug.Log("Done");

            //Object.Destroy(testDC);
        }

    }
}
