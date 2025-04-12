using BGS.Game;
using Firebase.Auth;
using Firebase.Database;
using Firebase.RemoteConfig;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseDataHandler : MonoBehaviour
{
    public static FirebaseDataHandler Instance;

    // internal IDictionary<string,ConfigValue> _configValues;
    internal IDictionary<string, ConfigValue> _configValues = new Dictionary<string, ConfigValue>();


    internal FirebaseAuth auth;
    internal DatabaseReference databaseRef;
    internal string firebaseUserId;
    internal string DeviceId;


    #region data
    public string SystemIdentifier;
    #endregion


    private void Awake()
    {
        Instance = this;
        SystemIdentifier = SystemInfo.deviceUniqueIdentifier;
        DontDestroyOnLoad(gameObject);
    }

    public void SetUserToDB<T>(T data,string DataBase,string ChildRef) where T : class
    {
        string json = JsonUtility.ToJson(data);
        databaseRef.Child(DataBase).Child(ChildRef).SetRawJsonValueAsync(json);
        
    }

    public T LoadUserData<T>( string DataBase, string ChildRef) where T : class
    {
        T data = default(T);
        DatabaseReference userRef = FirebaseDataHandler.Instance.databaseRef.Child(DataBase).Child(ChildRef);

        userRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Failed to retrieve user data from Firebase: " + task.Exception);
                return;
            }

            if (task.IsCompleted)
            {
                var snapshot = task.Result;
                if (snapshot != null && snapshot.Exists)
                {
                    string jsonData = snapshot.GetRawJsonValue();
                    data = JsonUtility.FromJson<T>(jsonData);
                
                }
                else
                {
                    Debug.Log("No data found for the user. Initializing new data.");
                    SetUserToDB(data, DataBase, ChildRef);
                }
            }
        });
        return data;
    }

}
