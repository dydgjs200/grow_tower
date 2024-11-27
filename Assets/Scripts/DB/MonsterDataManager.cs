using System.Collections.Generic;
using System.Threading;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;


public class MonsterInfo
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

[System.Serializable]
public class MonsterDataManager : MonoBehaviour
{

    public static MonsterDataManager Instance { get; private set; }
    private DatabaseReference databaseReference;
    public Dictionary<int, MonsterInfo> MonsterInfo = new Dictionary<int, MonsterInfo>();
    public Dictionary<int, GameObject> MonsterPrefabs = new Dictionary<int, GameObject>();

    public GameObject[] MonsterPrefabsArray;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Firebase �ʱ�ȭ
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Result == DependencyStatus.Available)
            {
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
                LoadMonsterData();

                //�ν����Ϳ� ����� Enemy ������Ʈ�� dictionary�� ����
                for(int i=0; i<MonsterPrefabsArray.Length; i++)
                {
                    MonsterPrefabs.Add(i, MonsterPrefabsArray[i]);
                }
            }
            else
            {
                Debug.LogError($"Firebase �ʱ�ȭ ����: {task.Result}");
            }
        });
    }

    private void LoadMonsterData()
    {
        databaseReference.Child("monsters").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                foreach (var monsterSnapshot in snapshot.Children)
                {
                    MonsterInfo monster = JsonUtility.FromJson<MonsterInfo>(monsterSnapshot.GetRawJsonValue());
                    MonsterInfo.Add(monster.ID, monster);
                    Debug.Log($"���� �ε�: {monster.Name}, HP: {monster.HP}");
                }
            }
            else
            {
                Debug.LogError($"������ �ε� ����: {task.Exception}");
            }
        });
    }

    public MonsterInfo GetMonsterInfo(int id)           // DB���� ���� ��������
    {
        if (MonsterInfo.ContainsKey(id))
        {
            return MonsterInfo[id];
        }
        Debug.LogWarning($"ID {id}�� �ش��ϴ� ���͸� ã�� �� �����ϴ�.");
        return null;
    }

    public GameObject GetMonsterPrefab(int id)          // ������ ��������
    {
        if (MonsterPrefabs.ContainsKey(id))
        {
            return MonsterPrefabs[id];
        }

        return null;
    }
}
