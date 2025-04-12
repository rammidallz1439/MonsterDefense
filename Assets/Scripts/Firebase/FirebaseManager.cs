using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using Firebase.RemoteConfig;
using Google;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vault;
public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;
    private IDictionary<string, ConfigValue> _configValues = new Dictionary<string, ConfigValue>();

    [Header("Instances")] 
    private FirebaseAuth auth;
    private FirebaseFirestore db;
    private DocumentReference UserLoginData;
    private FirebaseUser currentUser;
    private FirebaseRemoteConfig RemoteConfig;

    [Header("Cloud Data")]
    internal UserData UserData;
    [SerializeField] internal SkillTreeData SkillTreeData;
    [SerializeField] internal string PlayerUserId;


    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        StartFireBase();
    }

    private async void StartFireBase()
    {
        await MakeFirebaseInstance();
            //check User
            if (currentUser != null)
            {
                // If a user is already signed in, load their data and go to menu
                Debug.Log("User already logged in: " + currentUser.UserId);
                //UserLoginData = db.Collection(GameConstants.RootCollection).Document(currentUser.UserId);
                if (currentUser.DisplayName == "")
                {
                    PlayerUserId = currentUser.UserId;
                }
                else
                {
                    PlayerUserId = currentUser.DisplayName;
                }
                UserData = await LoadDataAsync<UserData>(GameConstants.RootCollection, PlayerUserId, async (userData) =>
                    {
                        EventManager.Instance.TriggerEvent(new SwitchLoginStateEvent(true));
                    },
                    async () =>
                    {
                        EventManager.Instance.TriggerEvent(new SwitchLoginStateEvent(false));
                    });


            }
            else
            {
                Debug.Log("No previous user found. Showing login screen.");
                EventManager.Instance.TriggerEvent(new SwitchLoginStateEvent(false));
            }
    
    }

    public async Task MakeFirebaseInstance()
    {
        await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(async task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError("Failed to initialize Firebase: " + task.Exception);
                return;
            }

            auth = FirebaseAuth.DefaultInstance;
            currentUser = auth.CurrentUser;
            db = FirebaseFirestore.DefaultInstance;
           
            RemoteConfig = FirebaseRemoteConfig.DefaultInstance;
            await FetchDataAsync();
     
        });
    }

    public Task FetchDataAsync()
    {
        Debug.Log("Fetching data...");
        Task fetchTask = RemoteConfig.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    private void FetchComplete(Task fetchTask)
    {
        if (!fetchTask.IsCompleted)
        {
            Debug.LogError("Retrieval hasn't finished.");
            return;
        }
        ConfigInfo info = RemoteConfig.Info;
        if (info.LastFetchStatus != LastFetchStatus.Success)
        {
            Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
            return;
        }

        // Fetch successful. Parameter values must be activated to use.
        RemoteConfig.ActivateAsync()
          .ContinueWithOnMainThread(
            task =>
            {
                Debug.Log($"Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");
                try
                {
                    _configValues = RemoteConfig.AllValues;
                    SkillTreeData = JsonUtility.FromJson<SkillTreeData>(_configValues["SkillTreeData"].StringValue);

                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

            });
    }


    public void SignInToGoogle()
    {
        GoogleSignIn.Configuration = new GoogleSignInConfiguration
        {
            RequestIdToken = true,
            WebClientId = "49771020959-jvgcncgdnetloh1rrkj31pmj4r7803or.apps.googleusercontent.com",
            RequestEmail = true
        };

        Task<GoogleSignInUser> signIn = GoogleSignIn.DefaultInstance.SignIn();

        TaskCompletionSource<FirebaseUser> signInCompleted = new TaskCompletionSource<FirebaseUser>();

        signIn.ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Google Sign-In was canceled.");
                signInCompleted.SetCanceled();
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("Google Sign-In encountered an error: " + task.Exception);
                signInCompleted.SetException(task.Exception);
            }
            else
            {
                Debug.Log("Google Sign-In successful. Fetching ID Token...");

                string idToken = task.Result.IdToken;
                Debug.Log("ID Token: " + idToken);

                Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(idToken, null);

                Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

                auth.SignInWithCredentialAsync(credential).ContinueWith(async authTask =>
                {
                    if (authTask.IsCanceled)
                    {
                        Debug.LogError("Firebase Authentication was canceled.");
                        signInCompleted.SetCanceled();
                    }
                    else if (authTask.IsFaulted)
                    {
                        Debug.LogError("Firebase Authentication encountered an error: " + authTask.Exception);
                        signInCompleted.SetException(authTask.Exception);
                    }
                    else
                    {
                        Firebase.Auth.FirebaseUser newUser = authTask.Result;
                        Debug.Log("Firebase Authentication successful. User: " + newUser.DisplayName);
                        signInCompleted.SetResult(newUser);
                        UserData data = new UserData
                        {
                            DisplayName = newUser.Email,
                            UserId = newUser.UserId,
                        };
                        await SaveDataAsync(GameConstants.RootCollection, data.DisplayName, data, () =>
                        {
                            MonoHelper.Instance.PrintMessage(data.DisplayName,"blue");
                            PlayerUserId = data.DisplayName;
                            EventManager.Instance.TriggerEvent(new SwitchLoginStateEvent(true));
                        });
                    }
                });
            }
        });
    }



    public void GuestLogin()
    {
        auth.SignInAnonymouslyAsync().ContinueWith(async task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }

            AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
            UserData data = new UserData
            {
                DisplayName = result.User.DisplayName,
                UserId = result.User.UserId,
            };
            await SaveDataAsync(GameConstants.RootCollection, result.User.UserId, data, () =>
            {
                PlayerUserId = data.UserId;
                MonoHelper.Instance.PrintMessage(data.DisplayName, "blue");
                EventManager.Instance.TriggerEvent(new SwitchLoginStateEvent(true));
            });

        });
    }

    private void SetUserLoginData(FirebaseUser userData)
    {
        DocumentReference docRef = db.Collection("users").Document(userData.UserId);
        Dictionary<string, object> user = new Dictionary<string, object>
{
    { "DisplayName", userData.DisplayName},
    { "Id", userData.UserId}
};
        docRef.SetAsync(user).ContinueWithOnMainThread(task =>
        {
           // DataManager.Instance.Save(userData.UserId, GameConstants.LoggedState);
            PlayerPrefs.SetString(UserData.UserId, GameConstants.LoggedState);
            
            Debug.Log("Added data to the alovelace document in the users collection.");
            SceneManager.LoadScene(1);
        });
    }

    private async void RetriveUserLoginData()
    {
        DocumentSnapshot snap = await UserLoginData.GetSnapshotAsync();
        if (snap.Exists)
        {
            // Deserialize the data into a Dictionary or a custom class
            Dictionary<string, object> userData = snap.ToDictionary();

            // Example: Access specific fields
            string displayName = userData.ContainsKey("DisplayName") ? userData["DisplayName"].ToString() : "No DisplayName found";
            string id = userData.ContainsKey("Id") ? userData["Id"].ToString() : "No ID found";

            Debug.Log($"DisplayName: {displayName}");
            Debug.Log($"ID: {id}");
            SceneManager.LoadSceneAsync(1);
        }
        else
        {
            Debug.Log("Document does not exist!");
            EventManager.Instance.TriggerEvent(new SwitchLoginStateEvent());
        }

    }

  

    #region Generic Methods

    /// <summary>
    /// Saves an object to Firestore.
    /// </summary>
    public async Task SaveDataAsync<T>(string collection, string document, T data, Action onFinish = null) where T : class
    {
        try
        {
            DocumentReference docRef = db.Collection(collection).Document(document);
            await docRef.SetAsync(data).ContinueWithOnMainThread(task =>
            {
                if (onFinish != null)
                    onFinish?.Invoke();
            });
            Debug.Log($"Data saved successfully in {collection}/{document}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving data: {e.Message}");
        }
    }

    /// <summary>
    /// use to save an object to firebase under the userId document
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <param name="document"></param>
    /// <param name="data"></param>
    /// <param name="onFinish"></param>
    /// <returns></returns>
    public async Task SaveCollectionDataAsync<T>(string collection, string document, T data, Action onFinish = null) where T : class
    {
        DocumentReference docRef = db.Collection(GameConstants.RootCollection).Document(PlayerUserId).Collection(collection).Document(document);
        await docRef.SetAsync(data).ContinueWithOnMainThread(task =>
        {
            if (onFinish != null)
                onFinish?.Invoke();
        });
        Debug.Log($"Data saved successfully in {collection}/{document}");

    }



    /// <summary>
    /// Updates specific fields of a Firestore document under the userId document using a generic class.
    /// </summary>
    /// <typeparam name="T">The type of data to update, must be a class.</typeparam>
    /// <param name="collection">The Firestore collection name.</param>
    /// <param name="document">The Firestore document name.</param>
    /// <param name="data">The data object with updated fields.</param>
    /// <param name="onFinish">Optional callback to invoke after the update completes.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task UpdateCollectionDataAsync<T>(string collection, string document, T data, Action onFinish = null) where T : class
    {
        try
        {
            DocumentReference docRef = db.Collection(GameConstants.RootCollection)
                                         .Document(PlayerUserId)
                                         .Collection(collection)
                                         .Document(document);

            // Update the document with the provided data
            await docRef.SetAsync(data, SetOptions.MergeAll).ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && !task.IsFaulted)
                {
                    Debug.Log($"Data updated successfully in {collection}/{document}");
                    onFinish?.Invoke();
                }
                else
                {
                    Debug.LogError($"Failed to update data in {collection}/{document}: {task.Exception?.Message}");
                }
            });
        }
        catch (Exception e)
        {
            Debug.LogError($"Error updating data: {e.Message}");
        }
    }



    /// <summary>
    /// Loads an object from Firestore.
    /// </summary>
    public async Task<T> LoadDataAsync<T>(string collection, string document, Action<T> onSucess = null, Action onFailure = null) where T : class, new()
    {

        DocumentReference docRef = db.Collection(collection).Document(document);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            T data = snapshot.ConvertTo<T>();
            if (onSucess != null)
                onSucess?.Invoke(data);
            Debug.Log($"Data loaded successfully from {collection}/{document}");
            return data;
        }
        else
        {
            if (onFailure != null)
                onFailure?.Invoke();
            Debug.LogWarning($"Document {collection}/{document} not found.");
            return null;
        }

    }

    public async Task<T> LoadCollectionDataAsync<T>(string collection, string document, Action<T> onSucess = null, Action onFailure = null) where T : class, new()
    {

        DocumentReference docRef = db.Collection(GameConstants.RootCollection).Document(PlayerUserId).Collection(collection).Document(document);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            T data = snapshot.ConvertTo<T>();
            if (onSucess != null)
                onSucess.Invoke(data);
            Debug.Log($"Data loaded successfully from {collection}/{document}");
            return data;
        }
        else
        {
            if (onFailure != null)
                onFailure.Invoke();
            Debug.LogWarning($"Document {collection}/{document} not found.");
            return null;
        }

    }



    #endregion
}


