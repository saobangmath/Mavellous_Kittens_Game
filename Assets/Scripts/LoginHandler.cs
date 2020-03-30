using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Unity;
using Firebase.Auth;
using Google;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    // Initialize Google Sign In SDK
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

    // Handle click on Google Sign In Button
    public void OnGoogleSignInClick()
    {
        Debug.Log("Google Sign In selected");
        SignInGoogle();
    }

    // Sign in to Google
    private void SignInGoogle()
    {
        GoogleSignIn.Configuration = googleConfiguration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
        Debug.Log("User Signed In using Google");
    }
    
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
    
    // Retrieves Google credentials and signs in to Firebase
    private void GoogleSignInFirebase(string idToken)
    {
        // Retrieve credentials from Google
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
        SignInFirebase(credential);
    }

    // Initialize Facebook Sign In SDK
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
    
    // Checks if FB SDK is successfully initialized
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

    // Handle click on Facebook Sign In Button
    public void OnFacebookSignInClick()
    {
        Debug.Log("Facebook Sign In selected");
        SignInFacebook();
    }

    // Sign in to Facebook
    private void SignInFacebook()
    {
        var permissions = new List<string>() {"public_profile", "email"};
        FB.LogInWithReadPermissions(permissions, AuthCallback);
    }
    
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

    // Retrieves facebook credentials and signs in to Firebase
    private void FacebookSignInFirebase(AccessToken accessToken)
    {
        Credential credential = FacebookAuthProvider.GetCredential(accessToken.TokenString);
        SignInFirebase(credential);
    }

    // Sign in to Firebase
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
