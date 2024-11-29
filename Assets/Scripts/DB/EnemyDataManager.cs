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

            // ���� �����͸� �а� Prefab �ʱ�ȭ
            LoadLocalEnemyData().ContinueWith(_ =>
            {
                InitializedPrefabs();
                Debug.Log("Enemy Prefab �ʱ�ȭ �Ϸ�");
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
            Debug.Log("���� JSON �����͸� �н��ϴ�...");
            string jsonFilePath = Path.Combine(Application.persistentDataPath, "grow_tower_enemies.json");

            if (!File.Exists(jsonFilePath))
            {
                Debug.LogWarning($"���� JSON ������ �������� �ʽ��ϴ�: {jsonFilePath}");
                return;
            }

            string json = await Task.Run(() => File.ReadAllText(jsonFilePath));
            Debug.Log($"���� JSON ������: {json}");

            // JSON �����͸� Dictionary<int, EnemyInfo>�� ������ȭ
            LocalDict = JsonConvert.DeserializeObject<Dictionary<int, EnemyInfo>>(json);

            Debug.Log($"���� JSON���� {LocalDict.Count}���� �� �����͸� �ε��߽��ϴ�.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"���� JSON ������ �ε� ����: {ex.Message}");
        }
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
