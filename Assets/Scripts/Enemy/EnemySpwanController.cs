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
        EnemySpwan(Random.Range(0, LocalEnemyDataManager.Instance.LocalDict.Count));
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

            GameObject newEnemy = EnemyFactory.CreateEnemy(enemyIndex, spawnPoint);


            time -= 0.3f;
            enemyCount++;
        }
    }
}
