using UnityEngine;

public class EnemySpwanController : MonoBehaviour
{
    public Transform[] enemySpwaner; // �� ���� ��ġ �迭
    public int enemyCount;           // ���� �� ����
    private float time;

    void Start()
    {
        time = 0;
        enemyCount = 0;
    }

    void Update()
    {
        // �������� ���� ����
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
