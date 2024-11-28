using System.Collections;
using System.Collections.Generic;
using System.IO;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Threading.Tasks;


[System.Serializable]
public class EnemyDataManager : MonoBehaviour
{
    public static EnemyDataManager Instance { get; private set; }
    private DatabaseReference databaseReference;
    public Dictionary<string, EnemyInfo> FireBaseDict = new Dictionary<string, EnemyInfo>();
    public Dictionary<string, GameObject> enemyPrefabs = new Dictionary<string, GameObject>();

    public GameObject[] enemyPrefabsArray;

    private async void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            var dependencyTask = await FirebaseApp.CheckAndFixDependenciesAsync();

            if (dependencyTask == DependencyStatus.Available)
            {
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

                try
                {
                    await LoadFireBaseEnemyData();
                    await SaveFirebaseDataToLocalJson();
                    await InitializedPrefabs();

                    Debug.Log("Enemy Prefab 초기화 완료");
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
        else
        {
            Destroy(gameObject);
        }
    }



    private void Start()
    {

    }

    private async Task LoadFireBaseEnemyData()
    {
        try
        {
            var dataSnapshot = await databaseReference.Child("enemies").GetValueAsync();

            if (dataSnapshot.Exists)
            {
                string rawJson = dataSnapshot.GetRawJsonValue();
                var enemies = JsonConvert.DeserializeObject<Dictionary<string, EnemyInfo>>(rawJson);

                foreach (var enemy in enemies)
                {
                    FireBaseDict.Add(enemy.Key, enemy.Value);
                    Debug.Log($"Enemy Name: {enemy.Key}, Data: {JsonConvert.SerializeObject(enemy.Value)}");
                }
            }
        }
        catch (System.Exception ex)
        {
            throw;
        }
    }

    private async Task SaveFirebaseDataToLocalJson()
    {
        try
        {
            string jsonFilePath = Path.Combine(Application.persistentDataPath, "grow_tower_enemies.json");

            // Dictionary 데이터를 JSON으로 직렬화
            string json = JsonConvert.SerializeObject(FireBaseDict, Formatting.Indented);

            // 파일로 저장
            await Task.Run(() => File.WriteAllText(jsonFilePath, json));

            Debug.Log($"Firebase 데이터를 로컬 JSON 파일로 저장 완료: {jsonFilePath}");
        }
        catch (System.Exception ex)
        {
            throw;
        }
    }


    private async Task InitializedPrefabs()
    {
        int count = 0;

        foreach (var enemy in FireBaseDict)
        {
            enemyPrefabs.Add(enemy.Key, enemyPrefabsArray[count]);
            count++;
        }
    }

    public GameObject GetEnemyPrefab(string key)
    {
        if (enemyPrefabs.ContainsKey(key))
        {
            return enemyPrefabs[key];
        }
        else
            return null;
    }

    public EnemyInfo GetEnemyInfo(string key)
    {
        if (FireBaseDict.ContainsKey(key))
        {
            return FireBaseDict[key];
        }
        else
            return null;
    }
}
