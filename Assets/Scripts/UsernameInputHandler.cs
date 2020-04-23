using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles username input on the username selection screen
/// </summary>
public class UsernameInputHandler : MonoBehaviour
{
    public InputField usernameField;
    
    /// <summary>
    /// Handles click on save button in username selection screen
    /// </summary>
    public void OnClickSaveUsername()
    {
        string username = usernameField.text;
        FirebaseHandler.SaveUsername(username);
        SceneManager.LoadSceneAsync("CharacterSelectionScreen");
    }
}
