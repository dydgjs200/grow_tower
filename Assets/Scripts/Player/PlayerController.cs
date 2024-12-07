using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 기본 스탯
    public string PlayerId { get; private set; }
    public float HP { get; private set; }
    public float Damage { get; private set; }
    public float AttackSpeed { get; private set; }

    // 총알 오브젝트
    public GameObject playerBullet;
    float shoot_time;
    float growBulletSpeed;

    // 컴포넌트 관리
    public HealthComponent healthComponent;     //체력 시스템

    //충돌처리
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
            Debug.Log("플레이어와 적 충돌!");
            TakeDamage(10);
        }
    }

    public void TakeDamage(float damage)
    {
        healthComponent.TakeDamage(damage);
        PlayerLocalCache.Instance.SetPlayerData(PlayerId, "HP", HP);
    }
}
