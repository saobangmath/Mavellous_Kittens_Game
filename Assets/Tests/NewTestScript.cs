using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;


namespace Tests
{
    public class NewTestScript
    {

        private DataController dc;
        [UnityTest]
        public IEnumerator DataControllerExists()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            SceneManager.LoadScene("Persistent");
            yield return new WaitForSeconds(3);
            dc = GameObject.FindObjectOfType<DataController>();

            Assert.IsTrue(dc != null);
        }
    }
}
