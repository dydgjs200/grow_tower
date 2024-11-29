using Firebase.Database;
using UnityEngine;
using System.Threading.Tasks;
using Firebase;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

public class FireBaseDataManager : MonoBehaviour
{
    private DatabaseReference databaseReference;

    public Dictionary<int, EnemyInfo> FireBaseDict = new Dictionary<int, EnemyInfo>();

    private async void Awake()
    {
        var dependencyTask = await FirebaseApp.CheckAndFixDependenciesAsync();

        if (dependencyTask == DependencyStatus.Available)
        {
            Debug.Log("Firebase 초기화 성공");
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

            try
            {
                await LoadFireBaseEnemyData();
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
    }

    private async Task LoadFireBaseEnemyData()
    {
        try
        {
            Debug.Log("Firebase에서 데이터를 가져옵니다...");
            var dataSnapshot = await databaseReference.Child("enemies").GetValueAsync();

            if (dataSnapshot.Exists)
            {
                string rawJson = dataSnapshot.GetRawJsonValue();
                Debug.Log($"Firebase에서 읽은 원시 JSON 데이터: {rawJson}");

                // 데이터가 배열인지 객체인지 확인
                if (rawJson.TrimStart().StartsWith("["))
                {
                    // JSON 배열로 처리
                    var enemiesList = JsonConvert.DeserializeObject<List<EnemyInfo>>(rawJson);
                    FireBaseDict.Clear();

                    for (int i = 0; i < enemiesList.Count; i++)
                    {
                        FireBaseDict.Add(i, enemiesList[i]);
                        Debug.Log($"[로드 성공] ID: {i}, Name: {enemiesList[i].Name}");
                    }
                }
                else
                {
                    // JSON 객체로 처리
                    FireBaseDict = JsonConvert.DeserializeObject<Dictionary<int, EnemyInfo>>(rawJson);

                    foreach (var enemy in FireBaseDict)
                    {
                        Debug.Log($"[로드 성공] ID: {enemy.Key}, Name: {enemy.Value.Name}");
                    }
                }

                Debug.Log($"Firebase에서 총 {FireBaseDict.Count}개의 적 데이터를 로드했습니다.");
            }
            else
            {
                Debug.LogWarning("Firebase에 'enemies' 데이터가 존재하지 않습니다.");
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
            string jsonFilePath = Path.Combine(Application.persistentDataPath, "grow_tower_enemies.json");

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
