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
        // Firebase 초기화
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Result == DependencyStatus.Available)
            {
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
                LoadMonsterData();

                //인스펙터에 연결된 Enemy 오브젝트를 dictionary에 저장
                for(int i=0; i<MonsterPrefabsArray.Length; i++)
                {
                    MonsterPrefabs.Add(i, MonsterPrefabsArray[i]);
                }
            }
            else
            {
                Debug.LogError($"Firebase 초기화 실패: {task.Result}");
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
                    Debug.Log($"몬스터 로드: {monster.Name}, HP: {monster.HP}");
                }
            }
            else
            {
                Debug.LogError($"데이터 로드 실패: {task.Exception}");
            }
        });
    }

    public MonsterInfo GetMonsterInfo(int id)           // DB에서 정보 가져오기
    {
        if (MonsterInfo.ContainsKey(id))
        {
            return MonsterInfo[id];
        }
        Debug.LogWarning($"ID {id}에 해당하는 몬스터를 찾을 수 없습니다.");
        return null;
    }

    public GameObject GetMonsterPrefab(int id)          // 프리팹 가져오기
    {
        if (MonsterPrefabs.ContainsKey(id))
        {
            return MonsterPrefabs[id];
        }

        return null;
    }
}
