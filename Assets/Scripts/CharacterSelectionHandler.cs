using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionHandler : MonoBehaviour
{
    public GameObject spriteObj;
    public Sprite[] spriteArr;
    private int characterIndex = 0;
    
    void Awake()
    {
        DisplayCharacter();
    }
    
    // Handles click on left arrow button in character selection screen
    public void OnLeftArrowClick()
    {
        if (characterIndex == 0) characterIndex = spriteArr.Length - 1;
        else characterIndex--;
        DisplayCharacter();
    }

    // Handles click on right arrow button in character selection screen
    public void OnRightArrowClick()
    {
        if (characterIndex == spriteArr.Length - 1) characterIndex = 0;
        else characterIndex++;
        DisplayCharacter();
    }

    // Renders character's sprite
    public void DisplayCharacter()
    {
        spriteObj.GetComponent<SpriteRenderer>().sprite = spriteArr[characterIndex];
        Debug.Log("Currently selected character: " + spriteArr[characterIndex]);
    }

    // Handles click on save button in character selection screen
    public void OnClickSaveCharacter()
    {
        FirebaseHandler.SaveCharacter(spriteArr[characterIndex]);
        FirebaseHandler.WriteNewUser();
        SceneManager.LoadSceneAsync("HomeScreen");
    }

    public void OnClickBackButton()
    {
        SceneManager.LoadSceneAsync("InputUsernameScreen");
    }
}
