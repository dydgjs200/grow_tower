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
    public Dictionary<int, EnemyInfo> Firebase_enemyInfo = new Dictionary<int, EnemyInfo>();
    public Dictionary<int, GameObject> enemyPrefabs = new Dictionary<int, GameObject>();

    public GameObject[] enemyPrefabsArray;

    // ���� ������ �ּ�
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
                    Debug.Log("Firebase �����͸� �ε��մϴ�.");

                    // 1. Firebase ������ �ε�
                    await LoadFireBaseEnemyData();

                    // 2. Firebase �����͸� ���� JSON ���Ϸ� ����
                    await SaveJsonToPersistentPath(Firebase_enemyInfo);

                    // 3. ���� JSON ���� �ε� �׽�Ʈ
                    await LoadJsonFromPersistentPath();

                    // 4. Enemy Prefab �ʱ�ȭ
                    for (int i = 0; i < enemyPrefabsArray.Length; i++)
                    {
                        enemyPrefabs.Add(i, enemyPrefabsArray[i]);
                    }

                    Debug.Log("Enemy Prefab �ʱ�ȭ �Ϸ�");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Firebase ������ �ε� �Ǵ� ó�� ����: {ex.Message}");
                }
            }
            else
            {
                Debug.LogError($"Firebase �ʱ�ȭ ����: {dependencyTask}");
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

            Debug.Log($"Firebase ������ �ε� �Ϸ�: {Firebase_enemyInfo.Count}���� ���� �ε�");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Firebase ������ �ε� ����: {ex.Message}");
            throw; // ���ܸ� ���� ȣ��η� ����
        }
    }

    private async Task SaveJsonToPersistentPath(Dictionary<int, EnemyInfo> enemyData)
    {
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "grow_tower_enemies.json");

        // Dictionary�� List�� ��ȯ
        var enemyList = new EnemyJsonInfo { enemies = new List<EnemyInfo>(enemyData.Values) };

        // JSON ��ȯ �� ����
        string json = JsonConvert.SerializeObject(enemyList, Formatting.Indented);
        File.WriteAllText(jsonFilePath, json);

        Debug.Log($"JSON ���� ���� ���: {jsonFilePath}");
    }


    private async Task<Dictionary<int, EnemyInfo>> LoadJsonFromPersistentPath()
    {
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "grow_tower_enemies.json");

        if (!File.Exists(jsonFilePath))
        {
            Debug.LogWarning("JSON ������ �������� �ʽ��ϴ�.");
            return new Dictionary<int, EnemyInfo>();
        }

        try
        {
            // �񵿱�� ���� �б�
            string json = await Task.Run(() => File.ReadAllText(jsonFilePath));
            Debug.Log($"JSON ���� ����: {json}");

            var enemyData = JsonConvert.DeserializeObject<EnemyJsonInfo>(json);
            Dictionary<int, EnemyInfo> localEnemyData = new Dictionary<int, EnemyInfo>();

            foreach (var enemy in enemyData.enemies)
            {
                localEnemyData[enemy.ID] = enemy;
            }

            Debug.Log($"JSON ������ �ε� �Ϸ�: {localEnemyData.Count}���� ���� �ε�");
            return localEnemyData;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"JSON ���� �ε� ����: {ex.Message}");
            return new Dictionary<int, EnemyInfo>();
        }
    }



    public EnemyInfo GetEnemyInfo(int id) // DB���� ���� ��������
    {
        if (Firebase_enemyInfo.ContainsKey(id))
        {
            return Firebase_enemyInfo[id];
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


}
