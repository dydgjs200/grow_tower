using UnityEngine;

public class PlayerBulletController : MonoBehaviour
{
    public float Damage;
    public float BulletSpeed = 10.0f;           // �ӽ� �Ѿ� �ӵ� -> AttackSpeed�� �߻� �����̹Ƿ� �ٸ�!!

    private Transform target;

    private void Awake()
    {
        Damage = GetComponentInParent<PlayerController>().Damage;
    }

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
            transform.Translate(direction * BulletSpeed * Time.deltaTime, Space.World);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            HealthComponent health = collision.GetComponent<HealthComponent>();
            
            if(health != null)
            {
                health.TakeDamage(Damage, "enemy");
            }
            Destroy(gameObject);
        }
    }

}
