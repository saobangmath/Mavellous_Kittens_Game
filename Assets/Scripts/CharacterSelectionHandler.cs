using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// The main controller that handles the inputs in the Character Selection screen.
/// </summary>
public class CharacterSelectionHandler : MonoBehaviour
{
    public GameObject spriteObj;
    public Sprite[] spriteArr;
    public RuntimeAnimatorController[] animArr;
    public GameObject backButton;
    private int characterIndex = 0;
    
    void Awake()
    {
        DisplayCharacter();
    }

    private void Start()
    {
        if (MenuScreenLoadParam.CharacterLoadFromMenu)
        {
            backButton.SetActive(false);
        }
        else
        {
            backButton.SetActive(true);
        }
    }

    /// <summary>
    /// Handles click on left arrow button in character selection screen. It changes the index of the character and displays the character sprite 
    /// that the index is pointing to.
    /// </summary>
    public void OnLeftArrowClick()
    {
        if (characterIndex == 0) characterIndex = animArr.Length - 1;
        else characterIndex--;
        DisplayCharacter();
    }

    /// <summary>
    /// Handles click on left arrow button in character selection screen. It changes the index of the character and displays the character sprite 
    /// that the index is pointing to.
    /// </summary>
    public void OnRightArrowClick()
    {
        if (characterIndex == animArr.Length - 1) characterIndex = 0;
        else characterIndex++;
        DisplayCharacter();
    }

    /// <summary>
    /// Renders the Character Sprite that is being pointed by the current index.
    /// </summary>
    public void DisplayCharacter()
    {
        spriteObj.GetComponent<Animator>().runtimeAnimatorController = animArr[characterIndex];
        Debug.Log("Currently selected character: " + animArr[characterIndex]);
    }

    /// <summary>
    /// Saves the character data to the user's data in the Firebase database.
    /// Also updates the DataController on the current character that is being used.
    /// </summary>
    public void OnClickSaveCharacter()
    {
        //If not loading from main menu (loading from username input)
        if (!MenuScreenLoadParam.CharacterLoadFromMenu)
        {
            FirebaseHandler.SaveCharacter(spriteArr[characterIndex]);
            FirebaseHandler.WriteNewUser();
            SceneManager.LoadSceneAsync("Persistent");            
        }
        else
        {
            FirebaseScript firebaseScript = FindObjectOfType<FirebaseScript>();
            DataController dataController = FindObjectOfType<DataController>();
            firebaseScript.UpdateUserChar("pipo-nekonin00" + (characterIndex + 1).ToString());
            dataController.currentUser.chr = "pipo-nekonin00" + (characterIndex + 1).ToString();
            SceneManager.LoadSceneAsync("MenuScreen");
        }

    }

    /// <summary>
    /// Handles the back button if the user wants to change his/her username.
    /// </summary>
    public void OnClickBackButton()
    {
        SceneManager.LoadSceneAsync("InputUsernameScreen");
    }
}
