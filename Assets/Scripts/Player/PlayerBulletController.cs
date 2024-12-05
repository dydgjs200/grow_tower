using UnityEngine;

public class PlayerBulletController : MonoBehaviour
{
    public float speed = 10.0f;
    private Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindClosesetEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        MoveBullet();
        FindEnemyAndStopShooting();
    }

    void MoveBullet()       // ã�� ���� ���� �Ѿ� �߻�
    {
        if (target != null)
        {
            // ��ǥ ���� ���
            Vector3 direction = (target.position - transform.position).normalized;

            // �Ѿ� �̵�
            transform.Translate(direction * speed * Time.deltaTime, Space.World);

            // ��ǥ�� �����ߴ��� Ȯ�� (�Ÿ� ����)
            if (Vector3.Distance(transform.position, target.position) < 0.01f)
            {
                Destroy(target.gameObject); // ��ǥ ������Ʈ �ı�
                Destroy(gameObject);        // �Ѿ� �ı�
            }
        }
    }

    void FindClosesetEnemy()        // player�� ���� ������ �� ã��
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closeDistance = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if(distance < closeDistance)
            {
                closeDistance = distance;
                target = enemy.transform;
            }
        }

        if (target != null)
            return;
    }

    void FindEnemyAndStopShooting()     // ���� ���� �� �Ѿ� ������Ʈ �ı�
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            Destroy(gameObject);
        }
    }
}
