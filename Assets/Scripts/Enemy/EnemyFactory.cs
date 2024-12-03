using UnityEngine;

public class EnemyFactory
{
    public static GameObject CreateEnemy(int ID, Transform SpwanPoint)
    {
        if (LocalEnemyDataManager.Instance == null)
        {
            Debug.LogError("EnemyDataManager.Instance가 null입니다. 팩토리를 사용할 수 없습니다.");
            return null;
        }

        // Prefab 가져오기
        GameObject prefab = LocalEnemyDataManager.Instance.GetEnemyPrefab(ID);
        if (prefab == null)
        {
            Debug.LogWarning($"ID {ID}에 해당하는 Prefab을 찾을 수 없습니다.");
            return null;
        }

        // EnemyInfo 가져오기
        EnemyInfo info = LocalEnemyDataManager.Instance.GetEnemyInfo(ID);
        if (info == null)
        {
            Debug.LogWarning($"ID {ID}에 해당하는 EnemyInfo를 찾을 수 없습니다.");
            return null;
        }

        // 적 생성
        GameObject newEnemy = GameObject.Instantiate(prefab, SpwanPoint.position, Quaternion.identity);
        EnemyController enemyController = newEnemy.GetComponent<EnemyController>();

        // EnemyController 초기화
        if (enemyController != null)
        {
            enemyController.InitializeEnemy(info);
        }
        else
        {
            Debug.LogWarning("생성된 오브젝트에 EnemyController가 없습니다.");
        }

        return newEnemy;
    }
}
