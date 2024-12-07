using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseDataLoader : MonoBehaviour
{
    public Action<Dictionary<string, Dictionary<string, object>>> OnDataLoaded;

    public void LoadPlayerData()
    {
        FirebaseDatabase.DefaultInstance.GetReference("players").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Dictionary<string, Dictionary<string, object>> playersData = new Dictionary<string, Dictionary<string, object>>();

                foreach (var child in snapshot.Children)
                {
                    Dictionary<string, object> playerData = new Dictionary<string, object>
                    {
                        { "HP", float.Parse(child.Child("HP").Value.ToString()) },
                        { "Damage", float.Parse(child.Child("Damage").Value.ToString()) },
                        { "AttackSpeed", float.Parse(child.Child("AttackSpeed").Value.ToString()) }
                    };

                    playersData[child.Key] = playerData;
                }

                Debug.Log("Player data loaded from Firebase.");
                OnDataLoaded?.Invoke(playersData);
            }
            else
            {
                Debug.LogError("Failed to load data from Firebase.");
            }
        });
    }
}
