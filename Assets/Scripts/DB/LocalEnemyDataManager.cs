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

    // 암호화 관련
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

        //SaveEnemyData(); -> 데이터 추가할 때만..
    }

    private void Start()
    {
        LoadEnemyData();
    }


    public void DataSave(List<EnemyInfo> enemies)
    {
        // 데이터를 리스트로 직렬화
        string json = JsonConvert.SerializeObject(enemies, Formatting.Indented);

        // JSON 문자열 암호화
        string encryptedJson = crypto.EncryptString(json);

        // 암호화된 데이터 파일에 저장
        File.WriteAllText(jsonPath, encryptedJson);

        Debug.Log("적 데이터가 성공적으로 저장되었습니다.");
    }

    public void DataLoad()
    {
        if (!File.Exists(jsonPath))
        {
            Debug.LogWarning("데이터 파일이 존재하지 않습니다!");
            return;
        }

        try
        {
            // 파일에서 암호화된 데이터 읽기
            string encryptedJson = File.ReadAllText(jsonPath);

            // 데이터를 복호화
            string json = crypto.DecryptString(encryptedJson);

            // JSON 문자열을 리스트로 역직렬화
            List<EnemyInfo> enemies = JsonConvert.DeserializeObject<List<EnemyInfo>>(json);

            // LocalDict 초기화
            LocalDict.Clear();
            for (int i = 0; i < enemies.Count; i++)
            {
                LocalDict.Add(i, enemies[i]);
            }

            // 로그로 데이터 출력
            foreach (var enemy in enemies)
            {
                Debug.Log($"이름: {enemy.Name}, HP: {enemy.HP}, 공격력: {enemy.Damage}");
            }

            Debug.Log("적 데이터가 성공적으로 로드되었습니다.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"데이터 로드 중 오류 발생: {ex.Message}");
        }
    }




    //save and Load

    public void SaveEnemyData()
    {
        // 예시 적 데이터 생성
        List<EnemyInfo> enemies = new List<EnemyInfo>
    {
        new EnemyInfo("Goblin", true, 100f, 10f, 5f, 2f, 1.5f),
        new EnemyInfo("Dragon", false, 300f, 50f, 20f, 10f, 0.8f),
        new EnemyInfo("Orc", true, 200f, 30f, 10f, 5f, 1.2f)
    };

        // 데이터 저장 호출
        DataSave(enemies);

        Debug.Log("Enemy 데이터가 저장되었습니다.");
    }


    public void LoadEnemyData()
    {
        // 데이터 로드 호출
        DataLoad();

        // Prefab 초기화
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
