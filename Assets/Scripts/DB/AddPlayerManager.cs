using Firebase.Database;
using Firebase;
using Newtonsoft.Json;
using UnityEngine;
using System.Collections.Generic;

public class AddPlayerManager : MonoBehaviour
{
    private DatabaseReference databaseReference;

    private async void Start()
    {
        // Firebase 초기화 확인
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == Firebase.DependencyStatus.Available)
        {
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

            // Firebase 초기화 후 샘플 데이터 추가
            AddSamplePlayers();
        }
        else
        {
            Debug.LogError($"Firebase 초기화 실패: {dependencyStatus}");
        }
    }

    public async void AddSamplePlayers()
    {
        try
        {
            var players = new Dictionary<string, PlayerInfo>();
        {
                players.Add("admin", new PlayerInfo(100, 10, 0.5f));
        };

            string json = JsonConvert.SerializeObject(players, Formatting.Indented);
            await databaseReference.Child("players").SetRawJsonValueAsync(json);

            Debug.Log("샘플 플레이어 데이터를 Firebase에 추가했습니다.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Firebase 데이터 추가 실패: {ex.Message}");
        }
    }
}
