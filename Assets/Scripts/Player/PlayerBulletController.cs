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

    void MoveBullet()       // 찾은 적을 향해 총알 발사
    {
        if (target != null)
        {
            // 목표 방향 계산
            Vector3 direction = (target.position - transform.position).normalized;

            // 총알 이동
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
    }

    void FindClosesetEnemy()        // player와 가장 근접한 적 찾음
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

    void FindEnemyAndStopShooting()     // 적이 없을 시 총알 오브젝트 파괴
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
            Destroy(gameObject);
        }
    }

}
