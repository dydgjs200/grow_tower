using Firebase.Database;
using Firebase;
using UnityEngine;
using Newtonsoft.Json;

public class AddEnemyManager : MonoBehaviour
{
    private DatabaseReference databaseReference;

    private async void Start()
    {
        // Firebase �ʱ�ȭ Ȯ��
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == Firebase.DependencyStatus.Available)
        {
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            Debug.Log("Firebase �ʱ�ȭ ����");

            // Firebase �ʱ�ȭ �� ���� ������ �߰�
            AddSampleData();
        }
        else
        {
            Debug.LogError($"Firebase �ʱ�ȭ ����: {dependencyStatus}");
        }
    }

    public void AddEnemyToDatabase(int EnemyID, EnemyInfo enemyInfo)
    {
        string enemyPath = $"enemies/{EnemyID}"; // Firebase������ ���
        string json = JsonConvert.SerializeObject(enemyInfo); // EnemyInfo ��ü�� JSON ���ڿ��� ��ȯ

        // Firebase�� ������ ����
        databaseReference.Child(enemyPath).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log($"Firebase�� '{EnemyID}' ������ �߰� �Ϸ�!");
            }
            else
            {
                Debug.LogError($"Firebase�� ������ �߰� ����: {task.Exception.InnerException?.Message ?? task.Exception.Message}");
            }
        });
    }

    private void AddSampleData()
    {
        EnemyInfo enemy0 = new EnemyInfo("������", true, 100, 1, 1, 0, 10);
        AddEnemyToDatabase(0, enemy0);

        EnemyInfo enemy1 = new EnemyInfo("���", false, 80, 3, 0, 0, 2);
        AddEnemyToDatabase(1, enemy1);

        EnemyInfo enemy2 = new EnemyInfo("������", true, 150, 3, 0, 0, 0);
        AddEnemyToDatabase(2, enemy2);

        EnemyInfo enemy3 = new EnemyInfo("������", false, 400, 3, 0, 0, 13);
        AddEnemyToDatabase(3, enemy3);
    }

}
