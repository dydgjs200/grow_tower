using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.IO;
using System;

[System.Serializable]
public class LocalEnemyDataManager : MonoBehaviour
{
    public static LocalEnemyDataManager Instance { get; private set; }
    public Dictionary<int, EnemyInfo> LocalDict = new Dictionary<int, EnemyInfo>();
    public Dictionary<int, GameObject> enemyPrefabs = new Dictionary<int, GameObject>();

    // ��ȣȭ ����
    private AESCrypto crypto;
    public string jsonPath;

    public GameObject[] enemyPrefabsArray;

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

        crypto = new AESCrypto();
        jsonPath = Path.Combine(Application.persistentDataPath, "grow_tower_enemies.json");

        //SaveEnemyData(); -> ������ �߰��� ����..
    }

    private void Start()
    {
        LoadEnemyData();
    }


    public void DataSave(List<EnemyInfo> enemies)
    {
        // �����͸� ����Ʈ�� ����ȭ
        string json = JsonConvert.SerializeObject(enemies, Formatting.Indented);

        // JSON ���ڿ� ��ȣȭ
        string encryptedJson = crypto.EncryptString(json);

        // ��ȣȭ�� ������ ���Ͽ� ����
        File.WriteAllText(jsonPath, encryptedJson);

        Debug.Log("�� �����Ͱ� ���������� ����Ǿ����ϴ�.");
    }

    public void DataLoad()
    {
        if (!File.Exists(jsonPath))
        {
            Debug.LogWarning("������ ������ �������� �ʽ��ϴ�!");
            return;
        }

        try
        {
            // ���Ͽ��� ��ȣȭ�� ������ �б�
            string encryptedJson = File.ReadAllText(jsonPath);

            // �����͸� ��ȣȭ
            string json = crypto.DecryptString(encryptedJson);

            // JSON ���ڿ��� ����Ʈ�� ������ȭ
            List<EnemyInfo> enemies = JsonConvert.DeserializeObject<List<EnemyInfo>>(json);

            // LocalDict �ʱ�ȭ
            LocalDict.Clear();
            for (int i = 0; i < enemies.Count; i++)
            {
                LocalDict.Add(i, enemies[i]);
            }

            // �α׷� ������ ���
            foreach (var enemy in enemies)
            {
                Debug.Log($"�̸�: {enemy.Name}, HP: {enemy.HP}, ���ݷ�: {enemy.Damage}");
            }

            Debug.Log("�� �����Ͱ� ���������� �ε�Ǿ����ϴ�.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"������ �ε� �� ���� �߻�: {ex.Message}");
        }
    }




    //save and Load

    public void SaveEnemyData()
    {
        // ���� �� ������ ����
        List<EnemyInfo> enemies = new List<EnemyInfo>
    {
        new EnemyInfo("Goblin", true, 100f, 10f, 5f, 2f, 1.5f),
        new EnemyInfo("Dragon", false, 300f, 50f, 20f, 10f, 0.8f),
        new EnemyInfo("Orc", true, 200f, 30f, 10f, 5f, 1.2f)
    };

        // ������ ���� ȣ��
        DataSave(enemies);

        Debug.Log("Enemy �����Ͱ� ����Ǿ����ϴ�.");
    }


    public void LoadEnemyData()
    {
        // ������ �ε� ȣ��
        DataLoad();

        // Prefab �ʱ�ȭ
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
