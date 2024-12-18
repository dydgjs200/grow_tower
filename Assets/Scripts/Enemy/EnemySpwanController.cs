using System.Collections;
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
        if (enemyCount == 30) // �� �ִ� ��������
            return;

        if (!GameObject.FindWithTag("Player")) // player�� ���� �� ���� ����
            return;

        time += Time.deltaTime;

        if (time > 0.3f) // �� ���� ������
        {
            if (!LocalEnemyDataManager.Instance.LocalDict.ContainsKey(enemyIndex))
            {
                Debug.LogWarning($"delog: No enemy data found for ID {enemyIndex}. Skipping spawn.");
                return;
            }

            Transform spawnPoint = enemySpwaner[Random.Range(0, enemySpwaner.Length)];
            GameObject newEnemy = EnemyFactory.CreateEnemy(enemyIndex, spawnPoint);

            if (newEnemy != null)
            {
                time -= 0.3f;
                enemyCount++;
            }
        }
    }

}
