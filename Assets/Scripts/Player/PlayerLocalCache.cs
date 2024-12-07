using System.Collections.Generic;
using Firebase.Extensions;
using UnityEngine;

public class PlayerLocalCache : MonoBehaviour
{
    public static PlayerLocalCache Instance { get; private set; }
    public Dictionary<string, Dictionary<string, object>> localCache = new Dictionary<string, Dictionary<string, object>>();
    public Dictionary<string, Dictionary<string, object>> changedData = new Dictionary<string, Dictionary<string, object>>();
    private float syncInterval = 30.0f;
    private float syncTimer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        syncTimer += Time.deltaTime;
        if (syncTimer >= syncInterval)
        {
            SyncDataWithFirebase();
            syncTimer = 0;

            PrintLocalCache();      // 임시 로컬저장 확인
        }
    }

    public void SetPlayerData(string playerId, string key, object value)
    {
        if (!localCache.ContainsKey(playerId))
        {
            localCache[playerId] = new Dictionary<string, object>();
        }
        localCache[playerId][key] = value;

        if (!changedData.ContainsKey(playerId))
        {
            changedData[playerId] = new Dictionary<string, object>();
        }
        changedData[playerId][key] = value;
    }

    public object GetPlayerData(string playerId, string key)
    {
        if (localCache.ContainsKey(playerId) && localCache[playerId].ContainsKey(key))
        {
            return localCache[playerId][key];
        }
        return null;
    }

    public void InitializeCache(Dictionary<string, Dictionary<string, object>> initialData)
    {
        localCache = initialData;
        changedData.Clear();
        Debug.Log("Local cache initialized.");
    }

    private void SyncDataWithFirebase()
    {
        if (changedData.Count == 0) return;

        foreach (var playerId in changedData.Keys)
        {
            Firebase.Database.FirebaseDatabase.DefaultInstance.GetReference($"players/{playerId}")
                .UpdateChildrenAsync(changedData[playerId]).ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompleted)
                    {
                        Debug.Log($"Player {playerId} data synced with Firebase.");
                        changedData[playerId].Clear();
                    }
                    else
                    {
                        Debug.LogError($"Failed to sync player {playerId} data.");
                    }
                });
        }
    }

    public void PrintLocalCache()           // 임시 로컬 확인 함수
    {
        Debug.Log("[LocalCache] Current cache contents:");
        foreach (var player in localCache)
        {
            string playerId = player.Key;
            string playerData = "";

            foreach (var kvp in player.Value)
            {
                playerData += $"{kvp.Key}: {kvp.Value}, ";
            }

            Debug.Log($"PlayerId: {playerId}, Data: {playerData}");
        }
    }

}
