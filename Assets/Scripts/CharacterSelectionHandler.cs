using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    // Handles click on left arrow button in character selection screen
    public void OnLeftArrowClick()
    {
        if (characterIndex == 0) characterIndex = animArr.Length - 1;
        else characterIndex--;
        DisplayCharacter();
    }

    // Handles click on right arrow button in character selection screen
    public void OnRightArrowClick()
    {
        if (characterIndex == animArr.Length - 1) characterIndex = 0;
        else characterIndex++;
        DisplayCharacter();
    }

    // Renders character's sprite
    public void DisplayCharacter()
    {
        spriteObj.GetComponent<Animator>().runtimeAnimatorController = animArr[characterIndex];
        Debug.Log("Currently selected character: " + animArr[characterIndex]);
    }

    // Handles click on save button in character selection screen
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

    public void OnClickBackButton()
    {
        SceneManager.LoadSceneAsync("InputUsernameScreen");
    }
}
