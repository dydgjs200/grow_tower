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

    public void AddEnemyToDatabase(string enemyName, EnemyInfo enemyInfo)
    {
        string enemyPath = $"enemies/{enemyName}"; // Firebase������ ���
        string json = JsonConvert.SerializeObject(enemyInfo); // EnemyInfo ��ü�� JSON ���ڿ��� ��ȯ

        // Firebase�� ������ ����
        databaseReference.Child(enemyPath).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log($"Firebase�� '{enemyName}' ������ �߰� �Ϸ�!");
            }
            else
            {
                Debug.LogError($"Firebase�� ������ �߰� ����: {task.Exception.InnerException?.Message ?? task.Exception.Message}");
            }
        });
    }

    private void AddSampleData()
    {
        EnemyInfo enemy1 = new EnemyInfo(0, true, 100, 1, 1, 0, 0);
        AddEnemyToDatabase("������", enemy1);

        EnemyInfo enemy2 = new EnemyInfo(1, false, 80, 3, 0, 0, 3);
        AddEnemyToDatabase("���", enemy2);

        EnemyInfo enemy3 = new EnemyInfo(2, true, 150, 3, 0, 0, 3);
        AddEnemyToDatabase("������", enemy3);

        EnemyInfo enemy4 = new EnemyInfo(3, false, 400, 3, 0, 0, 3);
        AddEnemyToDatabase("������", enemy3);
    }

}
