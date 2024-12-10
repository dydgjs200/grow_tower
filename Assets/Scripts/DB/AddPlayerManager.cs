using Firebase.Database;
using Firebase;
using Newtonsoft.Json;
using UnityEngine;
using System.Collections.Generic;

public class AddPlayerManager : MonoBehaviour
{
    private DatabaseReference databaseReference;

    private async void Start()
    {
        // Firebase �ʱ�ȭ Ȯ��
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == Firebase.DependencyStatus.Available)
        {
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

            // Firebase �ʱ�ȭ �� ���� ������ �߰�
            AddSamplePlayers();
        }
        else
        {
            Debug.LogError($"Firebase �ʱ�ȭ ����: {dependencyStatus}");
        }
    }

    public async void AddSamplePlayers()
    {
        try
        {
            var players = new Dictionary<string, PlayerInfo>();
        {
                players.Add("admin", new PlayerInfo(100, 10, 0.5f));
        };

            string json = JsonConvert.SerializeObject(players, Formatting.Indented);
            await databaseReference.Child("players").SetRawJsonValueAsync(json);

            Debug.Log("���� �÷��̾� �����͸� Firebase�� �߰��߽��ϴ�.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Firebase ������ �߰� ����: {ex.Message}");
        }
    }
}
