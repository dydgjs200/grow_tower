using System.Collections;
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
        if (enemyCount == 30)            // 적 최대 생성갯수
            return;

        if (!GameObject.FindWithTag("Player"))      // player가 없을 시 스폰 종료
            return;

        time += Time.deltaTime;

        if (time > 0.3f)        // 적 생성 딜레이
        {
            int randomSpwanIndex = Random.Range(0, enemySpwaner.Length);
            Transform spawnPoint = enemySpwaner[randomSpwanIndex];

            GameObject newEnemy = EnemyFactory.CreateEnemy(enemyIndex, spawnPoint);


            time -= 0.3f;
            enemyCount++;
        }
    }
}
