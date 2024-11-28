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

            // EnemyDataManager���� key ��������
            string enemyKey = GetEnemyKeyByIndex(enemyIndex);

            if (!string.IsNullOrEmpty(enemyKey))
            {
                // EnemyFactory�� ���� �� ����
                GameObject newEnemy = EnemyFactory.CreateEnemy(enemyKey, spawnPoint);

                if (newEnemy != null)
                {
                    enemyCount++;
                }
            }

            time -= 0.3f;
        }
    }

    // �ε����� ������� EnemyDataManager�� Ű�� �������� �޼���
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
