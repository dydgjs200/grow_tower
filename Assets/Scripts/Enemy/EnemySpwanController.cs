using UnityEngine;

public class EnemySpwanController : MonoBehaviour
{
    public Transform[] enemySpwaner;
    public int enemyCount;
    float time;

    void Start()
    {
        time = 0;
        enemyCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        EnemySpwan(Random.Range(0,2));
    }

    public void EnemySpwan(int EnemyID)
    {
        if (enemyCount == 20)
            return;

        time += Time.deltaTime;

        if (time > 0.3f)
        {
            int randomSpwanIndex = Random.Range(0, enemySpwaner.Length);
            Transform points = enemySpwaner[randomSpwanIndex];

            EnemyFactory.CreateEnemy(EnemyID, points);

            time -= 0.3f;
            enemyCount++;
        }
    }
}
