using System.Collections;
using System.Collections.Generic;
using System.IO;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

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
    public Dictionary<int, EnemyInfo> enemyInfo = new Dictionary<int, EnemyInfo>();
    public Dictionary<int, GameObject> enemyPrefabs = new Dictionary<int, GameObject>();

    public GameObject[] enemyPrefabsArray;

    // 로컬 데이터 주소
    private string jsonFileName = "grow_tower_enemies.json";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Firebase 초기화
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                if (task.Result == DependencyStatus.Available)
                {
                    databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
                    LoadEnemyData();

                    // 인스펙터에 연결된 Enemy 오브젝트를 dictionary에 저장
                    for (int i = 0; i < enemyPrefabsArray.Length; i++)
                    {
                        enemyPrefabs.Add(i, enemyPrefabsArray[i]);
                    }
                    Debug.Log("Firebase 초기화 성공");
                }
                else
                {
                    Debug.LogError($"Firebase 초기화 실패: {task.Result}");
                }
            });
        }
        else
        {
            Destroy(gameObject);
        }

        LoadLocalEnemyData();

    }

    private void Start()
    {
        
    }

    private void LoadEnemyData() // FireBase 에서 정보 가져오기
    {
        databaseReference.Child("enemies").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                foreach (var monsterSnapshot in snapshot.Children)
                {
                    EnemyInfo enemy = JsonUtility.FromJson<EnemyInfo>(monsterSnapshot.GetRawJsonValue());
                    enemyInfo[enemy.ID] = enemy;
                    Debug.Log($"몬스터 로드: {enemy.Name}, HP: {enemy.HP}");
                }
            }
            else
            {
                Debug.LogError($"데이터 로드 실패: {task.Exception}");
            }
        });
    }

    public EnemyInfo GetEnemyInfo(int id) // DB에서 정보 가져오기
    {
        if (enemyInfo.ContainsKey(id))
        {
            return enemyInfo[id];
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

    public Dictionary<int, EnemyInfo> LoadLocalEnemyData()
    {
        string jsonFilePath = Path.Combine(Application.streamingAssetsPath, "grow_tower_enemies.json");

        if (!File.Exists(jsonFilePath))
        {
            Debug.LogWarning("JSON 파일이 존재하지 않습니다.");
            return new Dictionary<int, EnemyInfo>();
        }

        string json = File.ReadAllText(jsonFilePath);
        Debug.Log($"JSON 파일 내용: {json}");

        // Newtonsoft.Json을 사용한 파싱
        var enemyData = JsonConvert.DeserializeObject<EnemyJsonInfo>(json);

        if (enemyData == null)
        {
            Debug.LogWarning("JSON 파싱 실패: enemyData가 null입니다.");
        }
        else if (enemyData.enemies == null)
        {
            Debug.LogWarning("JSON 파싱 실패: enemies 배열이 null입니다.");
        }
        else
        {
            Debug.Log($"JSON 파싱 성공: {enemyData.enemies.Count}개의 몬스터 로드");
        }

        if (enemyData == null || enemyData.enemies == null)
        {
            Debug.LogWarning("JSON 데이터가 올바르지 않습니다.");
            return new Dictionary<int, EnemyInfo>();
        }

        // List를 Dictionary로 변환
        Dictionary<int, EnemyInfo> localEnemyData = new Dictionary<int, EnemyInfo>();
        foreach (var enemy in enemyData.enemies)
        {
            localEnemyData[enemy.ID] = enemy;
        }

        Debug.Log($"JSON 데이터 로드 완료: {localEnemyData.Count}개의 몬스터 로드");
        return localEnemyData;
    }
}
