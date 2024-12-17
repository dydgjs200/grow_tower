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

        // 경로 초기화
        persistentPath = Path.Combine(Application.persistentDataPath, "grow_tower_enemies.json");
        streamingPath = Path.Combine(Application.streamingAssetsPath, "grow_tower_enemies.json");
    }

    private void Start()
    {
        LoadEnemyData();
    }

    // 데이터 저장
    public void DataSave(List<EnemyInfo> enemies)
    {
        try
        {
            string json = JsonConvert.SerializeObject(enemies, Formatting.Indented);
            File.WriteAllText(persistentPath, json);
            Debug.Log("적 데이터가 성공적으로 저장되었습니다.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"데이터 저장 중 오류 발생: {ex.Message}");
        }
    }

    // 데이터 로드
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
            Debug.LogError($"데이터 로드 중 오류 발생: {ex.Message}");
        }
    }

    private IEnumerator LoadFromStreamingAssets()
    {
        UnityWebRequest request = UnityWebRequest.Get(streamingPath);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"StreamingAssets에서 파일을 불러오지 못했습니다: {request.error}");
        }
        else
        {
            string json = request.downloadHandler.text;

            // 파일을 persistentDataPath로 복사
            File.WriteAllText(persistentPath, json);
            DeserializeAndLoad(json);

            Debug.Log("StreamingAssets에서 데이터를 불러와 저장했습니다.");
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

        Debug.Log("적 데이터가 성공적으로 로드되었습니다.");
    }

    // 데이터 저장 및 불러오기 호출
    public void SaveEnemyData()
    {
        List<EnemyInfo> enemies = new List<EnemyInfo>
        {
            new EnemyInfo("Goblin", true, 100f, 10f, 5f, 2f, 2f, 1.5f, 1f),
            new EnemyInfo("Dragon", false, 300f, 50f, 20f, 10f, 5f, 0.8f, 2f),
            new EnemyInfo("Orc", true, 200f, 30f, 10f, 5f, 0.3f, 1.2f, 3f)
        };

        DataSave(enemies);
        Debug.Log("Enemy 데이터가 저장되었습니다.");
    }

    public void LoadEnemyData()
    {
        DataLoad();
        InitializedPrefabs();
        Debug.Log("Enemy 데이터가 로드 및 Prefab이 초기화되었습니다.");
    }

    private void InitializedPrefabs()
    {
        if (LocalDict.Count == 0)
        {
            Debug.LogWarning("로컬 데이터가 비어 있어 Prefab 초기화를 건너뜁니다.");
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
                Debug.LogWarning("enemyPrefabsArray 배열의 크기를 초과했습니다.");
                break;
            }
        }
        Debug.Log("Prefabs가 성공적으로 초기화되었습니다.");
    }

    public GameObject GetEnemyPrefab(int id)
    {
        if (enemyPrefabs.ContainsKey(id))
        {
            return enemyPrefabs[id];
        }
        else
        {
            Debug.LogWarning($"ID {id}에 해당하는 Prefab이 없습니다.");
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
            Debug.LogWarning($"ID {id}에 해당하는 EnemyInfo가 없습니다.");
            return null;
        }
    }
}
