using Firebase.Database;
using UnityEngine;
using System.Threading.Tasks;
using Firebase;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;

public class FireBasePlayerDataManager : MonoBehaviour
{
    private DatabaseReference databaseReference;

    // ���� ���� �� �ѹ��� ó���� ���� �̱���
    public static FireBasePlayerDataManager instance { get; private set; }
    private bool isFirebaseLoad = false;

    public Dictionary<string, PlayerInfo> FireBaseDict = new Dictionary<string, PlayerInfo>();

    private async void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);

            if (!isFirebaseLoad)
            {
                var dependencyTask = await FirebaseApp.CheckAndFixDependenciesAsync();

                if (dependencyTask == DependencyStatus.Available)
                {
                    databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

                    try
                    {
                        await LoadFireBasePlayerData();
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

                isFirebaseLoad = true;
            }
        }
    }

    private async Task LoadFireBasePlayerData()
    {
        try
        {
            Debug.Log("Firebase���� �����͸� �����ɴϴ�...");
            var dataSnapshot = await databaseReference.Child("players").GetValueAsync();

            if (dataSnapshot.Exists)
            {
                string rawJson = dataSnapshot.GetRawJsonValue();
                Debug.Log($"Firebase���� ���� ���� JSON ������: {rawJson}");

                // �����Ͱ� �迭���� ��ü���� Ȯ��
                if (rawJson.TrimStart().StartsWith("["))
                {
                    // JSON �迭�� ó��
                    var playersList = JsonConvert.DeserializeObject<List<PlayerInfo>>(rawJson);
                    FireBaseDict.Clear();

                    for (int i = 0; i < playersList.Count; i++)
                    {
                        FireBaseDict.Add(i.ToString(), playersList[i]);
                        Debug.Log($"[�ε� ����] ID: {i}, Name: {playersList[i].HP}");
                    }
                }
                else
                {
                    // JSON ��ü�� ó��
                    FireBaseDict = JsonConvert.DeserializeObject<Dictionary<string, PlayerInfo>>(rawJson);

                    foreach (var enemy in FireBaseDict)
                    {
                        Debug.Log($"[�ε� ����] ID: {enemy.Key}, Name: {enemy.Value.HP}");
                    }
                }

                Debug.Log($"Firebase���� �� {FireBaseDict.Count}���� �� �����͸� �ε��߽��ϴ�.");
            }
            else
            {
                Debug.LogWarning("Firebase�� 'players' �����Ͱ� �������� �ʽ��ϴ�.");
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
            string jsonFilePath = Path.Combine(Application.persistentDataPath, "grow_tower_players.json");

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
