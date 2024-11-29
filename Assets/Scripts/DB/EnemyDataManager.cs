using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Threading.Tasks;

[System.Serializable]
public class EnemyDataManager : MonoBehaviour
{
    public static EnemyDataManager Instance { get; private set; }
    public Dictionary<int, EnemyInfo> LocalDict = new Dictionary<int, EnemyInfo>();
    public Dictionary<int, GameObject> enemyPrefabs = new Dictionary<int, GameObject>();

    public GameObject[] enemyPrefabsArray;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 로컬 데이터를 읽고 Prefab 초기화
            LoadLocalEnemyData().ContinueWith(_ =>
            {
                InitializedPrefabs();
                Debug.Log("Enemy Prefab 초기화 완료");
            });
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private async Task LoadLocalEnemyData()
    {
        try
        {
            Debug.Log("로컬 JSON 데이터를 읽습니다...");
            string jsonFilePath = Path.Combine(Application.persistentDataPath, "grow_tower_enemies.json");

            if (!File.Exists(jsonFilePath))
            {
                Debug.LogWarning($"로컬 JSON 파일이 존재하지 않습니다: {jsonFilePath}");
                return;
            }

            string json = await Task.Run(() => File.ReadAllText(jsonFilePath));
            Debug.Log($"로컬 JSON 데이터: {json}");

            // JSON 데이터를 Dictionary<int, EnemyInfo>로 역직렬화
            LocalDict = JsonConvert.DeserializeObject<Dictionary<int, EnemyInfo>>(json);

            Debug.Log($"로컬 JSON에서 {LocalDict.Count}개의 적 데이터를 로드했습니다.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"로컬 JSON 데이터 로드 실패: {ex.Message}");
        }
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
            enemyPrefabs.Add(enemy.Key, enemyPrefabsArray[count]);
            count++;
        }
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
