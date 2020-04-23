using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Unity;
using Firebase.Auth;
using Google;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// The main controller for handling Facebook and Google log in
/// </summary>
public class LoginHandler : MonoBehaviour
{
    public string webClientId = "942471948637-i2vg6skgrvjun1t3jpuglvthrtdr33ij.apps.googleusercontent.com";

    private FirebaseAuth auth;
    private FirebaseUser user;
    
    private GoogleSignInConfiguration googleConfiguration;

    void Awake()
    {
        InitializeGoogleSignIn();
        InitializeFacebookSignIn();
    }

    /// <summary>
    /// Initialise Google SDK with the configuration details
    /// </summary>
    private void InitializeGoogleSignIn()
    {
        // Create configuration for Google Sign In SDK
        googleConfiguration = new GoogleSignInConfiguration
        {
            WebClientId = webClientId,
            RequestEmail = true,
            RequestIdToken = true
        };
        Debug.Log("Google SDK Initialized");
    }

    /// <summary>
    /// Handles the Google Sign In button
    /// </summary>
    public void OnGoogleSignInClick()
    {
        Debug.Log("Google Sign In selected");
        SignInGoogle();
    }

    /// <summary>
    /// Triggers the browser for user to sign into google.
    /// </summary>
    private void SignInGoogle()
    {
        GoogleSignIn.Configuration = googleConfiguration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
        Debug.Log("User Signed In using Google");
    }

    /// <summary>
    /// Function call when Google sign in is complete. It takes a task parameter with consists of the successful/unsuccessful authentication details.
    /// It checks if the status of the authentication is successful before trying to sign the user in with the resulting IdToken.
    /// </summary>
    /// <param name="task">The variable consisting of the completed authentication details</param>
    private void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            // Check for exceptions
            using (IEnumerator<System.Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException) enumerator.Current;
                    Debug.LogError($"Error: {error.Status} {error.Message}");
                }
                else
                {
                    Debug.Log($"Unexpected Exception {task.Exception}");
                }
            }
        }
        else if (task.IsCanceled)
        {
            Debug.Log("Canceled");
        }
        else
        {
            Debug.Log($"Welcome {task.Result.DisplayName}");
            Debug.Log($"Email: {task.Result.Email}");
            Debug.Log($"Google ID Token {task.Result.IdToken}");
            
            GoogleSignInFirebase(task.Result.IdToken);
        }
    }

    /// <summary>
    /// Retrieves Google credentials and signs in to Firebase
    /// </summary>
    /// <param name="idToken">The id token retrieved from the completing the authentication process.</param>
    private void GoogleSignInFirebase(string idToken)
    {
        // Retrieve credentials from Google
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
        SignInFirebase(credential);
    }

    /// <summary>
    /// Initialise Facebook SDK with the configuration details
    /// </summary>
    private void InitializeFacebookSignIn()
    {
        // Check if FB has been initialized
        if (!FB.IsInitialized)
        {
            // Initializes SDK, then calls FacebookInitCallback
            FB.Init(FacebookInitCallback, OnHideUnity);
        }
        else
        {
            // FB already initialized, signal app activation App Event
            FB.ActivateApp();
        }
    }

    /// <summary>
    /// Checks if FB SDK is successfully initialized
    /// </summary>
    private void FacebookInitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            Debug.Log("Facebook SDK Initialized");
        }
        else
        {
            Debug.Log("Failed to initialize Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause game
            Time.timeScale = 0;
        }
        else
        {
            // Resume game
            Time.timeScale = 1;
        }
    }


    /// <summary>
    /// Handle click on Facebook Sign In Button
    /// </summary>
    public void OnFacebookSignInClick()
    {
        Debug.Log("Facebook Sign In selected");
        SignInFacebook();
    }

    /// <summary>
    /// Triggers the browser for user to sign into Facebook.
    /// It passes the FB SDK with the back callback to this application.
    /// </summary>
    private void SignInFacebook()
    {
        var permissions = new List<string>() {"public_profile", "email"};
        FB.LogInWithReadPermissions(permissions, AuthCallback);
    }

    /// <summary>
    /// This is the function callback for when the Facebook SDK is finished with the authentication.
    /// It passes the Callback with a result variable. This callback functions retrieves the resulting
    /// authentication access token for facebook and attempts to sign the user in using the access token.
    /// </summary>
    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            // AccessToken contains session details
            var accessToken = AccessToken.CurrentAccessToken;
            FacebookSignInFirebase(accessToken);
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }


    /// <summary>
    /// Get the full user credentials via the access token from authentication and sign the user into Firebase
    /// using those credentials.
    /// </summary>
    /// <param name="accessToken">The access token retrieved after a successful Facebook authentication</param>
    private void FacebookSignInFirebase(AccessToken accessToken)
    {
        Credential credential = FacebookAuthProvider.GetCredential(accessToken.TokenString);
        SignInFirebase(credential);
    }


    /// <summary>
    /// Sign in to firebase using the Firebase SDK with the credentials gotten from either the Facebook or Google SDK.
    /// If the Firebase SDK fails to find an existing user using the credentials, a new user is created and it brings the user
    /// to the input username scene rather than the game menu screen.
    /// </summary>
    /// <param name="credential">Credentials from Facebook or google SDK.</param>
    private void SignInFirebase(Credential credential)
    {
        auth = FirebaseAuth.DefaultInstance;
        // Sign in to Firebase Authentication using credentials from providers
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            user = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.UserId);
            
            // Check if user has registered before
            bool registered = FirebaseHandler.CheckIfUserIsRegistered().Result;
            
            // Registered user
            if (registered) 
            {
                Debug.Log("User has registered, proceed to home screen");
                SceneManager.LoadSceneAsync("Persistent");
            }
            // New user
            else
            {
                Debug.Log("User has not registered, proceed to username and character selection");
                FirebaseHandler.CreateNewUser(user.UserId);
                SceneManager.LoadSceneAsync("InputUsernameScreen");
            }
        });
    }

    // Handle click on Sign Out Button
    public void OnSignOutClick()
    {
        Debug.Log("Sign Out selected");
        SignOut();
    }

    private void SignOut()
    {
        FirebaseAuth.DefaultInstance.SignOut();
        Debug.Log("User Signed Out");
    }
}
