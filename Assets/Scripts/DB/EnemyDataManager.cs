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

// JSON �����͸� �迭�� ���δ� Wrapper Ŭ����
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

    // ���� ������ �ּ�
    private string jsonFileName = "grow_tower_enemies.json";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Firebase �ʱ�ȭ
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                if (task.Result == DependencyStatus.Available)
                {
                    databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
                    LoadEnemyData();

                    // �ν����Ϳ� ����� Enemy ������Ʈ�� dictionary�� ����
                    for (int i = 0; i < enemyPrefabsArray.Length; i++)
                    {
                        enemyPrefabs.Add(i, enemyPrefabsArray[i]);
                    }
                    Debug.Log("Firebase �ʱ�ȭ ����");
                }
                else
                {
                    Debug.LogError($"Firebase �ʱ�ȭ ����: {task.Result}");
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

    private void LoadEnemyData() // FireBase ���� ���� ��������
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
                    Debug.Log($"���� �ε�: {enemy.Name}, HP: {enemy.HP}");
                }
            }
            else
            {
                Debug.LogError($"������ �ε� ����: {task.Exception}");
            }
        });
    }

    public EnemyInfo GetEnemyInfo(int id) // DB���� ���� ��������
    {
        if (enemyInfo.ContainsKey(id))
        {
            return enemyInfo[id];
        }
        Debug.LogWarning($"ID {id}�� �ش��ϴ� ���͸� ã�� �� �����ϴ�.");
        return null;
    }

    public GameObject GetEnemyPrefab(int id) // ������ ��������
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
            Debug.LogWarning("JSON ������ �������� �ʽ��ϴ�.");
            return new Dictionary<int, EnemyInfo>();
        }

        string json = File.ReadAllText(jsonFilePath);
        Debug.Log($"JSON ���� ����: {json}");

        // Newtonsoft.Json�� ����� �Ľ�
        var enemyData = JsonConvert.DeserializeObject<EnemyJsonInfo>(json);

        if (enemyData == null)
        {
            Debug.LogWarning("JSON �Ľ� ����: enemyData�� null�Դϴ�.");
        }
        else if (enemyData.enemies == null)
        {
            Debug.LogWarning("JSON �Ľ� ����: enemies �迭�� null�Դϴ�.");
        }
        else
        {
            Debug.Log($"JSON �Ľ� ����: {enemyData.enemies.Count}���� ���� �ε�");
        }

        if (enemyData == null || enemyData.enemies == null)
        {
            Debug.LogWarning("JSON �����Ͱ� �ùٸ��� �ʽ��ϴ�.");
            return new Dictionary<int, EnemyInfo>();
        }

        // List�� Dictionary�� ��ȯ
        Dictionary<int, EnemyInfo> localEnemyData = new Dictionary<int, EnemyInfo>();
        foreach (var enemy in enemyData.enemies)
        {
            localEnemyData[enemy.ID] = enemy;
        }

        Debug.Log($"JSON ������ �ε� �Ϸ�: {localEnemyData.Count}���� ���� �ε�");
        return localEnemyData;
    }
}
