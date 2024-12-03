using Firebase.Database;
using UnityEngine;
using System.Threading.Tasks;
using Firebase;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;

public class FireBasePlayerDataManager : MonoBehaviour
{
    private DatabaseReference databaseReference;

    // 게임 입장 시 한번만 처리를 위한 싱글톤
    public static FireBasePlayerDataManager instance { get; private set; }
    private bool isFirebaseLoad = false;

    public Dictionary<string, PlayerInfo> FireBaseDict = new Dictionary<string, PlayerInfo>();

    private async void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);

            if (!isFirebaseLoad)
            {
                var dependencyTask = await FirebaseApp.CheckAndFixDependenciesAsync();

                if (dependencyTask == DependencyStatus.Available)
                {
                    databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

                    try
                    {
                        await LoadFireBasePlayerData();
                        await SaveFirebaseDataToLocalJson();
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError($"Firebase 데이터 로드 또는 처리 실패: {ex.Message}");
                    }
                }
                else
                {
                    Debug.LogError($"Firebase 초기화 실패: {dependencyTask}");
                }

                isFirebaseLoad = true;
            }
        }
    }

    private async Task LoadFireBasePlayerData()
    {
        try
        {
            Debug.Log("Firebase에서 데이터를 가져옵니다...");
            var dataSnapshot = await databaseReference.Child("players").GetValueAsync();

            if (dataSnapshot.Exists)
            {
                string rawJson = dataSnapshot.GetRawJsonValue();
                Debug.Log($"Firebase에서 읽은 원시 JSON 데이터: {rawJson}");

                // 데이터가 배열인지 객체인지 확인
                if (rawJson.TrimStart().StartsWith("["))
                {
                    // JSON 배열로 처리
                    var playersList = JsonConvert.DeserializeObject<List<PlayerInfo>>(rawJson);
                    FireBaseDict.Clear();

                    for (int i = 0; i < playersList.Count; i++)
                    {
                        FireBaseDict.Add(i.ToString(), playersList[i]);
                        Debug.Log($"[로드 성공] ID: {i}, Name: {playersList[i].HP}");
                    }
                }
                else
                {
                    // JSON 객체로 처리
                    FireBaseDict = JsonConvert.DeserializeObject<Dictionary<string, PlayerInfo>>(rawJson);

                    foreach (var enemy in FireBaseDict)
                    {
                        Debug.Log($"[로드 성공] ID: {enemy.Key}, Name: {enemy.Value.HP}");
                    }
                }

                Debug.Log($"Firebase에서 총 {FireBaseDict.Count}개의 적 데이터를 로드했습니다.");
            }
            else
            {
                Debug.LogWarning("Firebase에 'players' 데이터가 존재하지 않습니다.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Firebase 데이터 로드 실패: {ex.Message}");
        }
    }


    private async Task SaveFirebaseDataToLocalJson()
    {
        try
        {
            string jsonFilePath = Path.Combine(Application.persistentDataPath, "grow_tower_players.json");

            // `Newtonsoft.Json`을 사용하여 Dictionary 데이터를 JSON으로 직렬화
            string json = JsonConvert.SerializeObject(FireBaseDict, Formatting.Indented);

            // 파일로 저장
            await Task.Run(() => File.WriteAllText(jsonFilePath, json));

            Debug.Log($"Firebase 데이터를 로컬 JSON 파일로 저장 완료: {jsonFilePath}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Firebase 데이터를 로컬 JSON으로 저장하는 중 오류 발생: {ex.Message}");
        }
    }
}
