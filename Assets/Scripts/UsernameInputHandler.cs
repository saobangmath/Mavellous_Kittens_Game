using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UsernameInputHandler : MonoBehaviour
{
    public InputField usernameField;
    
    // Handles click on save button in username selection screen
    public void OnClickSaveUsername()
    {
        string username = usernameField.text;
        FirebaseHandler.SaveUsername(username);
        SceneManager.LoadSceneAsync("CharacterSelectionScreen");
    }
}
