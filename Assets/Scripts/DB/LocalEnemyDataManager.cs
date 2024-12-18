using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.IO;
using System;
using UnityEngine.Networking;
using System.Collections;

[System.Serializable]
public class LocalEnemyDataManager : MonoBehaviour
{
    public static LocalEnemyDataManager Instance { get; private set; }
    public Dictionary<int, EnemyInfo> LocalDict = new Dictionary<int, EnemyInfo>();
    public Dictionary<int, GameObject> enemyPrefabs = new Dictionary<int, GameObject>();

    public GameObject[] enemyPrefabsArray;

    private string persistentPath;
    private string streamingPath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialize paths
        persistentPath = Path.Combine(Application.persistentDataPath, "grow_tower_enemies.json");
        streamingPath = Path.Combine(Application.streamingAssetsPath, "grow_tower_enemies.json");
    }

    private void Start()
    {
        Debug.Log("delog: Starting to load enemy data.");
        LoadEnemyData();
    }

    private void LoadPrefabsFromResources()
    {
        // Resources/Prefabs/Enemies 폴더에서 모든 프리팹 로드
        enemyPrefabsArray = Resources.LoadAll<GameObject>("Prefabs/Enemies");

        if (enemyPrefabsArray == null || enemyPrefabsArray.Length == 0)
        {
            Debug.LogError("delog: Failed to load prefabs from Resources/Prefabs/Enemies.");
        }
        else
        {
            Debug.Log($"delog: Loaded {enemyPrefabsArray.Length} prefabs from Resources.");
        }
    }

    // Save data
    public void DataSave(List<EnemyInfo> enemies)
    {
        try
        {
            string json = JsonConvert.SerializeObject(enemies, Formatting.Indented);
            File.WriteAllText(persistentPath, json);
            Debug.Log("delog: Enemy data saved successfully.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"delog: Error saving enemy data: {ex.Message}");
        }
    }

    // Load data
    public void DataLoad()
    {
        if (File.Exists(persistentPath))
        {
            Debug.Log("delog: Loading data from persistent path.");
            LoadFromPath(persistentPath);
        }
        else
        {
            Debug.LogWarning("delog: Persistent file not found, attempting to load from StreamingAssets.");
            StartCoroutine(LoadFromStreamingAssets());
        }
    }

    private void LoadFromPath(string filePath)
    {
        try
        {
            string json = File.ReadAllText(filePath);
            DeserializeAndLoad(json);
        }
        catch (Exception ex)
        {
            Debug.LogError($"delog: Error loading data from path: {ex.Message}");
        }
    }

    private IEnumerator LoadFromStreamingAssets()
    {
        UnityWebRequest request = UnityWebRequest.Get(streamingPath);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"delog: Failed to load file from StreamingAssets: {request.error}");
        }
        else
        {
            try
            {
                string json = request.downloadHandler.text;

                // 파일 복사
                File.WriteAllText(persistentPath, json);
                DeserializeAndLoad(json);
                Debug.Log("delog: Successfully loaded and copied data from StreamingAssets.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"delog: Error during file copy or deserialization: {ex.Message}");
            }
        }
    }

    private void DeserializeAndLoad(string json)
    {
        try
        {
            List<EnemyInfo> enemies = JsonConvert.DeserializeObject<List<EnemyInfo>>(json);

            LocalDict.Clear();
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] != null)
                {
                    LocalDict.Add(i, enemies[i]);
                    Debug.Log($"delog: Added enemy with ID {i}, Name: {enemies[i].Name}");
                }
                else
                {
                    Debug.LogWarning($"delog: Null enemy data at index {i}.");
                }
            }

            Debug.Log($"delog: Total enemies loaded into LocalDict: {LocalDict.Count}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"delog: Error during deserialization: {ex.Message}");
        }
    }


    // Save and load data calls
    public void SaveEnemyData()
    {
        List<EnemyInfo> enemies = new List<EnemyInfo>
        {
            new EnemyInfo("Goblin", true, 100f, 10f, 5f, 2f, 2f, 1.5f, 1f),
            new EnemyInfo("Dragon", false, 300f, 50f, 20f, 10f, 5f, 0.8f, 2f),
            new EnemyInfo("Orc", true, 200f, 30f, 10f, 5f, 0.3f, 1.2f, 3f),
            new EnemyInfo("Ender", true, 1000f, 30f, 10f, 5f, 0.3f, 1.2f, 3f)
        };

        DataSave(enemies);
        Debug.Log("delog: Default enemy data saved.");
    }

    public void LoadEnemyData()
    {
        LoadPrefabsFromResources(); // 동적 프리팹 로드
        DataLoad();
        InitializedPrefabs();
        Debug.Log("delog: Enemy data loaded and prefabs initialized.");
    }

    private void InitializedPrefabs()
    {
        Debug.Log($"delog: LocalDict.Count {LocalDict.Count}");
        if (LocalDict.Count == 0)
        {
            Debug.LogWarning("delog: Local data is empty, skipping prefab initialization.");
            return;
        }

        if (enemyPrefabsArray == null || enemyPrefabsArray.Length == 0)
        {
            Debug.LogError("delog: enemyPrefabsArray is null or empty. Prefabs cannot be initialized.");
            return;
        }

        enemyPrefabs.Clear();

        foreach (var enemy in LocalDict)
        {
            string enemyName = enemy.Value.Name; // JSON 데이터의 Name 값
            GameObject prefab = Array.Find(enemyPrefabsArray, p => p.name == enemyName);

            if (prefab != null)
            {
                enemyPrefabs.Add(enemy.Key, prefab);
                Debug.Log($"delog: Prefab '{prefab.name}' assigned to enemy ID {enemy.Key} ({enemyName}).");
            }
            else
            {
                Debug.LogWarning($"delog: No prefab found for enemy Name '{enemyName}'.");
            }
        }
    }


    public GameObject GetEnemyPrefab(int id)
    {
        if (enemyPrefabs.ContainsKey(id))
        {
            Debug.Log($"delog: Returning prefab for ID {id}.");
            return enemyPrefabs[id];
        }
        else
        {
            Debug.LogWarning($"delog: No prefab found for ID {id}.");
            return null;
        }
    }

    public EnemyInfo GetEnemyInfo(int id)
    {
        if (LocalDict.ContainsKey(id))
        {
            Debug.Log($"delog: Returning enemy info for ID {id}.");
            return LocalDict[id];
        }
        else
        {
            Debug.LogWarning($"delog: No enemy info found for ID {id}.");
            return null;
        }
    }
}