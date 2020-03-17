using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Unity;
using Firebase;
using Firebase.Auth;
using Google;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public string webClientId = "942471948637-i2vg6skgrvjun1t3jpuglvthrtdr33ij.apps.googleusercontent.com";

    private FirebaseApp _app;
    private FirebaseAuth _auth;
    private FirebaseUser _user;
    
    private GoogleSignInConfiguration _googleConfiguration;

    void Awake()
    {
        InitializeFirebase();
        InitializeGoogleSignIn();
        InitializeFacebookSignIn();
    }
    
    private void InitializeFirebase()
    {
        Debug.Log("CheckFirebaseDependencies()");
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result == DependencyStatus.Available)
                {
                    _app = FirebaseApp.DefaultInstance;
                    _auth = FirebaseAuth.DefaultInstance;
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

    private void InitializeGoogleSignIn()
    {
        _googleConfiguration = new GoogleSignInConfiguration
        {
            WebClientId = webClientId,
            RequestEmail = true,
            RequestIdToken = true
        };
        Debug.Log("Google SDK Initialized");
    }

    private void InitializeFacebookSignIn()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(FacebookInitCallback, OnHideUnity);
        }
    }

    private void FacebookInitCallback()
    {
        if (FB.IsInitialized)
        {
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
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    // Call by clicking buttons
    public void OnGoogleSignInClick() { SignInGoogle(); }
    public void OnFacebookSignInClick() { SignInFacebook(); }
    public void OnSignOutClick() { SignOut(); }

    private void SignInGoogle()
    {
        GoogleSignIn.Configuration = _googleConfiguration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
        Debug.Log("User Signed In using Google");
    }

    private void SignInFacebook()
    {
        var permissions = new List<string>() {"public_profile", "email"};
        FB.LogInWithReadPermissions(permissions, AuthCallback);
    }

    private void SignOut()
    {
        FB.LogOut();
        GoogleSignIn.DefaultInstance.SignOut();
        _auth.SignOut();
        Debug.Log("User Signed Out");
    }
    
    private void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<System.Exception> enumerator =
                task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error =
                        (GoogleSignIn.SignInException) enumerator.Current;
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
    
    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            var accessToken = AccessToken.CurrentAccessToken;
            FacebookSignInFirebase(accessToken);
        }
    }
    
    private void GoogleSignInFirebase(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
        SignInFirebase(credential);
    }

    private void FacebookSignInFirebase(AccessToken accessToken)
    {
        Credential credential = FacebookAuthProvider.GetCredential(accessToken.TokenString);
        SignInFirebase(credential);
    }

    private void SignInFirebase(Credential credential)
    {
        _auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }
            FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            SceneManager.LoadScene(1);
        });
    }
}
