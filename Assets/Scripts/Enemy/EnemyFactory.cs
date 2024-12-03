using UnityEngine;

public class EnemyFactory
{
    public static GameObject CreateEnemy(int ID, Transform SpwanPoint)
    {
        if (LocalEnemyDataManager.Instance == null)
        {
            Debug.LogError("EnemyDataManager.Instance�� null�Դϴ�. ���丮�� ����� �� �����ϴ�.");
            return null;
        }

        // Prefab ��������
        GameObject prefab = LocalEnemyDataManager.Instance.GetEnemyPrefab(ID);
        if (prefab == null)
        {
            Debug.LogWarning($"ID {ID}�� �ش��ϴ� Prefab�� ã�� �� �����ϴ�.");
            return null;
        }

        // EnemyInfo ��������
        EnemyInfo info = LocalEnemyDataManager.Instance.GetEnemyInfo(ID);
        if (info == null)
        {
            Debug.LogWarning($"ID {ID}�� �ش��ϴ� EnemyInfo�� ã�� �� �����ϴ�.");
            return null;
        }

        // �� ����
        GameObject newEnemy = GameObject.Instantiate(prefab, SpwanPoint.position, Quaternion.identity);
        EnemyController enemyController = newEnemy.GetComponent<EnemyController>();

        // EnemyController �ʱ�ȭ
        if (enemyController != null)
        {
            enemyController.InitializeEnemy(info);
        }
        else
        {
            Debug.LogWarning("������ ������Ʈ�� EnemyController�� �����ϴ�.");
        }

        return newEnemy;
    }
}
