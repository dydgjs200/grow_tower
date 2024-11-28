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

public class EnemyInfo
{
    public int ID;
    public string Name;
    public bool Melee;
    public float HP;
    public float Damage;
    public float Armor;
    public float Magic_resist;
    public float Speed;
}

// JSON 데이터를 배열로 감싸는 Wrapper 클래스
[System.Serializable]
public class EnemyJsonInfo
{
    public List<EnemyInfo> enemies;
}

[System.Serializable]
public class EnemyDataManager : MonoBehaviour
{
    public static EnemyDataManager Instance { get; private set; }
    private DatabaseReference databaseReference;
    public Dictionary<int, EnemyInfo> Firebase_enemyInfo = new Dictionary<int, EnemyInfo>();
    public Dictionary<int, GameObject> enemyPrefabs = new Dictionary<int, GameObject>();

    public GameObject[] enemyPrefabsArray;

    // 로컬 데이터 주소
    private string jsonFileName = "grow_tower_enemies.json";

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
                    Debug.Log("Firebase 데이터를 로드합니다.");

                    // 1. Firebase 데이터 로드
                    await LoadFireBaseEnemyData();

                    // 2. Firebase 데이터를 로컬 JSON 파일로 저장
                    await SaveJsonToPersistentPath(Firebase_enemyInfo);

                    // 3. 로컬 JSON 파일 로드 테스트
                    await LoadJsonFromPersistentPath();

                    // 4. Enemy Prefab 초기화
                    for (int i = 0; i < enemyPrefabsArray.Length; i++)
                    {
                        enemyPrefabs.Add(i, enemyPrefabsArray[i]);
                    }

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
        var dataSnapshotTask = databaseReference.Child("enemies").GetValueAsync();

        try
        {
            DataSnapshot snapshot = await dataSnapshotTask;

            foreach (var enemySnapshot in snapshot.Children)
            {
                EnemyInfo enemy = JsonUtility.FromJson<EnemyInfo>(enemySnapshot.GetRawJsonValue());
                Firebase_enemyInfo[enemy.ID] = enemy;
            }

            Debug.Log($"Firebase 데이터 로드 완료: {Firebase_enemyInfo.Count}개의 몬스터 로드");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Firebase 데이터 로드 실패: {ex.Message}");
            throw; // 예외를 상위 호출부로 전달
        }
    }

    private async Task SaveJsonToPersistentPath(Dictionary<int, EnemyInfo> enemyData)
    {
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "grow_tower_enemies.json");

        // Dictionary를 List로 변환
        var enemyList = new EnemyJsonInfo { enemies = new List<EnemyInfo>(enemyData.Values) };

        // JSON 변환 및 저장
        string json = JsonConvert.SerializeObject(enemyList, Formatting.Indented);
        File.WriteAllText(jsonFilePath, json);

        Debug.Log($"JSON 파일 저장 경로: {jsonFilePath}");
    }


    private async Task<Dictionary<int, EnemyInfo>> LoadJsonFromPersistentPath()
    {
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "grow_tower_enemies.json");

        if (!File.Exists(jsonFilePath))
        {
            Debug.LogWarning("JSON 파일이 존재하지 않습니다.");
            return new Dictionary<int, EnemyInfo>();
        }

        try
        {
            // 비동기로 파일 읽기
            string json = await Task.Run(() => File.ReadAllText(jsonFilePath));
            Debug.Log($"JSON 파일 내용: {json}");

            var enemyData = JsonConvert.DeserializeObject<EnemyJsonInfo>(json);
            Dictionary<int, EnemyInfo> localEnemyData = new Dictionary<int, EnemyInfo>();

            foreach (var enemy in enemyData.enemies)
            {
                localEnemyData[enemy.ID] = enemy;
            }

            Debug.Log($"JSON 데이터 로드 완료: {localEnemyData.Count}개의 몬스터 로드");
            return localEnemyData;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"JSON 파일 로드 실패: {ex.Message}");
            return new Dictionary<int, EnemyInfo>();
        }
    }



    public EnemyInfo GetEnemyInfo(int id) // DB에서 정보 가져오기
    {
        if (Firebase_enemyInfo.ContainsKey(id))
        {
            return Firebase_enemyInfo[id];
        }
        Debug.LogWarning($"ID {id}에 해당하는 몬스터를 찾을 수 없습니다.");
        return null;
    }

    public GameObject GetEnemyPrefab(int id) // 프리팹 가져오기
    {
        if (enemyPrefabs.ContainsKey(id))
        {
            return enemyPrefabs[id];
        }

        return null;
    }


}
