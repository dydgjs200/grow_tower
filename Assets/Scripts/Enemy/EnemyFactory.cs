using UnityEngine;

public class EnemyFactory
{
    public static GameObject CreateEnemy(string key, Transform SpwanPoint)
    {
        GameObject prefab = EnemyDataManager.Instance.GetEnemyPrefab(key);

        if (prefab == null) return null;

        EnemyInfo info = EnemyDataManager.Instance.GetEnemyInfo(key);

        if (info == null) return null;

        GameObject newEnemy = GameObject.Instantiate(prefab, SpwanPoint.position, Quaternion.identity);
        EnemyController enemyController = newEnemy.GetComponent<EnemyController>();

        if(enemyController != null)
        {
            enemyController.InitializeEnemy(info);
        }

        return newEnemy;
    }
}
