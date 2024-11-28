using Firebase.Database;
using Firebase;
using UnityEngine;
using Newtonsoft.Json;

public class AddEnemyManager : MonoBehaviour
{
    private DatabaseReference databaseReference;

    private async void Start()
    {
        // Firebase 초기화 확인
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == Firebase.DependencyStatus.Available)
        {
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            Debug.Log("Firebase 초기화 성공");

            // Firebase 초기화 후 샘플 데이터 추가
            AddSampleData();
        }
        else
        {
            Debug.LogError($"Firebase 초기화 실패: {dependencyStatus}");
        }
    }

    public void AddEnemyToDatabase(string enemyName, EnemyInfo enemyInfo)
    {
        string enemyPath = $"enemies/{enemyName}"; // Firebase에서의 경로
        string json = JsonConvert.SerializeObject(enemyInfo); // EnemyInfo 객체를 JSON 문자열로 변환

        // Firebase에 데이터 쓰기
        databaseReference.Child(enemyPath).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log($"Firebase에 '{enemyName}' 데이터 추가 완료!");
            }
            else
            {
                Debug.LogError($"Firebase에 데이터 추가 실패: {task.Exception.InnerException?.Message ?? task.Exception.Message}");
            }
        });
    }

    private void AddSampleData()
    {
        EnemyInfo enemy1 = new EnemyInfo(0, true, 100, 1, 1, 0, 0);
        AddEnemyToDatabase("돌격자", enemy1);

        EnemyInfo enemy2 = new EnemyInfo(1, false, 80, 3, 0, 0, 3);
        AddEnemyToDatabase("사수", enemy2);

        EnemyInfo enemy3 = new EnemyInfo(2, true, 150, 3, 0, 0, 3);
        AddEnemyToDatabase("추적자", enemy3);

        EnemyInfo enemy4 = new EnemyInfo(3, false, 400, 3, 0, 0, 3);
        AddEnemyToDatabase("대포맨", enemy3);
    }

}
