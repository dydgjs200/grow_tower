using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{

    public GameObject player;
    public Rigidbody2D rg2D;


    public float Damage;        // ������ �� ����
    public float BulletSpeed = 5.0f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        rg2D = GetComponent<Rigidbody2D>();

        Damage = GetComponentInParent<EnemyController>().Damage;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveBullet();
        FindPlayerAndStopShooting();
    }

    public void MoveBullet()            // �� �Ѿ� �߻�
    {
        if(player != null)
        {
            Vector3 dir = (player.transform.position - transform.position).normalized;

            transform.Translate(dir * Time.deltaTime * BulletSpeed);
        }
    }
    public void FindPlayerAndStopShooting()     // ���� ���� �� �Ѿ� ������Ʈ �ı�
    {
        if (!GameObject.FindGameObjectWithTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //�Ѿ˰� �ε��� �÷��̾��� healthComponent ���� ���� Ȯ�� -> takeDamage 
            HealthComponent health = collision.GetComponent<HealthComponent>();
            PlayerController controller = collision.GetComponent<PlayerController>();
            if(health != null)
            {
                health.TakeDamage(Damage, "player");
                //PlayerLocalCache.Instance.SetPlayerData(controller.PlayerId, "HP", controller.HP);          // player�� ���� ���� ����
            }

            Destroy(gameObject);         // �÷��̾� ���� �� EnemyBullet ����
        }
    }
}
