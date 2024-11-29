using Firebase.Database;
using UnityEngine;
using System.Threading.Tasks;
using Firebase;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

public class FireBaseDataManager : MonoBehaviour
{
    private DatabaseReference databaseReference;

    public Dictionary<int, EnemyInfo> FireBaseDict = new Dictionary<int, EnemyInfo>();

    private async void Awake()
    {
        var dependencyTask = await FirebaseApp.CheckAndFixDependenciesAsync();

        if (dependencyTask == DependencyStatus.Available)
        {
            Debug.Log("Firebase �ʱ�ȭ ����");
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

            try
            {
                await LoadFireBaseEnemyData();
                await SaveFirebaseDataToLocalJson();
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

    private async Task LoadFireBaseEnemyData()
    {
        try
        {
            Debug.Log("Firebase���� �����͸� �����ɴϴ�...");
            var dataSnapshot = await databaseReference.Child("enemies").GetValueAsync();

            if (dataSnapshot.Exists)
            {
                string rawJson = dataSnapshot.GetRawJsonValue();
                Debug.Log($"Firebase���� ���� ���� JSON ������: {rawJson}");

                // �����Ͱ� �迭���� ��ü���� Ȯ��
                if (rawJson.TrimStart().StartsWith("["))
                {
                    // JSON �迭�� ó��
                    var enemiesList = JsonConvert.DeserializeObject<List<EnemyInfo>>(rawJson);
                    FireBaseDict.Clear();

                    for (int i = 0; i < enemiesList.Count; i++)
                    {
                        FireBaseDict.Add(i, enemiesList[i]);
                        Debug.Log($"[�ε� ����] ID: {i}, Name: {enemiesList[i].Name}");
                    }
                }
                else
                {
                    // JSON ��ü�� ó��
                    FireBaseDict = JsonConvert.DeserializeObject<Dictionary<int, EnemyInfo>>(rawJson);

                    foreach (var enemy in FireBaseDict)
                    {
                        Debug.Log($"[�ε� ����] ID: {enemy.Key}, Name: {enemy.Value.Name}");
                    }
                }

                Debug.Log($"Firebase���� �� {FireBaseDict.Count}���� �� �����͸� �ε��߽��ϴ�.");
            }
            else
            {
                Debug.LogWarning("Firebase�� 'enemies' �����Ͱ� �������� �ʽ��ϴ�.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Firebase ������ �ε� ����: {ex.Message}");
        }
    }


    private async Task SaveFirebaseDataToLocalJson()
    {
        try
        {
            string jsonFilePath = Path.Combine(Application.persistentDataPath, "grow_tower_enemies.json");

            // `Newtonsoft.Json`�� ����Ͽ� Dictionary �����͸� JSON���� ����ȭ
            string json = JsonConvert.SerializeObject(FireBaseDict, Formatting.Indented);

            // ���Ϸ� ����
            await Task.Run(() => File.WriteAllText(jsonFilePath, json));

            Debug.Log($"Firebase �����͸� ���� JSON ���Ϸ� ���� �Ϸ�: {jsonFilePath}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Firebase �����͸� ���� JSON���� �����ϴ� �� ���� �߻�: {ex.Message}");
        }
    }
}
