using UnityEngine;

public class EnemySpwanController : MonoBehaviour
{
    public Transform[] enemySpwaner; // 적 생성 위치 배열
    public int enemyCount;           // 현재 적 개수
    private float time;

    void Start()
    {
        time = 0;
        enemyCount = 0;
    }

    void Update()
    {
        // 랜덤으로 적을 생성
        EnemySpwan(Random.Range(0, EnemyDataManager.Instance.FireBaseDict.Count));
    }

    public void EnemySpwan(int enemyIndex)
    {
        if (enemyCount == 20)
            return;

        time += Time.deltaTime;

        if (time > 0.3f)
        {
            int randomSpwanIndex = Random.Range(0, enemySpwaner.Length);
            Transform spawnPoint = enemySpwaner[randomSpwanIndex];

            // EnemyDataManager에서 key 가져오기
            string enemyKey = GetEnemyKeyByIndex(enemyIndex);

            if (!string.IsNullOrEmpty(enemyKey))
            {
                // EnemyFactory를 통해 적 생성
                GameObject newEnemy = EnemyFactory.CreateEnemy(enemyKey, spawnPoint);

                if (newEnemy != null)
                {
                    enemyCount++;
                }
            }

            time -= 0.3f;
        }
    }

    // 인덱스를 기반으로 EnemyDataManager의 키를 가져오는 메서드
    private string GetEnemyKeyByIndex(int index)
    {
        int i = 0;
        foreach (var key in EnemyDataManager.Instance.FireBaseDict.Keys)
        {
            if (i == index)
                return key;
            i++;
        }

        return null;
    }
}
