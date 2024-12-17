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

        // ��� �ʱ�ȭ
        persistentPath = Path.Combine(Application.persistentDataPath, "grow_tower_enemies.json");
        streamingPath = Path.Combine(Application.streamingAssetsPath, "grow_tower_enemies.json");
    }

    private void Start()
    {
        LoadEnemyData();
    }

    // ������ ����
    public void DataSave(List<EnemyInfo> enemies)
    {
        try
        {
            string json = JsonConvert.SerializeObject(enemies, Formatting.Indented);
            File.WriteAllText(persistentPath, json);
            Debug.Log("�� �����Ͱ� ���������� ����Ǿ����ϴ�.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"������ ���� �� ���� �߻�: {ex.Message}");
        }
    }

    // ������ �ε�
    public void DataLoad()
    {
        if (File.Exists(persistentPath))
        {
            LoadFromPath(persistentPath);
        }
        else
        {
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
            Debug.LogError($"������ �ε� �� ���� �߻�: {ex.Message}");
        }
    }

    private IEnumerator LoadFromStreamingAssets()
    {
        UnityWebRequest request = UnityWebRequest.Get(streamingPath);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"StreamingAssets���� ������ �ҷ����� ���߽��ϴ�: {request.error}");
        }
        else
        {
            string json = request.downloadHandler.text;

            // ������ persistentDataPath�� ����
            File.WriteAllText(persistentPath, json);
            DeserializeAndLoad(json);

            Debug.Log("StreamingAssets���� �����͸� �ҷ��� �����߽��ϴ�.");
        }
    }

    private void DeserializeAndLoad(string json)
    {
        List<EnemyInfo> enemies = JsonConvert.DeserializeObject<List<EnemyInfo>>(json);

        LocalDict.Clear();
        for (int i = 0; i < enemies.Count; i++)
        {
            LocalDict.Add(i, enemies[i]);
        }

        Debug.Log("�� �����Ͱ� ���������� �ε�Ǿ����ϴ�.");
    }

    // ������ ���� �� �ҷ����� ȣ��
    public void SaveEnemyData()
    {
        List<EnemyInfo> enemies = new List<EnemyInfo>
        {
            new EnemyInfo("Goblin", true, 100f, 10f, 5f, 2f, 2f, 1.5f, 1f),
            new EnemyInfo("Dragon", false, 300f, 50f, 20f, 10f, 5f, 0.8f, 2f),
            new EnemyInfo("Orc", true, 200f, 30f, 10f, 5f, 0.3f, 1.2f, 3f)
        };

        DataSave(enemies);
        Debug.Log("Enemy �����Ͱ� ����Ǿ����ϴ�.");
    }

    public void LoadEnemyData()
    {
        DataLoad();
        InitializedPrefabs();
        Debug.Log("Enemy �����Ͱ� �ε� �� Prefab�� �ʱ�ȭ�Ǿ����ϴ�.");
    }

    private void InitializedPrefabs()
    {
        if (LocalDict.Count == 0)
        {
            Debug.LogWarning("���� �����Ͱ� ��� �־� Prefab �ʱ�ȭ�� �ǳʶݴϴ�.");
            return;
        }

        int count = 0;
        foreach (var enemy in LocalDict)
        {
            if (count < enemyPrefabsArray.Length)
            {
                enemyPrefabs.Add(enemy.Key, enemyPrefabsArray[count]);
                count++;
            }
            else
            {
                Debug.LogWarning("enemyPrefabsArray �迭�� ũ�⸦ �ʰ��߽��ϴ�.");
                break;
            }
        }
        Debug.Log("Prefabs�� ���������� �ʱ�ȭ�Ǿ����ϴ�.");
    }

    public GameObject GetEnemyPrefab(int id)
    {
        if (enemyPrefabs.ContainsKey(id))
        {
            return enemyPrefabs[id];
        }
        else
        {
            Debug.LogWarning($"ID {id}�� �ش��ϴ� Prefab�� �����ϴ�.");
            return null;
        }
    }

    public EnemyInfo GetEnemyInfo(int id)
    {
        if (LocalDict.ContainsKey(id))
        {
            return LocalDict[id];
        }
        else
        {
            Debug.LogWarning($"ID {id}�� �ش��ϴ� EnemyInfo�� �����ϴ�.");
            return null;
        }
    }
}
