using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // �⺻ ����
    public string PlayerId { get; private set; }
    public float HP { get; private set; }
    public float Damage { get; private set; }
    public float AttackSpeed { get; private set; }

    // �Ѿ� ������Ʈ
    public GameObject playerBullet;
    float shoot_time;
    float growBulletSpeed;

    // ������Ʈ ����
    public HealthComponent healthComponent;     //ü�� �ý���

    //�浹ó��
    public Rigidbody2D rg2D;


    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
        rg2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        growBulletSpeed = 1.0f;
    }

    void Update()
    {
        ShootPlayerBullet();
    }

    public void InitializedPlayer(string playerId, float hp, float damage, float attackSpeed)
    {
        PlayerId = playerId;
        HP = hp;
        Damage = damage;
        AttackSpeed = attackSpeed;

        healthComponent.InitializedHP(HP);
    }

    void ShootPlayerBullet()
    {
        shoot_time += Time.deltaTime;

        if (shoot_time >= growBulletSpeed)
        {
            if (GameObject.FindWithTag("Enemy"))
                Instantiate(playerBullet, transform.position, Quaternion.identity);

            shoot_time -= growBulletSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("�÷��̾�� �� �浹!");
            TakeDamage(10);
        }
    }

    public void TakeDamage(float damage)
    {
        healthComponent.TakeDamage(damage);
        PlayerLocalCache.Instance.SetPlayerData(PlayerId, "HP", HP);
    }
}
