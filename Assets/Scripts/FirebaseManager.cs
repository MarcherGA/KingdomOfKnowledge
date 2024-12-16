using System.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using static PlayerCharacterCustomized.Customization.SaveObject;
using System.Linq;

public class FirebaseManager : MonoBehaviour
{
    public UnityEvent onFirebaseInit;
    public UnityEvent<object> onRegisterSuccess;
    public UnityEvent onRegisterFailure;
    public UnityEvent<bool> onUsernameCheckCompleted;
    public UnityEvent<object> onSignInSuccess;
    public UnityEvent onSignInFailure;

    private FirebaseApp _app;
    private FirebaseAuth _auth;
    private FirebaseFirestore _firestore;

    private void Awake()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                _app = Firebase.FirebaseApp.DefaultInstance;
                _auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                _firestore = FirebaseFirestore.DefaultInstance;
                // Set a flag here to indicate whether Firebase is ready to use by your app.
                onFirebaseInit?.Invoke();
            }
            else
            {
                Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    public void SignInWithEmail(string email, string password)
    {
        _auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(authTask => {
            if (authTask.IsFaulted)
            {
                Debug.LogError($"Sign-in failed: {authTask.Exception}");
                onSignInFailure?.Invoke();
                return;
            }

            FirebaseUser user = authTask.Result.User;
            Debug.Log($"Signed in as {user.Email}");

            FetchUserData(user.UserId,
                onSuccess: userData => { onSignInSuccess?.Invoke(userData); },
                onFailure: error => { onSignInFailure?.Invoke(); });
        });
    }
    public void CheckUsernameAvailability(string username, Action<bool> onResult)
    {
        _firestore.Collection("users")
            .WhereEqualTo("username", username)
            .GetSnapshotAsync()
            .ContinueWithOnMainThread(task => {
                if (task.IsFaulted)
                {
                    Debug.LogError($"Error checking username availability: {task.Exception}");
                    onResult?.Invoke(false); // Assume unavailable if query fails
                    return;
                }

                // Username is available if no documents are returned
                bool isAvailable = task.Result.Count == 0;
                onResult?.Invoke(isAvailable);
                onUsernameCheckCompleted?.Invoke(isAvailable);
            });
    }


    public void SignInWithUsername(string username, string password)
    {
        _firestore.Collection("users")
            .WhereEqualTo("username", username)
            .GetSnapshotAsync()
            .ContinueWithOnMainThread(task => {
                if (task.IsFaulted || task.Result.Count == 0)
                {
                    Debug.LogError("Username not found.");
                    onSignInFailure?.Invoke();
                    return;
                }

                string email = task.Result.Documents.ElementAt(0).GetValue<string>("email");
                // Proceed with email sign-in
                SignInWithEmail(email, password);

            });
    }

    
    public void FetchUserData(string userId, Action<object> onSuccess, Action<Exception> onFailure)
    {
        _firestore.Collection("users").Document(userId).GetSnapshotAsync()
            .ContinueWithOnMainThread(task => {
                if (task.IsFaulted || !task.Result.Exists)
                {
                    onFailure?.Invoke(task.Exception ?? new Exception("User data not found."));
                    return;
                }

                // Retrieve the data as a dictionary
                Dictionary<string, object> userDataDict = task.Result.ToDictionary();

                // Convert the dictionary to a UserData object
                var userData = DictionaryToObjectHelper.ToObject<object>(userDataDict);

                onSuccess?.Invoke(userData);
            });
    }

    public void RegisterUser(string email, string password, UserData userData)
    {
        CheckUsernameAvailability(userData.username, isAvailable => {
            if (!isAvailable)
            {
                Debug.LogError("Username is already taken.");
                onRegisterFailure?.Invoke();
                return;
            }

            _auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
                if (task.IsFaulted)
                {
                    Debug.LogError($"Registration failed: {task.Exception}");
                    onRegisterFailure?.Invoke();
                    return;
                }

                if (task.IsCompletedSuccessfully)
                {
                    FirebaseUser user = task.Result.User;
                    var profile = new UserProfile { DisplayName = userData.username };

                    user.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(profileTask => {
                        if (profileTask.IsFaulted)
                        {
                            Debug.LogError($"Failed to set display name: {profileTask.Exception}");
                        }
                    });

                    SaveUserDataToFirestore(user.UserId, userData).ContinueWithOnMainThread(resultTask => {
                        if (resultTask.IsCompletedSuccessfully)
                        {
                            onRegisterSuccess?.Invoke(userData);
                        }
                        else
                        {
                            onRegisterFailure?.Invoke();
                        }
                    });
                }
            });
        });
    }

    private Task SaveUserDataToFirestore(string userId, object userData)
    {
        // Add user data to Firestore
        var serializedUserData = ObjectToDictionaryHelper.ToDictionary(userData);
        return _firestore.Collection("users").Document(userId).SetAsync(serializedUserData).ContinueWithOnMainThread(dbTask => {
            if (dbTask.IsFaulted)
            {
                Debug.LogError($"Failed to save user data to Firestore: {dbTask.Exception}");
            }
            else
            {
                Debug.Log("User data successfully saved to Firestore.");
            }
        });
    }


}
