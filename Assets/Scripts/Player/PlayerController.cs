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

    // ������Ʈ ����
    public HealthComponent healthComponent;     //ü�� �ý���
    public AttackComponent attackComponent;

    //�浹ó��
    public Rigidbody2D rg2D;


    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
        attackComponent = GetComponent<AttackComponent>();
        rg2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        AttackCoolDown();
    }

    public void InitializedPlayer(string playerId, float hp, float damage, float attackSpeed)
    {
        PlayerId = playerId;
        HP = hp;
        Damage = damage;
        AttackSpeed = attackSpeed;

        healthComponent.InitializedHP(HP);
        attackComponent.InitializedAttack(Damage, AttackSpeed, playerBullet);
        Debug.Log($"Init Player > {playerId}, {damage}, {attackSpeed}");
    }

    public void AttackCoolDown()
    {
        attackComponent.AttackCoolDown(AttackSpeed);
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
