using System.Threading.Tasks;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;


/// <summary>
/// The main controller class to handling authentication in the Firebase auth SDK
/// </summary>
public class FirebaseHandler : MonoBehaviour
{
    private static DatabaseReference _reference;
    private FirebaseAuth auth;
    private FirebaseUser user;
    private static User _currentUser;
    private static string _newUserId;
    
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        
        // Check Firebase Dependencies before initializing Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Firebase Dependencies check completed");
                if (task.Result == DependencyStatus.Available)
                {
                    // Set up the Editor before calling into the realtime database.
                    FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://marvellous-kittens.firebaseio.com/");
                    // Get the root of the database
                    _reference = FirebaseDatabase.DefaultInstance.RootReference;
                    auth = FirebaseAuth.DefaultInstance;
                    auth.StateChanged += AuthStateChanged;
                    AuthStateChanged(this, null);
                    Debug.Log("Firebase Initialized");
                }
                else
                {
                    Debug.Log("Could not resolve all Firebase Dependencies");
                }
            }
            else
            {
                Debug.Log("Dependency check not completed");
            }
        });
    }

    /// <summary>
    /// Track state changes of the auth object.
    /// </summary>
    void AuthStateChanged(object sender, System.EventArgs eventArgs) {
        if (auth.CurrentUser != user) {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null) {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn) {
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }
    
    void OnDestroy() {
        Debug.Log("Firebase object destroyed");
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }


    /// <summary>
    /// Check if user is registered in the database
    /// </summary>
    public static async Task<bool> CheckIfUserIsRegistered()
    {
        FirebaseUser currentUser = FirebaseAuth.DefaultInstance.CurrentUser;
        DatabaseReference userRef = _reference.Child("Users");
        bool registered = false;

        // Get data of users from database
        DataSnapshot snapshot = await userRef.OrderByKey().GetValueAsync();
        
        // Searches for current user's ID in the database
        foreach (DataSnapshot user in snapshot.Children)
        {
            if (user.Key == currentUser.UserId)
            {
                Debug.Log("User found" + user.Key);
                registered = true;
                break;
            }
        }

        return registered;
    }

    /// <summary>
    /// Create User object for the new user
    /// </summary>
    public static void CreateNewUser(string userId)
    {
        _newUserId = userId;
        _currentUser = new User();
    }

    /// <summary>
    /// Save user's character selection
    /// </summary>
    public static void SaveCharacter(Sprite sprite)
    {
        string name = sprite.name.Remove(sprite.name.Length - 2);
        _currentUser.chr = name;
    }

    /// <summary>
    /// Save user's username input
    /// </summary>
    public static void SaveUsername(string username)
    {
        _currentUser.usr = username;
    }

    /// <summary>
    /// Create new user in the database
    /// </summary>
    public static void WriteNewUser()
    {
        string json = _currentUser.SaveToJsonString();
        Debug.Log(json);
        _reference.Child("Users").Child(_newUserId).SetRawJsonValueAsync(json);
    }
}
